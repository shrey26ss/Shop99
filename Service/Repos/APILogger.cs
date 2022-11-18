using Data;
using Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Repos
{
    public class APILogger : IAPILogger
    {
        private IDapperRepository _dapper;
        public APILogger(IDapperRepository dapper)
        {
            _dapper = dapper;
        }
        public async Task<bool> SaveLog(string request, string response, string method,bool IsIncmOut = false, string CallingFrom = "")
        {
            bool res = false;
            int i = await _dapper.ExecuteAsync("insert into APILog(Request,Response,Method,EntryOn,IsIncomingOutgoing,CallingFrom) Values (@request,@response,@method,getDate(),@IsIncmOut,@CallingFrom)", new
            {
                request,
                response,
                method,
                IsIncmOut,
                CallingFrom
            }, System.Data.CommandType.Text);
            if (i > 0)
            {
                res = true;
            }
            return res;
        }
    }
}
