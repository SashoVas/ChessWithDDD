namespace Shared.Domain
{
    public abstract class Entity
    {
        public Guid Id { get; private init; }
        protected Entity(Guid Id)
        {
            this.Id = Id;
        }
    }
}
