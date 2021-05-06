using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MartianRobot.Domain.Repositories;
using MartianRobot.Infrastructure.Repositories;

namespace MartianRobot.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IMartianRobotRepository MartianRobotRepository { get; }
        void Save();
    }
}
