using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Blog.Web.Infrastructure
{
    public interface IResolver
    {
        T Resolve<T>();
        object Resolve(Type service);
    }

    public interface IRegistrar
    {
        void Register<T>(T service);
        void Register<T>(Func<IResolver, T> creator);
    }

    public interface IContainer : IResolver, IRegistrar { }

    public interface IModule
    {
        void Execute(IContainer container);
    }

    public class Vessel : IResolver, IRegistrar, IContainer
    {
        private delegate object Resolver(IResolver resolver);
        private readonly IDictionary<int, Resolver> _registrations;

        public void Register<T>(T service)
        {
            Register(typeof(T), c => service);
        }

        public void Register<T>(Func<IResolver, T> creator)
        {
            Register(typeof(T), c => creator(c));
        }

        void Register(Type type, Resolver resolver)
        {
            _registrations.Add(type.GetHashCode(), resolver);
        }

        public void RegisterModules()
        {
            var modules =
                from assembly in AppDomain.CurrentDomain.GetAssemblies()
                from t in assembly.GetLoadableTypes()
                where t.IsClass
                where typeof (IModule).IsAssignableFrom(t)
                select Activator.CreateInstance(t) as IModule; //TODO: https://stackoverflow.com/a/1805609/214073

            foreach (var module in modules)
            {
                module.Execute(this);
            }
        }

        public T Resolve<T>()
        {
            return (T)Resolve(typeof (T));
        }

        public object Resolve(Type service)
        {
            Resolver resolver;
            var ok = _registrations.TryGetValue(service.GetHashCode(), out resolver);
            return ok ? resolver(this) : null;
        }

        public Vessel()
        {
            _registrations = new Dictionary<int, Resolver>();
        }
    }

    public class VesselDependencyResolver : IDependencyResolver
    {
        private readonly IResolver _resolver;

        public VesselDependencyResolver(IResolver resolver)
        {
            _resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return _resolver.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return Enumerable.Empty<object>(); //wayback machine rocks ! blog was deleted. https://web.archive.org/web/20110204020311/https://davidhayden.com/blog/dave/archive/2011/02/01/IDependencyResolverAspNetMvc3.aspx
        }
    }

    public static class AssemblyExtension
    {
        public static IEnumerable<Type> GetLoadableTypes(this Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                return e.Types.Where(t => t != null);
            }
        }
    }
}

/*

https://geekswithblogs.net/mrsteve/archive/2012/02/19/a-fast-c-sharp-extension-method-using-expression-trees-create-instance-from-type-again.aspx
https://ayende.com/blog/3167/creating-objects-perf-implications
https://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/
var constructor = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public,
                null,
                CallingConventions.HasThis,
                new[]{typeof(IMediator)},
                new ParameterModifier[0]);

            var arg = Expression.Constant(new Mediator());
            var constructorCallExpression = Expression.New(constructor, arg);
            var lambda = Expression.Lambda<Func<IController>>(constructorCallExpression).Compile();


*/