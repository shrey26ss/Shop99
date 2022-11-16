using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public interface IConnectionString
    {
        string connectionString { get; set; }
    }

    public class ConnectionString : IConnectionString
    {
        public string connectionString { get; set; }
    }
}
