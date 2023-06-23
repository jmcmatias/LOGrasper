using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
