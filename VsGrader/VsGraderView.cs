using System;
using System.Runtime.InteropServices;
using AsyncToolWindowSample.Models;
using AsyncToolWindowSample.ToolWindows;
using Grader;
using Microsoft.VisualStudio.Shell;
using Unity;

namespace VsGrader
{
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("8523bd13-1011-4835-8e33-26be5fc682c2")]
    public class VsGraderView : ToolWindowPane
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VsGraderView"/> class.
        /// </summary>
        public VsGraderView() : base(null)
        {
            this.Caption = "VsGraderView";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            var boot = new Bootstrapper();
            var con = boot.Initialize();
            con.RegisterInstance(VsGraderPackage.Package);
            con.RegisterType<IVisualStudioService, VisualStudioService>();

            var mainView = con.Resolve<MainView>();
           
            this.Content = mainView;

            mainView.ToPage("HomeView");
        }
    }
}
