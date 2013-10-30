﻿using System;
using System.Collections.Generic;
using System.Linq;
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

	public class Vessel : IResolver, IRegistrar
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
			return Enumerable.Empty<object>(); //wayback machine rocks ! blog was deleted. http://web.archive.org/web/20110204020311/http://davidhayden.com/blog/dave/archive/2011/02/01/IDependencyResolverAspNetMvc3.aspx
		}
	}
}

/*

http://geekswithblogs.net/mrsteve/archive/2012/02/19/a-fast-c-sharp-extension-method-using-expression-trees-create-instance-from-type-again.aspx
http://ayende.com/blog/3167/creating-objects-perf-implications
http://rogeralsing.com/2008/02/28/linq-expressions-creating-objects/
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