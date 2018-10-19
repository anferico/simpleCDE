using System.Collections.Generic;
namespace Progetto {
	public class Specification {
		public readonly List<Service> Services;
		public readonly List<Agent> Agents;
		public readonly List<Relation> Relations;

		public Specification(List<Service> services, List<Agent> agents, List<Relation> relations) {
			Services = services;
			Agents = agents;
			Relations = relations;
		}
	}
}