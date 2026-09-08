using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace PrismEventJournalDemo.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        //声明接口
        private readonly IRegionManager _regionManager;

        private IRegionNavigationJournal _journal;  //IRegionNavigationJournal: 区域导航日志接口;    保存从导航服务中抓取出来的导航日志实例

        //属性
        private string _title = "Prism Application";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        //构造函数:注入 IRegionManager
        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;

            // 命令实例化，并将执行方法与状态检测挂钩
            NavigateCommand = new DelegateCommand<string>(Navigate);

            BackCommand = new DelegateCommand(GoBack, CanGoBack);

            ForwardCommand = new DelegateCommand(GoForward, CanGoForward);
        }


        //声明命令
        public DelegateCommand<string> NavigateCommand { get; }

        public DelegateCommand BackCommand { get; }

        public DelegateCommand ForwardCommand { get; }



        //执行方法

        private void Navigate(string viewName)
        {
            _regionManager.RequestNavigate("MainRegion", viewName, result =>
            {
                //如果导航成功
                if(result.Result == true)
                {
                    //result:NavigationResult（导航结果） Context:NavigationContext（本次导航的上下文快照）
                    //NavigationService:IRegionNavigationService（驱动本次导航的服务实例）; Journal:IRegionNavigationJournal（本区域绑定的历史记录日志）
                    _journal = result.Context.NavigationService.Journal;

                    //【核心修复点】：导航成功后，必须通知前台按钮重新检测 CanGoBack / CanGoForward 状态
                    RefreshJournalCommands();
                }
            });
        }

        private void GoBack()
        {
            if (_journal != null && _journal.CanGoBack)
            {
                _journal.GoBack();  //从导航记录里往前退一页

                RefreshJournalCommands();
            }
        }

        private bool CanGoBack()
        {
            //有_journal + 前面确实还有历史页面  = 按钮才能点击
            return _journal != null && _journal.CanGoBack;
        }

        private void GoForward()
        {
            if (_journal != null && _journal.CanGoForward)
            {
                _journal.GoForward();

                RefreshJournalCommands();
            }
        }

        private bool CanGoForward()
        {
            //有_journal + 后面确实还有历史页面  = 按钮才能点击
            return _journal != null && _journal.CanGoForward;
        }

        /// <summary>
        /// 刷新按钮状态
        /// </summary>
        private void RefreshJournalCommands()
        {
            //通知按钮重新检查 CanExecute
            BackCommand.RaiseCanExecuteChanged();

            ForwardCommand.RaiseCanExecuteChanged();
        }
    }
}
