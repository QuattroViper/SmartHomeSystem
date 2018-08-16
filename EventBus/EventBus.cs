using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EventBus
{
    public class EventBus
    {

        private static EventBus instance;
        private EventBus() { }
        private List<EventListenerWrapper> listeners = new List<EventListenerWrapper>();

        public static EventBus Instance { get { return instance ?? (instance = new EventBus()); } }

        public void Register(object listener)
        {
            if (!listeners.Any(l => l.Listener == listener))
                listeners.Add(new EventListenerWrapper(listener));
        }

        public void Unregister(object listener)
        {
            listeners.RemoveAll(l => l.Listener == listener);
        }

        public void PostEvent(object e)
        {
            listeners.Where(l => l.EventType == e.GetType()).ToList().ForEach(l => l.PostEvent(e));
        }

        private class EventListenerWrapper
        {
            public object Listener { get; private set; }
            public Type EventType { get; private set; }

            private MethodBase method;

            public EventListenerWrapper(object listener)
            {
                Listener = listener;

                Type type = listener.GetType();

                method = type.GetMethod("OnEvent");
                if (method == null)
                    throw new ArgumentException("Class " + type.Name + " does not containt method OnEvent");

                ParameterInfo[] parameters = method.GetParameters();
                if (parameters.Length != 1)
                    throw new ArgumentException("Method OnEvent of class " + type.Name + " have invalid number of parameters (should be one)");

                EventType = parameters[0].ParameterType;
            }

            public void PostEvent(object e)
            {
                method.Invoke(Listener, new[] { e });
            }
        }

        //EventBus.Instance.Register(this);
        //EventBus.Instance.Unregister(this);

        //EventBus.Instance.PostEvent(new OnProgressChangedEvent(progress));


    }

    public class CustomEvent
    {

        public enum EventType
        {
            warning = 0, critical = 1, accept = 2
        }

        private string eventString;
        private string eventMessage;
        private EventType type;



        public CustomEvent()
        {

        }

        public CustomEvent(string eventString)
        {
            this.eventString = eventString;
        }

        public CustomEvent(string eventString, string eventMessage)
        {
            this.eventString = eventString;
            this.eventMessage = eventMessage;
        }

        public CustomEvent(string eventString, string eventMessage, EventType type)
        {
            this.eventString = eventString;
            this.eventMessage = eventMessage;
            this.type = type;
        }

        public EventType Type
        {
            get { return type; }
            set { type = value; }
        }

        public string EventMessage
        {
            get { return eventMessage; }
            set { eventMessage = value; }
        }

        public string EventString
        {
            get { return eventString; }
            set { eventString = value; }
        }


    }

}
