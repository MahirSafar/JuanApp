namespace JuanApp.Domain.Common
{
    public interface IBaseEntity
    {
        DateTime Created { get; set; }
        DateTime? LastModified { get; set; }
        DateTime? Deleted { get; set; }
        bool IsDeleted { get; set; }
    }
}