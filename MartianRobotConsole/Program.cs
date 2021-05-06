using System;
using System.Collections.Generic;
using MartianRobot.Application.Services.MartianRobot;
using MartianRobot.Domain.Entities;

namespace MartianRobotConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("------- Martian Robot ------- (Press Enter)");

            while (true)
            {
                var input = Console.ReadLine();
                if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Console.Write("X Limit:");
                string xLimit;
                xLimit = Console.ReadLine();
                Console.Write("Y Limit:");
                string yLimit;
                yLimit = Console.ReadLine();

                Console.Write("Start X Position: ");
                string xStartPosition = Console.ReadLine();

                Console.Write("Start Y Position: ");
                string yStartPosition = Console.ReadLine();

                Console.Write("Start Direction: ");
                string startDirection = Console.ReadLine();


                Console.Write("Commands: ");
                string robotCommands = Console.ReadLine();
                List<MartianRobotDTO> result = null;
                try
                {
                        var martianService = new MartianRobotService();
                        result = martianService.GetAsync(Convert.ToInt32(xLimit), Convert.ToInt32(yLimit), new List<InstructionDTO>() { new InstructionDTO(){
                        XPosition = Convert.ToInt32(xStartPosition),
                        YPosition = Convert.ToInt32(yStartPosition),
                        Direction = startDirection,
                        Commands = robotCommands
                    }}).Result;
                }catch(Exception ex)
                {
                    Console.WriteLine("Incorrect parameters");
                }
                if (result != null)
                {
                    foreach (var item in result)
                    {
                        Console.WriteLine(item.Result);
                    }
                    var lastItem = result[result.Count - 1];
                    Console.WriteLine(lastItem.IsValid + ", " + lastItem.Direction + ", (" + lastItem.X + ", " + lastItem.Y + ")");
                }


            }
        }
    }
}
