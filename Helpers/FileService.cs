using WebApplication2.Interfaces;

namespace WebApplication2.Helpers
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<string> UploadAsync(IFormFile file)
        {
            var filename = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}_{file.FileName}"; 
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", filename);

            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                await file.CopyToAsync(fileStream);
            }
            return filename;
        }

        public bool IsImage(IFormFile file)
        {
            return file.ContentType.Contains("image/");
        }
        public bool CheckSize(IFormFile file, int maxSize)
        {
            if (file.Length / 1024 > maxSize)
            {
                return false;
            }
            return true;
        }
        public void Delete(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

    }
}
