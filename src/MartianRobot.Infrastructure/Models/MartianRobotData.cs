using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MartianRobot.Infrastructure.Models
{
    public class MartianRobotData : ExtendedFields
    {
        [Required]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }




    }
}
