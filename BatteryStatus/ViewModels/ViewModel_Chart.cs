using GalaSoft.MvvmLight;
using LiveCharts;
using LiveCharts.Uwp;
using System;
using System.Collections.ObjectModel;

namespace BatteryStatus.ViewModels
{
    public class ViewModel_Chart : ViewModelBase
    {
        #region events

        #endregion

        #region vars

        #endregion

        #region properties
        private ObservableCollection<string> _YLabels = new ObservableCollection<string>();
        public ObservableCollection<string> YLabels
        {
            get { return _YLabels; }
            set { Set(nameof(YLabels), ref _YLabels, value); }
        }

        private ObservableCollection<string> _XLabels = new ObservableCollection<string>();
        public ObservableCollection<string> XLabels
        {
            get { return _XLabels; }
            set { Set(nameof(XLabels), ref _XLabels, value); }
        }

        private SeriesCollection _Series = new SeriesCollection();
        public SeriesCollection Series
        {
            get { return _Series; }
            set { Set(nameof(Series), ref _Series, value); }
        }

        private LineSeries _dischargeRateSeries = new LineSeries();
        public LineSeries DischargeRateSeries
        {
            get { return _dischargeRateSeries; }
            set { Set(nameof(DischargeRateSeries), ref _dischargeRateSeries, value); }
        }

        private LineSeries _remainingCapacity = new LineSeries();
        public LineSeries RemainingCapacity
        {
            get { return _remainingCapacity; }
            set { Set(nameof(RemainingCapacity), ref _remainingCapacity, value); }
        }

        private DateTime _inititalDateTime = DateTime.Now;
        public DateTime InititalDateTime
        {
            get { return _inititalDateTime; }
            set { Set(nameof(InititalDateTime), ref _inititalDateTime, value); }
        }
        #endregion

        #region commands

        #endregion

        #region ctors
        public ViewModel_Chart()
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

        }

        void RuntimeData()
        {
            for (int i = 100; i >= 0; i -= 10)
            {
                YLabels.Add(i.ToString());
            }

            DischargeRateSeries.Title = "Discharge Rate";
            DischargeRateSeries.Values = new ChartValues<int>();

            RemainingCapacity.Title = "Remaining Capacity";
            RemainingCapacity.Values = new ChartValues<int>();

            Series.Add(RemainingCapacity);
            Series.Add(DischargeRateSeries);
        }
        #endregion
    }
}
