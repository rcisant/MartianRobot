using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MartianRobot.Domain.Entities
{

    public class InstructionDTO 
    {
        [Required]
        public decimal XPosition { get; set; }

        public decimal YPosition { get; set; }
        public string Direction { get; set; }
        public string Commands { get; set; }

        



    }
}
