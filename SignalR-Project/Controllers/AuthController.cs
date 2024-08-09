using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SignalR_Project.Context;
using SignalR_Project.Dtos;
using SignalR_Project.Models;
using IHostingEnvironment = Microsoft.Extensions.Hosting.IHostingEnvironment;

namespace SignalR_Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController : ControllerBase
    {
        // we wrote it here cause it is for edu.
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webEnvironment;

        public AuthController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _webEnvironment = environment;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterDto register, CancellationToken cancellationToken)
        {
            bool isNameExists = await _context.Users.AnyAsync(p => p.Name == register.Name, cancellationToken);
            if (isNameExists)
            {
                return BadRequest(new { Message = "Bu kullanıcı adı daha önce kullanılmış." });
            }

            string folderPath = Path.Combine(_webEnvironment.ContentRootPath, "file-storage");
            string newFileName = Guid.NewGuid().ToString();
            string fileExtension = Path.GetExtension(register.Avatar.FileName).ToLower();
            string fileFullName = newFileName + fileExtension;

            string filePath = "/avatars"; // fileType değerleri filePath içeriyor.
            filePath = filePath.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            folderPath = Path.Combine(folderPath, filePath);

            using (var stream = new FileStream(Path.Combine(folderPath, fileFullName), FileMode.Create))
            {
                await register.Avatar.CopyToAsync(stream, cancellationToken);
            }

            User user = new()
            {
                Name = register.Name,
                Avatar = Path.Combine(filePath, fileFullName)
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return Ok(user);
        }


        [HttpGet]
        public async Task<IActionResult> Login(string name, CancellationToken cancellationToken)
        {
            User? user = await _context.Users.FirstOrDefaultAsync(p => p.Name == name, cancellationToken);

            if (user == null)
            {
                return BadRequest(new { Message = "Kullanıcı bulunamadı." });
            }

            //user.Status = "online";
            //await _context.SaveChangesAsync(cancellationToken);
            return Ok(user);

        }
    }
}
