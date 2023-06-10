using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOGrasper.Commands
{
    internal class SaveOutputCommand : CommandBase
    {
        private readonly OutputWindowViewModel _outputWindowViewModel;

        public SaveOutputCommand(OutputWindowViewModel outputWindowViewModel) 
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
            _outputWindowViewModel.SaveOutput();
        }

        public override bool CanExecute(object? parameter)
        {
            return !_outputWindowViewModel.FoundInFilesEmpty;
        }
    }
}
