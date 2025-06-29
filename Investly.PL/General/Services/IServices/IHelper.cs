namespace Investly.PL.General.Services.IServices
{
    public interface IHelper
    {
        public string? UploadFile(IFormFile file, string folderName, string? subFolder);
        public int DeleteFile(string oldfilepath);
        public string ExtractTxtFromFile(byte[] fileBytes, string fileName);


    }
}
