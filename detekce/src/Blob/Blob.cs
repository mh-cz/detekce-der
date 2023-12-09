using detekce.src.Other;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace detekce.src
{
    internal class Blob
    {
        // skupina blobu pixelů stejné barvy
        // uchovává celkovou velikost a počet pixelů

        public Rect rect;
        public int totalPixels = 0;

        public Blob(Point start)
        {
            rect = new Rect(start.X, start.Y, start.X, start.Y);
        }

        // přidej pixel do blobu a případně zvětši ohraničení
        public void AddPixelPosition(Point p)
        {
            totalPixels++;

            if(p.X < rect.left) rect.left = p.X;
            else if(p.X > rect.right) rect.right = p.X;
            if(p.Y < rect.bottom) rect.bottom = p.Y;
            else if(p.Y > rect.top) rect.top = p.Y;
        }

    }
}
