using AsyncToolWindowSample.ToolWindows;
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
            _container.RegisterType<MainView>();

            return _container;
        }


    }
}