using LOGrasper.ViewModels;

namespace LOGrasper.Commands
{
    internal class SaveOutputCommand : CommandBase
    {
        private readonly OutputWindowViewModel _outputWindowViewModel;  // Reference to the OutputWindowViewModel
        private readonly SearchViewViewModel _searchViewViewModel;  // Reference to the SearchViewViewModel
        private string _stopwatch;  // Holds the stopwatch value

        public SaveOutputCommand(OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel)
        {
            _stopwatch = string.Empty;  // Initialize the stopwatch string

            _searchViewViewModel = searchViewViewModel;  // Assign the SearchViewViewModel passed as an argument
            _outputWindowViewModel = outputWindowViewModel;  // Assign the OutputWindowViewModel passed as an argument

            _searchViewViewModel.PropertyChanged += _searchViewViewModel_PropertyChanged;  // Subscribe to the PropertyChanged event of the SearchViewViewModel
            _outputWindowViewModel.PropertyChanged += _outputWindowViewModel_PropertyChanged;  // Subscribe to the PropertyChanged event of the OutputWindowViewModel
        }

        private void _searchViewViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_searchViewViewModel.StopwatchString))
            {
                _stopwatch = _searchViewViewModel.StopwatchString;  // Update the stopwatch value when the StopwatchString property changes
            }
        }

        private void _outputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();  // Call the base class method to notify that the CanExecute state may have changed
        }

        public override void Execute(object? parameter)
        {
            _outputWindowViewModel.SaveOutput(_stopwatch);  // Execute the SaveOutput method of the OutputWindowViewModel and pass the stopwatch value
        }

        public override bool CanExecute(object? parameter)
        {
            return !_outputWindowViewModel.FoundInFilesEmpty;  // Determine if the command can be executed based on the FoundInFilesEmpty property of the OutputWindowViewModel
        }
    }
}
