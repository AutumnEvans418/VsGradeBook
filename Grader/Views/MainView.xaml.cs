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
using AsyncToolWindowSample.Views;
using Grader;
using Grader.Views;
using Unity;
using Task = System.Threading.Tasks.Task;

namespace AsyncToolWindowSample.ToolWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainView : UserControl, INavigationService
    {
        private IUnityContainer _container;
        public MainView(IUnityContainer bootstrapper)
        {
            _container = bootstrapper;
            InitializeComponent();
          
            Add<ProjectView>();
            Add<ClassesView>();
        }
        private Dictionary<string, Func<Control>> pages = new Dictionary<string, Func<Control>>();
        void Add<T>() where T : Control
        {
            _container.RegisterSingleton<T>();
            pages.Add(typeof(T).Name, () => _container.Resolve<T>());
        }

        public Task ToPage(string page)
        {
           return ToPage(page, new NavigationParameter());
        }

        public async Task ToPage(string page, INavigationParameter parameter)
        {
            Content = pages[page]();
            if (Content is Control c)
            {
                if (c.DataContext is INavigationAware aware)
                {
                    aware.Initialize(parameter);
                }

                if (c.DataContext is INavigationAwareAsync awareAsync)
                {
                    await awareAsync.InitializeAsync(parameter);
                }
            }
        }
    }
}
