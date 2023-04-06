using Entities.Enums;
using System.Collections.Generic;

namespace Entities.Models
{
    public class PageSetting
    {
        public int TotoalRows { get; set; }
        public int CurrentPage { get; set; }
        public int Draw { get; set; }
    }

    public class JDataTableResponse
    {
        public int draw { get; set; }
        public int start { get; set; }
        public SortingOption OrderBy { get; set; }
        public int recordsTotal { get; set; }
        public int recordsFiltered { get; set; }
        public PageSetting PageSetting { get; set; }
    }

    public class JDataTableResponse<T>: JDataTableResponse
    {
        public List<T> Data { get; set; }
    }
    public class JSONAOData
    {
        public int draw { get; set; }
        public int start { get; set; } = 0;
        public int length { get; set; } = 100;
        public jsonAODataSearch search { get; set; }
        public object param { get; set; }
    }

    public class jsonAODataSearch
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
