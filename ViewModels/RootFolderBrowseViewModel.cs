using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LOGrasper.Commands;
using System.Windows.Input;
using System.IO;

namespace LOGrasper.ViewModels
{
    public class RootFolderBrowseViewModel : ViewModelBase
    {
        public string _rootFolder;

        public ICommand RootFolderBrowseCommand { get; }

        public RootFolderBrowseViewModel(string rootFolder)
        {
            _rootFolder = rootFolder;
        }

        public RootFolderBrowseViewModel() 
        {
            _rootFolder = "test";
            RootFolderBrowseCommand = new RootFolderBrowseCommand(this);

        }


    }
}
