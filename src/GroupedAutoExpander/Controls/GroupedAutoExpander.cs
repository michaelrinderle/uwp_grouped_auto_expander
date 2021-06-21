using GroupedAutoExpander.Models;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace GroupedAutoExpander.Controls
{
    public class GroupedAutoExpander : Expander, INotifyPropertyChanged
    {
        public static readonly DependencyProperty ItemIdProperty =
            DependencyProperty.Register(
                "ItemId",
                typeof(string),
                typeof(GroupedAutoExpander),
                new PropertyMetadata(null));

        public static DependencyProperty ItemsSourceProperty =
                DependencyProperty.Register(
               "ItemsSource",
               typeof(ObservableCollection<GroupedAutoExpanderListItem>),
               typeof(GroupedAutoExpander),
               new PropertyMetadata(null));
        
        public string ItemId
        {
            get => (string)GetValue(ItemIdProperty);
            set => SetValue(ItemIdProperty, value);
        }

        public ObservableCollection<GroupedAutoExpanderListItem> ItemsSource
        {
            get => (ObservableCollection<GroupedAutoExpanderListItem>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public GroupedAutoExpander() : base() { }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            // set header, default to item id if header name is not specificed
            var item = ItemsSource.FirstOrDefault(x => x.ItemID == ItemId);
            this.Header = !string.IsNullOrEmpty(item.HeaderName) ? item.HeaderName : item.ItemID;

            // set item id expand state
            var isExpanded = ItemsSource.FirstOrDefault(x => x.ItemID == ItemId)?.IsExpanded;
            this.IsExpanded = (isExpanded != null) && (bool)isExpanded;

            // set expander list update callback
            this.RegisterPropertyChangedCallback(ItemsSourceProperty, (ObservableCollectionUpdateCallback));
        }

        private void ObservableCollectionUpdateCallback(DependencyObject sender, DependencyProperty dp)
        {
            if (dp != ItemsSourceProperty) return;

            // set collection items expansion set
            var isExpanded = ItemsSource.FirstOrDefault(x => x.ItemID == ItemId)?.IsExpanded;
            this.IsExpanded = (bool)isExpanded;
        }

        protected override void OnExpanded(EventArgs args)
        {
            base.OnExpanded(args);

            // check user preference if to auto close expander

            foreach (var item in ItemsSource)
            {
                if (item.ItemID != ItemId)
                {
                    item.IsExpanded = false;
                    continue;
                }

                item.IsExpanded = true;
            }

            // need to replace whole obj to get dp update
            ItemsSource = new ObservableCollection<GroupedAutoExpanderListItem>(ItemsSource);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
