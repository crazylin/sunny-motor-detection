using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using MathNet.Filtering;
using MathNet.Filtering.FIR;
using MathNet.Numerics;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

namespace MotorDetection.Algorithm
{
    public static class SpectrumAnalysis
    {
        /// <summary>
        /// 时域转频谱  傅立叶变换
        /// </summary>
        /// <param name="vibData"></param>
        /// <param name="calibration"></param>
        /// <param name="sampleNumber"></param>
        /// <param name="ignoreDc"></param>
        /// <param name="foldingFactor"></param>
        /// <param name="windowFunction"></param>
        /// <param name="averageMode"></param>
        /// <returns></returns>
        public static double[] GetSpectrum(double[] vibData, double calibration, int sampleNumber,double sampleRate,
            bool ignoreDc,
            double foldingFactor,
            WindowFunction windowFunction, AverageMode averageMode,SpectrumType spectrumType)
        {
            List<Complex[]> vs = new List<Complex[]>();

            var windowArray = GetWindow(windowFunction, sampleNumber);
            int halfSize = sampleNumber / 2;


            var factorStep = sampleNumber - (int)(sampleNumber * foldingFactor);


            var avgCount = 0;
            var amplitudeSpectrum = new Complex[halfSize];

            var tempV = new double[sampleNumber];
            for (int i = 0; i < vibData.Length; i += factorStep)
            {
                var startPos = i;
                if (i + sampleNumber > vibData.Length)
                    startPos = vibData.Length - sampleNumber;
                //取一段数据
                Array.Copy(vibData, startPos, tempV, 0, sampleNumber);
                if (ignoreDc)
                    tempV.RemoveDc();

                //加窗函数
                Complex[] cdv = new Complex[tempV.Length];
                for (int index = 0; index < cdv.Length; ++index)
                    cdv[index] = new Complex(tempV[index] * calibration * windowArray[index], 0);

                Fourier.Forward(cdv, FourierOptions.Matlab);


                vs.Add(cdv);

                switch (averageMode)
                {
                    case AverageMode.NoAverage:
                    case AverageMode.LinearAverage:
                        if (avgCount == 0)
                        {
                            Array.Copy(cdv, amplitudeSpectrum, halfSize);
                            avgCount = 1;
                        }
                        else
                        {
                            avgCount++;
                            var N = avgCount;
                            var A = 1.0 / N;
                            for (var index = 0; index < halfSize; index++)
                                amplitudeSpectrum[index] = cdv[index] * A + amplitudeSpectrum[index] * (1 - A);
                        }

                        break;
                    case AverageMode.PeakHolding:
                        if (avgCount == 0)
                        {
                            Array.Copy(cdv, amplitudeSpectrum, halfSize);
                            avgCount = 1;
                        }
                        else
                        {
                            for (var index = 0; index < halfSize; index++)
                                amplitudeSpectrum[index] = amplitudeSpectrum[index].Magnitude > cdv[index].Magnitude
                                    ? amplitudeSpectrum[index]
                                    : cdv[index];
                        }

                        break;
                    case AverageMode.ExponentAverage:
                        if (avgCount == 0)
                        {
                            Array.Copy(cdv, amplitudeSpectrum, halfSize);
                            avgCount = (int)Math.Ceiling(vibData.Length / (double)factorStep);
                        }
                        else
                        {
                            var N = avgCount;
                            var A = 1.0 - N;
                            for (var index = 0; index < amplitudeSpectrum.Length; index++)
                                amplitudeSpectrum[i] = cdv[i] * A + amplitudeSpectrum[i] * (1 - A);
                        }

                        break;
                }

            }
            switch (spectrumType)
            {
                case SpectrumType.Amplitude:

                    return amplitudeSpectrum.Amplitude();
                case SpectrumType.Rms:
                    return amplitudeSpectrum.RMS();
                case SpectrumType.AutoPower:
                    return amplitudeSpectrum.AutoPower(windowFunction);
                case SpectrumType.AutoPowerDensity:
                    return amplitudeSpectrum.AutoPowerDensity(windowFunction, sampleRate);
            }
            return null;
        }

        /// <summary>
        /// 幅值谱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double[] Amplitude(this Complex[] data)
        {
            var amplitudes = new double[data.Length];
            var length = amplitudes.Length;
            for (int i = 0; i < length; ++i)
            {
                amplitudes[i] = (data[i] / length).Magnitude;
            }

            amplitudes[0] = amplitudes[0] * 0.5;
            return amplitudes;
        }

