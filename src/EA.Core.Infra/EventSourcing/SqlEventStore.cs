﻿using EA.NetDevPack.Events;
using EA.Core.Infra.Repository.EventSourcing; 
using EA.NetDevPack.Messaging;
using Newtonsoft.Json;
using EA.NetDevPack.Context;

namespace EA.Core.Infra.EventSourcing
{
    public class SqlEventStore : IEventStore
    {
        private readonly IEventStoreRepository _eventStoreRepository;
        private readonly IContextUser _user;

        public SqlEventStore(IEventStoreRepository eventStoreRepository, IContextUser user)
        {
            _eventStoreRepository = eventStoreRepository;
            _user = user;
        }

        public void Save<T>(T theEvent) where T : Event
        {
            // Using Newtonsoft.Json because System.Text.Json
            // is a sad joke to be considered "Done"

            // The System.Text don't know how serialize a
            // object with inherited properties, I said is sad...
            // Yes! I tried: options = new JsonSerializerOptions { WriteIndented = true };

            var serializedData = JsonConvert.SerializeObject(theEvent);

            var storedEvent = new StoredEvent(
               theEvent,
               serializedData,
               _user.GetUserId().ToString() ?? _user.GetUserEmail());

            _eventStoreRepository.Store(storedEvent);
        }
    }
}