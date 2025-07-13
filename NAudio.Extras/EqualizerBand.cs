namespace NAudio.Extras
{
    /// <summary>
    /// Equalizer Band
    /// </summary>
    public class EqualizerBand
    {
        /// <summary>
        /// Frequency
        /// </summary>
        public float Frequency { get; set; }
        /// <summary>
        /// Gain
        /// </summary>
        public float Gain { get; set; }
        /// <summary>
        /// Bandwidth
        /// </summary>
        public float Bandwidth { get; set; }

        public EqualizerBand()
        { }
        public EqualizerBand(float frequency, float gain, float bandwidth)
        {
            this.Frequency = frequency;
            this.Gain = gain;
            this.Bandwidth = bandwidth;
        }
    }
}