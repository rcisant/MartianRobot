using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace MartianRobot.Domain.Entities
{

    public class MartianRobotDTO 
    {


        public string Name { get; set; }

        public string Result { get; set; }
        public bool IsValid { get; set; }
        public string Direction { get; set; }
        public decimal X { get; set; }
        public decimal Y { get; set; }




    }
}
