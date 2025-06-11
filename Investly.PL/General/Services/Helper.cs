using Investly.PL.General.Services.IServices;

namespace Investly.PL.General.Services
{
    public class Helper:IHelper
    {
        public string? UploadFile(IFormFile file, string mainFolder, string? subFolder)
        {
            if (file == null || file.Length == 0)
            {
                return null;
            }
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            var folderPath = Path.Combine(uploadsFolder, mainFolder,subFolder??"");
            
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath); 
            }
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }
           var urlPath=Path.Combine("uploads", mainFolder, subFolder ?? "", fileName).Replace("\\", "/");
           return urlPath;
        }
        public int DeleteFile(string oldfilepath)
        {

            if (string.IsNullOrEmpty(oldfilepath))
                return -1;
            var physicalpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldfilepath.Replace("\\", "/"));
            if (File.Exists(physicalpath))
            {
                File.Delete(physicalpath);
                return 1;
            }
            else
            {
                return 0;
            }
        }

    }
}
