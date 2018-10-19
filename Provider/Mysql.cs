using System;
using System.Threading;
namespace Progetto {
	public class Mysql : IAgent {
		public event CDE.EventHandler EventCompleted;
		private Thread handlersThread;
		private Random random;

		public Mysql() {
			random = new Random();
		}

		public void install(string eventName) {
			try {
				Thread.Sleep(random.Next(500, 1000));

				if (EventCompleted != null)
					EventCompleted(eventName, "install");

				Console.WriteLine(string.Format("Mysql: event {0} completed.", eventName));
			} catch (ThreadAbortException) {
				Console.WriteLine("Mysql: halted.");
			}
		}

		public void start(string eventName) {
			try {
				Thread.Sleep(random.Next(5000, 10000));

				if (EventCompleted != null)
					EventCompleted(eventName, "start");

				Console.WriteLine(string.Format("Mysql: event {0} completed.", eventName));
			} catch (ThreadAbortException) {
				Console.WriteLine("Mysql: halted.");
			}
		}

		public void OnEventTriggered(string eventName, string actionName) {
			switch (actionName) {
				case "install": handlersThread = new Thread(() => install(eventName));
					break;
				case "start": handlersThread = new Thread(() => start(eventName));
					break;
			}
			handlersThread.Start();
		}

		public void Halt() {
			if (handlersThread != null && handlersThread.IsAlive)
				handlersThread.Abort();
		}
	}
}