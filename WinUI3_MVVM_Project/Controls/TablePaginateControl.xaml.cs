using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinUI3_MVVM_Project.Interfaces; // Namespace của ITablePaginationControl
using System.ComponentModel; // For PropertyChangedEventArgs

namespace WinUI3_MVVM_Project.Controls
{
    public sealed partial class TablePaginateControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ITablePaginationControl),
                typeof(TablePaginateControl),
                new PropertyMetadata(null, OnViewModelPropertyChanged));

        public ITablePaginationControl ViewModel
        {
            get => (ITablePaginationControl)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        public TablePaginateControl()
        {
            this.InitializeComponent();
            this.Unloaded += TablePaginateControl_Unloaded;
        }

        private static void OnViewModelPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TablePaginateControl control)
            {
                if (e.OldValue is INotifyPropertyChanged oldVm)
                {
                    oldVm.PropertyChanged -= control.ViewModel_PropertyChanged!;
                }

                if (e.NewValue is INotifyPropertyChanged newVm)
                {
                    newVm.PropertyChanged += control.ViewModel_PropertyChanged!;
                    control.UpdateCurrentPageInfoText((newVm as ITablePaginationControl)!);
                }
                else if (e.NewValue == null)
                {
                    control.UpdateCurrentPageInfoText(null!);
                }
            }
        }

        // Xử lý sự kiện PropertyChanged từ ViewModel
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Trong trường hợp này, ViewModel đã tự thông báo cho "CurrentPageInfo"
            if (e.PropertyName == nameof(ITablePaginationControl.CurrentPageInfo))
            {
                UpdateCurrentPageInfoText(this.ViewModel);
            }
        }

        // DependencyProperty mới để hiển thị Text, và TextBlock sẽ bind vào đây
        public static readonly DependencyProperty DisplayCurrentPageInfoProperty =
            DependencyProperty.Register("DisplayCurrentPageInfo", typeof(string), typeof(TablePaginateControl), new PropertyMetadata("1 / 1"));

        public string DisplayCurrentPageInfo
        {
            get { return (string)GetValue(DisplayCurrentPageInfoProperty); }
            set { SetValue(DisplayCurrentPageInfoProperty, value); }
        }

        private void UpdateCurrentPageInfoText(ITablePaginationControl vm)
        {
            if (vm != null)
            {
                this.DisplayCurrentPageInfo = vm.CurrentPageInfo;

            }
            else
            {
                this.DisplayCurrentPageInfo = "N/A"; // Hoặc giá trị fallback
            }
        }


        private void TablePaginateControl_Unloaded(object sender, RoutedEventArgs e)
        {
            // Quan trọng: Hủy đăng ký sự kiện khi control bị unload để tránh memory leak
            if (this.ViewModel is INotifyPropertyChanged vm)
            {
                vm.PropertyChanged -= ViewModel_PropertyChanged!;
            }
        }
    }
}