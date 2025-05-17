using System;
using System.Collections.Generic;

namespace Infrastructure.DI
{
	public sealed class DIContainer
	{
		private readonly DIContainer _parentContainer;
		private readonly Dictionary<(string, Type), DIRegistration> _registrations = new();
		private readonly HashSet<(string, Type)> _resolutions = new();


		public DIContainer(DIContainer parentContainer = null) => _parentContainer = parentContainer;

		public void RegisterAsSingle<T>(Func<DIContainer, T> factory) => RegisterAsSingle(null, factory);

		public void RegisterAsSingle<T>(string tag, Func<DIContainer, T> factory)
		{
			var key = (tag, typeof(T));
			Register(key, factory, isSingle: true);
		}

		public void RegisterAsTransient<T>(Func<DIContainer, T> factory) => RegisterAsTransient(null, factory);

		public void RegisterAsTransient<T>(string tag, Func<DIContainer, T> factory)
		{
			var key = (tag, typeof(T));
			Register(key, factory, isSingle: false);
		}

		public void RegisterInstance<T>(T instance) => RegisterInstance(null, instance);

		public void RegisterInstance<T>(string tag, T instance)
		{
			var key = (tag, typeof(T));
			if (_registrations.ContainsKey(key))
				throw new ArgumentException($"DI: Factory with tag: {key.Item1} & type: {key.Item2.FullName} already registered.");

			_registrations[key] = new DIRegistration
			{
				Instance = instance,
				IsSingle = true
			};
		}

		public T Resolve<T>(string tag = null)
		{
			var key = (tag, typeof(T));

			if (_resolutions.Contains(key))
				throw new ArgumentException($"DI: Cycle dependency for tag: {key.Item1} & type: {key.Item2.FullName}.");

			_resolutions.Add((tag, typeof(T)));

			try
			{
				if (_registrations.TryGetValue(key, out var registration))
				{
					if (registration.IsSingle)
					{
						if (registration.Instance == null && registration.Factory != null)
							registration.Instance = registration.Factory(this);

						return (T)registration.Instance;
					}

					return (T)registration.Factory(this);
				}

				if (_parentContainer != null)
					return _parentContainer.Resolve<T>(tag);
			}
			finally
			{
				_resolutions.Remove(key);
			}

			throw new ArgumentException($"DI: Couldn't find dependency for tag: {key.Item1} & type: {key.Item2.FullName}.");
		}

		private void Register<T>((string, Type) key, Func<DIContainer, T> factory, bool isSingle)
		{
			if (_registrations.ContainsKey(key))
				throw new ArgumentException($"DI: Factory with tag: {key.Item1} & type: {key.Item2.FullName} already registered.");

			_registrations[key] = new DIRegistration
			{
				Factory = c => factory(c),
				IsSingle = isSingle
			};
		}
	}
}