using System;
using System.Collections.Generic;
using System.Drawing;

namespace SoundInterference
{
    internal class WaveSourceDrawer
    {
        public float SceneWidthInMeters = 0.5f;
        private const int MaxAreaDimentionInCalculatedUnits = 500;
        public WaveSourceDrawer()
        {

        }
        public void Draw(DirectBitmap dbm, Rectangle bounds, ICollection<WaveSource> waveSources, double time)
        {
            var min = Math.Min(bounds.Height, bounds.Width);
            var pixelsInMeter = min / SceneWidthInMeters;
            var step = pixelsInMeter < MaxAreaDimentionInCalculatedUnits
                ? 1
                : (int)(pixelsInMeter / MaxAreaDimentionInCalculatedUnits);
            var colorCoefficient = byte.MaxValue * 1.4/ (float)waveSources.Count;
            for (var x = bounds.Left; x < bounds.Right; x += step)
            {
                for (var y = bounds.Top; y < bounds.Bottom; y += step)
                {
                    var xMeters = (x - bounds.Width / 2) / pixelsInMeter;
                    var yMeters = ((bounds.Bottom - y) - bounds.Height / 2) / pixelsInMeter;
                    double ampl = 0;
                    foreach (var waveSource in waveSources)
                    {
                        var target = new PointF(xMeters, yMeters);
                        ampl += waveSource.GetAmplitude(time, target);
                    }

                    var color = AmplitudeToArgb((int)(ampl * colorCoefficient));
                    if (step > 1)
                    {
                        for (int xpx = x; xpx < x + step && xpx < bounds.Right; xpx++)
                        {
                            for (int ypx = y; ypx < y + step && ypx < bounds.Bottom; ypx++)
                            {
                                dbm.SetPixel(xpx, ypx, color);
                            }
                        }
                    }
                    else
                    {
                        dbm.SetPixel(x, y, color);
                    }
                }
            }

            using (var g = Graphics.FromImage(dbm.Bitmap))
            {
                foreach (var waveSource in waveSources)
                {
                    var x = (int)(waveSource.LocationInMeters.X * pixelsInMeter) + bounds.Width / 2;
                    var y = bounds.Bottom - (int)(waveSource.LocationInMeters.Y * pixelsInMeter) - bounds.Height / 2;
                    g.DrawEllipse(Pens.Red, x - 2, y - 2, 4, 4);
                }
            }
        }

        private static int AmplitudeToArgb(int a)
        {
            var alpha = byte.MaxValue << 24;
            if (a < 0)
                return alpha | 0 | (-a << 8) |0;
            if (a > 0)
                return alpha | 0 | ((a/2) << 8) | 0;
            return alpha;
        }
    }
}
