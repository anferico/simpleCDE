using System;
using System.Collections.Generic;

namespace simpleCDE 
{
    public class CloudProvider 
    {
        private List<Tuple<IAgent, AgentBridge>> activeAgents;

        public CloudProvider() 
        {
            activeAgents = new List<Tuple<IAgent, AgentBridge>>();
        }

        public void StartAgent(string className, AgentBridge bridge) 
        {
            var agent = (IAgent)Activator.CreateInstance(
                Type.GetType("simpleCDE." + className)
            );
            agent.EventCompleted += bridge.OnEventCompleted;
            bridge.EventTriggered += agent.OnEventTriggered;
            activeAgents.Add(new Tuple<IAgent, AgentBridge>(agent, bridge));
        }

        public void HaltAgent(AgentBridge bridge) 
        {
            activeAgents.RemoveAll(pair => {
                var candidateBridge = pair.Item2;
        
                if (bridge != candidateBridge)
                    return false;
        
                var agent = pair.Item1;
        
                agent.Halt();
                agent.EventCompleted -= bridge.OnEventCompleted;
                bridge.EventTriggered -= agent.OnEventTriggered;
        
                return true;
            });
        }
    }
}
