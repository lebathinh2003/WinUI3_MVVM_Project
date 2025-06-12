// Trong file mới, ví dụ: GridColumnSyncHelper.cs
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.Generic;
using System.Diagnostics;
namespace WinUI3_MVVM_Project.Helpers;
public class GridColumnSyncHelper
{
    // Dùng một Dictionary để theo dõi Grid nào đang đồng bộ với Grid nào
    // và lưu trữ cả event handler để có thể hủy đăng ký sau này.
    private static readonly Dictionary<Grid, SizeChangedEventHandler> _syncEventHandlers = new();

    public static readonly DependencyProperty SyncWithProperty =
        DependencyProperty.RegisterAttached(
            "SyncWith",
            typeof(Grid),
            typeof(GridColumnSyncHelper),
            new PropertyMetadata(null, OnSyncWithChanged));

    public static Grid GetSyncWith(DependencyObject obj)
    {
        return (Grid)obj.GetValue(SyncWithProperty);
    }

    public static void SetSyncWith(DependencyObject obj, Grid value)
    {
        obj.SetValue(SyncWithProperty, value);
    }

    private static void OnSyncWithChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not Grid targetGrid) return;

        // --- Hủy đăng ký sự kiện cũ để tránh memory leak ---
        if (_syncEventHandlers.TryGetValue(targetGrid, out var oldEventHandler))
        {
            if (e.OldValue is Grid oldSourceGrid)
            {
                oldSourceGrid.SizeChanged -= oldEventHandler;
            }
            _syncEventHandlers.Remove(targetGrid);
        }

        // --- Đăng ký sự kiện mới ---
        if (e.NewValue is Grid newSourceGrid)
        {
            // QUAN TRỌNG: Tạo trình xử lý sự kiện với DispatcherQueue
            SizeChangedEventHandler newEventHandler = (s, args) =>
            {
                // Lên lịch cho việc đồng bộ, tách nó ra khỏi chu trình layout chính
                targetGrid.DispatcherQueue.TryEnqueue(() =>
                {
                    // Kiểm tra để chắc chắn các control vẫn còn tồn tại
                    if (newSourceGrid.IsLoaded && targetGrid.IsLoaded)
                    {
                        SyncColumnWidths(newSourceGrid, targetGrid);
                    }
                });
            };

            newSourceGrid.SizeChanged += newEventHandler;
            _syncEventHandlers[targetGrid] = newEventHandler;

            // Thực hiện đồng bộ ngay lần đầu tiên, cũng qua DispatcherQueue
            targetGrid.DispatcherQueue.TryEnqueue(() =>
            {
                if (newSourceGrid.IsLoaded && targetGrid.IsLoaded)
                {
                    SyncColumnWidths(newSourceGrid, targetGrid);
                }
            });
        }
    }
    private static void SyncColumnWidths(Grid source, Grid target)
    {
        // Đảm bảo source có cột
        if (source.ColumnDefinitions.Count == 0) return;

        var targetColumns = target.ColumnDefinitions;
        var sourceColumns = source.ColumnDefinitions;

        // Thêm/bớt cột cho target để khớp với source
        while (targetColumns.Count < sourceColumns.Count)
        {
            targetColumns.Add(new ColumnDefinition());
        }
        while (targetColumns.Count > sourceColumns.Count)
        {
            targetColumns.RemoveAt(targetColumns.Count - 1);
        }

        //target.Width = source.ActualWidth;

        for (int i = 0; i < sourceColumns.Count - 1; i++)
        {
            targetColumns[i].Width = new GridLength(sourceColumns[i].ActualWidth);
        }

        targetColumns[sourceColumns.Count-1].Width = new GridLength(1, GridUnitType.Star);

    }
}