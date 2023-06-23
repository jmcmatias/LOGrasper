﻿using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;


namespace LOGrasper.Views
{
    public partial class LOGrasperSearchView : UserControl
    {
        public LOGrasperSearchView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Method to keep the entries for the TextInput of the  numerical and > 1
        /// </summary>
        private void IntegerUpDown_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!int.TryParse(e.Text, out _) || int.Parse(e.Text) < 1)
            {
                e.Handled = true;
            }
        }


    }
}
