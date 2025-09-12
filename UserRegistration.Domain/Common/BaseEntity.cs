namespace UserRegistration.Domain.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
