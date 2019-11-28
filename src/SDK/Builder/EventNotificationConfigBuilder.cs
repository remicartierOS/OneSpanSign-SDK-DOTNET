using System;
using System.Collections.Generic;

namespace Silanis.ESL.SDK
{
    public class EventNotificationConfigBuilder
    {
        private string url;
        private string key;
        private List<NotificationEvent> events;

        private EventNotificationConfigBuilder(string url)
        {
            this.url = url;
            this.events = new List<NotificationEvent>();
        }

        private EventNotificationConfigBuilder(string url, string key)
        {
            this.url = url;
            this.key = key;
            this.events = new List<NotificationEvent>();
        }

        public static EventNotificationConfigBuilder NewEventNotificationConfig( string url ) {
            return new EventNotificationConfigBuilder(url);
        }

        public EventNotificationConfigBuilder WithKey( string key ) {
            this.key = key;
            return this;
        }

        public EventNotificationConfigBuilder ForEvent( NotificationEvent notificationEvent ) {
            events.Add( notificationEvent );
            return this;
        }

        public EventNotificationConfigBuilder SetEvents( List<NotificationEvent> events ) {
            this.events = events;
            return this;
        }

        public EventNotificationConfig build() {
            EventNotificationConfig result = new EventNotificationConfig(url);
            result.Key = key;
            foreach (NotificationEvent notificationEvent in events)
            {
                result.NotificationEvents.Add(notificationEvent);
            }

            return result;
        }
    }
}

