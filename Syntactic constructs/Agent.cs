using System.Collections.Generic;

namespace Progetto {
	public class Agent {
		public readonly string ServiceImplemented;
		public readonly string ImplementingClass;
		public readonly List<Event> Events;
		public readonly List<string> Requirements;
		public readonly List<string> Provisions;

		public Agent(string serviceImplemented, string implementingClass, List<Event> events, List<string> requirements, List<string> provisions) {
			ServiceImplemented = serviceImplemented;
			ImplementingClass = implementingClass;
			Events = events;
			Requirements = requirements;
			Provisions = provisions;
		}

		public Agent DeepClone() {
			return new Agent(
				ServiceImplemented,
				ImplementingClass,
				Events.ConvertAll(e =>
					new Event(
						e.Name,
						e.Preconditions == null ? null : new List<string>(e.Preconditions),
						e.Handler
					)
				),
				Requirements == null ? null : new List<string>(Requirements),
				Provisions == null ? null : new List<string>(Provisions)
			);
		}
	}
}