//    __ _/| _/. _  ._/__ /
// _\/_// /_///_// / /_|/
//            _/
// sof digital 2021
// written by michael rinderle <michael@sofdigital.net>

// mit license
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
