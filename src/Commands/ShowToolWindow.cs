using System;
using System.ComponentModel.Design;
using AsyncToolWindowSample.ToolWindows;
using Microsoft.VisualStudio.Shell;
using Task = System.Threading.Tasks.Task;

namespace AsyncToolWindowSample
{
    internal sealed class ShowToolWindow
    {
        private const string guidMyPackageCmdSet = "9cc1062b-4c82-46d2-adcb-f5c17d55fb85";
        public static async Task InitializeAsync(AsyncPackage package)
        {
            var commandService = (IMenuCommandService)await package.GetServiceAsync(typeof(IMenuCommandService));
            {
                var cmdId = new CommandID(Guid.Parse(guidMyPackageCmdSet), 0x0100);
                var cmd = new MenuCommand((s, e) => ExecuteShowToolCommand(package), cmdId);
                commandService.AddCommand(cmd);
            }

            {
                //create the checkbox menu item the command id matches the id in the symbolids
                var cmdId = new CommandID(Guid.Parse(guidMyPackageCmdSet), 0x0101);
                var cmd = new MenuCommand((s, e) => ExecuteEnableCodyDocs(s,package), cmdId);
                cmd.Checked = GeneralSettings.Default.EnableToolWindow;
                commandService.AddCommand(cmd);
            }

           
        }

        private static void ExecuteEnableCodyDocs(object sender ,AsyncPackage package)
        {
            GeneralSettings.Default.EnableToolWindow = !GeneralSettings.Default.EnableToolWindow;
            GeneralSettings.Default.Save();
            if(sender is MenuCommand cmd)
            {
                cmd.Checked = GeneralSettings.Default.EnableToolWindow;
            }
        }

        private static void ExecuteShowToolCommand(AsyncPackage package)
        {
            package.JoinableTaskFactory.RunAsync(async () =>
            {
                ToolWindowPane window = await package.ShowToolWindowAsync(
                    typeof(SampleToolWindow),
                    0,
                    create: true,
                    cancellationToken: package.DisposalToken);
            });
        }
    }
}
