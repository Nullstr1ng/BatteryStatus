using GalaSoft.MvvmLight;
using System;

namespace BatteryStatus.WPF.Models
{
    public class Model_BatteryDetails : ViewModelBase
    {
        private int _chargeDischargeRate = 0;
        public int ChargeDischargeRate
        {
            get { return _chargeDischargeRate; }
            set
            {
                int v = value;

                Set(nameof(ChargeDischargeRate), ref _chargeDischargeRate, v);

                if (v > this.HighestDischargeRate) this.HighestDischargeRate = v;
                if (this.LowestDischargeRate == -1) this.LowestDischargeRate = value;
                else if (v < this.LowestDischargeRate) this.LowestDischargeRate = v;
            }
        }

        private int _designCapacity = 0;
        public int DesignCapacity
        {
            get { return _designCapacity; }
            set { Set(nameof(DesignCapacity), ref _designCapacity, value); }
        }

        private int? _fullCharge = 0;
        public int? FullCharge
        {
            get { return _fullCharge; }
            set { Set(nameof(FullCharge), ref _fullCharge, value); }
        }

        private int? _remainingCapacity = 0;
        public int? RemainingCapacity
        {
            get { return _remainingCapacity; }
            set { Set(nameof(RemainingCapacity), ref _remainingCapacity, value); }
        }

        private string _batteryStatus = string.Empty;
        public string BatteryStatus
        {
            get { return _batteryStatus; }
            set { Set(nameof(BatteryStatus), ref _batteryStatus, value); }
        }

        private double _remainingCapacityInPercent = 0.0d;
        public double RemainingCapacityInPercent
        {
            get { return _remainingCapacityInPercent; }
            set { Set(nameof(RemainingCapacityInPercent), ref _remainingCapacityInPercent, value); }
        }

        private double _batteryHealthInPercent = 0.0d;
        public double BatteryHealthInPercent
        {
            get { return _batteryHealthInPercent; }
            set { Set(nameof(BatteryHealthInPercent), ref _batteryHealthInPercent, value); }
        }

        private string _chargeDischargeText = "Charge Rate";
        public string ChargeDischargeText
        {
            get { return _chargeDischargeText; }
            set { Set(nameof(ChargeDischargeText), ref _chargeDischargeText, value); }
        }

        private TimeSpan _remainingTime = new TimeSpan();
        public TimeSpan RemainingTime
        {
            get { return _remainingTime; }
            set { Set(nameof(RemainingTime), ref _remainingTime, value); }
        }

        private TimeSpan _TimeOnBatteryStart = new TimeSpan(0);
        public TimeSpan TimeOnBatteryStart
        {
            get { return _TimeOnBatteryStart; }
            set { Set(nameof(TimeOnBatteryStart), ref _TimeOnBatteryStart, value); }
        }

        private TimeSpan _timeOnBattery = new TimeSpan();
        public TimeSpan TimeOnBattery
        {
            get { return _timeOnBattery; }
            set { Set(nameof(TimeOnBattery), ref _timeOnBattery, value); }
        }

        private string _remainingTimeText = string.Empty;
        public string RemainingTimeText
        {
            get { return _remainingTimeText; }
            set { Set(nameof(RemainingTimeText), ref _remainingTimeText, value); }
        }

        private int _highestDischargeRate = 0;
        public int HighestDischargeRate
        {
            get { return _highestDischargeRate; }
            set { Set(nameof(HighestDischargeRate), ref _highestDischargeRate, value); }
        }

        private int _lowestDischargeRate = -1;
        public int LowestDischargeRate
        {
            get { return _lowestDischargeRate; }
            set { Set(nameof(LowestDischargeRate), ref _lowestDischargeRate, value); }
        }

        public void UpdateRemainingCapacityInPercent()
        {
            if (this.FullCharge != null && this.RemainingCapacity != null)
            {
                this.RemainingCapacityInPercent = Math.Round(((double)this.RemainingCapacity.Value / (double)this.FullCharge.Value) * 100, 2);
            }
        }

        public void UpdateBatteryHealth()
        {
            if (this.FullCharge != null && this.DesignCapacity != 0)
            {
                this.BatteryHealthInPercent = Math.Round(((double)this.FullCharge.Value / (double)this.DesignCapacity) * 100, 2);
            }
        }

        private bool _Update = true;
        public bool Update
        {
            get { return _Update; }
            set { Set(nameof(Update), ref _Update, value); }
        }
    }
}
