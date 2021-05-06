using AutoMapper;

namespace MartianRobot.Application.Services.MartianRobot
{
    public class MartianRobotMapper : Profile
    {
        public MartianRobotMapper()
        {
            CreateMap<MartianRobotUpdateable, Domain.Entities.MartianRobotDTO>()
                .ReverseMap();
        }
    }
}
