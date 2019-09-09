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
using AsyncToolWindowSample.Views;
using Grader;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace AsyncToolWindowSample.ToolWindows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UserControl, IToolWindow
    {
      
        public MainWindow(SampleToolWindowState state)
        {
            InitializeComponent();
            var db = new CreateDatabase();
            db.Initialize();
            pages.Add("Sample", () => new SampleToolWindowControl(state, this));
            pages.Add("Login", () => new LoginView(new LoginViewModel(this, new GradeBookRepository(db.GetGradeBookDbContext))));
            pages.Add("ProjectView", () => new ProjectView(new ProjectViewModel(new VisualStudioService(state.AsyncPackage), new ConsoleAppGrader())));
            ToPage("Login");
        }
        private Dictionary<string, Func<Control>> pages = new Dictionary<string, Func<Control>>();
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
