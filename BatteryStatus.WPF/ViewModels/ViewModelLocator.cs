using GalaSoft.MvvmLight.Ioc;

namespace BatteryStatus.WPF.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ViewModel_MainWindow>();
            SimpleIoc.Default.Register<ViewModel_Chart>();
        }

        public ViewModel_MainWindow Main
        {
            get { return SimpleIoc.Default.GetInstance<ViewModel_MainWindow>(); }
        }

        public ViewModel_Chart Chart
        {
            get { return SimpleIoc.Default.GetInstance<ViewModel_Chart>(); }
        }
    }
}
