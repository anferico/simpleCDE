using System;
using System.Collections.Generic;

namespace Progetto {
	public class CDE {
		public delegate void EventHandler(string eventName, string actionName);
		protected List<Tuple<Agent, AgentBridge>> activeAgents;
		protected List<Relation> activeRelations;
		protected CloudProvider provider;
		protected CDEYamlParser parser;

		public CDE() {
			activeAgents = new List<Tuple<Agent, AgentBridge>>();
			activeRelations = new List<Relation>();
			provider = new CloudProvider();
			parser = new CDEYamlParser();
		}

		public void DeployApplication(string fileName) {
			var specification = parser.Parse(fileName);
			foreach (var service in specification.Services)
				foreach (var agent in specification.Agents)
					if (agent.ServiceImplemented == service.ServiceName)
						CreateServiceInstances(specification, service, agent, service.Units);

			if (specification.Relations != null)
				activeRelations.AddRange(specification.Relations);
			UpdateState();
		}

		protected void UpdateState() {
			foreach (var triple in AvailableEvents()) {
				var bridge = triple.Item1;
				var eventName = triple.Item2;
				var actionName = triple.Item3;
				bridge.OnEventTriggered(eventName, actionName);
			}
		}

		protected IEnumerable<Tuple<AgentBridge, string, string>> AvailableEvents() {
			foreach (var pair in activeAgents) {
				var agent = pair.Item1;
				foreach (var evt in agent.Events) {
					if (evt.State == EventState.Handling ||
						evt.State == EventState.Handled  ||
						(evt.Preconditions != null && evt.Preconditions.Count > 0))
						continue;
					var bridge = pair.Item2;
					evt.State = EventState.Handling;
					yield return new Tuple<AgentBridge, string, string>(bridge, evt.Name, evt.Handler);
				}
			}
		}

		public bool CheckRequirements(Specification specification, Agent agent) {
			if (agent.Requirements == null)
				return true;
			return agent.Requirements.TrueForAll(req => {
				bool selfEnabled = specification.Agents.Exists(a =>
					a.Provisions != null && a.Provisions.Contains(req)
				);
				bool enabledByOthers = activeAgents.Exists(pair => {
					var agn = pair.Item1;
					return agn.Provisions != null && agn.Provisions.Contains(req);
				});
				return selfEnabled || enabledByOthers;
			});
		}

		public void CreateServiceInstances(Specification specification, Service service, Agent agent, int amount) {
			if (!CheckRequirements(specification, agent))
				throw new Exception("Can't meet requirements for an agent.");

			for (int i = 0; i < amount; i++) {
				var bridge = new AgentBridge();
				bridge.EventCompleted += (eventName, actionName) => {
					foreach (var rel in activeRelations)
						if (rel.LeadingService == service.ServiceName &&
							rel.LeadingEvent == eventName) {
							foreach (var pair in activeAgents) {
								var agn = pair.Item1;
								agn.Events.ForEach(e => {
									if (agn.ServiceImplemented == rel.SubordinateService)
										e.Preconditions?.RemoveAll(evt => evt == rel.SubordinateEvent);
								});
							}
						}

					activeAgents.ForEach(pair => {
						var brg = pair.Item2;
						if (brg == bridge) {
							var agn = pair.Item1;
							agn.Events.ForEach(evt => {
								if (evt.Name == eventName)
									evt.State = EventState.Handled;
								evt.Preconditions?.Remove(eventName);
							});
						}
					});
					UpdateState();
				};
				provider.StartAgent(agent.ImplementingClass, bridge);
				activeAgents.Add(new Tuple<Agent, AgentBridge>(agent.DeepClone(), bridge));
			}
		}
	}
}