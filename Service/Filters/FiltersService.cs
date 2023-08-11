using AppUtility.Helper;
using Dapper;
using Data;
using Data.Models;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using Service.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Service.Categories
{
    public class FiltersService : IFiltersService
    {
        private IDapperRepository _dapper;
        private readonly ILogger<DapperRepository> _logger;
        public FiltersService(IDapperRepository dapper, ILogger<DapperRepository> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<IEnumerable<Filters>>> GetFiltersByCategory(int CategoryId)
        {
            string sp = string.Empty;
            var res = new Response<IEnumerable<Filters>>();
            try
            {
                sp = @"select a.AttributeId,ab.Name,ab.Id,	a.AttributeValue from AttributeValue a inner join Attributes ab on ab.Id=a.AttributeId inner join CategoryAttributeMapping cam on a.AttributeId=cam.AttributeId inner join Category c on c.CategoryId = cam.CategoryId where cam.CategoryId=@CategoryId and cam.IsActive=1 and c.IsPublish = 1"
                    /*@"select a.AttributeId,ab.Name,ab.Id,	a.AttributeValue from AttributeValue a inner join Attributes ab on ab.Id=a.AttributeId inner join CategoryAttributeMapping cam on a.AttributeId=cam.AttributeId where cam.CategoryId=@CategoryId and cam.IsActive=1"*/;
                var Result = await _dapper.GetAllAsync<Filters>(sp, new { CategoryId }, CommandType.Text);
                var distinctfilter = Result
                        .Select(m => new { m.Name, m.AttributeId })
                        .Distinct()
                        .ToList();
                var fls = new List<Filters>();
                foreach (var item in distinctfilter)
                {
                    List<FiltersAttributes> att = Result.Where(x => x.Name == item.Name).Select(x => new FiltersAttributes{ AttributeValue= x.AttributeValue }).ToList();
                    var filters = new Filters()
                    {
                        Name = item.Name,
                        AttributeId = item.AttributeId,
                        attributes = att
                    };
                    fls.Add(filters);
                }
                res.Result = fls;
                if (res.Result.Count()== 0)
                {
                    res.StatusCode = ResponseStatus.Failed;
                    res.ResponseText = ResponseStatus.Failed.ToString();
                }
                else
                {
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = ResponseStatus.Success.ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return res;
        }
    }
}
