using System.Collections.Generic;

namespace simpleCDE 
{
    public enum EventState 
    {
        Unhandled, Handling, Handled
    }

    public class Event 
    {
        public readonly string Name;
        public readonly List<string> Preconditions;
        public readonly string Handler;
        public EventState State;

        public Event(string name, List<string> preconditions, string handler) 
        {
            Name = name;
            Preconditions = preconditions;
            Handler = handler;
            State = EventState.Unhandled;
        }
    }
}
