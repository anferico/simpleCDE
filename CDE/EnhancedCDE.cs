using System.Linq;

namespace Progetto {
	public class EnhancedCDE : CDE {
		public void UpdateSpecification(string newSpec) {
			var specification = parser.Parse(newSpec);

			foreach (var service in specification.Services)
				foreach (var agent in specification.Agents)
					if (agent.ServiceImplemented == service.ServiceName) {
						int activeInstances = activeAgents.Count(pair => {
							var agn = pair.Item1;
							return agn.ServiceImplemented == agent.ServiceImplemented;
						});

						if (activeInstances < service.Units) 
							CreateServiceInstances(specification, service, agent, service.Units - activeInstances);
						else 
							RemoveServiceInstances(service, activeInstances - service.Units);
					}
			
			if (specification.Relations != null)
				activeRelations.AddRange(specification.Relations);
		}

		private void RemoveServiceInstances(Service service, int amount) {
			for (int i = 0; i < amount; i++) {
				var tuple = activeAgents.First(pair => {
					var agn = pair.Item1;
					return agn.ServiceImplemented == service.ServiceName;
				});

				var bridge = tuple.Item2;
				provider.HaltAgent(bridge);
				activeAgents.Remove(tuple);
			}
		}
	}
}