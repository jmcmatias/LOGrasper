using LOGrasper.ViewModels;

namespace LOGrasper.Commands
{
    internal class SetNumberOfTasksCommand : CommandBase
    {
        private readonly SearchViewViewModel _searchViewViewModel;

        public SetNumberOfTasksCommand(SearchViewViewModel searchViewViewModel)
        {
            _searchViewViewModel = searchViewViewModel;
        }

        public override void Execute(object? parameter)
        {
            _searchViewViewModel.SemaphoreUpdate = _searchViewViewModel.NumberOfTasks;
            _searchViewViewModel.GetSystemInfo();
        }
    }
}
