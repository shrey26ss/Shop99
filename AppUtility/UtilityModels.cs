using Entities.Enums;

namespace AppUtility
{
    public class UtilityModels
    {
        public ResponseStatus StatusCode { get; set; } = ResponseStatus.Failed;
        public string ResponseText { get; set; } = ResponseStatus.Failed.ToString();
    }
    public class ExcelResponse<T> : UtilityModels
    {
        public T data { get; set; }
    }
    public class InMemoryFile
    {
        public string FileName { get; set; }
        public byte[] Content { get; set; }
    }
}
