using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Gemini.Framework.Services;

namespace Gemini.Modules.Shell.ViewModels
{
    [Export(typeof(BusyIndicatorModel))]
    public class BusyIndicatorModel:PropertyChangedBase
    {
        private bool _isBusy = false;
        private string _busyMessage = string.Empty;
        private double _minPercentage = 0;
        private double _percentage = 50;
        private double _maxPercentage = 100;
        private bool _showPercentage = false;
        private bool _showButton = false;
        private string _tip = "提示：";

        public bool IsBusy
        {
            set
            {
                Set(ref _isBusy, value);
            }
            get => _isBusy;
        }

        public string BusyMessage
        {
            set => Set(ref _busyMessage, value);
            get => _busyMessage;
        }

        public double MinPercentage
        {
            set => Set(ref _minPercentage, value);
            get => _minPercentage;
        }

        public double Percentage
        {
            set => Set(ref _percentage, value);
            get => _percentage;
        }

        public double MaxPercentage
        {
            set => Set(ref _maxPercentage, value);
            get => _maxPercentage;
        }

        public bool ShowPercentage
        {
            set => Set(ref _showPercentage, value);
            get => _showPercentage;
        }

        public bool ShowButton
        {
            set => Set(ref _showButton, value);
            get => _showButton;
        }

        public string Tip
        {
            set => Set(ref _tip, value);
            get => _tip;
        }

        public void Pause()
        {

        }

        public void Cancel()
        {

        }
    }
}
