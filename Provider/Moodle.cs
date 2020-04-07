using System;
using System.Threading;

namespace simpleCDE 
{
    public class Moodle : IAgent 
    {
        public event CDE.EventHandler EventCompleted;
        private Thread handlersThread;
        private Random random;

        public Moodle() 
        {
            random = new Random();
        }

        public void install(string eventName) 
        {
            try 
            {
                Thread.Sleep(random.Next(500, 1000));

                if (EventCompleted != null) 
                {
                    EventCompleted(eventName, "install");
                }

                Console.WriteLine(
                    string.Format("Moodle: event {0} completed.", eventName)
                );
            } 
            catch (ThreadAbortException) 
            {
                Console.WriteLine("Moodle: halted.");
            }
        }

        public void connect(string eventName) 
        {
            try 
            {
                Thread.Sleep(random.Next(1500, 2000));

                if (EventCompleted != null)
                {
                    EventCompleted(eventName, "connect");
                }

                Console.WriteLine(
                    string.Format("Moodle: event {0} completed.", eventName)
                );
            } 
            catch (ThreadAbortException) 
            {
                Console.WriteLine("Moodle: halted.");
            }
        }

        public void start(string eventName) 
        {
            try 
            {
                Thread.Sleep(random.Next(3000, 5000));

                if (EventCompleted != null)
                {
                    EventCompleted(eventName, "start");
                }

                Console.WriteLine(
                    string.Format("Moodle: event {0} completed.", eventName)
                );
            } 
            catch (ThreadAbortException) 
            {
                Console.WriteLine("Moodle: halted.");
            }
        }

        public void OnEventTriggered(string eventName, string actionName) 
        {
            switch (actionName) 
            {
                case "install": 
                    handlersThread = new Thread(() => install(eventName));
                    break;

                case "connect": 
                    handlersThread = new Thread(() => connect(eventName));
                    break;

                case "start": 
                    handlersThread = new Thread(() => start(eventName));
                    break;
            }
            handlersThread.Start();
        }

        public void Halt() 
        {
            if (handlersThread != null && handlersThread.IsAlive)
            {
                handlersThread.Abort();
            }
        }
    }
}
