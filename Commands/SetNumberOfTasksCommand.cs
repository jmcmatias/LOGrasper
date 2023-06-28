using LOGrasper.ViewModels;

namespace LOGrasper.Commands
{
    internal class SetNumberOfTasksCommand : CommandBase
    {
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel

        public SetNumberOfTasksCommand(SearchViewViewModel searchViewViewModel)
        {
            _searchViewViewModel = searchViewViewModel; // Assign the SearchViewViewModel reference
        }

        public override void Execute(object? parameter)
        {
            _searchViewViewModel.SemaphoreUpdate = _searchViewViewModel.NumberOfTasks; // Update the SemaphoreUpdate property with the value of NumberOfTasks
            _searchViewViewModel.GetSystemInfo(); // Retrieve system information
        }
    }
}
