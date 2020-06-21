using System;
using NAudio.Dsp;

namespace MediaPlayer
{
    class Visualizer
    {
        private Complex[] channelData;
        private int bufferSize;
        private int binaryExponentitation;
        private int channelDataPosition;

        public float LeftMaxVolume { get; private set; }

        public float LeftMinVolume { get; private set; }

        public float RightMaxVolume { get; private set; }

        public float RightMinVolume { get; private set; }

        public Visualizer(int bufferSize)
        {
            this.bufferSize = bufferSize;
            binaryExponentitation = (int)Math.Log(bufferSize, 2);
            channelData = new Complex[bufferSize];
        }

        public void Clear()
        {
            LeftMaxVolume = float.MinValue;
            RightMaxVolume = float.MinValue;
            LeftMinVolume = float.MaxValue;
            RightMinVolume = float.MaxValue;
            channelDataPosition = 0;
        }
                
        public void Add(float leftValue, float rightValue)
        {
            if (channelDataPosition == 0)
            {
                LeftMaxVolume = float.MinValue;
                RightMaxVolume = float.MinValue;
                LeftMinVolume = float.MaxValue;
                RightMinVolume = float.MaxValue;
            }

            // Maakt het data kanaal stereo
            channelData[channelDataPosition].X = (leftValue + rightValue) / 2.0f;
            channelData[channelDataPosition].Y = 0;
            channelDataPosition++;

            LeftMaxVolume = Math.Max(LeftMaxVolume, leftValue);
            LeftMinVolume = Math.Min(LeftMinVolume, leftValue);
            RightMaxVolume = Math.Max(RightMaxVolume, rightValue);
            RightMinVolume = Math.Min(RightMinVolume, rightValue);

            if (channelDataPosition >= channelData.Length)
            {
                channelDataPosition = 0;
            }
        }
       
        public void GetFFTResults(float[] fftBuffer)
        {
            Complex[] channelDataClone = new Complex[bufferSize];
            channelData.CopyTo(channelDataClone, 0);
            FastFourierTransform.FFT(true, binaryExponentitation, channelDataClone);
            for (int i = 0; i < channelDataClone.Length / 2; i++)
            {
                // Berekening voor FFT result.
                fftBuffer[i] = (float)Math.Sqrt(channelDataClone[i].X * channelDataClone[i].X + channelDataClone[i].Y * channelDataClone[i].Y);
            }
        }

    }
}
