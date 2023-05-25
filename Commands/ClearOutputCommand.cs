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
            _outputWindowViewModel.PropertyChanged += OutputWindowViewModel_PropertyChanged;
        }

        private void OutputWindowViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            
        }

        public override void Execute(object? parameter)
        {
            _outputWindowViewModel.ClearOutput();

        }
    }
}
