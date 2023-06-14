using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOGrasper.Commands;
using System.Windows.Input;
using System.IO;
using System.Windows.Forms;

namespace LOGrasper.ViewModels
{
    public class RootFolderBrowseViewModel : ViewModelBase
    {
        private string _rootFolderPath;
        private bool _folderExists = false;

        public long totalSizeBytes = 0;
        public int folderCount = 0;
        public int fileCount = 0;
        private double _totalSizeMB = 0;

        public string RootFolderPath
        {
            get { return _rootFolderPath; }
            set
            {
                _rootFolderPath = value;
                OnPropertyChanged(nameof(RootFolderPath));
            }
        }

        public bool FolderExists
        {
            get { return _folderExists; }
            set
            {
                _folderExists = value; 
                OnPropertyChanged(nameof(FolderExists));
            }
        }

        public double TotalSizeMB
        {
            get { return _totalSizeMB; }
            set
            {
                _totalSizeMB = value;
                OnPropertyChanged(nameof(TotalSizeMB)); 
            }
        }
        public ICommand RootFolderBrowseCommand { get; }

        public RootFolderBrowseViewModel(string rootFolder)
        {
            _rootFolderPath = rootFolder;
        }

        public RootFolderBrowseViewModel(SearchViewViewModel searchViewViewmodel) 
        {
            RootFolderPath = "Please Select Root Folder";
            RootFolderBrowseCommand = new RootFolderBrowseCommand(this, searchViewViewmodel);

        }


        public void CalculateDirectoryStats(string directoryPath, ref long totalSizeBytes, ref int folderCount, ref int fileCount)
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

        public double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        public int GetTotalFilesSelected()
        {
            return fileCount;
        }
    }


}
