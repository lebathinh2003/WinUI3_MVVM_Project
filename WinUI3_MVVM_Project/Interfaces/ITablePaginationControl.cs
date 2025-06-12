using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WinUI3_MVVM_Project.Interfaces;

public interface ITablePaginationControl
{
    ICommand FirstPageCommand { get; }
    ICommand PreviousPageCommand { get; }
    ICommand NextPageCommand { get; }
    ICommand LastPageCommand { get; }
    string CurrentPageInfo { get; }
}
