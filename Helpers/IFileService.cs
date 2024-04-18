namespace WebApplication2.Helpers
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile file);
        public bool IsImage(IFormFile file);
        public bool CheckSize(IFormFile file, int maxSize);
        public void Delete(string path);
    }
}
