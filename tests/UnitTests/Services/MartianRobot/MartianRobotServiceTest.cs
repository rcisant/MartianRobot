using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using MartianRobot.Domain.Entities;
using MartianRobot.Domain.Instrumentation.Exceptions;
using UnitTests.Extensions;
using Xunit;
using UnitTests.Builders;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using MartianRobot.Application.Services.MartianRobot;
using System.Collections.Generic;

namespace UnitTests.Services.MartianRobot
{
    public class MartianRobotServiceTest : BaseTest
    {
        private readonly IMartianRobotService martianRobotService;

        public MartianRobotServiceTest()
        {
            // Mock Services
            martianRobotService = new MartianRobotService();
        }


        [Fact]
        public async Task GetAsync_WithInvalidIdArgument()
        {
            //Act //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => martianRobotService.GetAsync(0, 0, new List<InstructionDTO>()));
        }
        [Fact]
        public async Task GetAsync_Test1_OK()
        {
            //Arrange
            MartianRobotDTO martianRobot = new MartianRobotBuilder()
              .withName("")
              .build();

            //Act
            var objectToTest = new List<InstructionDTO>();
            objectToTest.Add(new InstructionDTO() { XPosition = 1, YPosition = 1, Direction = "E", Commands = "RFRFRFRF" });
            var result = await martianRobotService.GetAsync(5, 3, objectToTest);

            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count == 8);
            Assert.True(result[result.Count - 1].Result == "1 1 E");



        }


        [Fact]
        public async Task GetAsync_Test2_Lost()
        {
            //Arrange
            MartianRobotDTO martianRobot = new MartianRobotBuilder()
          .withName("")
          .build();

            //Act
            var objectToTest = new List<InstructionDTO>();
            objectToTest.Add(new InstructionDTO() { XPosition = 1, YPosition = 1, Direction = "E", Commands = "RFRFRFRF" });
            objectToTest.Add(new InstructionDTO() { XPosition = 3, YPosition = 2, Direction = "N", Commands = "FRRFLLFFRRFLL" });
            var result = await martianRobotService.GetAsync(5, 3, objectToTest);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 1);
            Assert.True(result[result.Count - 1].Result == "3 3 N LOST");

            


        }

        [Fact]
        public async Task GetAsync_Test3_OK()
        {
            //Arrange
            MartianRobotDTO martianRobot = new MartianRobotBuilder()
          .withName("")
          .build();

            //Act
            var objectToTest = new List<InstructionDTO>();
            
            objectToTest.Add(new InstructionDTO() { XPosition = 0, YPosition = 3, Direction = "W", Commands = "LLFFFLFLFL" });
            var result = await martianRobotService.GetAsync(5, 3, objectToTest);
            //Assert
            Assert.NotNull(result);
            Assert.True(result.Count > 1);
            Assert.True(result[result.Count - 1].Result != "2 3 S");




        }

    }
}
