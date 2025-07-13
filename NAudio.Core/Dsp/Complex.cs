namespace NAudio.Dsp
{
    /// <summary>
    /// Type to represent complex number
    /// </summary>
    public struct Complex
    {
        /// <summary>
        /// Real Part
        /// </summary>
        public float X;
        /// <summary>
        /// Imaginary Part
        /// </summary>
        public float Y;
    }

    /// <summary>
    /// Complex (array) struct for a multi-channel signal
    /// </summary>
    public struct ComplexAudio
    {
        float[] _x;
        float[] _y;

        /// <summary>
        /// Gets X value for specific channel
        /// </summary>
        public float GetX(int channel)
        {
            return _x[channel];
        }

        /// <summary>
        /// Gets Y value for specific channel
        /// </summary>
        public float GetY(int channel)
        {
            return _y[channel];
        }

        /// <summary>
        /// Gets the number of channels
        /// </summary>
        public int NumberChannels { get; private set; }

        /// <summary>
        /// Sets the complex value for a specific channel
        /// </summary>
        public void Set(int channel, Complex value)
        {
            _x[channel] = value.X;
            _y[channel] = value.Y;
        }

        /// <summary>
        /// Sets the complex value for a specific channel
        /// </summary>
        public void Set(int channel, float real, float imag)
        {
            _x[channel] = real;
            _y[channel] = imag;
        }

        /// <summary>
        /// Constructs a multi-channel complex structure
        /// </summary>
        /// <param name="numberChannels">Number of channels present in the signal</param>
        public ComplexAudio(int numberChannels)
        {
            _x = new float[numberChannels];
            _y = new float[numberChannels];
            this.NumberChannels = numberChannels;
        }
    }
}
