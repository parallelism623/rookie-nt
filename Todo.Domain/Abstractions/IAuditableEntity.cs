namespace Todo.Domain.Abstractions
{
    public interface IAuditableEntity<TKey>
    {
        public DateTime CreatedAt { get; set; }
        public TKey CreateBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public TKey? ModifiedBy { get; set; }
    }
}
