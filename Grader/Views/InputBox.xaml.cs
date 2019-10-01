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
using AsyncToolWindowSample.ToolWindows;

namespace Grader.Views
{
   
    public class InputBoxService : IInputBoxService
    {
        public async Task<string> Show(string title = null, string msg = null)
        {
            var win = new InputBox(msg);
            win.Title = title;
            win.ShowDialog();
            return win.TextBox.Text;
        }
    }

    /// <summary>
    /// Interaction logic for InputBox.xaml
    /// </summary>
    public partial class InputBox : Window
    {
        public InputBox(string msg)
        {
            InitializeComponent();
            this.Label.Content = msg;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
