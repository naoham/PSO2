using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace VREnergyManagerMinimum
{
    public partial class VREMMin : Form
    {
        private Pen _pen_Base = new Pen(Color.FromArgb(80, Color.Aqua), 1f);
        private readonly Dictionary<float, Pen> _specialLines = new Dictionary<float, Pen>
        {
            { 70f, new Pen(Color.FromArgb(80, Color.Red), 1f) },
            { 85f, new Pen(Color.FromArgb(80, Color.Orange), 1f) },
            { 95f, new Pen(Color.FromArgb(80, Color.Red), 1f) },
        };

        private readonly IniFile _ini;
        private const string IniSection = "WindowPosition";

        public VREMMin()
        {
            InitializeComponent();
            Text = "VRエネルギー管理ミニマム";
            TopMost = true;
            TransparencyKey = BackColor;
            DoubleBuffered = true;

            string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
            _ini = new IniFile(iniPath);

            Load += (s, e) =>
            {
                int w = _ini.ReadInt(IniSection, "Width", 300);
                int h = _ini.ReadInt(IniSection, "Height", 50);
                int x = _ini.ReadInt(IniSection, "X", -1);
                int y = _ini.ReadInt(IniSection, "Y", -1);

                Size = new Size(w, h);

                if (x < 0 || y < 0)
                {
                    Rectangle scr_rect = Screen.PrimaryScreen.WorkingArea;
                    x = scr_rect.X + (scr_rect.Width - w) / 2;
                    y = scr_rect.Y + (scr_rect.Height - h) / 2;
                }

                Location = new Point(x, y);
            };

            FormClosing += (s, e) =>
            {
                _ini.Write(IniSection, "X", Location.X.ToString());
                _ini.Write(IniSection, "Y", Location.Y.ToString());
                _ini.Write(IniSection, "Width", Width.ToString());
                _ini.Write(IniSection, "Height", Height.ToString());
            };

            Resize += (s, e) => Invalidate();

            Paint += (s, e) =>
            {
                float margin = 10F;
                RectangleF rect = new RectangleF(
                    margin, margin,
                    ClientSize.Width - margin * 2,
                    ClientSize.Height - margin * 2);

                e.Graphics.DrawLine(_pen_Base, rect.Left, rect.Bottom, rect.Right, rect.Bottom);
                e.Graphics.DrawLine(_pen_Base, rect.Left, rect.Top, rect.Left, rect.Bottom);

                for (float percent = 50f; percent < 95f; percent += 2.5f)
                {
                    float x = rect.Left + rect.Width * percent / 100f;
                    if (percent > 69f)
                    {
                        Pen pen = _specialLines.ContainsKey(percent) ? _specialLines[percent] : _pen_Base;
                        e.Graphics.DrawLine(pen, x, rect.Top, x, rect.Bottom);
                    }
                }

                for (float percent = 95f; percent <= 100f; percent += 5f)
                {
                    float x = rect.Left + rect.Width * percent / 100f;
                    Pen pen = _specialLines.ContainsKey(percent) ? _specialLines[percent] : _pen_Base;
                    e.Graphics.DrawLine(pen, x, rect.Top, x, rect.Bottom);
                }
            };
        }
    }
}