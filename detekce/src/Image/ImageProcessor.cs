using Forms0.src;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace detekce.src.Other
{
    internal class ImageProcessor
    {
        public Image originalImage;
        public OpenBitmap resizedBmp;
        public Sampler sampler;

        // přednastavené hodnoty pro balanc rychlosti a přesnosti
        public int cropSides = 150;
        public int newWidth = 100;

        public ImageProcessor(Image sourceImage, int newWidth = 100, int cropSides = 150)
        {
            this.newWidth = newWidth;
            this.cropSides = cropSides;

            sampler = new Sampler();
            originalImage = sourceImage;

            // zmenši a odstraň zbytečné okraje
            float scaleBy = (float)newWidth / (originalImage.Width);
            int newW = (int)((sourceImage.Width - cropSides * 2) * scaleBy);
            int newH = (int)(sourceImage.Height * scaleBy);

            Bitmap rsBmp = new Bitmap(newW, newH, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(rsBmp))
            {
                g.DrawImage(originalImage,
                    new Rectangle(0, 0, newW, newH),
                    new Rectangle((int)(cropSides), 0, (int)(sourceImage.Width - cropSides * 2), sourceImage.Height),
                    GraphicsUnit.Pixel);
            }

            resizedBmp = new OpenBitmap(rsBmp);
        }

        // rozděl obrázek na části a ty pak zkontroluj
        public void SampleAndProcess()
        {
            sampler.SampleImage(resizedBmp);
            sampler.ProcessSamples();
        }

    }
}
