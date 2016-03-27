using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using HrtzCraft.Extensions;
using HrtzCraft.Interfaces;
using HrtzCraft.Relays;

namespace HrtzCraft.ViewModels
{
    public class NavigationViewModel : ObservableObject
    {
        #region Constructor

        public NavigationViewModel()
        {
            InitializePageViewModels();
        }

        private void InitializePageViewModels()
        {
            PageViewModelsCollection.Add(new ConsoleViewModel());
            PageViewModelsCollection.Add(new PluginsViewModel());
            PageViewModelsCollection.Add(new BuildToolsViewModel());
            PageViewModelsCollection.Add(new ServerConfigViewModel());

            if (PageViewModelsCollection.Count > 0)
                CurrentPageViewModel = PageViewModelsCollection[0];
        }

        #endregion

        #region Private Fields

        private IPageViewModel _currentPageViewModel;
        private ObservableCollection<IPageViewModel> _pageViewModelsCollection;
        private ICommand _commandChangePage;

        #endregion

        #region Public Fields

        public ObservableCollection<IPageViewModel> PageViewModelsCollection
            => _pageViewModelsCollection ?? (_pageViewModelsCollection = new ObservableCollection<IPageViewModel>());

        #endregion

        #region Properties

        public IPageViewModel CurrentPageViewModel
        {
            get { return _currentPageViewModel; }
            set
            {
                if (value == _currentPageViewModel) return;
                _currentPageViewModel = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand CommandChangePage
        {
            get
            {
                return _commandChangePage ??
                       (_commandChangePage =
                           new RelayCommand(p => Execute_ChangePage(p as IPageViewModel), p => p is IPageViewModel));
            }
        }

        #endregion

        #region Methods

        private void Execute_ChangePage(IPageViewModel viewModel)
        {
            if (!PageViewModelsCollection.Contains(viewModel))
                PageViewModelsCollection.Add(viewModel);

            CurrentPageViewModel = PageViewModelsCollection.FirstOrDefault(vm => vm == viewModel);
        }

        #endregion
    }
}
