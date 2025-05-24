namespace VVM.UI
{
	public interface IWindowBinder
	{
		void Bind(WindowViewModel viewModel);
		void Close();
	}
}