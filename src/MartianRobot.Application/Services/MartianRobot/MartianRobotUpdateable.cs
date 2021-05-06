using System;
using System.Collections.Generic;
using MartianRobot.Domain.Entities;

namespace MartianRobot.Application.Services.MartianRobot
{
    public class MartianRobotUpdateable
    {
        public decimal XPosition { get; set; }
        public decimal YPosition { get; set; }

        public string Direction { get; set; }
        public string Commands { get; set; }
        public decimal XLimit { get; set; }
        public decimal YLimit { get; set; }

        public bool IsLost { get; set; }
    }
}
