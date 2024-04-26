namespace WebApplication2.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file);
        public bool IsImage(IFormFile file);
        public bool CheckSize(IFormFile file, int maxSize);
        public void Delete(string path);
        bool IsImage(string headerLogo);
        bool CheckSize(string headerLogo, int maxSize);
        Task<string> UploadAsync(string headerLogo);
    }
}
