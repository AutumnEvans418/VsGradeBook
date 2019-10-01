using System;
using System.Net.Http;
using AsyncToolWindowSample.ToolWindows;
using Grader.Views;
using Unity;

namespace Grader
{
    public class Bootstrapper
    {
        private IUnityContainer _container;
        public IUnityContainer Initialize()
        {
            _container = new UnityContainer();

            _container.RegisterType<ProjectViewModel>();
            _container.RegisterType<LoginViewModel>();
            _container.RegisterType<ClassesViewModel>();
            _container.RegisterType<IConsoleAppGrader, ConsoleAppGrader>();
            _container.RegisterType<ICSharpGenerator, CSharpGenerator>();

            _container.RegisterInstance(new HttpClient(){BaseAddress = new Uri("https://localhost:44301/") });
            _container.RegisterType<IGradeBookRepository, GradeBookRepositoryHttpClient>();
            _container.RegisterType<MainView>();
            _container.RegisterType<IInputBoxService, InputBoxService>();
            return _container;
        }


    }
}