using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.DAL;
using WebApplication2.Interfaces;
using WebApplication2.Models;

namespace WebApplication2.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin")]
    public class StudentQuoteController : Controller
	{
		private readonly AppDbContext _context;
		private readonly IWebHostEnvironment _webHostEnvironment;
		private readonly IFileService _fileService;


		public StudentQuoteController(AppDbContext context, IWebHostEnvironment webHostEnvironment, IFileService fileService)
        {
			_context=context;
			_webHostEnvironment=webHostEnvironment;
			_fileService=fileService;
		}
        public async Task <IActionResult>Index()
		{
			List<StudentQuote> studentQuote = await _context.StudentQuote.ToListAsync();
			return View(studentQuote);		
		}

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(StudentQuote studentQuote)
        {
            if (!ModelState.IsValid) return View(studentQuote);
            if (studentQuote.Photo != null)
            {
                if (!_fileService.IsImage(studentQuote.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(studentQuote);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(studentQuote.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} KB.");
                    return View(studentQuote);
                }

                var filename = await _fileService.UploadAsync(studentQuote.Photo);
                studentQuote.FilePath = filename;
            }
            studentQuote.CreatedAt = DateTime.UtcNow.AddHours(4);
            await _context.StudentQuote.AddAsync(studentQuote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
#endregion

        #region Update
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var StudentQuote= await _context.StudentQuote
                .FirstOrDefaultAsync(c => c.Id == id);
            if (StudentQuote== null) return NotFound();
            var model = new StudentQuote
            {
                Id =StudentQuote.Id,
                FullName = StudentQuote.FullName,
                Position= StudentQuote.Position,
                Description = StudentQuote.Description,
                FilePath = StudentQuote.FilePath,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(StudentQuote studentQuote, int id)
        {

            if (id != studentQuote.Id) return BadRequest();

            if (!ModelState.IsValid) return View(studentQuote);
            var dbStudentQuote = await _context.StudentQuote.FindAsync(id);
            dbStudentQuote.FullName = studentQuote.FullName;
            dbStudentQuote.Description =studentQuote.Description;
            dbStudentQuote.Position =studentQuote.Position;
            if (studentQuote.Photo != null)
            {
                if (!_fileService.IsImage(studentQuote.Photo))
                {
                    ModelState.AddModelError("Photo", "The file must be in Image format.");
                    return View(studentQuote);
                }
                int maxSize = 30;
                if (!_fileService.CheckSize(studentQuote.Photo, maxSize))
                {
                    ModelState.AddModelError("Photo", $"The size of the image should not exceed {maxSize} MB");
               
                    return View(studentQuote);
                }
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img");
                _fileService.Delete(path);
                var filename = await _fileService.UploadAsync(studentQuote.Photo);
                dbStudentQuote.FilePath = filename;
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region Delete
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var dbStudentQuote = await _context.StudentQuote.FindAsync(id);
            if (dbStudentQuote == null) return NotFound();
            var path = Path.Combine(_webHostEnvironment.WebRootPath, "assets/img", dbStudentQuote.FilePath);
            _fileService.Delete(path);
            _context.StudentQuote.Remove(dbStudentQuote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion






    }
}
