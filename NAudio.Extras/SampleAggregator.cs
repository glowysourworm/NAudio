using System;
using System.Diagnostics;
using NAudio.Dsp;
using NAudio.Wave;

namespace NAudio.Extras
{
    /// <summary>
    /// Demo sample provider that performs FFTs
    /// </summary>
    public class SampleAggregator : ISampleProvider
    {
        Complex[] _FFTResult;

        /// <summary>
        /// Result of FFT
        /// </summary>
        public Complex[] FFTResult
        {
            get
            {
                lock (_lock)
                {
                    return _FFTResult;
                }
            }
        }

        /// <summary>
        /// Provides the wave format for the source sample provider
        /// </summary>
        public WaveFormat WaveFormat 
        {
            get { return _source.WaveFormat; }
        }

        private readonly ISampleProvider _source;
        private readonly int _powerRoot;                // This would be the "m" parameter in the FFT calculation

        object _lock = new object();

        private int _fftPosition;

        /// <summary>
        /// Creates a new SampleAggregator
        /// </summary>
        /// <param name="source">source sample provider</param>
        /// <param name="fftLength">FFT length, must be a power of 2</param>
        public SampleAggregator(ISampleProvider source, int fftLength = 1024)
        {
            if (!IsPowerOfTwo(fftLength))
            {
                throw new ArgumentException("FFT Length must be a power of two");
            }

            _powerRoot = (int)Math.Log(fftLength, 2.0);
            _FFTResult = new Complex[fftLength];
            _source = source;
        }

        static bool IsPowerOfTwo(int x)
        {
            return (x & (x - 1)) == 0;
        }

        private void Add(float value, int channelIndex)
        {
            lock (_lock)
            {
                _FFTResult[_fftPosition].X = (float)(value * FastFourierTransform.HammingWindow(_fftPosition, _FFTResult.Length));
                _FFTResult[_fftPosition].Y = 0;
                _fftPosition++;

                if (_fftPosition >= _FFTResult.Length)
                {
                    _fftPosition = 0;

                    // 1024 = 2^10
                    FastFourierTransform.FFT(true, _powerRoot, _FFTResult);
                }
            }
        }

        /// <summary>
        /// Reads samples from this sample provider
        /// </summary>
        public int Read(float[] buffer, int offset, int count)
        {
            var samplesRead = _source.Read(buffer, offset, count);

            for (int n = 0; n < samplesRead; n += _source.WaveFormat.Channels)
            {
                // We were worried about whether we were not using the channel properly. So, there was 
                // a need to just take a specific channel.
                Add(buffer[n+offset], 0);
            }
            return samplesRead;
        }
    }
}
