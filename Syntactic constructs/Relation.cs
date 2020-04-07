namespace simpleCDE 
{
    public class Relation 
    {
        public readonly string SubordinateService;
        public readonly string SubordinateEvent;
        public readonly string LeadingService;
        public readonly string LeadingEvent;

        public Relation(
            string subordinateService, 
            string subordinateEvent, 
            string leadingService, 
            string leadingEvent
        ) {
            SubordinateService = subordinateService;
            SubordinateEvent = subordinateEvent;
            LeadingService = leadingService;
            LeadingEvent = leadingEvent;
        }
    }
}
