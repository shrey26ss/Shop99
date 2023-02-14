using Data;
using Entities.Enums;
using Entities.Models;
using Infrastructure.Interface;
using Microsoft.Extensions.Logging;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using Service.Models;
using Service.OrderDetails;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Service.Developer
{
    public class DeveloperService : IDeveloper
    {
        public readonly IDapperRepository _dapper;
        public readonly ILogger<OrderDetailsService> _logger;
        public DeveloperService(IDapperRepository dapper, ILogger<OrderDetailsService> logger)
        {
            _dapper = dapper;
            _logger = logger;
        }
        public async Task<IResponse<IEnumerable<PictureSubInfo>>> GetImgList(string Role)
        {
            string sp = "Select ImagePath Path, ImgVariant Variant from PictureInformation(nolock)";
            var res = new Response<IEnumerable<PictureSubInfo>>();
            try
            {
                if(Role == "4")
                {
                    res.Result = await _dapper.GetAllAsync<PictureSubInfo>(sp, null, CommandType.Text);
                    res.StatusCode = ResponseStatus.Success;
                    res.ResponseText = "";
                }                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return res;
        }
    }
}