        /// <summary>
        /// 均方根值谱
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double[] RMS(this Complex[] data)
        {
            return data.Amplitude().Select(d => d * Math.Sqrt(0.5)).ToArray();
        }

        /// <summary>
        /// 功率谱
        /// </summary>
        /// <param name="data"></param>
        /// <param name="windowFunction"></param>
        /// <returns></returns>
        public static double[] AutoPower(this Complex[] data, WindowFunction windowFunction)
        {
            double noise = GetNoiseFactor(windowFunction);
            return data.RMS().Select(r => Math.Pow(r, 2) / noise).ToArray();
        }

        /// <summary>
        /// 功率谱密度
        /// </summary>
        /// <param name="data"></param>
        /// <param name="windowFunction"></param>
        /// <param name="sampleRate"></param>
        /// <returns></returns>
        public static double[] AutoPowerDensity(this Complex[] data, WindowFunction windowFunction, double sampleRate)
        {
            var f = 1 / sampleRate;
            return data.AutoPower(windowFunction).Select(d => d * f).ToArray();
        }

        private static double GetNoiseFactor(WindowFunction type)
        {
            double noise = 1.0;
            switch (type)
            {
                case WindowFunction.Rectangle:
                    noise = 1.0;
                    break;
                case WindowFunction.Hann:
                    noise = 1.5014; //1.633;
                    break;
                case WindowFunction.Hamming:
                    noise = 1.3637; //1.586;
                    break;
                case WindowFunction.Blackman:
                    noise = 1.812;
                    break;
                case WindowFunction.BlackmanHarris:
                    noise = 1.71;
                    break;
                case WindowFunction.Triangular:
                    noise = 1.333; //1.732;
                    break;
                case WindowFunction.FlatTop:
                    noise = 3.774; //1.069;
                    break;
                default:
                    noise = 1.0;
                    break;
            }

            return noise;
        }

        /// <summary>
        /// 计算相位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length">data.Length/2</param>
        /// <param name="Phase"></param>
        public static double[] Phase(this Complex[] data, int length = 0)
        {
            if (length == 0)
                length = data.Length;
            var phase = new double[length];
            for (int i = 0; i < length; ++i)
            {
                phase[i] = data[i].Phase;
            }

            return phase;
        }

        /// <summary>
        /// 计算相位
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length">data.Length/2</param>
        /// <param name="Phase"></param>
        public static double[] Phase(this Complex[] data, int length = 0, int baseAngle = 180)
        {
            if (length == 0)
                length = data.Length;
            var phase = new double[length];
            for (int i = 0; i < length; ++i)
            {
                phase[i] = data[i].Phase * baseAngle / Math.PI; //360/2π  弧度转角度
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (phase[i] == baseAngle)
                    phase[i] = 0;
            }

            return phase;
        }

        /// <summary>
        /// 计算实部
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length">data.Length</param>
        /// <param name="Phase"></param>
        public static double[] Real(this Complex[] data, int length = 0)
        {
            if (length == 0)
                length = data.Length;
            var real = new double[length];
            for (int i = 0; i < length; ++i)
            {
                real[i] = data[i].Real;
            }

            return real;
        }

        /// <summary>
        /// 计算虚部
        /// </summary>
        /// <param name="data"></param>
        /// <param name="length">data.Length/2</param>
        /// <param name="Phase"></param>
        public static double[] Imaginary(this Complex[] data, int length = 0)
        {
            if (length == 0)
                length = data.Length;
            var imaginary = new double[length];
            for (int i = 0; i < length; ++i)
            {
                imaginary[i] = data[i].Imaginary;
            }

            return imaginary;
        }

        public static Complex[] Conjugate(this Complex[] data, int length = 0)
        {
            if (length == 0)
                length = data.Length;
            for (int i = 0; i < length; ++i)
            {
                data[i] = data[i].Conjugate();
            }

            return data;
        }

        public static void RemoveDc(this double[] d)
        {
            var avg = d.Sum() / d.Length;
            for (var i = 0; i < d.Length; i++)
            {
                d[i] = d[i] - avg;
            }
        }

