using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Controls;
using AsyncToolWindowSample.Models;
using Grader;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Unity;
using Task = System.Threading.Tasks.Task;

namespace AsyncToolWindowSample.ToolWindows
{
    [Guid(WindowGuidString)]
    public class SampleToolWindow : ToolWindowPane
    {
        public const string WindowGuidString = "e4e2ba26-a455-4c53-adb3-8225fb696f8b"; // Replace with new GUID in your own code
        public const string Title = "Sample Tool Window";

        // "state" parameter is the object returned from MyPackage.InitializeToolWindowAsync
        public SampleToolWindow(SampleToolWindowState state) : base()
        {
            Setup(state);
        }

        private async void Setup(SampleToolWindowState state)
        {
            Caption = Title;
            BitmapImageMoniker = KnownMonikers.ImageIcon;
            var boot = new Bootstrapper();
            var container = boot.Initialize();
            container.RegisterType<IVisualStudioService, VisualStudioService>();
            container.RegisterInstance(state.AsyncPackage);
            var view = container.Resolve<MainView>();
            await view.ToPage("LoginView");
            Content = view;
        }
    }
}
