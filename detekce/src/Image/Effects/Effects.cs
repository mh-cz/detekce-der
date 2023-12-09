using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace detekce.src
{
    internal class Effects
    {
        // obrázek do černobílé
        public static void Grayscale(OpenBitmap bmp)
        {
            for(int x = 0; x < bmp.W; x++)
                for(int y = 0; y < bmp.H; y++)
                {
                    Color c = bmp.GetPixel(x, y);
                    int gray = (int)(c.R * 0.299 + c.G * 0.587 + c.B * 0.114);
                    bmp.SetPixel(x, y, Color.FromArgb(gray, gray, gray));
                }

            bmp.WriteChanges();
        }
        
        // změň pixely na barvy podle úrovně světlosti pixelů (černobílého) obrázku
        public static void BrighterThan(OpenBitmap bmp, byte brightness, Color brightEnough, Color notBrightEnough)
        {
            for(int x = 0; x < bmp.W; x++)
                for(int y = 0; y < bmp.H; y++)
                {
                    byte gray = bmp.GetPixel(x, y).R;
                    bmp.SetPixel(x, y, gray >= brightness ? brightEnough : notBrightEnough);
                }

            bmp.WriteChanges();
        }

        // efekt nad obrázkem s použití konvoluce a kernelu
        public static void CheapConvolution(OpenBitmap bmp, int[,] kernel, int repeat = 1)
        {
            int kSize = kernel.GetLength(0);
            int extend = (kSize - 1) / 2;

            int kSum = 0;
            for (int ky = 0; ky < kSize; ky++)
                for (int kx = 0; kx < kSize; kx++)
                    kSum += kernel[ky, kx];

            double kDiv = 1.0 / (kSum == 0 ? 1 : kSum);

            int sumR, sumG, sumB, k, pos = 0;
            int w = bmp.W;
            int bpp = bmp.BitsPerPixel;

            byte[] copiedPixels = new byte[bmp.Pixels.Length];

            for (int i = 0; i < repeat; i++)
            {
                Buffer.BlockCopy(bmp.Pixels, 0, copiedPixels, 0, bmp.Pixels.Length);

                for (int y = extend; y < bmp.H - extend * 2; y++)
                    for (int x = extend; x < bmp.W - extend * 2; x++)
                    {
                        sumR = 0;
                        sumG = 0;
                        sumB = 0;

                        for (int ky = 0; ky < kSize; ky++)
                            for (int kx = 0; kx < kSize; kx++)
                            {
                                pos = ((w * (y + ky)) + (x + kx)) * bpp;
                                k = kernel[ky, kx];

                                sumR += copiedPixels[pos + 2] * k;
                                sumG += copiedPixels[pos + 1] * k;
                                sumB += copiedPixels[pos] * k;
                            }

                        bmp.SetPixel(x + extend, y + extend, Color.FromArgb((byte)(sumR * kDiv), (byte)(sumG * kDiv), (byte)(sumB * kDiv)));
                    }
            }

            bmp.WriteChanges();
        }



    }
}
