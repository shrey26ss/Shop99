using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IAPILogger
    {
        Task<bool> SaveLog(string request, string response, string method, bool IsIncmOut = false, string CallingFrom = "");
    }
}
