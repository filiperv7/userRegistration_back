namespace UserRegistration.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; protected set; }

        public DateTime CreationDate { get; protected set; }

        public DateTime? UpdateDate { get; set; }

    }
}
