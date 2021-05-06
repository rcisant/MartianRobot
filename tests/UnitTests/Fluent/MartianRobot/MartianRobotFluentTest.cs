using System;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using Moq;
using MartianRobot.Domain.Entities;
using Xunit;
using FluentValidation.TestHelper;
using MartianRobot.Application.Services.MartianRobot;

namespace UnitTests.Fluent.MartianRobot
{
    public class MartianRobotFluentTest : BaseTest
    {

        public MartianRobotFluentTest()
        {

        }


        [Fact]
        public void Fluent_MartianRobot_Name_WithInvalidNameArgument()
        {
            MartianRobotValidator validator = new MartianRobotValidator();
            var model = new MartianRobotDTO { Name = ""};
            var result = validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }




    }
}
