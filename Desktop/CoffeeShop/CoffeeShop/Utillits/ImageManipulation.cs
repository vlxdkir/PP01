using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace CoffeeShop.Utillits
{
    public class ImageManipulation
    {
        public byte[] ConvertImageToByteArray(ImageSource image)
        {
            if (image is BitmapImage bitmapImage)
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
                using (MemoryStream mem = new MemoryStream())
                {
                    encoder.Save(mem);
                    return mem.ToArray();
                }
            }
            return null;
        }
        public ImageSource LoadImageFromDatabase(byte[] imageData)
        {
            if (imageData != null)
            {
                var image = new BitmapImage();
                using (MemoryStream mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
            else
            {
                Uri uri = new Uri("pack://application:,,,/Image/city.jpg");
                return new BitmapImage(uri);
            }
        }

        public byte[] ConvertImageToByteArray(string filePath)
        {
            byte[] imageBytes;
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var memoryStream = new MemoryStream())
                {
                    stream.CopyTo(memoryStream);
                    imageBytes = memoryStream.ToArray();
                }
            }
            return imageBytes;
        }
    }
}