        public static double[] GetWindow(WindowFunction windowFunction, int len)
        {
            // add coherent gain - http://www.ni.com/white-paper/4278/en
            var data = new double[len];
            switch (windowFunction)
            {
                case WindowFunction.Hamming:
                {
                    var factor = 1.852;
                    var tempData = MathNet.Numerics.Window.Hamming(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = tempData[i] * factor;
                    }
                }
                    break;
                case WindowFunction.Hann:
                {
                    double factor = 2.0;
                    var tempData = MathNet.Numerics.Window.Hann(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = tempData[i] * factor;
                    }
                }
                    break;
                case WindowFunction.Blackman:
                {
                    double factor = 2.3809524;
                    var tempData = MathNet.Numerics.Window.Blackman(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = tempData[i] * factor;
                    }
                }
                    break;
                case WindowFunction.BlackmanHarris:
                {
                    double factor = 2.7874564;
                    var tempData = MathNet.Numerics.Window.BlackmanHarris(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = tempData[i] * factor;
                    }
                }
                    break;
                case WindowFunction.FlatTop:
                    return MathNet.Numerics.Window.FlatTop(len);
                case WindowFunction.Triangular:
                {
                    double factor = 2.0;
                    var tempData = MathNet.Numerics.Window.Triangular(len);
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = tempData[i] * factor;
                    }
                }
                    break;
                case WindowFunction.Rectangle:
                default:
                    for (int i = 0; i < data.Length; i++)
                    {
                        data[i] = 1;
                    }

                    break;

            }

            return data;
        }


        public static double[] ApplyFilter(FilterType type, double[] inData, double sampleRate, double lowPass,
            double highPass,
            double firstPass, double secondPass)
        {
            switch (type)
            {
                case FilterType.LowPass:
                    return OnlineFilter.CreateLowpass(ImpulseResponse.Finite, sampleRate, lowPass)
                        .ProcessSamples(inData);
                case FilterType.HighPass:
                    return OnlineFilter.CreateHighpass(ImpulseResponse.Finite, sampleRate, lowPass)
                        .ProcessSamples(inData);
                case FilterType.BandPass:
                    return OnlineFilter.CreateBandpass(ImpulseResponse.Finite, sampleRate, firstPass, secondPass)
                        .ProcessSamples(inData);
                case FilterType.BandStop:
                    return OnlineFilter.CreateBandstop(ImpulseResponse.Finite, sampleRate, firstPass, secondPass)
                        .ProcessSamples(inData);
            }

            return inData;
        }

