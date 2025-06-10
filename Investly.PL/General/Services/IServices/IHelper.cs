namespace Investly.PL.General.Services.IServices
{
    public interface IHelper
    {
        public string? UploadFile(IFormFile file, string folderName, string? subFolder);
    }
}
