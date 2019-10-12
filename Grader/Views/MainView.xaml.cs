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

            _container.RegisterInstance<INavigationService>(this);
            Add<ProjectView, ProjectViewModel>();
            Add<SubmissionsView, SubmissionsViewModel>();
            Add<HomeView, HomeViewModel>();
            Add<ProjectPublishedView, ProjectPublishedViewModel>();
            Add<SubmissionProjectView, ProjectViewModel>();
        }
        private Dictionary<string, Func<Control>> pages = new Dictionary<string, Func<Control>>();
        void Add<T, Tv>() where T : Control
        {
            _container.RegisterType<Tv>();
            _container.RegisterSingleton<T>();
            pages.Add(typeof(T).Name, () =>
            {
                var view = _container.Resolve<T>();
                view.DataContext = _container.Resolve<Tv>();
                return view;
            });
        }

        public Task ToPage(string page)
        {
           return ToPage(page, new NavigationParameter());
        }

        public async Task ToPage(string page, INavigationParameter parameter)
        {
            Content = pages[page]();
            await Initialize(Content,parameter);
        }

        private async Task Initialize(object ctrl, INavigationParameter parameter)
        {
            if (ctrl is Control c)
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

        private Window modalWindow;
        public async Task ToModalPage(string page, INavigationParameter parameter)
        {
            var view = pages[page]();

            modalWindow = new Window();
            modalWindow.Content = view;

            await Initialize(view, parameter);

            modalWindow.ShowDialog();
            
        }

        public async Task Back()
        {
            modalWindow.Close();
        }
    }
}
