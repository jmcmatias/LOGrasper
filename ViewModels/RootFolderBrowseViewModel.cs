using System;
using LOGrasper.Commands;
using System.Windows.Input;
using System.IO;


namespace LOGrasper.ViewModels
{
    public class RootFolderBrowseViewModel : ViewModelBase
    {
        private static SearchViewViewModel _searchViewViewModel;

        private string _rootFolderPath;
        private bool _folderExists = false;

        // Variables for directory statistics
        public long totalSizeBytes = 0;
        public int folderCount = 0;
        public int fileCount = 0;
        private double _totalSizeMB = 0;

        // Property for binding the root folder path
        public string RootFolderPath
        {
            get { return _rootFolderPath; }
            set
            {
                _rootFolderPath = value;
                OnPropertyChanged(nameof(RootFolderPath));
            }
        }

        // Property for indicating if the folder exists
        public bool FolderExists
        {
            get { return _folderExists; }
            set
            {
                _folderExists = value;
                OnPropertyChanged(nameof(FolderExists));
            }
        }

        // Property for binding the total size in megabytes
        public double TotalSizeMB
        {
            get { return _totalSizeMB; }
            set
            {
                _totalSizeMB = value;
                OnPropertyChanged(nameof(TotalSizeMB));
            }
        }

        // Command for browsing the root folder
        public ICommand RootFolderBrowseCommand { get; }

        // Constructor
        public RootFolderBrowseViewModel(SearchViewViewModel searchViewViewmodel)
        {
            _searchViewViewModel = searchViewViewmodel;
            RootFolderPath = "Please Select Root Folder";
            RootFolderBrowseCommand = new RootFolderBrowseCommand(this, searchViewViewmodel);
        }

        // Method for calculating directory statistics
        public void CalculateDirectoryStats(string directoryPath, ref long totalSizeBytes, ref int folderCount, ref int fileCount)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(directoryPath);

                    // Update folder count
                    folderCount++;

                    // Calculate size of files in the current directory
                    foreach (FileInfo file in directoryInfo.GetFiles())
                    {
                        totalSizeBytes += file.Length;
                        fileCount++;
                    }

                    // Calculate size of subdirectories recursively
                    foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories())
                    {
                        // Recursive call to calculate stats for subdirectory
                        CalculateDirectoryStats(subDirectory.FullName, ref totalSizeBytes, ref folderCount, ref fileCount);
                    }
                }
            }
            catch (Exception ex)
            {
                _searchViewViewModel.MessageDispenser = "An error ocurred while retriving DirectoryStats: " + ex.Message;
            }
        }

        // Method for converting bytes to megabytes
        public double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        // Method for getting the total number of files selected
        public int GetTotalFilesSelected()
        {
            return fileCount;
        }
    }


}
