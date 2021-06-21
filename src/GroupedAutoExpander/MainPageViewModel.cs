using GroupedAutoExpander.Models;
using System.Collections.ObjectModel;

namespace GroupedAutoExpander
{
    public class MainPageViewModel : BasePageViewModel
    {
        private ObservableCollection<GroupedAutoExpanderListItem> _Headers { get; set; }

        public ObservableCollection<GroupedAutoExpanderListItem> Headers
        {
            get => _Headers;
            set
            {
                if (_Headers == value) return;
                _Headers = value;
                OnPropertyChanged(nameof(Headers));
                RaisePropertyChanged(nameof(Headers));
            }
        }

        public MainPageViewModel()
        {
            Headers = new ObservableCollection<GroupedAutoExpanderListItem>()
            {
                new GroupedAutoExpanderListItem { ItemID = "Header 1" },
                new GroupedAutoExpanderListItem { ItemID = "Header 2" },
                new GroupedAutoExpanderListItem { ItemID = "Header 3" },
                new GroupedAutoExpanderListItem { ItemID = "Header 4" },
            };
        }
    }
}
