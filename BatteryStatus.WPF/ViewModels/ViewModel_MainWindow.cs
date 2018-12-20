using BatteryStatus.WPF.Models;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Windows.Devices.Power;
using Windows.System.Power;

namespace BatteryStatus.WPF.ViewModels
{
    public class ViewModel_MainWindow : ViewModelBase
    {
        #region events

        #endregion

        #region vars
        DispatcherTimer dispatcherTimer = new DispatcherTimer(DispatcherPriority.Normal) { Interval = TimeSpan.FromSeconds(1) };
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

        private Model_BatteryDetails _OldBatteryDetailValues = new Model_BatteryDetails();
        public Model_BatteryDetails OldBatteryDetailValues
        {
            get { return _OldBatteryDetailValues; }
            set { Set(nameof(OldBatteryDetailValues), ref _OldBatteryDetailValues, value); }
        }

        private DateTime _timeOnBattery = DateTime.Now;
        public DateTime TimeOnBattery
        {
            get { return _timeOnBattery; }
            set { Set(nameof(TimeOnBattery), ref _timeOnBattery, value); }
        }

        private DateTime _InititalDateTime = DateTime.Now;
        public DateTime InititalDateTime
        {
            get { return _InititalDateTime; }
            set { Set(nameof(InititalDateTime), ref _InititalDateTime, value); }
        }
        #endregion

        #region commands
        public ICommand Command_ShowAboutMe { get; set; }
        public ICommand Command_ShowBatteryInformaiton { get; set; }
        public ICommand Command_ExitApp { get; set; }
        #endregion

        #region ctors
        public ViewModel_MainWindow()
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
        void Command_ShowAboutMe_Click()
        {
            MessageBox.Show("© Jayson Ragasa - Jara.IO - 2018");
        }

        void Command_ShowBatteryInformaiton_Click()
        {
            MessageBox.Show("Coming Soon.");
        }

        void Command_ExitApp_Click()
        {
            Application.Current.Shutdown();
        }
        #endregion

        #region methods
        void InitCommands()
        {
            if (Command_ShowAboutMe == null) Command_ShowAboutMe = new RelayCommand(Command_ShowAboutMe_Click);
            if (Command_ShowBatteryInformaiton == null) Command_ShowBatteryInformaiton = new RelayCommand(Command_ShowBatteryInformaiton_Click);
            if (Command_ExitApp == null) Command_ExitApp = new RelayCommand(Command_ExitApp_Click);
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

                if(this.BatteryDetails.TimeOnBatteryStart.Ticks == 0)
                {
                    this.BatteryDetails.TimeOnBatteryStart = DateTime.Now.TimeOfDay;
                }
                else
                {
                    this.BatteryDetails.TimeOnBattery = DateTime.Now.TimeOfDay - this.BatteryDetails.TimeOnBatteryStart;
                }

                this.OldBatteryDetailValues.Update = false;
            }
            else
            {
                if (!this.OldBatteryDetailValues.Update)
                {
                    //this.OldBatteryDetailValues = new Model_BatteryDetails()
                    //{
                    //    RemainingTime = this.BatteryDetails.RemainingTime,
                    //    TimeOnBattery = this.BatteryDetails.TimeOnBattery,
                    //    HighestDischargeRate = this.BatteryDetails.HighestDischargeRate,
                    //    LowestDischargeRate = this.BatteryDetails.LowestDischargeRate
                    //};
                    //this.OldBatteryDetailValues = this.BatteryDetails;

                    this.OldBatteryDetailValues.RemainingTime = this.BatteryDetails.RemainingTime;
                    this.OldBatteryDetailValues.TimeOnBattery = this.BatteryDetails.TimeOnBattery;
                    this.OldBatteryDetailValues.HighestDischargeRate = this.BatteryDetails.HighestDischargeRate;
                    this.OldBatteryDetailValues.LowestDischargeRate = this.BatteryDetails.LowestDischargeRate;
                    this.OldBatteryDetailValues.Update = true;
                }
                //this.OldBatteryDetailValues = this.BatteryDetails;

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
        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var rep = Battery.AggregateBattery.GetReport();

            if(rep.RemainingCapacityInMilliwattHours != this.BatteryDetails.RemainingCapacity)
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
