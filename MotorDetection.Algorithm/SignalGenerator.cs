using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MotorDetection.Algorithm
{
    public class SignalGenerator
    {
        public enum SignalGeneratorType
        {
            /// <summary>
            ///     Sine wave
            /// </summary>
            Sine,
            /// <summary>
            ///     Square wave
            /// </summary>
            Square,
            /// <summary>
            ///     Triangle Wave
            /// </summary>
            Triangle,
            /// <summary>
            ///     Sawtooth wave
            /// </summary>
            Sawtooth,
        }

        public enum SweepMode
        {
            None,
            Linear,
            Logarithmic,
        }

        public static double[] GenerateWaveform(double sampleRate, double startFreq, double endFreq, double amplitude,
            double phase, SignalGeneratorType signalGeneratorType, SweepMode sweepType, double duration)
        {
            int numSamples = (int)(sampleRate * duration);
            double[] waveform = new double[numSamples];
            double currentFreq = startFreq;
            double freqDelta = 0;

            if (sweepType != SweepMode.None)
            {
                freqDelta = (endFreq - startFreq) / duration;
            }

            for (int i = 0; i < numSamples; i++)
            {
                double t = i / sampleRate;

                // Sweep
                if (sweepType == SweepMode.Linear)
                {
                    currentFreq = startFreq + freqDelta * t;
                }
                else if (sweepType == SweepMode.Logarithmic)
                {
                    currentFreq = startFreq * Math.Pow(endFreq / startFreq, t / duration);
                }

                // Waveform
                switch (signalGeneratorType)
                {
                    case SignalGeneratorType.Sine:
                        waveform[i] = amplitude * Math.Sin(2.0 * Math.PI * currentFreq * t + phase);
                        break;
                    case SignalGeneratorType.Square:
                        waveform[i] = amplitude * Math.Sign(Math.Sin(2.0 * Math.PI * currentFreq * t + phase));
                        break;
                    case SignalGeneratorType.Triangle:
                        double tri_t = (2.0 * Math.PI * currentFreq * t + phase) % (2.0 * Math.PI);
                        waveform[i] = amplitude * (1.0 - 4.0 * Math.Abs(tri_t - Math.PI) / Math.PI);
                        break;
                    case SignalGeneratorType.Sawtooth:
                        double saw_t = (2.0 * Math.PI * currentFreq * t + phase) % (2.0 * Math.PI);
                        waveform[i] = amplitude * (2.0 * saw_t / Math.PI - 1.0);
                        break;
                }
            }

            return waveform;
        }
    }
}
