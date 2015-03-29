using System;
using System.Collections.Generic;
using MvcDemo.Services;
using MvcDemo.ViewModels;
using Ninject;

namespace MvcDemo.Infrastructure.DependencyResolvers {
    public class NinjectDependencyResolver : System.Web.Mvc.IDependencyResolver,
            System.Web.Http.Dependencies.IDependencyResolver {
        readonly IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel) {
            _kernel = kernel;

            AddBindings();
        }

        public object GetService(Type serviceType) {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType) {
            return _kernel.GetAll(serviceType);
        }

        void AddBindings() {
            _kernel.Bind<IContentManager>().To<ContentManager>();
            _kernel.Bind<IViewModelBuilder<ViewUploadViewModel>>().To<ViewUploadViewModelBuilder>();
        }

        public System.Web.Http.Dependencies.IDependencyScope BeginScope() {
            return this;
        }

        public void Dispose() {
            // When BeginScope returns 'this', the Dispose method must be a no-op.
        }
    }
}

