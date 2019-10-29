using System;
using System.Net.Http;
using AsyncToolWindowSample.ToolWindows;
using Grader.Views;
using Unity;

namespace Grader
{
    public sealed class Bootstrapper : IDisposable
    {
        private IUnityContainer _container;
        public IUnityContainer Initialize()
        {
            _container = new UnityContainer();

            _container.RegisterType<ProjectViewModel>();
            _container.RegisterType<IConsoleAppGrader, ConsoleAppGrader>();
            _container.RegisterType<ICSharpGenerator, CSharpGenerator>();

#pragma warning disable CA2000 // Dispose objects before losing scope
            _container.RegisterInstance(new HttpClient(){BaseAddress = new Uri("https://graderweb20191011040446.azurewebsites.net") });
#pragma warning restore CA2000 // Dispose objects before losing scope
            _container.RegisterType<IGradeBookRepository, GradeBookRepositoryHttpClient>();
            _container.RegisterType<MainView>();
            _container.RegisterType<IMessageService, InputBoxService>();
            return _container;
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}