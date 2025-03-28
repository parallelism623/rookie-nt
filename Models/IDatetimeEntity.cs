namespace aspnetcore.Models
{
    public interface IDatetimeEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid CreateBy { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
