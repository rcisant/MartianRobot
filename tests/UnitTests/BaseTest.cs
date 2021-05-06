using AutoMapper;
using MartianRobot.Api.Infrastructure.Extensions;
using Moq;

namespace UnitTests
{
    public abstract class BaseTest
    {
        protected readonly IMapper Mapper;

        protected BaseTest()
        {
            Mapper = CreateMapper();
        }

        public IMapper CreateMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(new string[] { "MartianRobot.Application" });
                MapperSettings.GetMapperSettings(cfg);

            });
            return config.CreateMapper();
        }
    }
}
