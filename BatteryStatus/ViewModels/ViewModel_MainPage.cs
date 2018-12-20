using BatteryStatus.Models;
using GalaSoft.MvvmLight;
using Windows.Devices.Power;
using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.System.Power;
using BatteryStatus.WPF.ViewModels;
using GalaSoft.MvvmLight.Ioc;

namespace BatteryStatus.ViewModels
{
    public class ViewModel_MainPage : ViewModelBase
    {
        #region events

        #endregion

        #region vars
        DispatcherTimer dispatcherTimer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
        #endregion

        #region properties
        public ViewModel_Chart Chart
        {
            get { return SimpleIoc.Default.GetInstance<ViewModel_Chart>(); }
        }

        private Model_BatteryDetails _batteryDetails = new Model_BatteryDetails();
        public Model_BatteryDetails BatteryDetails
        {
            get { return _batteryDetails; }
            set { Set(nameof(BatteryDetails), ref _batteryDetails, value); }
        }

        private DateTime _timeOnBattery = DateTime.Now;
        public DateTime TimeOnBattery
        {
            get { return _timeOnBattery; }
            set { Set(nameof(TimeOnBattery), ref _timeOnBattery, value); }
        }
        #endregion

        #region commands

        #endregion

        #region ctors
        public ViewModel_MainPage()
        {
            InitCommands();

            // used only in UWP & WPF
            // or anything that supports design time updates
            if (base.IsInDesignMode)
            {
                DesignData();
            }
            else
            {
                RuntimeData();
            }
        }
        #endregion

        #region command methods

        #endregion

        #region methods
        void InitCommands()
        {

        }

        void DesignData()
        {
            this.BatteryDetails.BatteryStatus = "Discharging";
            this.BatteryDetails.ChargeDischargeRate = 22800;
            this.BatteryDetails.DesignCapacity = 48994;
            this.BatteryDetails.FullCharge = 45858;
            this.BatteryDetails.RemainingCapacity = 33227;
            this.BatteryDetails.RemainingCapacityInPercent = 64.5;
            this.BatteryDetails.BatteryHealthInPercent = 93.5;
        }

        void RuntimeData()
        {
            InitEvents();

            var aggBat = Battery.AggregateBattery;
            UpdateAll(aggBat.GetReport());
        }

        void InitEvents()
        {
            this.dispatcherTimer.Tick += DispatcherTimer_Tick;
            this.dispatcherTimer.Start();
            //Battery.AggregateBattery.ReportUpdated += AggregateBattery_ReportUpdated;
            //Windows.System.Power.PowerManager.RemainingDischargeTimeChanged += PowerManager_RemainingDischargeTimeChanged;
        }

        void UpdateAll(BatteryReport rep)
        {
            Update(rep);
            UpdateRemainingDischargeTime();

            Chart.DischargeRateSeries.Values.Add(this.BatteryDetails.ChargeDischargeRate);
            Chart.RemainingCapacity.Values.Add(this.BatteryDetails.RemainingCapacity);
        }

        void Update(BatteryReport rep)
        {
            this.BatteryDetails.BatteryStatus = rep.Status.ToString();
            this.BatteryDetails.RemainingCapacity = rep.RemainingCapacityInMilliwattHours;
            this.BatteryDetails.ChargeDischargeRate = Math.Abs(rep.ChargeRateInMilliwatts.Value);
            this.BatteryDetails.DesignCapacity = rep.DesignCapacityInMilliwattHours.Value;
            this.BatteryDetails.FullCharge = rep.FullChargeCapacityInMilliwattHours;

            if (rep.Status == Windows.System.Power.BatteryStatus.Discharging)
            {
                this.BatteryDetails.ChargeDischargeText = "Discharge Rate";

                if (this.BatteryDetails.TimeOnBatteryStart.Ticks == 0)
                {
                    this.BatteryDetails.TimeOnBatteryStart = DateTime.Now.TimeOfDay;
                }
                else
                {
                    this.BatteryDetails.TimeOnBattery = DateTime.Now.TimeOfDay - this.BatteryDetails.TimeOnBatteryStart;
                }
            }
            else
            {
                this.BatteryDetails.ChargeDischargeText = "Charge Rate";

                this.BatteryDetails.TimeOnBatteryStart = TimeSpan.FromTicks(0);
                this.BatteryDetails.TimeOnBattery = TimeSpan.FromTicks(0);
                this.BatteryDetails.HighestDischargeRate = 0;
                this.BatteryDetails.LowestDischargeRate = 0;
            }

            UpdateRemainingDischargeTime();

            this.BatteryDetails.UpdateBatteryHealth();
            this.BatteryDetails.UpdateRemainingCapacityInPercent();
        }

        void UpdateRemainingDischargeTime()
        {
            this.BatteryDetails.RemainingTime = PowerManager.RemainingDischargeTime;
            TimeSpan remaining = PowerManager.RemainingDischargeTime;
            this.BatteryDetails.RemainingTimeText = $"{remaining.Hours}H {remaining.Minutes}m {remaining.Seconds}s";
        }
        #endregion

        #region event subscriptions
        private void DispatcherTimer_Tick(object sender, object e)
        {
            var rep = Battery.AggregateBattery.GetReport();

            if (rep.RemainingCapacityInMilliwattHours != this.BatteryDetails.RemainingCapacity)
            {
                UpdateAll(rep);
            }
        }

        private void AggregateBattery_ReportUpdated(Battery bat, object args)
        {
            //if (Window.Current == null) return;

            //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    Update(bat.GetReport());
            //});
        }

        private void PowerManager_RemainingDischargeTimeChanged(object sender, object e)
        {
            //if (Window.Current == null) return;

            //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    UpdateRemainingDischargeTime();
            //});
        }
        #endregion
    }
}
