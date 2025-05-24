using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace VREnergyManagerMinimum
{
    public class ScreenCapture
    {
        private Rectangle _captureArea;
        private Bitmap _capturedImage;
        private System.Windows.Forms.Timer _captureTimer;
        private float _scaleFactor = 2.0f; // 拡大倍率

        public ScreenCapture(Rectangle captureArea)
        {
            _captureArea = captureArea;
            _capturedImage = new Bitmap(_captureArea.Width, _captureArea.Height);
            _captureTimer = new System.Windows.Forms.Timer();
            _captureTimer.Interval = 100; // 100msごとにキャプチャ
            _captureTimer.Tick += (s, e) => CaptureScreen();
            _captureTimer.Start();
        }

        private void CaptureScreen()
        {
            using (Graphics g = Graphics.FromImage(_capturedImage))
            {
                g.CopyFromScreen(_captureArea.Location, Point.Empty, _captureArea.Size);
            }
        }

        public void DrawCapturedImage(Graphics g, Rectangle displayArea)
        {
            if (_capturedImage != null)
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(_capturedImage, displayArea);
            }
        }

        public void Stop()
        {
            _captureTimer.Stop();
            _captureTimer.Dispose();
        }
    }
}