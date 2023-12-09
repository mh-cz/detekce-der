using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace detekce.src
{
    internal class BlobFinder
    {
        public List<Blob> blobs;

        public BlobFinder() 
        { 
            blobs = new List<Blob>();
        }

        // skenuj pixely dokud nenarazíš na danou barvu
        public BlobFinder FindBlobs(OpenBitmap bmp, Color findColor, Color repaintBlobs, int minBlobPixels, int precision = 1)
        {
            blobs.Clear();

            for (int x = 0; x < bmp.W; x += precision)
                for (int y = 0; y < bmp.H; y += precision)
                {
                    if (OpenBitmap.ColorEquals(bmp.GetPixel(x, y), findColor))
                    {
                        Blob blob = FloodFill(bmp, new Point(x, y), findColor, repaintBlobs);
                        // pokud je blob dostatečně velký, přidej ho (je vadný)
                        if (blob.totalPixels >= minBlobPixels) blobs.Add(blob);
                    }
                }

            bmp.WriteChanges();
            return this;
        }

        // vyplňovací algoritmus pro vytvoření nového blobu a zjištění jeho velikosti
        // barví pixely na jinou barvu takže se tento blob dá detekovat jenom jednou
        Blob FloodFill(OpenBitmap bmp, Point start, Color findColor, Color repaintBlobs)
        {
            Stack<Point> pixels = new Stack<Point>();
            Blob blob = new Blob(start);
            
            pixels.Push(start);

            while (pixels.Count > 0)
            {
                Point p = pixels.Pop();
                if (OpenBitmap.ColorEquals(bmp.GetPixel(p.X, p.Y), findColor))
                {
                    bmp.SetPixel(p.X, p.Y, repaintBlobs);
                    blob.AddPixelPosition(p);

                    if (FFInside(bmp, p.X - 1, p.Y)) pixels.Push(new Point(p.X - 1, p.Y));
                    if (FFInside(bmp, p.X + 1, p.Y)) pixels.Push(new Point(p.X + 1, p.Y));
                    if (FFInside(bmp, p.X, p.Y - 1)) pixels.Push(new Point(p.X, p.Y - 1));
                    if (FFInside(bmp, p.X, p.Y + 1)) pixels.Push(new Point(p.X, p.Y + 1));
                }
            }

            return blob;
        }

        bool FFInside(OpenBitmap bmp, int x, int y)
        {
            return (x < bmp.W && x > 0 && y < bmp.H && y > 0);
        }
    }
}
