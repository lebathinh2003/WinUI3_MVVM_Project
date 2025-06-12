// Helpers/SelectableRepeaterBehavior.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System.Windows.Input;

namespace WinUI3_MVVM_Project.Helpers
{
    public class SelectableRepeaterBehavior : DependencyObject
    {
        #region Attached Properties
        // Property để gắn Behavior vào ItemsRepeater
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(SelectableRepeaterBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

        // Property để nhận Command từ ViewModel
        public static readonly DependencyProperty ItemTappedCommandProperty =
            DependencyProperty.RegisterAttached("ItemTappedCommand", typeof(ICommand), typeof(SelectableRepeaterBehavior), new PropertyMetadata(null));

        public static ICommand GetItemTappedCommand(DependencyObject obj) => (ICommand)obj.GetValue(ItemTappedCommandProperty);
        public static void SetItemTappedCommand(DependencyObject obj, ICommand value) => obj.SetValue(ItemTappedCommandProperty, value);

        // Property để binding SelectedItem. Behavior này không xử lý nhưng vẫn cung cấp để XAML có thể binding.
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(SelectableRepeaterBehavior), new PropertyMetadata(null));

        public static object GetSelectedItem(DependencyObject obj) => obj.GetValue(SelectedItemProperty);
        public static void SetSelectedItem(DependencyObject obj, object value) => obj.SetValue(SelectedItemProperty, value);
        #endregion

        // Biến private để giữ tham chiếu đến Behavior instance
        private static readonly DependencyProperty BehaviorProperty =
            DependencyProperty.RegisterAttached("Behavior", typeof(SelectableRepeaterBehavior), typeof(SelectableRepeaterBehavior), new PropertyMetadata(null));

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ItemsRepeater repeater)
            {
                var behavior = repeater.GetValue(BehaviorProperty) as SelectableRepeaterBehavior;
                if (behavior != null) // Dọn dẹp cái cũ trước
                {
                    repeater.ElementPrepared -= behavior.OnElementPrepared;
                    repeater.ElementClearing -= behavior.OnElementClearing;
                }

                if ((bool)e.NewValue)
                {
                    behavior = new SelectableRepeaterBehavior();
                    repeater.SetValue(BehaviorProperty, behavior);
                    repeater.ElementPrepared += behavior.OnElementPrepared;
                    repeater.ElementClearing += behavior.OnElementClearing;
                }
            }
        }

        private void OnElementPrepared(ItemsRepeater sender, ItemsRepeaterElementPreparedEventArgs args)
        {
            // Chỉ gắn sự kiện Tapped
            if (args.Element is FrameworkElement element)
            {
                element.Tapped += OnElementTapped;
            }
        }

        private void OnElementClearing(ItemsRepeater sender, ItemsRepeaterElementClearingEventArgs args)
        {
            // Chỉ dọn dẹp sự kiện Tapped
            if (args.Element is FrameworkElement element)
            {
                element.Tapped -= OnElementTapped;
            }
        }

        private void OnElementTapped(object sender, TappedRoutedEventArgs e)
        {
            var element = sender as FrameworkElement;
            var repeater = FindParent<ItemsRepeater>(element!);
            if (repeater == null) return;

            var command = GetItemTappedCommand(repeater);
            if (command == null) return;

            // Lấy data item một cách an toàn
            int index = repeater.GetElementIndex(element);
            if (index != -1)
            {
                var dataItem = repeater.ItemsSourceView.GetAt(index);
                // Chỉ thực thi command nếu có thể
                if (command.CanExecute(dataItem))
                {
                    command.Execute(dataItem);
                }
            }
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return null!;
            return parent is T parentAsT ? parentAsT : FindParent<T>(parent);
        }
    }
}