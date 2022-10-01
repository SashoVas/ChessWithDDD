namespace Shared.Domain
{
    public abstract class AggregateRoot : Entity
    {
        public IEnumerable<IDomainEvent> Events => _events;
        private readonly List<IDomainEvent> _events = new();
        protected AggregateRoot(Guid Id) : base(Id)
        {
        }
        protected void AddEvent(IDomainEvent @event) 
            => _events.Add(@event);
        public void ClearEvents()=>_events.Clear();


    }
}
