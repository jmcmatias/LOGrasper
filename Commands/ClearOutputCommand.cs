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


        public ClearOutputCommand(OutputWindowViewModel outputWindowViewModel)
        {
            _outputWindowViewModel = outputWindowViewModel;
            _outputWindowViewModel.PropertyChanged += _outputWindowViewModel_PropertyChanged;
        }

        private void _outputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnCanExecuteChanged();
        }

        public override void Execute(object? parameter)
        {
            _outputWindowViewModel.ClearOutput();
        }

        public override bool CanExecute(object? parameter)
        {
            return !_outputWindowViewModel.FoundInFilesEmpty;
        }
    }
}
