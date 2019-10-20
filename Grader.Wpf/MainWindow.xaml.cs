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
using AsyncToolWindowSample.Models;
using AsyncToolWindowSample.ToolWindows;
using Grader.Views;
using Unity;

namespace Grader.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IColorService
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            
            InitializeComponent();
            Setup();
        }

        private MainView _mainView;
        private async void Setup()
        {
            
            var boot = new Bootstrapper();
            var container = boot.Initialize();

            vm = new MainWindowViewModel();
            container.RegisterInstance<IVisualStudioService>(vm);
            container.RegisterInstance<IColorService>(this);
            DataContext = vm;


            _mainView = container.Resolve<MainView>();
            await _mainView.ToPage("HomeView");
            SetTheme(this);


            Region.Content = _mainView;
        }

        private bool isDark;

        public void SetTheme(Control control)
        {
            if (isDark)
            {
                control.Resources["Background"] = new SolidColorBrush(Colors.White);
                control.Resources["TextColor"] = new SolidColorBrush(Colors.Black);
            }
            else
            {
                control.Resources["Background"] = new SolidColorBrush(Color.FromRgb(64,64,64));
                control.Resources["TextColor"] = new SolidColorBrush(Colors.White);
            }
        }

        private void ThemeClick(object sender, RoutedEventArgs e)
        {
            isDark = !isDark;
            SetTheme(this);
        }
    }
}
