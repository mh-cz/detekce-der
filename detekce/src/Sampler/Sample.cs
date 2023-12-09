using detekce.src.Other;
using Forms0.src;
using System.Drawing;
using System.Windows.Forms;

namespace detekce.src
{
    internal class Sample
    {
        // jednotlivá rozdělená část obsahující metodu pro hledání defektu

        public OpenBitmap bmp;
        public BlobFinder blobFinder;

        // pozice v hlavním obrázku z kterého tenhle sample je (pro vykreslování)
        public Rect rect;
        public Rect brect;
        public int border = 0;

        bool ok = false;
        public bool OK { get => ok; }

        public Sample(OpenBitmap bmp, Rect rect, int border = 0)
        {
            this.bmp = bmp;
            this.rect = rect;
            this.border = border;

            brect = new Rect(rect);
            brect.Offset(border);

            blobFinder = new BlobFinder();
        }
    
        // hlavní metoda pro hledání děr
        public void ProcessSample()
        {
            bmp.Lock();
            Effects.Grayscale(bmp);
            Effects.CheapConvolution(bmp, Kernels.x5_GaussianBlur, 4);
            Effects.BrighterThan(bmp, 187, Color.White, Color.Black);
            blobFinder.FindBlobs(bmp, Color.White, Color.Red, 30, 2);
            bmp.Unlock();

            // tahle část je ok pokud v něm není ani jeden blob
            ok = blobFinder.blobs.Count == 0;
        }
    
    }
}
