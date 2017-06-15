using System;
using System.Drawing;
using System.IO;

namespace FloorsPlanGGGWebApi.Misc
{
    public static class PhotoProcessorUtils
    {
        public const short EllipseSize = 15;

        public static Image GetImageFromFileName(this string fileName)
        {
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath($@"~/Static/{fileName}");
                return Image.FromFile(path);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static bool SaveImageToFile(this Image image, string fileName)
        {
            try
            {
                string path = System.Web.Hosting.HostingEnvironment.MapPath($@"~/Static/{fileName}");
                image.Save(path);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Image PlacePinOnPhotoWithCoordinates(this Image inputImage, int coordX, int coordY, int customSize = EllipseSize)
        {
            using (Graphics g = Graphics.FromImage(inputImage))
            {
                Color customColor = Color.Red;
                SolidBrush solidColorBrush = new SolidBrush(customColor);
                g.FillEllipse(solidColorBrush, new RectangleF(coordX - customSize / 2, coordY - customSize / 2, customSize, customSize));
            }

            return inputImage;
        }        
    }
}