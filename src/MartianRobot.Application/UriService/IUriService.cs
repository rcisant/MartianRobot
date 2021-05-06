using System;
using System.Collections.Generic;
using System.Text;
using MartianRobot.Domain.SeedWork;

namespace MartianRobot.Application.Services.UriService
{
    public interface IUriService
    {
        public Uri GetPageUri(PaginationFilter filter, string route);
    }
}
