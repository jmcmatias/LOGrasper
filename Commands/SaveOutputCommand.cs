using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class SaveOutputCommand : CommandBase
    {
        private readonly OutputWindowViewModel _outputWindowViewModel;
        private readonly SearchViewViewModel _searchViewViewModel;
        private string _stopwatch;

        public SaveOutputCommand(OutputWindowViewModel outputWindowViewModel, SearchViewViewModel searchViewViewModel) 
        {
            _searchViewViewModel = searchViewViewModel;
            _outputWindowViewModel = outputWindowViewModel;
            _searchViewViewModel.PropertyChanged += _searchViewViewModel_PropertyChanged;
            _outputWindowViewModel.PropertyChanged += _outputWindowViewModel_PropertyChanged;
            
        }

        private void _searchViewViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_searchViewViewModel.StopwatchString))
            {
                _stopwatch = _searchViewViewModel.StopwatchString;
            }
        }

        private void _outputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            _outputWindowViewModel.SaveOutput(_stopwatch);
        }

        public override bool CanExecute(object? parameter)
        {
            return !_outputWindowViewModel.FoundInFilesEmpty;
        }
    }
}
