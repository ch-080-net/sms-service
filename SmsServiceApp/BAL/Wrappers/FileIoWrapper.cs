using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.IO;

namespace BAL.Wrappers
{
    public class FileIoWrapper : IFileIoWrapper
    {
        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }

        public bool Exists(string path)
        {
            return Directory.Exists(path);
        }

        public void FileDelete(string path)
        {
            File.Delete(path);
        }

        public bool FileExists(string path)
        {
            return File.Exists(path);
        }

        public void SaveBitmap(Bitmap bitmap, string path, ImageFormat imageFormat)
        {
            bitmap.Save(path, imageFormat);
        }
    }
}
