namespace simpleCDE 
{
    public interface IAgent 
    {
        event CDE.EventHandler EventCompleted;
        void OnEventTriggered(string eventName, string actionName);
        void Halt();
    }
}
