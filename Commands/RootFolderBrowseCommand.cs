using LOGrasper.ViewModels;
using Ookii.Dialogs.Wpf;
using System;
using System.IO;
using System.Threading.Tasks;

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
            }

            _rootFolderBrowseViewModel.totalSizeBytes = 0;
            _rootFolderBrowseViewModel.folderCount = 0;
            _rootFolderBrowseViewModel.fileCount = 0;
            

            _rootFolderBrowseViewModel.CalculateDirectoryStats(dialog.SelectedPath,ref _rootFolderBrowseViewModel.totalSizeBytes, ref _rootFolderBrowseViewModel.folderCount, ref _rootFolderBrowseViewModel.fileCount);
            _rootFolderBrowseViewModel.TotalSizeMB = _rootFolderBrowseViewModel.ConvertBytesToMegabytes(_rootFolderBrowseViewModel.totalSizeBytes);
            _searchViewViewModel.MessageDispenser = "You Picked a total of " + Math.Round(_rootFolderBrowseViewModel.TotalSizeMB, 2).ToString() + " MB " + " from a total of " + _rootFolderBrowseViewModel.folderCount + " folders and " + _rootFolderBrowseViewModel.fileCount + " files";
        }

    }
}
