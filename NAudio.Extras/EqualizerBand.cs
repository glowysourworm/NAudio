using System;

namespace NAudio.Extras
{
    /// <summary>
    /// Equalizer Band
    /// </summary>
    public class EqualizerBand
    {
        /// <summary>
        /// This was added for convenience when processing with the equalizer. There's no code in the demo to
        /// relate the FFT to the BiQuad Equalizer implementation. So, this will have to be a simple solution
        /// for visualizing the EqualizerBand. The running average is calculated during streaming. So, the 
        /// FFT window is lost; but that won't really matter unless you're trying to do something more sensitive.
        /// </summary>
        float[] _levelRunningAverages;
        int[] _counters;
        int _counterMax = 41000;

        float _frequency;
        float _gain;
        float _bandwidth;
        int _channels;

        object _lock = new object();

        public float GetFrequency()
        {
            return _frequency;
        }
        public float GetGain()
        {
            return _gain;
        }
        public float GetBandwidth()
        {
            return _bandwidth;
        }
        public int GetChannels()
        {
            return _channels;
        }

        public float GetLevelRunningAverage(int channel)
        {
            return _levelRunningAverages[channel];
        }

        public EqualizerBand(float frequency, float gain, float bandwidth, int channels)
        {
            if (frequency <= 0)
                throw new ArgumentException("NAudio EqualizerBand must have frequency greater than zero");

            if (gain < 0 || gain > 1)
                throw new ArgumentException("NAudio EqualizerBand must have gain between zero and one");

            if (bandwidth < 0 || bandwidth > 1)
                throw new ArgumentException("NAudio EqualizerBand must have bandwidth between zero and one");

            if (channels <= 0)
                throw new ArgumentException("NAudio EqualizerBand must have positive number of channels");

            _frequency = frequency;
            _gain = gain;
            _bandwidth = bandwidth;
            _channels = channels;

            _levelRunningAverages = new float[channels];
            _counters = new int[channels];

            // This should give an approx. sampling window for the given frequency based on the Nyquist rate for audio.
            _counterMax = 100; // (tried 41000 / frequency)
        }

        public void UpdateParameters(float gain)
        {
            _gain = gain;
        }

        public void UpdateLevelAverage(float sample, int channel)
        {
            // Sample may be negative
            var absSample = Math.Abs(sample);

            if (_channels <= 0)
                throw new Exception("Must set the number of channels for NAudio EqualizerBand");

            if (_counters[channel] >= _counterMax)
            {
                _counters[channel] = 0;
                _levelRunningAverages[channel] = absSample;
            }
            else
            {
                // Moving Average
                _levelRunningAverages[channel] = (absSample + (_counters[channel] * _levelRunningAverages[channel])) / (float)(_counters[channel] + 1);
            }

            _counters[channel]++;
        }
    }
}