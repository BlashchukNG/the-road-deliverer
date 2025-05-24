using Infrastructure.DI;

namespace VVM.Root
{
	public abstract class UIService
	{
		protected readonly DIContainer _diContainer;

		
		protected UIService(DIContainer diContainer)
		{
			_diContainer = diContainer;
		}
	}
}