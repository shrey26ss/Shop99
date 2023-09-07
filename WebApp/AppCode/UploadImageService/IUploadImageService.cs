using AppUtility.Helper;
using Service.Models;

namespace WebApp.AppCode.UploadImageService
{
    public interface IUploadImageService
    {
        Response Upload(FileUploadModel request);
    }
}
