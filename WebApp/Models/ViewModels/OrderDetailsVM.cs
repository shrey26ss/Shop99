using Entities.Enums;

namespace WebApp.Models.ViewModels
{
    public class OrderDetailsVM
    {
        public int ID { get; set; }
        public StatusType StatusID { get; set; }
    }
    public class TrackingModel
    {
        public int Id { get; set; }
        public string URL { get; set; }
    }
}
