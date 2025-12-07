using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.AttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        private readonly string[] allowedExtenstions = { ".jpg", ".jpeg", ".png" };
        private readonly long maxSize = 2 * 1024 * 1024;
        public string? Upload(string folderName, IFormFile file)
        {
            try
            {
                if (folderName is null || file is null || file.Length == 0) return null;

                if (file.Length > maxSize) return null;

                var ext = Path.GetExtension(file.FileName).ToLower();
                if (!allowedExtenstions.Contains(ext)) return null;

                var FolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName); // PL
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }

                // Guid FileName
                var FileName = Guid.NewGuid().ToString() + ext;

                var FilePath = Path.Combine(FolderPath, FileName);

                // Stream
                // using => To Close Stream after Done 
                using var FileStream = new FileStream(FilePath, FileMode.Create);

                file.CopyTo(FileStream);

                return FileName;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Faild To Upload Photo {ex}");
                return null;
            }
        }
        public bool Delete(string fileName, string folderName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;

                var FullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", folderName, fileName);
                if (File.Exists(FullPath))
                {
                    File.Delete(FullPath);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
