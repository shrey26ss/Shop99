using Infrastructure.Interface;

namespace Service.Models
{
    public class HotDealsResponse: IHotDealsResponse
    {
        public string Description { get; set; }
        public string DealEndsOn { get; set; }
    }
}
