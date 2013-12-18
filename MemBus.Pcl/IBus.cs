using System;
using MemBus.Messages;
using MemBus.Subscribing;

namespace MemBus
{
    /// <summary>
    /// Descibes just the publishing features of MemBus. The <see cref="IBus"/> itf inherits from this one.
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publish a message
        /// </summary>
        void Publish(object message);

        /// <summary>
        /// Publishes an observable that provides objects of type M. 
        /// Once the observer that MemBus attaches to this observable receives the call to "OnCompleted", the message <see cref="MessageStreamCompleted{M}"/> will be sent.
        /// An exception will be mapped to a message of type <see cref="ExceptionOccurred"/>.
        /// Disposing the returned IDisposable will remove the generated observer from the observable.
        /// </summary>
        IDisposable Publish<M>(IObservable<M> observable);
    }

    /// <summary>
    /// Descibes just the subscribing features of MemBus. The <see cref="IBus"/> itf inherits from this one.
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Subscribe anything matching the signature
        /// </summary>
        IDisposable Subscribe<M>(Action<M> subscription);
        
        /// <summary>
        /// Subscribe any methods defined on the subscriber. You need to have added and configured the <see cref="FlexibleSubscribeAdapter"/>
        /// for this to work
        /// </summary>
        IDisposable Subscribe(object subscriber);

        /// <summary>
        /// Obtain an <see cref="IObservable{T}"/> instance to observe any incoming objects of te desired types
        /// </summary>
        IObservable<M> Observe<M>();
    }

    /// <summary>
    /// The IBus brings together the <see cref="IPublisher"/> and <see cref="ISubscriber"/> capabilities of MemBus.
    /// You obtain an instance via the <see cref="BusSetup"/> class.
    /// </summary>
    public interface IBus : IDisposable, IPublisher, ISubscriber
    {
    }
}