namespace Progetto {
	public class AgentBridge {
		public event CDE.EventHandler EventTriggered;
		public event CDE.EventHandler EventCompleted;

		public void OnEventTriggered(string eventName, string actionName) {
			if (EventTriggered != null)
				EventTriggered(eventName, actionName);
		}

		public void OnEventCompleted(string eventName, string actionName) {
			if (EventCompleted != null)
				EventCompleted(eventName, actionName);
		}
	}
}