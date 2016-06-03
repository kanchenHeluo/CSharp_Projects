using Efiling.DataAccess.Interface;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Efiling.DataAccess
{
    public static class UnityConfig
    {
        private static IUnityContainer _currentContainer;

        private static IEfileDataAccess _db;
        private static IEfileService _svc;

        private static IUnityContainer CurrentContainer
        {
            get
            {
                if (_currentContainer == null)
                {
                    _currentContainer = new UnityContainer();
                    _currentContainer.LoadConfiguration();
                }
                return _currentContainer;
            }
        }

        private static T Resolve<T>()
        {
            return CurrentContainer.Resolve<T>();
        }


        public static IEfileDataAccess Db
        {
            get { return _db ?? (_db = Resolve<IEfileDataAccess>()); }
        }

        public static IEfileService Service
        {
            get { return _svc ?? (_svc = Resolve<IEfileService>() ); }
        }


    }
}
