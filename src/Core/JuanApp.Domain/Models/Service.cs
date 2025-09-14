using JuanApp.Domain.Common;

namespace JuanApp.Domain.Models
{
    public class Service : AuditEntity
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Description { get; set; }
    }
}
