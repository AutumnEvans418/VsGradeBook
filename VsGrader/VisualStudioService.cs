using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AsyncToolWindowSample.Models;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.PlatformUI;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Process = System.Diagnostics.Process;

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
            try
            {
                var dte = await _package.GetServiceAsync(typeof(DTE)) as DTE2;
                //var project = await SelectedProject(package);
                var enumer = dte.Solution.Projects.GetEnumerator();
                enumer.MoveNext();
                var project = enumer.Current;
                var projectItems = new List<ProjectItem>();
                await _package.JoinableTaskFactory.SwitchToMainThreadAsync();
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
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.ToString());
                throw;
            }
        
        }

        public async void OpenOrCreateCSharpFile(string fileName, string content)
        {
            try
            {
                var dte = await _package.GetServiceAsync(typeof(DTE)) as DTE2;

                await _package.JoinableTaskFactory.SwitchToMainThreadAsync();

                var tempPath = Path.GetTempPath();

                var file = Path.Combine(tempPath, fileName);

                File.WriteAllText(file, content);


                dte.Documents.Open(file, ReadOnly: true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                MessageBox.Show(e.ToString());
            }
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