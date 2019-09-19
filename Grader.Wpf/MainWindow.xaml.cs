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
            /* NEED TO FIX
             *  C:\Users\cevans.PEERDOM\.nuget\packages\nswag.msbuild\13.0.6\tools\Win\nswag.exe   aspnetcore2openapi /assembly:C:\Source\VSSDK-Extensibility-Samples-master\VSSDK-Extensibility-Samples-master\AsyncToolWindow\Grader.Web/bin/Debug/netcoreapp2.1/Grader.Web.dll /output:swagger.json
  NSwag command line tool for .NET 4.6.1+ WinX64, toolchain v13.0.6.0 (NJsonSchema v10.0.23.0 (Newtonsoft.Json v11.0.0.0))
  Visit http://NSwag.org for more information.
  NSwag bin directory: C:\Users\cevans.PEERDOM\.nuget\packages\nswag.msbuild\13.0.6\tools\Win
  System.IO.FileNotFoundException: Could not load file or assembly 'Microsoft.EntityFrameworkCore.Relational, Version=2.1.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60' or one of its dependencies. The system cannot find the file specified.
  File name: 'Microsoft.EntityFrameworkCore.Relational, Version=2.1.1.0, Culture=neutral, PublicKeyToken=adb9793829ddae60'

             */
            var boot = new Bootstrapper();
            var container = boot.Initialize();

            vm = new MainWindowViewModel();
            container.RegisterInstance<IVisualStudioService>(vm);
            DataContext = vm;


            var view = container.Resolve<MainView>();
            await view.ToPage("LoginView");



            Region.Content = view;
        }

        
    }
}
