using JuanApp.Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace JuanApp.Domain.Models
{
    public class Setting : AuditEntity
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
