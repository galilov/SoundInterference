using System;
using System.Drawing;
using System.Windows.Forms;

namespace SoundInterference
{
    public partial class MainForm : Form
    {
        private WaveSource[] _waveSources;
        private readonly WaveSourceDrawer _drawer;
        private DirectBitmap _dbm;
        private double _time;
        //private float _dStart = -0.07f, _dStop = 0.07f, _dStep = 0.0001f, _d;

        public MainForm()
        {
            //_d = _dStart;
            PhazedArray1();
            _drawer = new WaveSourceDrawer();
            InitializeComponent();
        }

        //private void PhazedArray2()
        //{
        //    var step = 0.023f * 0.25f;
        //    var n = 21;
        //    _waveSources = new WaveSource[n];
        //    var y = -step * _waveSources.Length / 2;
        //    var k = -_waveSources.Length / 2;

        //    for (var i = 0; i < _waveSources.Length; i++)
        //    {
        //        _waveSources[i] = new WaveSource();
        //        _waveSources[i].LocationInMeters = new PointF(-0.42f, y);
        //        var delta = i * (_d / _waveSources.Length);
        //        var phase = delta * _waveSources[i].Freq / _waveSources[i].Speed * 2 * Math.PI;
        //        _waveSources[i].Phase = -phase;
        //        y += step;
        //        k++;
        //    }
        //}

        private void PhazedArray1()
        {
            var step = 0.023f * 0.707f;
            var d = 0.5f;
            var n = 21;
            _waveSources = new WaveSource[n];
            var y = -step * _waveSources.Length / 2;
            var k = -_waveSources.Length / 2;

            for (var i = 0; i < _waveSources.Length; i++)
            {
                _waveSources[i] = new WaveSource();
                _waveSources[i].LocationInMeters = new PointF(-0.2f, y);
                var cat1 = step * k;
                var hyp = Math.Sqrt(cat1 * cat1 + d * d);
                var delta = hyp - d;
                var phase = delta * _waveSources[i].Freq / _waveSources[i].Speed * 2 * Math.PI;
                _waveSources[i].Phase = -phase;
                y += step;
                k++;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_dbm == null) return;
            _drawer.Draw(_dbm, pictureBox.ClientRectangle, _waveSources, _time);
            pictureBox.Invalidate();
            _time += 0.000002;
            //if (_dStep > 0)
            //{
            //    if (_d >= _dStop)
            //    {
            //        _dStep = -_dStep;
            //    }
            //}
            //else
            //{
            //    if (_d <= _dStart)
            //    {
            //        _dStep = -_dStep;
            //    }
            //}
            //_d += _dStep;
            //PhazedArray2();
        }

        private void pictureBox_Resize(object sender, EventArgs e)
        {
            _dbm = new DirectBitmap(pictureBox.ClientRectangle.Width, pictureBox.ClientRectangle.Height);
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(_dbm.Bitmap, 0, 0);
        }
    }
}