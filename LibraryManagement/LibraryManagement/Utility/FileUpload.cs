namespace LibraryManagement.Utility
{
    public static class FileUpload
    {
        public static string UploadImage(IFormFile file, string webRootPath)
        {
            if (file == null)
                return "";

            string uploadsFolder = Path.Combine(webRootPath, "images", "books");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string fileName = Guid.NewGuid().ToString()
                            + Path.GetExtension(file.FileName);

            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/images/books/" + fileName;
        }

        public static void DeleteImage(string webRootPath, string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath))
                return;

            string fullPath = Path.Combine(
                webRootPath,
                imagePath.TrimStart('/').Replace("/", "\\"));

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}