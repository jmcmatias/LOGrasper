using LOGrasper.ViewModels;
using Ookii.Dialogs.Wpf;
using System.IO;

namespace LOGrasper.Commands
{
    internal class RootFolderBrowseCommand : CommandBase
    {
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel;
        private readonly SearchViewViewModel _searchViewViewModel;

        public RootFolderBrowseCommand(RootFolderBrowseViewModel rootFolderBrowseViewModel,SearchViewViewModel searchViewViewmodel)
        {
            _rootFolderBrowseViewModel = rootFolderBrowseViewModel;
            _searchViewViewModel = searchViewViewmodel;
        }

        /// <summary>
        /// Opens a Folder Dialog using Ookii.Dialogs.Wpf and gets the selected path
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            VistaFolderBrowserDialog dialog = new();
            dialog.ShowDialog();
            _rootFolderBrowseViewModel.FolderExists = Directory.Exists(dialog.SelectedPath);
            if (_rootFolderBrowseViewModel.FolderExists)
            {
                _rootFolderBrowseViewModel.RootFolderPath = dialog.SelectedPath;
                _searchViewViewModel.HasRootFolder = true;
                int debug = 1;
            }
        }
    }
}
