using System;
using System.Drawing;
using System.Windows.Forms;

namespace detekce.src.Other
{
    internal class Rect
    {
        // uchovává informace o ohraničujícím obdélníku 
        // musí se dávat pozor na orientaci

        public int left = 0;
        public int top = 0;
        public int right = 0;
        public int bottom = 0;

        public Rectangle Rectangle { get => new Rectangle(Math.Min(left, right), Math.Max(top, bottom), W, H); }
        public int W { get => Math.Abs(right - left); }
        public int H { get => Math.Abs(bottom - top); }
        
        public Rect(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public Rect(Rect rect)
        {
            this.left = Math.Min(rect.left, rect.right);
            this.top = Math.Max(rect.top, rect.bottom);
            this.right = Math.Max(rect.left, rect.right);
            this.bottom = Math.Min(rect.top, rect.bottom);
        }

        public void Scale(float scale)
        {
            left = (int)(left * scale);
            top = (int)(top * scale);
            right = (int)(right * scale);
            bottom = (int)(bottom * scale);
        }

        public void Offset(int px)
        {
            if (left <= right)
            {
                left -= px;
                right += px;
            }
            else
            {
                left += px;
                right -= px;
            }
            
            if (bottom <= top)
            {
                bottom -= px;
                top += px;
            }
            else
            {
                bottom += px;
                top -= px;
            }


    
        }

        public void Move(int x, int y)
        {
            left += x;
            top += y;
            right += x;
            bottom += y;
        }

        // pro vykreslování
        public void Draw(Bitmap bmp, Pen pen)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                int l = left;
                int r = right;
                int t = top;
                int b = bottom;

                g.DrawLine(pen, l, t, l, b);
                g.DrawLine(pen, r, t, r, b);
                g.DrawLine(pen, r, t, l, t);
                g.DrawLine(pen, r, b, l, b);
            }
        }

    }
}
