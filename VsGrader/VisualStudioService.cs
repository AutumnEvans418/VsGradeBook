using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;

namespace VsGrader
{
    public class VisualStudioService : IVisualStudioService
    {
        private readonly AsyncPackage _package;

        public VisualStudioService(AsyncPackage package)
        {
            _package = package;
        }
        public async Task<IEnumerable<FileContent>> GetCSharpFilesAsync()
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
                .Select(p => new FileContent()
                {
                    Content = File.ReadAllText(p.Properties.Item("FullPath").Value.ToString()),
                    FileName = p.Properties.Item("FullPath").Value.ToString()
                }).ToList();
            return code;
        }

        public async void OpenOrCreateCSharpFile(string fileName, string content)
        {
            var dte = await _package.GetServiceAsync(typeof(DTE)) as DTE2;

            var tempPath = Path.GetTempPath();

            var file = Path.Combine(tempPath, fileName);

            File.WriteAllText(file, content);
            var kind = dte.ActiveWindow.Kind;
            var kind2 = dte.ActiveDocument.Kind;
            var kinds = new List<string>();
            var en = dte.Windows.GetEnumerator();
            while (en.MoveNext())
            {
                var win = en.Current as Window;
                kinds.Add(win.Kind);
            }
            
            dte.OpenFile("Document", file);
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
}