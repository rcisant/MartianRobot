using System;
using System.Collections.Generic;
using System.Text;
using MartianRobot.Domain.Entities;

namespace UnitTests.Builders
{
    public class MartianRobotBuilder
    {

        //account fields with default values
        string result = "";
        string name = "123456789";


        public MartianRobotBuilder()
        {
        }

        public MartianRobotBuilder withResult(string result)
        {
            this.result = result;
            return this;
        }



        public MartianRobotBuilder withName(string name)
        {
            this.name = name;
            return this;
        }




        public MartianRobotDTO build()
        {
            return new MartianRobotDTO()
            {
                  Name = name,
                  Result = result
            };
        }
    }
}
