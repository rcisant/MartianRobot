using FluentValidation;

namespace MartianRobot.Application.Services.MartianRobot
{
    public class MartianRobotUpdateableValidator : AbstractValidator<MartianRobotUpdateable>
    {
        public MartianRobotUpdateableValidator()
        {
            //RuleFor(x => x.Id)
            //    .NotEmpty()
            //    .WithMessage("Please specify an ID for Asset");

        }
    }
}
