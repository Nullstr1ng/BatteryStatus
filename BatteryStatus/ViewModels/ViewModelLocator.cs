using GalaSoft.MvvmLight.Ioc;

namespace BatteryStatus.ViewModels
{
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            SimpleIoc.Default.Register<ViewModel_MainPage>();
        }

        public ViewModel_MainPage Main
        {
            get { return SimpleIoc.Default.GetInstance<ViewModel_MainPage>(); }
        }
    }
}
