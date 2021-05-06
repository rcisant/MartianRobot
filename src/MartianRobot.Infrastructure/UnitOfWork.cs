using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using MartianRobot.Domain.Repositories;
using MartianRobot.Infrastructure.Interfaces;
using MartianRobot.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace MartianRobot.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MartianRobotContext _context;
       private IMartianRobotRepository _martianRobotRepository;
        private readonly IMapper _mapper;

        public UnitOfWork(MartianRobotContext context
            , IMartianRobotRepository martianRobotRepository
            , IMapper mapper)
        {
            _context = context;
            _martianRobotRepository = martianRobotRepository;
            _mapper = mapper;
        }


        public IMartianRobotRepository MartianRobotRepository
        {
            get
            {
                return _martianRobotRepository;
            }
        }


        public async void Save()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
