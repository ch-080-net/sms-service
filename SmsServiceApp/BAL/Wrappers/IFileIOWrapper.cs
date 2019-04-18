using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace BAL.Wrappers
{
    public interface IFileIoWrapper
    {
        bool Exists(string path);
        bool FileExists(string path);
        void FileDelete(string path); 
        void CreateDirectory(string path);
        void SaveBitmap(Bitmap bitmap, string path, ImageFormat imageFormat);
    }
}
