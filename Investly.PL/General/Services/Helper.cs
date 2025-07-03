using Investly.PL.General.Services.IServices;
using System.Text;

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
            var ext=file.FileName.Split('.').LastOrDefault()?.ToLower();
            var fileName = $"{Guid.NewGuid()}.{ext}";
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

        public string ExtractTxtFromFile(byte[] fileBytes, string fileName)
        {

            string ext = Path.GetExtension(fileName).ToLower();

            return ext switch
            {
                ".txt" => System.Text.Encoding.UTF8.GetString(fileBytes),
                ".pdf" => ExtractTextFromPdf(fileBytes),
               // ".docx" => ExtractTextFromDocx(fileBytes),
                _ => throw new NotSupportedException("File format not supported")
            };
        }

        private string ExtractTextFromPdf(byte[] fileBytes)
        {
            using var pdfReader = new iText.Kernel.Pdf.PdfReader(new MemoryStream(fileBytes));
            using var pdfDocument = new iText.Kernel.Pdf.PdfDocument(pdfReader);
            var strategy = new iText.Kernel.Pdf.Canvas.Parser.Listener.SimpleTextExtractionStrategy();

            StringBuilder sb = new StringBuilder();
            for (int i = 1; i <= pdfDocument.GetNumberOfPages(); i++)
            {
                var text = iText.Kernel.Pdf.Canvas.Parser.PdfTextExtractor.GetTextFromPage(pdfDocument.GetPage(i), strategy);
                sb.AppendLine(text);
            }
            return sb.ToString();
        }

    }
}
