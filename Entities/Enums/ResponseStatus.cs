using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Enums
{
    public enum ResponseStatus
    {
        Failed = -1,
        Success = 1,
        Pending = 2,
        info = 3,
        warning = 4,
        Expired = -2,
    }
}
