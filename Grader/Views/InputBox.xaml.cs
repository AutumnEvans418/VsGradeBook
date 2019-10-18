using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Grader.Views
{
    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        private readonly Func<string> _importFunc;

        public InputBox(string msg, Func<string> importFunc)
        {
            _importFunc = importFunc;
            InitializeComponent();
            this.Label.Content = msg;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ImportClick(object sender, RoutedEventArgs e)
        {
            if (_importFunc != null)
                this.TextBox.Text = _importFunc();
        }
    }
}
