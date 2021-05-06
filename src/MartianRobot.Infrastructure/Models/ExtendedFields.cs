using System;
using System.Collections.Generic;
using System.Text;

namespace MartianRobot.Infrastructure.Models
{
    public partial class ExtendedFields
    {
        public Guid Timestamp { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }
        public int? LastUpdatedBy { get; set; }
    }
}
