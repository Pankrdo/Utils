namespace Anxilaris.Utils
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Drawing.Drawing2D;
    using System.IO;
    using System.Linq;
    using Utils;
    public static class ImageUtils
    {
        public enum Dimensions
        {
            Width,
            Height
        }
        public enum AnchorPosition
        {
            Top,
            Center,
            Bottom,
            Left,
            Right
        }

        public static Image ScaleByPercent(string imageFullPath, int Percent)
        {
            using (Image imgPhoto = Image.FromFile(imageFullPath))
            {
                return ScaleByPercent(imgPhoto, Percent);
            }
        }

        public static Image ScaleByPercent(Image imgPhoto, int Percent)
        {

            //using (Image imgPhoto = imgPhoto_)
            //{
            float nPercent = ((float)Percent / 100);

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;

            int destX = 0;
            int destY = 0;
            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();

            return bmPhoto;

        }

        public static Image ConstrainProportions(string imageFullPath, int Size, Dimensions Dimension)
        {

            using (Image imgPhoto = Image.FromFile(imageFullPath))
            {
                return ConstrainProportions(imgPhoto, Size, Dimension);
            }
        }

        public static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;
            float nPercent = 0;

            switch (Dimension)
            {
                case Dimensions.Width:
                    nPercent = ((float)Size / (float)sourceWidth);
                    break;
                default:
                    nPercent = ((float)Size / (float)sourceHeight);
                    break;
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
            new Rectangle(destX, destY, destWidth, destHeight),
            new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
            GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image FixedSize(string imageFullPath, int Width, int Height)
        {
            using (Image imgPhoto = Image.FromFile(imageFullPath))
            {
                return FixedSize(imgPhoto, Width, Height);
            }
        }

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
        {

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentW;
                switch (Anchor)
                {
                    case AnchorPosition.Top:
                        destY = 0;
                        break;
                    case AnchorPosition.Bottom:
                        destY = (int)(Height - (sourceHeight * nPercent));
                        break;
                    default:
                        destY = (int)((Height - (sourceHeight * nPercent)) / 2);
                        break;
                }
            }
            else
            {
                nPercent = nPercentH;
                switch (Anchor)
                {
                    case AnchorPosition.Left:
                        destX = 0;
                        break;
                    case AnchorPosition.Right:
                        destX = (int)(Width - (sourceWidth * nPercent));
                        break;
                    default:
                        destX = (int)((Width - (sourceWidth * nPercent)) / 2);
                        break;
                }
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static Image Compress(this Image image, ImageFormat format, int desiredSize)
        {
            int compression = 100;

            long fileSize = image.GetSize(format);
            //Compress()
            ImageCodecInfo imageEncoder = GetEncoder(format);
            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            Encoder myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            while (fileSize > desiredSize)
            {

                var stream_ = new MemoryStream();
                //Begins compress image              
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compression);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //Ends compress image
                image.Save(stream_, imageEncoder, myEncoderParameters);
                image = (Image)Image.FromStream(stream_).Clone();
                //img = (Image)result.Clone();
                fileSize = stream_.Length;

                compression -= 5;
            }

            return image;
        }

        private static string scaledFullPath;

        private static string scaledDirectoryPath;

        /// <summary>
        /// Adjusts the image to the height, width and size specified
        /// </summary>
        /// <param name="directoryPath">absolute path of image folder</param>
        /// <param name="fileName">image name to resize</param>
        /// <param name="width">desired width</param>
        /// <param name="height">desired height</param>
        /// <param name="size">desired weight</param>
        /// <returns>the path to the scaled image</returns>
        public static Image Standardize(this Image img, string fileName, int width, int height, int size)
        {
            ImageFormat format = img.GetFormat(fileName);

            Image result = (Image)img.Clone();// Image.FromFile(fullPath);

            long fileSize = img.GetSize(format);//(int)new System.IO.FileInfo(fullPath).Length;

            int scale = 95, compression = 100;

            size = size * 1024;

            while (result.Width > width || result.Height > height)
            {
                result.Dispose();
                result = ImageUtils.ScaleByPercent(img, scale);
                scale -= 2;
            }

            img.Dispose();

            //Compress()
            ImageCodecInfo imageEncoder = GetEncoder(format);
            // Create an Encoder object based on the GUID
            // for the Quality parameter category.
            Encoder myEncoder = Encoder.Quality;

            // Create an EncoderParameters object.
            // An EncoderParameters object has an array of EncoderParameter
            // objects. In this case, there is only one
            // EncoderParameter object in the array.
            EncoderParameters myEncoderParameters = new EncoderParameters(1);

            while (fileSize > size)
            {
                var stream_ = new MemoryStream();
                //Begins compress image              
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, compression);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //Ends compress image
                result.Save(stream_, imageEncoder, myEncoderParameters);
                result = (Image)Image.FromStream(stream_).Clone();               
                fileSize = stream_.Length;

                compression -= 5;
            }

            return result;
        }

        public static Image Clone(this Image img, ImageFormat format)
        {
            var stream_ = new MemoryStream();
            img.Save(stream_, format);
            return (Image)Image.FromStream(stream_).Clone();
        }


        /// <summary>
        /// Adjusts the image to the height and width desired and compress size if is necesary
        /// </summary>
        /// <param name="directoryPath">absolute path of image folder</param>
        /// <param name="fileName">image name to resize</param>
        /// <param name="width">desired width</param>
        /// <param name="height">desired height</param>
        /// <param name="size">desired weight</param>
        /// <returns>the path to the scaled image</returns>
        public static Image Correct(this Image image, string imageName, int width, int height, int size)
        {
            int scale = 95;

            size = size * 1024;           

            while (image.Width > width || image.Height > height)
            {
                image = ScaleByPercent(image, scale);
                scale -= 2;
            }

            ImageFormat format = image.GetFormat(imageName);

            image = image.Compress(format, size);

            image = image.Clone(format);

            return image;
        }


        private static long GetSize(this Image image, ImageFormat format)
        {
            using (var memoryStream = new MemoryStream())
            {
                image.Save(memoryStream, format);

                return memoryStream.Length;
            }
        }

        public static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public static ImageFormat GetFormat(this Image img, string sourceString)
        {
            string pattern = string.Empty;

            if (string.IsNullOrEmpty(sourceString))
                throw new Exception("File name cannot be null or empty");

            if (sourceString.Contains("."))
            {
                pattern = sourceString.Substring((sourceString.LastIndexOf('.') + 1));
                pattern = pattern.ToUpper();
            }

            switch (pattern)
            {
                case "JPEG":
                    return ImageFormat.Jpeg;
                case "image/jpg":
                    return ImageFormat.Jpeg;
                case "image/jpeg":
                    return ImageFormat.Jpeg;
                case "JPG":
                    return ImageFormat.Jpeg;
                case "image/png":
                    return ImageFormat.Png;
                case "PNG":
                    return ImageFormat.Png;
                case "image/bmp":
                    return ImageFormat.Bmp;
                case "BMP":
                    return ImageFormat.Bmp;
                case "image/gif":
                    return ImageFormat.Gif;
                case "GIF":
                    return ImageFormat.Gif;
                default:
                    return ImageFormat.Jpeg;
            }
        }

        //public static ImageFormat GetFormat(Image img)
        //{
        //    return new ImageFormat(img.RawFormat.Guid);
        //}        

        private static string GetMimeType(this Image image)
        {
            return image.RawFormat.GetMimeType();
        }

        private static string GetMimeType(this ImageFormat imageFormat)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            return codecs.First(codec => codec.FormatID == imageFormat.Guid).MimeType;
        }
    }
}
