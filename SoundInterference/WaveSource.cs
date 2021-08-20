using System;
using System.Drawing;

namespace SoundInterference
{
    internal class WaveSource
    {
        private float _freq = 15000.0f;
        private float _speed = 345f;
        public double Phase { get; set; } = 0f;
        private double _omega;

        public PointF LocationInMeters { get; set; } = new PointF(0, 0);
        private double _waveLength;

        public float Freq
        {
            get => _freq;
            set
            {
                _freq = value;
                CalcWaveLength();
                CalcOmega();
            }
        }

        public float Speed
        {
            get => _speed;
            set
            {
                _speed = value;
                CalcWaveLength();
            }
        }

        public WaveSource()
        {
            CalcWaveLength();
            CalcOmega();
        }


        private void CalcWaveLength()
        {
            _waveLength = _speed / _freq;
        }

        private void CalcOmega()
        {
            _omega = 2 * Math.PI * _freq;
        }

        public double GetAmplitude(double time, PointF targetPointInMeters)
        {
            var cathetusX = targetPointInMeters.X - LocationInMeters.X;
            var cathetusY = targetPointInMeters.Y - LocationInMeters.Y;
            var hypotenuse = Math.Sqrt(cathetusX * cathetusX + cathetusY * cathetusY);
            var phase = Phase + 2 * Math.PI * hypotenuse / _waveLength;
            var attenuation =  0.1 > hypotenuse ? 1.0 : 0.1 / hypotenuse;
            var ampl = Math.Sin(_omega * -time + phase) * attenuation;
            return ampl;
        }
    }
}
