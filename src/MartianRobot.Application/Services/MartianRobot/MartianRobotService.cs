using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using MartianRobot.Application.Extensions;
using MartianRobot.Core.Validations;
using MartianRobot.Domain.Entities;
using MartianRobot.Domain.Instrumentation.Exceptions;
using MartianRobot.Domain.SeedWork;
using Entities = MartianRobot.Domain.Entities;

namespace MartianRobot.Application.Services.MartianRobot
{
    public class MartianRobotService : IMartianRobotService
    {

        public MartianRobotService()
        {
        }


        public async Task<List<MartianRobotDTO>> GetAsync(decimal xLimit, decimal yLimit, List<InstructionDTO> instructions)
        {
            List<MartianRobotDTO> response = new List<MartianRobotDTO>();
            try
            {
                Guard.NotNullZero(xLimit, nameof(xLimit));
                Guard.NotNullZero(yLimit, nameof(yLimit));
                ValidateLimits(xLimit, yLimit);
                ValidateInstructions(instructions);
                foreach (InstructionDTO item in instructions)
                {
                    ValidateStartPosition(item.XPosition, item.YPosition, xLimit, yLimit);
                    ValidateStartDirection(item.Direction);
                    ValidateCommand(item.Commands);

                    response.AddRange(CalculateCommands(new MartianRobotUpdateable()
                    {
                        XPosition = item.XPosition,
                        YPosition = item.YPosition,
                        Direction = item.Direction,
                        Commands = item.Commands,
                        XLimit = xLimit,
                        YLimit = yLimit,
                        IsLost = false
                    }));
                }
            }catch (Exception ex)
            {
                response = new List<MartianRobotDTO>() { new MartianRobotDTO() { Result = ex.Message } };
            }
            return await Task.FromResult(response);
            //return await _unitOfWork.MartianRobotRepository.GetOrThrowAsync(id);
        }
        private void ValidateInstructions(List<InstructionDTO> instructions)
        {
            if (instructions == null || instructions.Count == 0)
            {
                new BusinessValidationException("The parameters not contain instructions");
            }
        }
        private void ValidateLimits(decimal xLimit, decimal yLimit)
        {
            if (xLimit > 50 || yLimit > 50)
            {
                new BusinessValidationException("the maximum of Limits are 50");
            }

        }
        private void ValidateStartPosition(decimal startXPosition, decimal startYPosition, decimal xLimit, decimal yLimit)
        {
            if(startXPosition > xLimit || startYPosition > yLimit)
            {
                new BusinessValidationException("the coordinates (" + startXPosition + " - " + startXPosition + "are without range of Limits");
            }

        }
        private void ValidateStartDirection(string startDirection)
        {
            string pattern = @"[^NESWnesw]";
            Match m = Regex.Match(startDirection, pattern, RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                new BusinessValidationException("Direction not permitted. Required: N, E, S, W");
            }

        }

        private void ValidateCommand(string startCommand)
        {
            string pattern = @"[^lrfLRF]";
            Match m = Regex.Match(startCommand, pattern, RegexOptions.IgnoreCase);
            if (!m.Success)
            {
                new BusinessValidationException("Command not permitted. Required: L, R, F");
            }else if (m.Length > 99)
            {
                new BusinessValidationException("Length of Command not permitted. less than 100");
            }

        }


        private List<MartianRobotDTO> CalculateCommands(MartianRobotUpdateable martianRobot)
        {
            List<MartianRobotDTO> response = new List<MartianRobotDTO>();
            var commandsAsArray = martianRobot.Commands.ToStringArray();
            foreach (var item in commandsAsArray)
            {
                CalculateCommand(martianRobot, item);
                var output = martianRobot.XPosition.ToString() + " " + martianRobot.YPosition.ToString() + " " + martianRobot.Direction;
                var result = new MartianRobotDTO() { Result = output, X = martianRobot.XPosition, Y = martianRobot.YPosition, Direction = martianRobot.Direction, IsValid = true };
                if (martianRobot.IsLost)
                {
                    output = output + " LOST";
                    result.IsValid = false;
                    
                }
                response.Add(result);
            }
            return response;
        }
        
        private void CalculateCommand(MartianRobotUpdateable martianRobot, string command)
        {
            if (!martianRobot.IsLost)
            {
                switch (command)
                {
                    case "F":
                        MoveForward(martianRobot);
                        break;
                    case "L":
                        MoveLeft(martianRobot);
                        break;
                    case "R":
                        MoveRight(martianRobot);
                        break;
                }
            }
        }

        private void MoveForward(MartianRobotUpdateable martianRobot) {
            if (martianRobot.Direction.Equals("N")) {
              if (HasLimits(martianRobot) && martianRobot.YPosition + 1 > martianRobot.YLimit) {
                    martianRobot.IsLost = true;
                } else {
                    martianRobot.YPosition += 1;
                }
                } else if (martianRobot.Direction.Equals("E")) {
              if (HasLimits(martianRobot) && martianRobot.XPosition + 1 > martianRobot.XLimit) {
                    martianRobot.IsLost = true;
                } else {
                    martianRobot.XPosition += 1;
                }
                } else if (martianRobot.Direction.Equals("S")) {
              if (HasLimits(martianRobot) && martianRobot.YPosition - 1 < 0) {
                    martianRobot.IsLost = true;
                } else {
                    martianRobot.YPosition -= 1;
                }
                } else if (martianRobot.Direction.Equals("W")) {
              if (HasLimits(martianRobot) && martianRobot.XPosition - 1 < 0) {
                    martianRobot.IsLost = true;
                } else {
                    martianRobot.XPosition -= 1;
                }
                }
            }

            private void MoveLeft(MartianRobotUpdateable martianRobot)
            {
            switch (martianRobot.Direction) {
              case "N":
                    martianRobot.Direction = "W";
                break;   
              case "E":
                    martianRobot.Direction = "N";
                break;   
              case "S":
                    martianRobot.Direction = "E";
                break;   
              case "W":
                    martianRobot.Direction = "S";
                break;   
                }
              }

              private void MoveRight(MartianRobotUpdateable martianRobot)
                {
                    switch (martianRobot.Direction)
                    {
                        case "N":
                    martianRobot.Direction = "E";
                            break;
                        case "E":
                    martianRobot.Direction = "S";
                            break;
                        case "S":
                    martianRobot.Direction = "W";
                            break;
                        case "W":
                    martianRobot.Direction = "N";
                            break;
                    }
                }
        public bool HasLimits(MartianRobotUpdateable martianRobot)
        {
            return (martianRobot.XLimit > 0 && martianRobot.YLimit > 0);
        }
    }
    public static class StringExtensions
    {
        public static string[] ToStringArray(this string str)
        {
            string[] arr = new string[str.Length];

            for (int i = 0; i < str.Length; i++)
            {
                arr[i] = str[i].ToString();
            }

            return arr;
        }
    }

  
}
