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
using Unity;

namespace Grader.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel vm;
        public MainWindow()
        {
            
            InitializeComponent();
            Setup();
        }

        private async void Setup()
        {
            
            var boot = new Bootstrapper();
            var container = boot.Initialize();

            vm = new MainWindowViewModel();
            container.RegisterInstance<IVisualStudioService>(vm);
            DataContext = vm;


            var view = container.Resolve<MainView>();
            await view.ToPage("HomeView");



            Region.Content = view;
        }

        
    }
}
