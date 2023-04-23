using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LOGrasper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonAddAndKeyword_click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(keyword.Text) && !keywordsList.Items.Contains(keyword.Text))
            {
                keywordsList.Items.Add(keyword.Text);
                keyword.Clear();
            }
        }



    }
}
