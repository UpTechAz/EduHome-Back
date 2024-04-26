﻿namespace WebApplication2.Helpers
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
            var filename = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}_{Guid.NewGuid()}{file.FileName}"; // DateTime.UtcNow
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

        public bool IsImage(string headerLogo)
        {
            throw new NotImplementedException();
        }

        public bool CheckSize(string headerLogo, int maxSize)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadAsync(string headerLogo)
        {
            throw new NotImplementedException();
        }
    }
}
