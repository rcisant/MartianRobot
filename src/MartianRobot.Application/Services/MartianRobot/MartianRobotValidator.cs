using System;
using FluentValidation;
using FluentValidation.Validators;

namespace MartianRobot.Application.Services.MartianRobot
{
    public class MartianRobotValidator : AbstractValidator<Domain.Entities.MartianRobotDTO>
    {
        public MartianRobotValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Please specify a Name for Martian Robot");

            // TODO: Service Module mus be validated when the schema exists
        }


    }
}
