using Entities.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interface
{
    public interface IDeveloper
    {
        Task<IResponse<IEnumerable<PictureSubInfo>>> GetImgList(string Role);
    }
}
