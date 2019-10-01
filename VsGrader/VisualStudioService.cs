using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AsyncToolWindowSample.Models;
using EnvDTE;
using EnvDTE80;
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
}