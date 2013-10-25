using System;
using System.Collections.Generic;

namespace Blog.Web.Infrastructure
{
	public interface ISubscribeHandlers
	{
		void Subscribe<TMessage>(Action<TMessage> handler);
		void Subscribe<TMessage, TResult>(Func<TMessage, TResult> handler);
	}

	public interface IMediator
	{
		void Send<TMessage>(TMessage message);
		TResult Send<TMessage, TResult>(TMessage message);
	}

	public class Mediator : ISubscribeHandlers, IMediator
	{
		private readonly IDictionary<int, Delegate> _subscriptions;

		public void Subscribe<TMessage>(Action<TMessage> handler)
		{
			Subscribe<TMessage, Unit>(message =>
			{
				handler(message);
				return new Unit();
			});
		}

		public void Subscribe<TMessage, TResult>(Func<TMessage, TResult> handler)
		{
			_subscriptions.Add(typeof(TMessage).GetHashCode(), handler);
		}

		public void Send<TMessage>(TMessage message)
		{
			Send<TMessage, Unit>(message);
		}

		public TResult Send<TMessage, TResult>(TMessage message)
		{
			Delegate value;
			if (!_subscriptions.TryGetValue(typeof(TMessage).GetHashCode(), out value))
				throw new ApplicationException(string.Format("No Handler subscribed for message {0}.", typeof(TMessage).Name));

			var handler = value as Func<TMessage, TResult>;
			if (handler == null) throw new ApplicationException(string.Format("The handler subscribed for {0} does not have result type of {1}.", typeof(TMessage).Name, typeof(TResult).Name));

			return handler(message);
		}

		public Mediator()
		{
			_subscriptions = new SortedDictionary<int, Delegate>();
		}

		class Unit { }
	}
}