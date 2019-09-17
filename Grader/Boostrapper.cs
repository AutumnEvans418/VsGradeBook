using Unity;

namespace Grader
{
    public class Bootstrapper
    {
        private IUnityContainer unityContainer;
        public IUnityContainer Initialize()
        {
            unityContainer = new UnityContainer();



            return unityContainer;
        }


    }
}