using System;
using System.CodeDom;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace detekce
{
    public class OpenBitmap : IDisposable
    {
        // verze bitmapy s rychlým přístupem k pixelům

        Bitmap bmp;
        BitmapData bmpData;

        // bitmap data
        byte[] pixels;
        int colorDepth;
        int bitsPerPixel;

        // pointer do paměti
        IntPtr ptr = IntPtr.Zero;

        // rozlišení
        int w = 0;
        int h = 0;

        // public gettry
        public int W { get => w; }
        public int H { get => h; }
        public byte[] Pixels { get => pixels; }
        public int BitsPerPixel { get => bitsPerPixel; }
        public Bitmap Bitmap { get => bmp; }


        public OpenBitmap() { }

        public OpenBitmap(Bitmap sourceBmp)
        {
            Open(sourceBmp);
        }

        // otevři a načti obrázek
        public void Open(Bitmap sourceBmp)
        {
            bmp = sourceBmp;

            w = bmp.Width;
            h = bmp.Height;

            colorDepth = Image.GetPixelFormatSize(bmp.PixelFormat);
            bitsPerPixel = colorDepth / 8;
            pixels = new byte[w * h * bitsPerPixel];
        }
        // odemkni přístup k obrázku a zakaž přístup k pixelům
        public void Lock()
        {
            bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadWrite, bmp.PixelFormat);
            ptr = bmpData.Scan0;

            Marshal.Copy(ptr, pixels, 0, pixels.Length);
        }
        // uzamkni obrázek a povol přístup k pixelům
        public void Unlock()
        {
            bmp.UnlockBits(bmpData);
        }
        // zkopíruj změny v pixelech
        public void WriteChanges()
        {
            Marshal.Copy(pixels, 0, ptr, pixels.Length);
        }

        public Color GetPixel(int x, int y)
        {
            int pos = ((w * y) + x) * bitsPerPixel;

            return Color.FromArgb(
                pixels[pos + 3], 
                pixels[pos + 2], 
                pixels[pos + 1], 
                pixels[pos]);
        }

        public void SetPixel(int x, int y, Color color)
        {
            int pos = ((w * y) + x) * bitsPerPixel;

            pixels[pos] = color.B;
            pixels[pos + 1] = color.G;
            pixels[pos + 2] = color.R;
            pixels[pos + 3] = color.A;
        }
        // hromadná kopie pixelů
        public void CopyPixels(byte[] dest)
        {
            Array.Copy(pixels, dest, pixels.Length);
        }
        // zahození obrázku
        public void Dispose()
        {
            Unlock();
            bmp.Dispose();
        }

        // pomocná funkce pro porovnání barev
        public static bool ColorEquals(Color c1, Color c2)
        {
            return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
        }
    }

}
