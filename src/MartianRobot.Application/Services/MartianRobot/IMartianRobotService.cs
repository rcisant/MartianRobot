using System.Collections.Generic;
using System.Threading.Tasks;
using MartianRobot.Domain.SeedWork;
using MartianRobot.Domain.Entities;

namespace MartianRobot.Application.Services.MartianRobot
{
    public interface IMartianRobotService
    {

        Task<List<MartianRobotDTO>> GetAsync(decimal xLimit, decimal yLimit, List<InstructionDTO> instructions);
    }
}
