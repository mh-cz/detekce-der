using detekce.src.Other;
using Forms0.src;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace detekce.src
{
    internal class Sampler
    {
        // objekt rozdělující celek na menší části (samples)

        public List<Sample> samples;
        public List<Rect> sampleRects;

        public int scaleDown = 3;
        public OpenBitmap samplingBmp;
        
        public Sampler()
        {
            samples = new List<Sample>();
            sampleRects = new List<Rect>();
        }

        // rozdělení na menší části
        public void SampleImage(OpenBitmap bmp, int scaleDown = 3, int precision = 1)
        {
            int sampleStart = 0;
            bool inSample = false;
            this.scaleDown = scaleDown;

            Bitmap sbmp = new Bitmap(bmp.W / scaleDown, bmp.H / scaleDown, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using (Graphics g = Graphics.FromImage(sbmp))
            {
                g.DrawImage(bmp.Bitmap, new Rectangle(0, 0, bmp.W / scaleDown, bmp.H / scaleDown));
            }
            samplingBmp = new OpenBitmap(sbmp);

            samplingBmp.Lock();
            Effects.Grayscale(samplingBmp);
            Effects.CheapConvolution(samplingBmp, Kernels.x5_GaussianBlur, 1);
            Effects.BrighterThan(samplingBmp, 70, Color.White, Color.Black);
            samplingBmp.Unlock();

            // vertikální rozdělení
            for (int y = 0; y < samplingBmp.H; y += precision)
            {
                if (OpenBitmap.ColorEquals(samplingBmp.GetPixel(samplingBmp.W / 2, y), Color.White))
                {
                    if (!inSample)
                    {
                        inSample = true;
                        if (sampleRects.Count != 0) sampleStart = Math.Max(y - precision, 0);
                    }
                }
                else if (inSample)
                {
                    inSample = false;
                    sampleRects.Add(new Rect(0, y, samplingBmp.W, sampleStart));
                }
            }

            if (inSample) sampleRects.Add(new Rect(0, samplingBmp.H, samplingBmp.W, sampleStart));

            // horizontální "obejmutí"
            foreach (Rect r in sampleRects)
            {
                while (!OpenBitmap.ColorEquals(samplingBmp.GetPixel(r.left + precision, r.top - (int)(r.H * 0.75)), Color.White)
                    && !OpenBitmap.ColorEquals(samplingBmp.GetPixel(r.left + precision, r.top - (int)(r.H * 0.25)), Color.White))
                    r.left += precision;

                while (!OpenBitmap.ColorEquals(samplingBmp.GetPixel(r.right - precision, r.top - (int)(r.H * 0.75)), Color.White)
                    && !OpenBitmap.ColorEquals(samplingBmp.GetPixel(r.right - precision, r.top - (int)(r.H * 0.25)), Color.White))
                    r.right -= precision;
            }

            GenerateSamples(bmp);
        }

        // generování samplů
        // větší blur potřebuje místo okolo takže je potřeba zvětšit okraje o pár pixelů přes border
        void GenerateSamples(OpenBitmap bmp, int border = 5)
        {
             foreach (Rect r in sampleRects)
             {
                Rect scaledRect = new Rect(r);
                scaledRect.Scale(scaleDown);

                Rect offsetted = new Rect(scaledRect);
                offsetted.Offset(border);

                Bitmap sampleBmp = new Bitmap(offsetted.W, offsetted.H, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                using(Graphics g = Graphics.FromImage(sampleBmp))
                {
                    g.Clear(Color.Black);
                    g.DrawImage(bmp.Bitmap,
                        new Rectangle(border, border, scaledRect.W, scaledRect.H), 
                        new Rectangle(scaledRect.left, scaledRect.bottom, scaledRect.W, scaledRect.H), 
                        GraphicsUnit.Pixel);
                }

                samples.Add(new Sample(new OpenBitmap(sampleBmp), scaledRect, border));
             }
             
        }

        // vyhledej v samplech díry
        public void ProcessSamples()
        {
            Parallel.ForEach(samples, sample => sample.ProcessSample());
        }

    }
}
