using DryIoc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CandySugar.Core.CandyUtily
{
    public class CandyContainer
    {
        private static CandyContainer _Instance;
        private static IContainer Container;
        private static readonly object locker = new object();

        public static CandyContainer Instance
        {
            get
            {
                lock (locker)
                {
                    if (_Instance == null)
                    {
                        _Instance = new CandyContainer();
                        Container = new Container();
                    }
                }
                return _Instance;
            }
        }

        public void Regiest<TService, TImplementation>() where TImplementation : TService
        {
            Container.Register<TService, TImplementation>(Reuse.Singleton);
        }

        public void Regiest(Type serviceType)
        {
            Container.Register(serviceType,Reuse.Singleton);
        }

        public void Regiest(Type serviceType, Type implementationType)
        {
            Container.Register(serviceType, implementationType, Reuse.Singleton, serviceKey: implementationType.Name);
        }

        public object Resolve(Type serviceType, Type implementationType)
        {
            return Container.Resolve(serviceType, serviceKey: implementationType.Name);
        }

        public TService Resolves<TService>()
        {
            return (TService)Container.Resolve(typeof(TService));
        }

        public TService Resolve<TService>()
        {
            return Container.Resolve<TService>();
        }
    }
}
