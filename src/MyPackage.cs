using System;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using AsyncToolWindowSample.ToolWindows;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Task = System.Threading.Tasks.Task;

namespace AsyncToolWindowSample
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("Async Tool Window Sample", "Shows how to use an Async Tool Window in Visual Studio 15.6+", "1.0")]
    [ProvideToolWindow(typeof(SampleToolWindow), Style = VsDockStyle.Tabbed, DockedWidth = 300, Window = "DocumentWell", Orientation = ToolWindowOrientation.Left)]
    [Guid("6e3b2e95-902b-4385-a966-30c06ab3c7a6")]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    //These allow this extension to load in the background when visual studio starts up: https://michaelscodingspot.com/visual-studio-2017-extension-development-tutorial-part-2-add-menu-item/
    [ProvideAutoLoad(VSConstants.UICONTEXT.NoSolution_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideAutoLoad(VSConstants.UICONTEXT.SolutionExists_string, PackageAutoLoadFlags.BackgroundLoad)]
    [ProvideBindingPath]
    public sealed class MyPackage : AsyncPackage
    {
        public MyPackage()
        {
            
        }
        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await JoinableTaskFactory.SwitchToMainThreadAsync();
            await ShowToolWindow.InitializeAsync(this);
        }

        public override IVsAsyncToolWindowFactory GetAsyncToolWindowFactory(Guid toolWindowType)
        {
            return toolWindowType.Equals(Guid.Parse(SampleToolWindow.WindowGuidString)) ? this : null;
        }

        protected override string GetToolWindowTitle(Type toolWindowType, int id)
        {
            return toolWindowType == typeof(SampleToolWindow) ? SampleToolWindow.Title : base.GetToolWindowTitle(toolWindowType, id);
        }

      
        protected override async Task<object> InitializeToolWindowAsync(Type toolWindowType, int id, CancellationToken cancellationToken)
        {
            
            // Perform as much work as possible in this method which is being run on a background thread.
            // The object returned from this method is passed into the constructor of the SampleToolWindow 
            var dte = await GetServiceAsync(typeof(EnvDTE.DTE)) as EnvDTE80.DTE2;
            
            return new SampleToolWindowState
            {
                DTE = dte,
                AsyncPackage = this
            };
        }
    }
}
