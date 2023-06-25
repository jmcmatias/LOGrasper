using Accessibility;
using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class ClearOutputCommand : CommandBase
    {
        private readonly OutputWindowViewModel _outputWindowViewModel;
        private readonly SearchViewViewModel _searchViewViewModel;

        public ClearOutputCommand(OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel)
        {
            _outputWindowViewModel = outputWindowViewModel;
            _searchViewViewModel = searchViewViewModel;

            // Subscribe to PropertyChanged event of OutputWindowViewModel
            _outputWindowViewModel.PropertyChanged += _outputWindowViewModel_PropertyChanged;
        }

        // Event handler for PropertyChanged event of OutputWindowViewModel
        private void _outputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Trigger CanExecuteChanged when a property of OutputWindowViewModel changes
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            // Execute the command
            _outputWindowViewModel.ClearOutput(); // Clear the output
            _searchViewViewModel.GetDirectoryStatistics(); // Retrieve directory statistics
            _searchViewViewModel.GetSystemInfo(); // Retrieve system information
        }

        public override bool CanExecute(object? parameter)
        {
            // Determine if the command can execute
            return !_outputWindowViewModel.FoundInFilesEmpty; // Return true if FoundInFiles collection is not empty
        }
    }
}
