

using System;
using System.Drawing;
using System.Web;

namespace onlinePlatby.Models
{
    public class ImageManager
    {
        private float maxWidth;
        private float maxHeight;

        public ImageManager()
        {
            this.maxWidth = 1920;
            this.maxHeight = 1080;
        }

        public ImageManager(int maxWidth)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = 1080;
        }

        public ImageManager(int maxWidth, int maxHeight)
        {
            this.maxWidth = maxWidth;
            this.maxHeight = maxHeight;
        }

        /// <summary>
        /// saves image
        /// </summary>
        /// <param name="image">The image you want to save</param>
        /// <param name="path">path that shows the image location</param>
        public void SaveImage(HttpPostedFileBase image, string path)
        {
            float width;
            float height;

            if (image.ContentLength > 0) // is there any data in file?
            {
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(image.InputStream))
                {
                    if (img.Width >= img.Height) // is image horizontal?
                    {
                        if (img.Width > this.maxWidth) // is image bigger than max width?
                        {
                            height = (int)(img.Height / (img.Width / this.maxWidth));
                            width = this.maxWidth;
                        }
                        else
                        {
                            width = img.Width;
                            height = img.Height;
                        }
                    }
                    else // is image vertical?
                    {
                        if (img.Height > this.maxHeight)
                        {
                            width = (int)(img.Width / (img.Height / this.maxHeight)); // is image bigger than max height?
                            height = this.maxHeight;
                        }
                        else
                        {
                            width = img.Width;
                            height = img.Height;
                        }
                    }

                    using (Bitmap newBitmap = new Bitmap(img, (int)width, (int)height))
                    {
                        try
                        {
                            newBitmap.Save(path);
                        }
                        finally
                        {
                            newBitmap.Dispose();
                        }
                    }

                }
            }
            else
            {
                throw new Exception("Image saving failed");
            }
        }
    }
}
