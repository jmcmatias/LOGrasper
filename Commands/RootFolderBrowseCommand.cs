using LOGrasper.ViewModels;
using Ookii.Dialogs.Wpf;
using System.IO;


namespace LOGrasper.Commands
{
    internal class RootFolderBrowseCommand : CommandBase
    {
        private readonly RootFolderBrowseViewModel _rootFolderBrowseViewModel; // Reference to the RootFolderBrowseViewModel
        private readonly SearchViewViewModel _searchViewViewModel; // Reference to the SearchViewViewModel

        public RootFolderBrowseCommand(RootFolderBrowseViewModel rootFolderBrowseViewModel, SearchViewViewModel searchViewViewModel)
        {
            _rootFolderBrowseViewModel = rootFolderBrowseViewModel; // Assign the RootFolderBrowseViewModel reference
            _searchViewViewModel = searchViewViewModel; // Assign the SearchViewViewModel reference
        }

        /// <summary>
        /// Opens a Folder Dialog using Ookii.Dialogs.Wpf and gets the selected path
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object? parameter)
        {
            VistaFolderBrowserDialog dialog = new VistaFolderBrowserDialog(); // Create an instance of the folder browser dialog
            dialog.ShowDialog(); // Show the folder browser dialog

            // Check if the selected folder exists
            _rootFolderBrowseViewModel.FolderExists = Directory.Exists(dialog.SelectedPath);

            if (_rootFolderBrowseViewModel.FolderExists)
            {
                // Update the RootFolderPath property in RootFolderBrowseViewModel with the selected folder path
                _rootFolderBrowseViewModel.RootFolderPath = dialog.SelectedPath;

                _searchViewViewModel.HasRootFolder = true; // Indicate that a root folder has been selected in the SearchViewViewModel
            }

            // Reset directory statistics
            _rootFolderBrowseViewModel.totalSizeBytes = 0;
            _rootFolderBrowseViewModel.folderCount = 0;
            _rootFolderBrowseViewModel.fileCount = 0;

            // Calculate directory statistics for the selected folder
            _rootFolderBrowseViewModel.CalculateDirectoryStats(dialog.SelectedPath, ref _rootFolderBrowseViewModel.totalSizeBytes, ref _rootFolderBrowseViewModel.folderCount, ref _rootFolderBrowseViewModel.fileCount);

            // Convert total size to megabytes
            _rootFolderBrowseViewModel.TotalSizeMB = _rootFolderBrowseViewModel.ConvertBytesToMegabytes(_rootFolderBrowseViewModel.totalSizeBytes);

            // Update directory statistics in the SearchViewViewModel
            _searchViewViewModel.GetDirectoryStatistics();
        }
    }
}
