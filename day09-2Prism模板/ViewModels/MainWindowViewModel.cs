using Prism.Mvvm;

namespace day09_2Prism模板.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "傻龙宝宝吹泡泡";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel()
        {

        }
    }
}
