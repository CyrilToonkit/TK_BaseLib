using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace TK.BaseLib.Picture
{
    public class PictureHelper
    {
        public static Bitmap CreateBitmapImage(int iWidth, int iHeight, string sImageText, Color backColor, Color textColor)
        {
            Bitmap objBmpImage = new Bitmap(iWidth, iHeight);

            // Create a graphics object to measure the text's width and height.
            Graphics objGraphics = Graphics.FromImage(objBmpImage);

            Font objFont = new Font("Arial", 12, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            int intWidth = (int)objGraphics.MeasureString(sImageText, objFont).Width;

            // Set Background color

            objGraphics.Clear(backColor);
            objGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            objGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            objGraphics.DrawString(sImageText, objFont, new SolidBrush(textColor), (float)((iWidth - intWidth) / 2.0), (float)(iHeight/4.0));
            objGraphics.Flush();

            return (objBmpImage);
        }
    }
}
