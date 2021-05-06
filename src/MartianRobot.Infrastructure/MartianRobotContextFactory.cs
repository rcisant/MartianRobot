//using System;
//using System.Collections.Generic;
//using System.Text;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;

//namespace MartianRobot.Infrastructure
//{
//    public class MartianRobotContextFactory : IDesignTimeDbContextFactory<MartianRobotContext>
//    {
//        public MartianRobotContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<MartianRobotContext>();
//            optionsBuilder.UseSqlServer(@"Server=localhost;Database=IoTHub;Trusted_Connection=True;");

//            return new MartianRobotContext(optionsBuilder.Options);
//        }
//    }
//}
