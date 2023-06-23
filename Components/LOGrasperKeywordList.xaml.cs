using LOGrasper.Models;
using LOGrasper.Views;
using System.Windows;
using System.Windows.Controls;


namespace LOGrasper.Components
{
    /// <summary>
    /// Interação lógica para LOGrasperKeywordList.xaml
    /// </summary>
    public partial class LOGrasperKeywordList : UserControl 
    {
        private bool firstFocus = true;
        public LOGrasperKeywordList()
        {
            InitializeComponent();
        }

        public void KeywordFormGotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            
            if (firstFocus)
            {
                textBox.Text = string.Empty;
                firstFocus = false;
            }
            
        }

        private void KeywordColumn_Loaded(object sender, RoutedEventArgs e)
        {
            if(sender is DataGridTextColumn column)
            {
                if(FindName("Notice") is TextBox noticeTextBox)
                {
                    column.MinWidth = noticeTextBox.ActualWidth;
                }
            }
        }
    }
}
