using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Social_Network.BLL.Services;
using Social_Network.BLL.Interfaces;
using System.Web.Mvc;

namespace Social_Network.WEB.Util
{
    public class NinjectDependencyResolver: IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
            kernel.Bind<IUserPhotoService>().To<UserPhotoService>();
            kernel.Bind<IPostsService>().To<PostsService>();
            kernel.Bind<IUserService>().To<UserService>();
        }
    }
}