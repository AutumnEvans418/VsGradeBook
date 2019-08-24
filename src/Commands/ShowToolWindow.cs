using System;
using System.ComponentModel.Design;
using System.Threading.Tasks;
using AsyncToolWindowSample.ToolWindows;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TextManager.Interop;
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
            {
                var cmdId = new CommandID(Guid.Parse(guidMyPackageCmdSet), 0x0102);
                var cmd = new MenuCommand((s, e) => ExecuteAddDocumentation(package), cmdId);
                commandService.AddCommand(cmd);
            }

           
        }

        static async void ExecuteAddDocumentation(AsyncPackage package)
        {
            var selection = await GetSelection(package);
            string document = await GetActiveFilePath(package);
            ShowAddDocumentationWindow(document, selection);
        }

        private static void ShowAddDocumentationWindow(string document, TextViewSelection selection)
        {
            throw new NotImplementedException();
        }

        private static async Task<TextViewSelection> GetSelection(AsyncPackage serviceProvider)
        {
            var service = await serviceProvider.GetServiceAsync(typeof(SVsTextManager));
            var textManager = service as IVsTextManager2;
            IVsTextView view;
            int result = textManager.GetActiveView2(1, null, (uint)_VIEWFRAMETYPE.vftCodeWindow, out view);

            view.GetSelection(out int startLine, out int startColumn, out int endLine, out int endColumn);//end could be before beginning
            var start = new TextViewPosition(startLine, startColumn);
            var end = new TextViewPosition(endLine, endColumn);

            view.GetSelectedText(out string selectedText);

            TextViewSelection selection = new TextViewSelection(start, end, selectedText);
            return selection;
        }
        static private async Task<string> GetActiveFilePath(AsyncPackage serviceProvider)
        {
            EnvDTE80.DTE2 applicationObject = await serviceProvider.GetServiceAsync(typeof(DTE)) as EnvDTE80.DTE2;
            return applicationObject.ActiveDocument.FullName;
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
