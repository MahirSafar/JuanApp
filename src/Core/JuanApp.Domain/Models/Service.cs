using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Service : AuditEntity
    {
        [StringLength(100)]
        public string Title { get; set; }

        [StringLength(50)]
        public string Icon { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
