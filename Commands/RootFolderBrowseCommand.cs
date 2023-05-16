using LOGrasper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs.Wpf;
using System.IO;

namespace LOGrasper.Commands
{
    internal class RootFolderBrowseCommand : CommandBase
    {

        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;

        public RootFolderBrowseCommand(RootFolderBrowseViewModel rootFolderBrowseViewModel)
        {
            _rootFolderBrowseViewModel = rootFolderBrowseViewModel;
        }

        /// <summary>
        /// Opens a Folder Dialog using Ookii.Dialogs.Wpf and gets the selected path
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            VistaFolderBrowserDialog dialog = new();
            dialog.ShowDialog();
            if(Directory.Exists( dialog.SelectedPath))
            {
                _rootFolderBrowseViewModel.RootFolderPath = dialog.SelectedPath;
            }
           
        }
    }
}
