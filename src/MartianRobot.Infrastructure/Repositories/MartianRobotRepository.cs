using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MartianRobot.Domain.Entities;
using MartianRobot.Domain.Instrumentation.Exceptions;
using MartianRobot.Domain.Repositories;
using MartianRobot.Domain.SeedWork;
using MartianRobot.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace MartianRobot.Infrastructure.Repositories
{
    public class MartianRobotRepository : IMartianRobotRepository
    {
        private readonly MartianRobotContext _context;
        private readonly IMapper _mapper;

        public MartianRobotRepository(MartianRobotContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MartianRobotDTO> AddNewAsync(MartianRobotDTO requests)
        {
            //if (!Inspections.Exists(u => u.Id == asset.Id))
            //{
            //    Inspections.Add(asset);
            //}
            //else
            //{
            //    throw new ResourceAlreadyExistException();
            //}

            return await Task.FromResult(requests);
        }

        public async Task<MartianRobotDTO> DeleteAsync(int id)
        {

            var dataEntity = await _context.MartianRobot
             .Where(x => x.Id == id).FirstOrDefaultAsync();

            if (dataEntity != null)
            {
                _context.Remove(dataEntity);
                await _context.SaveChangesAsync();
                await _context.DisposeAsync();
                return await Task.FromResult(_mapper.Map<MartianRobotDTO>(dataEntity));
            }
            else
            {
                throw new ResourceNotFoundException($"Item '{id}' is not found");
            }
        }

        public async Task<MartianRobotDTO> GetAsync(int id)
        {
            var dataEntity = await _context.MartianRobot.AsNoTracking()
             .Where(x => x.Id == id).FirstOrDefaultAsync();
            var responseEF = await Task.FromResult(_mapper.Map<MartianRobotDTO>(dataEntity));
            return responseEF;
        }

        public async Task<PagedResult<MartianRobotViewDTO>> GetAllAsync(PaginationFilter paginationFilter)
        {
            if (paginationFilter.SearchString != null)
            {
                paginationFilter.PageNumber = 1;
            }
            else
            {
                paginationFilter.SearchString = paginationFilter.CurrentFilter;
            }
            var requests = from s in _context.MartianRobot
                          select s;
            if (!String.IsNullOrEmpty(paginationFilter.SearchString))
            {
                int testNumber = 0;
                Int32.TryParse(paginationFilter.SearchString, out testNumber);
                requests = requests.Where(s => (s.UserName != null && testNumber.Equals(0) && Convert.ToString(s.UserName).Contains(paginationFilter.SearchString))
                                || (Convert.ToString(s.Id).Contains(paginationFilter.SearchString))
                                );
            }
            if (paginationFilter.Ids != null)
            {
                requests = requests.Where(s => paginationFilter.Ids.Contains(s.Id));
            }
            switch (paginationFilter.SortOrder)
            {
                case "id_desc":
                    requests = requests.OrderByDescending(s => s.Id);
                    break;
                case "id_asc":
                    requests = requests.OrderBy(s => s.Id);
                    break;
                case "name_desc":
                    requests = requests.OrderByDescending(s => s.UserName);
                    break;
                case "name_asc":
                    requests = requests.OrderBy(s => s.UserName);
                    break;
                default:
                    requests = requests.OrderByDescending(s => s.Id);
                    break;
            }

            var requestsDB = requests.AsNoTracking();
            var requestsView = await Task.FromResult(_mapper.Map<List<MartianRobotViewDTO>>(requestsDB));
            await _context.DisposeAsync();
            return await Task.FromResult(PagedResult<MartianRobotViewDTO>.Create(requestsView, paginationFilter.PageNumber, paginationFilter.PageSize));


        }

        public async Task<MartianRobotDTO> UpdateAsync(MartianRobotDTO requests)
        {

            //MartianRobotData response = null;
            //var dbItem = _context.MartianRobot
            //        .Where(x => x.Id == requests.Id).SingleOrDefault();
            //var updateItem = _mapper.Map<MartianRobotData>(requests);
            //updateItem = _mapper.Map(updateItem, dbItem);
            //if (dbItem != null)
            //{
            //    _context.Entry(dbItem).OriginalValues.SetValues(dbItem);
            //    _context.Entry(dbItem).CurrentValues.SetValues(updateItem);
            //    await _context.SaveChangesAsync();
            //    response = dbItem;
            //}
            //else
            //{
            //    await _context.MartianRobot.AddAsync(updateItem);
            //    await _context.SaveChangesAsync();
            //    response = updateItem;
            //}

            //await _context.DisposeAsync();
            //return await Task.FromResult(_mapper.Map<MartianRobotDTO>(response));
            return null;

        }
    }
}
