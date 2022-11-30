using Entities.Enums;

namespace AppUtility
{
    public class UtilityModels
    {
        public ResponseStatus StatusCode { get; set; } = ResponseStatus.Failed;
        public string ResponseText { get; set; } = ResponseStatus.Failed.ToString();
    }
}
