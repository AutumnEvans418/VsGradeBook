using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using AsyncToolWindowSample.ToolWindows;
using EnvDTE;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using EnvDTE80;
using Grader;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TextManager.Interop;
using Console = System.Console;
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
                var cmd = new MenuCommand((s, e) => ExecuteEnableCodyDocs(s, package), cmdId);
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
            var codes = await GetCode(package);


            var selection = await GetSelection(package);
            string document = await GetActiveFilePath(package);
            ShowAddDocumentationWindow(document, selection);
        }

        private static async Task<IEnumerable<string>> GetCode(AsyncPackage package)
        {
            var dte = await package.GetServiceAsync(typeof(DTE)) as DTE2;
            //var project = await SelectedProject(package);
            var enumer = dte.Solution.Projects.GetEnumerator();
            enumer.MoveNext();
            var project = enumer.Current;
            var projectItems = new List<ProjectItem>();
            var items = ((Project)project).ProjectItems.GetEnumerator();
            while (items.MoveNext())
            {
                var item = (ProjectItem)items.Current;
                //Recursion to get all ProjectItems
                projectItems.Add(GetFiles(item, projectItems));
            }

            var code = projectItems
                .Where(p => Path.GetExtension(p.Name)?.ToLower() == ".cs")
                .Select(p => File.ReadAllText(p.Properties.Item("FullPath").Value.ToString())).ToList();
            return code;
        }

        static ProjectItem GetFiles(ProjectItem item, List<ProjectItem> projectItems)
        {
            //base case
            if (item.ProjectItems == null)
                return item;

            var items = item.ProjectItems.GetEnumerator();
            while (items.MoveNext())
            {
                var currentItem = (ProjectItem)items.Current;
                projectItems.Add(GetFiles(currentItem, projectItems));
            }

            return item;
        }
        //public static async Task<Project> SelectedProject(AsyncPackage package)
        //{
        //    await package.JoinableTaskFactory.SwitchToMainThreadAsync();
        //    IntPtr hierarchyPointer, selectionContainerPointer;
        //    Object selectedObject = null;
        //    IVsMultiItemSelect multiItemSelect;
        //    uint projectItemId;

        //    IVsMonitorSelection monitorSelection =
        //        (IVsMonitorSelection)Package.GetGlobalService(
        //            typeof(SVsShellMonitorSelection));

        //    monitorSelection.GetCurrentSelection(out hierarchyPointer,
        //        out projectItemId,
        //        out multiItemSelect,
        //        out selectionContainerPointer);

        //    IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
        //        hierarchyPointer,
        //        typeof(IVsHierarchy)) as IVsHierarchy;

        //    if (selectedHierarchy != null)
        //    {
        //        ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
        //            projectItemId,
        //            (int)__VSHPROPID.VSHPROPID_ExtObject,
        //            out selectedObject));
        //    }

        //    Project selectedProject = selectedObject as Project;
        //    return selectedProject;
        //}
        private static void ShowAddDocumentationWindow(string document, TextViewSelection selection)
        {
            var documentationControl = new AddDocumentationView(document, selection);
            documentationControl.ShowDialog();
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
        private static async Task<string> GetActiveFilePath(AsyncPackage serviceProvider)
        {
            EnvDTE80.DTE2 applicationObject = await serviceProvider.GetServiceAsync(typeof(DTE)) as EnvDTE80.DTE2;
            return applicationObject.ActiveDocument.FullName;
        }
        private static void ExecuteEnableCodyDocs(object sender, AsyncPackage package)
        {
            GeneralSettings.Default.EnableToolWindow = !GeneralSettings.Default.EnableToolWindow;
            GeneralSettings.Default.Save();
            if (sender is MenuCommand cmd)
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
