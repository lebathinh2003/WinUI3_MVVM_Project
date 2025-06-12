// Helpers/InteractivePopupBehavior.cs
using CommunityToolkit.WinUI;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Foundation;

namespace WinUI3_MVVM_Project.Helpers
{
    public class CustomPopupBehavior : DependencyObject
    {
        // MỚI: Biến static để theo dõi Popup đang mở
        private static Popup _currentlyOpenPopup = null!;

        #region State Management
        private class HoverState
        {
            public Popup Popup { get; set; } = null!;
            public DispatcherTimer HideTimer { get; set; } = null!;
        }

        private static readonly DependencyProperty StateProperty =
            DependencyProperty.RegisterAttached("State", typeof(HoverState), typeof(CustomPopupBehavior), new PropertyMetadata(null));

        private static HoverState GetState(DependencyObject obj) => (HoverState)obj.GetValue(StateProperty);
        private static void SetState(DependencyObject obj, HoverState value) => obj.SetValue(StateProperty, value);
        #endregion

        #region IsEnabled Attached Property
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(CustomPopupBehavior), new PropertyMetadata(false, OnIsEnabledChanged));

        public static bool GetIsEnabled(DependencyObject d) => (bool)d.GetValue(IsEnabledProperty);
        public static void SetIsEnabled(DependencyObject d, bool value) => d.SetValue(IsEnabledProperty, value);
        #endregion

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element)
            {
                element.PointerEntered -= Element_PointerEntered;
                element.PointerExited -= Element_PointerExited;

                if ((bool)e.NewValue)
                {
                    element.PointerEntered += Element_PointerEntered;
                    element.PointerExited += Element_PointerExited;
                }
            }
        }

        private static void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (sender is not FrameworkElement element) return;

            var textBlock = FindVisualChild<TextBlock>(element);
            if (textBlock == null || !textBlock.IsTextTrimmed) return;

            var state = GetOrCreateState(element);
            state.HideTimer.Stop();

            ShowPopup(element, state);
        }

        private static void Element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var state = GetState((sender as DependencyObject)!);
            state?.HideTimer.Start();
        }

        private static void ShowPopup(FrameworkElement element, HoverState state)
        {
            // MỚI: Đóng bất kỳ Popup nào đang mở trước khi hiển thị cái mới
            if (_currentlyOpenPopup != null && _currentlyOpenPopup != state.Popup)
            {
                _currentlyOpenPopup.IsOpen = false;
            }

            if (state.Popup == null)
            {
                var textBlock = FindVisualChild<TextBlock>(element);
                if (textBlock == null) return;

                var popupTextBlock = new TextBlock
                {
                    Text = textBlock.Text,
                    Foreground = textBlock.Foreground,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(8)
                };

                var rowGrid = element.FindParent<Grid>();
                var sourceBrush = rowGrid?.Background;
                Brush finalBackgroundBrush;
                if (sourceBrush != null && !(sourceBrush is SolidColorBrush solid && solid.Color == Colors.Transparent))
                {
                    finalBackgroundBrush = sourceBrush;
                }
                else
                {
                    finalBackgroundBrush = new SolidColorBrush(Colors.Black);
                }

                var popupGrid = new Grid
                {
                    Background = finalBackgroundBrush,
                    BorderBrush = new SolidColorBrush(Colors.Gray),
                    BorderThickness = new Thickness(1),
                    IsHitTestVisible = true
                };
                popupGrid.Children.Add(popupTextBlock);

                popupGrid.Tag = state;
                popupGrid.PointerEntered += PopupGrid_PointerEntered;
                popupGrid.PointerExited += PopupGrid_PointerExited;

                state.Popup = new Popup { Child = popupGrid };

                // MỚI: Lắng nghe sự kiện Closed để dọn dẹp bộ nhớ
                state.Popup.Closed += Popup_Closed!;
            }

            state.Popup.XamlRoot = element.XamlRoot;

            var childTextBlock = FindVisualChild<TextBlock>(element);
            double additionHeight = childTextBlock?.ActualHeight ?? 0.0;

            double elementWidth = element.ActualWidth;
            double elementHeight = element.ActualHeight;

            (state.Popup.Child as FrameworkElement)!.Width = elementWidth;

            var transform = element.TransformToVisual(null);
            Point elementPosition = transform.TransformPoint(new Point(0, 0));

            state.Popup.HorizontalOffset = elementPosition.X;
            state.Popup.VerticalOffset = elementPosition.Y + (elementHeight / 2) + (additionHeight / 2);

            state.Popup.IsOpen = true;

            // MỚI: Cập nhật "bộ nhớ" để trỏ đến Popup vừa được mở
            _currentlyOpenPopup = state.Popup;
        }

        // MỚI: Event handler để dọn dẹp bộ nhớ khi Popup bị đóng
        private static void Popup_Closed(object sender, object e)
        {
            var closedPopup = sender as Popup;
            // Nếu popup vừa bị đóng chính là cái đang được lưu trong bộ nhớ, hãy xóa nó đi.
            if (_currentlyOpenPopup == closedPopup)
            {
                _currentlyOpenPopup = null!;
            }
        }

        private static void PopupGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is HoverState state)
            {
                state.HideTimer.Stop();
            }
        }

        private static void PopupGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.Tag is HoverState state)
            {
                state.HideTimer.Start();
            }
        }

        private static HoverState GetOrCreateState(FrameworkElement element)
        {
            var state = GetState(element);
            if (state == null)
            {
                state = new HoverState
                {
                    HideTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(150) }
                };

                state.HideTimer.Tick += (s, args) =>
                {
                    state.HideTimer.Stop();
                    if (state.Popup != null)
                    {
                        state.Popup.IsOpen = false;
                    }
                };
                SetState(element, state);
            }
            return state;
        }

        private static T FindVisualChild<T>(DependencyObject obj) where T : DependencyObject
        {
            if (obj is T t) return t;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = VisualTreeHelper.GetChild(obj, i);
                var childOfChild = FindVisualChild<T>(child);
                if (childOfChild != null) return childOfChild;
            }
            return null!;
        }
    }
}