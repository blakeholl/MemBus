using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MemBus.Support;
using Microsoft.Practices.ServiceLocation;
using System.Linq;

namespace MemBus
{
    internal class ServiceLocationBasedResolver : ISubscriptionResolver
    {

        private IServiceLocator locator;
        private readonly ConcurrentDictionary<Type, Type> typeCache = new ConcurrentDictionary<Type, Type>();

        public IServices Service
        {
            set
            {
                locator = value.Get<IServiceLocator>();
                if (locator == null)
                    throw new MemBusException("ServiceLocationBasedResolver requires a service locator");
            }
        }

        public IEnumerable<ISubscription> GetSubscriptionsFor(object message)
        {
            var handlesType = constructHandlesType(message.GetType());
            return locator.GetAllInstances(handlesType).Select(svc => (ISubscription) svc);
        }

        private Type constructHandlesType(Type messageType)
        {
            return typeCache.GetOrAdd(messageType, msgT => typeof (IHandles<>).MakeGenericType(messageType));
        }

        public bool Add(ISubscription subscription)
        {
            // We just resolve here, no adding.
            return false;
        }
    }
}