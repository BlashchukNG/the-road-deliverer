using System;

namespace Infrastructure.DI
{
	public class DIRegistration
	{
		public Func<DIContainer, object> Factory { get; set; }
		public bool IsSingle { get; set; }
		public object Instance { get; set; }
	}
}