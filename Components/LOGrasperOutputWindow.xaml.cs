using LOGrasper.ViewModels;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Controls;
using System.Windows.Input;


namespace LOGrasper.Components
{
    /// <summary>
    /// Interação lógica para LOGrasperOutputWindow.xam
    /// </summary>
    public partial class LOGrasperOutputWindow : UserControl
    {
        private OutputWindowViewModel _outputWindowViewModel;

        public LOGrasperOutputWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Event Handler for opening a file in the output
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _outputWindowViewModel = DataContext as OutputWindowViewModel;          // Instantiate the current viewmodel for the Datacontext.
            try
            {
                if (sender is Label label)
                {
                    if (label.Content.ToString() != null)
                    {
                        string filename = label.Content.ToString();
                        if (File.Exists(filename))
                        {
                            // Start the process for the specific file using the default application
                            ProcessStartInfo processStartInfo = new(filename)
                            {
                                UseShellExecute = true                      // Uses the windows shell to execute the default application for the extension
                            };
                            Process.Start(processStartInfo);
                        }
                        else
                        {
                            _outputWindowViewModel.MainViewModel.MessageDispenser = "Error Opening File " + filename + ", file does not exist.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _outputWindowViewModel.MainViewModel.MessageDispenser = ex.Message;
            }
        }

        private void Image_MouseClick(object sender, MouseButtonEventArgs e)
        {
            _outputWindowViewModel = DataContext as OutputWindowViewModel;
            string url = string.Empty;
            if (sender is Image image)
            {
                switch (image.Name)
                {
                    case "UAB":
                        url = "https://portal.uab.pt/";
                        break;
                    case "Omnibees":
                        url = "https://omnibees.com/";
                        break;
                }
                try
                {
                    // Start the process for the specific file using the default application
                    ProcessStartInfo processStartInfo = new(url)
                    {
                        UseShellExecute = true                      // Uses the windows shell to execute the default application for the extension
                    };
                    Process.Start(processStartInfo);
                }
                catch (Exception ex)
                {
                    _outputWindowViewModel.MainViewModel.MessageDispenser = ex.Message;
                }
            }
        }
    }
}

