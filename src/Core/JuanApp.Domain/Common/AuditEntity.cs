namespace JuanApp.Domain.Common
{
    public class AuditEntity : BaseEntity
    {
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public DateTime? Deleted { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
