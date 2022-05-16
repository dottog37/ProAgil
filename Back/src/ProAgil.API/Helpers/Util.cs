using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;


namespace ProAgil.API.Helpers
{
    public class Util : IUtil
    {
        private readonly IWebHostEnvironment hostEnvironment;
        public Util(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;

        }
      
        public async Task<string> SaveImage(IFormFile imageFile, string destino)
        {
            string imageName = new String(Path.GetFileNameWithoutExtension(imageFile.FileName)
                                                .ToArray())
                                                .Replace(' ', '-');

            imageName = $"{imageName}{DateTime.UtcNow.ToString("yymmssfff")}{Path.GetExtension(imageFile.FileName)}";

            var imagePath = Path.Combine(hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
            return imageName;
        }

    
        public void DeleteImage(string imageName, string destino)
        {
            var imagePath = Path.Combine(hostEnvironment.ContentRootPath, @$"Resources/{destino}", imageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);
        }

        public Task<string> Saveimage(IFormFile imageFile, string destino)
        {
            throw new NotImplementedException();
        }
    }
}