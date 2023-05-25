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


    }
}
