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
            _outputWindowViewModel.PropertyChanged += _outputWindowViewModel_PropertyChanged;
        }

        private void _outputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            _outputWindowViewModel.ClearOutput();
            _searchViewViewModel.MessageDispenser = "";

        }

        public override bool CanExecute(object? parameter)
        {
            return !_outputWindowViewModel.FoundInFilesEmpty;
        }

    }
}
