using detekce.src;
using detekce.src.Other;
using Forms0.src;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace detekce
{
    public partial class Form1 : Form
    {
        ImageProcessor processor;

        public Form1()
        {
            InitializeComponent();

            pbx_result.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btn_openImg_Click(object sender, EventArgs e)
        {
            // načtení ze souboru
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "Files|*.jpg;*.jpeg;*.png;*.bmp;";

                if (dialog.ShowDialog() == DialogResult.OK)
                {


                    Image img = Image.FromFile(dialog.FileName);
                    
                    // načtení a zmenšeí fotky
                    processor = new ImageProcessor(img);
                    
                    // zpracování a kontrola
                    processor.SampleAndProcess();

                    img.Dispose();




                    // vykreslení

                    var greenPen = new Pen(Color.Lime, 1);
                    var redPen = new Pen(Color.Red, 1);

                    OpenBitmap drawBmp = new OpenBitmap(processor.resizedBmp.Bitmap);

                    foreach (Sample s in processor.sampler.samples)
                    {
                        // obrysy sítí
                        s.rect.Draw(drawBmp.Bitmap, greenPen);

                        // obrysy děr
                        foreach (Blob b in s.blobFinder.blobs)
                        {
                            Rect correctPosRect = new Rect(b.rect);
                            correctPosRect.Move(s.rect.left - s.border, s.rect.bottom - s.border);
                            correctPosRect.Offset(4);
                            correctPosRect.Draw(drawBmp.Bitmap, redPen);
                        }
                    }

                    pbx_result.Image = drawBmp.Bitmap;
                    // na šířku se to líp vleze
                    pbx_result.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                }
            }
        }
        
    }
}