        /// <summary>
        /// 积分
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sampleRate"></param>
        /// <param name="preIntegral"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public static double[] GetIntegralData(double[] data, double sampleRate, double preIntegral,
            IntegralDifferentialMethod method = IntegralDifferentialMethod.TRAPEZOID)
        {
            var resultData = new double[data.Length];
            resultData[0] = preIntegral;
            switch (method)
            {
                case IntegralDifferentialMethod.TRAPEZOID:
                {
                    for (int i = 1; i < data.Length; i++)
                    {
                        resultData[i] = resultData[i - 1] + 0.5 * (data[i] + data[i - 1]) / sampleRate;
                    }
                }
                    break;
                case IntegralDifferentialMethod.SIMPSON:
                {
                    resultData[1] = preIntegral;
                    double onethird = 1.0 / 3.0;
                    for (int i = 2; i < data.Length; i++)
                    {
                        resultData[i] = resultData[i - 1] +
                                        onethird * (data[i] + 4 * data[i - 1] + data[i - 2]) / sampleRate;
                    }
                }
                    break;
            }

            return resultData;
        }

        /// <summary>
        /// 微分
        /// </summary>
        /// <param name="fftData"></param>
        /// <param name="sampleRate"></param>
        /// <returns></returns>
        public static double[] GetDifferentialData(double[] fftData, double sampleRate)
        {
            var resultData = new double[fftData.Length];
            int i;
            for (i = 0; i < fftData.Length - 1; i++)
            {
                resultData[i] = (fftData[i + 1] - fftData[i]) * sampleRate;
            }

            resultData[i] = resultData[i - 1]; // the end tail

            return resultData;
        }

        /// <summary>
        /// 微积分转换
        /// </summary>
        /// <param name="data"></param>
        /// <param name="calibration"></param>
        /// <param name="calculusType"></param>
        /// <param name="sampleRate"></param>
        /// <returns></returns>
        public static double[] GetCalculusTypeTimeData(double[] data, CalculusType calculusType,
            double sampleRate)
        {
            var retData = new double[data.Length];
            switch (calculusType)
            {
                case CalculusType.None:
                    break;
                case CalculusType.OneIntegral:
                    for (int j = 0; j < 1; j++)
                    {
                        retData =
                            GetIntegralData(retData,
                                sampleRate, 0);
                    }

                    break;

                case CalculusType.TwoIntegral:
                    for (int j = 0; j < 2; j++)
                    {
                        retData =
                            GetIntegralData(retData,
                                sampleRate, 0);
                    }

                    break;

                case CalculusType.OneDifferential:
                    for (int j = 0; j < 1; j++)
                    {
                        retData =
                            GetDifferentialData(retData,
                                sampleRate);
                    }

                    break;

                case CalculusType.TwoDifferential:
                    for (int j = 0; j < 2; j++)
                    {
                        retData =
                            GetDifferentialData(retData,
                                sampleRate);
                    }

                    break;
            }

            return retData;
        }
        //public static void Filter(FilterType type, Complex[] inData, out Complex[] outData, double sampleRate,
        //    double firstPass = 0,
        //    double secondPass = 0,double attenuation = -1)
        //{
        //    if (attenuation < 0)
        //    {
        //        //0~140 Db
        //        attenuation = Math.Pow(10.0, 140 / 20.0);
        //    }

        //    switch (type)
        //    {
        //        case FilterType.LowPass:
        //            LowPass(inData, firstPass, sampleRate, out outData, attenuation);
        //            return;
        //        case FilterType.HighPass:
        //            HighPass(inData, firstPass, sampleRate, out outData, attenuation);
        //            return;
        //        case FilterType.BandPass:
        //            BandPass(inData, firstPass, secondPass, sampleRate, out outData, attenuation);
        //            return;
        //        //case FilterType.AdvancedFilter:
        //        //    MultiBandPass(inData, sampleRate, bandFilters, isBandStop, out outData);
        //        //    return;
        //    }

        //    outData = inData;

        //}

        //public static void LowPass(Complex[] inData, double lowPass, double sampleRate, out Complex[] outData,
        //    double attenuation)
        //{
        //    double cutoffIndex = inData.Length * lowPass / sampleRate;
        //    int halfSize = inData.Length / 2;

        //    outData = new Complex[inData.Length];
        //    int k = inData.Length - 1;
        //    for (int i = 0; i < halfSize; ++i)
        //    {
        //        if (i > cutoffIndex)
        //        {
        //            outData[i] = new Complex(inData[i].Real / attenuation, inData[i].Imaginary / attenuation);
        //            outData[k] = new Complex(inData[k].Real / attenuation, inData[k].Imaginary / attenuation);
        //        }
        //        else
        //        {
        //            outData[i] = new Complex(inData[i].Real, inData[i].Imaginary);
        //            outData[k] = new Complex(inData[k].Real, inData[k].Imaginary);
        //        }

        //        --k;
        //    }
        //}

        //public static void HighPass(Complex[] inData, double highPass, double sampleRate, out Complex[] outData,
        //    double attenuation)
        //{
        //    double cutoffIndex = inData.Length * highPass / (double)sampleRate;
        //    int halfSize = inData.Length / 2;

        //    outData = new Complex[inData.Length];
        //    int k = inData.Length - 1;
        //    for (int i = 0; i < halfSize; ++i)
        //    {
        //        if (i < cutoffIndex)
        //        {
        //            outData[i] = new Complex(inData[i].Real / attenuation, inData[i].Imaginary / attenuation);
        //            outData[k] = new Complex(inData[k].Real / attenuation, inData[k].Imaginary / attenuation);

        //        }
        //        else
        //        {
        //            outData[i] = new Complex(inData[i].Real, inData[i].Imaginary);
        //            outData[k] = new Complex(inData[k].Real, inData[k].Imaginary);
        //        }

        //        --k;
        //    }
        //}

        //public static void BandPass(Complex[] inData, double firstPass, double secondPass, double sampleRate,
        //    out Complex[] outData,
        //    double attenuation)
        //{
        //    double lowCutoffIdx = inData.Length * firstPass / sampleRate;
        //    double highCutoffIdx = inData.Length * secondPass / sampleRate;
        //    int halfSize = inData.Length / 2;

        //    outData = new Complex[inData.Length];
        //    int k = inData.Length - 1;
        //    for (int i = 0; i < halfSize; ++i)
        //    {
        //        if (i < lowCutoffIdx || i > highCutoffIdx)
        //        {
        //            outData[i] = new Complex(inData[i].Real / attenuation, inData[i].Imaginary / attenuation);
        //            outData[k] = new Complex(inData[k].Real / attenuation, inData[k].Imaginary / attenuation);
        //        }
        //        else
        //        {
        //            outData[i] = new Complex(inData[i].Real, inData[i].Imaginary);
        //            outData[k] = new Complex(inData[k].Real, inData[k].Imaginary);
        //        }

        //        --k;
        //    }
        //}
    }
}
