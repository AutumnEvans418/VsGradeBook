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
using AsyncToolWindowSample.Models;
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

    public class VisualStudioService : IVisualStudioService
    {
        private readonly AsyncPackage _package;

        public VisualStudioService(AsyncPackage package)
        {
            _package = package;
        }
        public async Task<IEnumerable<string>> GetCSharpFilesAsync()
        {
            var dte = await _package.GetServiceAsync(typeof(DTE)) as DTE2;
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
    }

    internal sealed class ShowToolWindow
    {
        private const string guidMyPackageCmdSet = "9cc1062b-4c82-46d2-adcb-f5c17d55fb85";
        private static VisualStudioService visualStudioService;
        public static async Task InitializeAsync(AsyncPackage package)
        {
            visualStudioService = new VisualStudioService(package);
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
            var codes = await visualStudioService.GetCSharpFilesAsync();


            var selection = await GetSelection(package);
            string document = await GetActiveFilePath(package);
            ShowAddDocumentationWindow(document, selection);
        }

      
        private static void ShowAddDocumentationWindow(string document, TextViewSelection selection)
        {
            AddDocumentationView view =null;
             view = new AddDocumentationView(new AddDocumentationViewModel(()=> view.Close(), document, selection, new CSharpGenerator()));
            view.ShowDialog();
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
