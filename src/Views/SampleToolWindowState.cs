using Microsoft.VisualStudio.Shell;

namespace AsyncToolWindowSample.ToolWindows
{
    public class SampleToolWindowState
    {
        public EnvDTE80.DTE2 DTE { get; set; }
        public AsyncPackage AsyncPackage { get; set; }
    }
}
