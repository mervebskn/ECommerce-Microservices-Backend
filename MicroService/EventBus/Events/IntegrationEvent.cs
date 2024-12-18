﻿using System;
namespace EventBus.Events
{
    public abstract class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public DateTime OccurredOn { get; }
    }
}
