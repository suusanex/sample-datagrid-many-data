using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sample_datagrid_many_data_WPF
{
    public interface IDataItemObserver
    {
        event Action<DataItem>? ItemAdded;
    }

    public interface IDataItemSubject
    {
        void OnItemAdded(DataItem item);

    }

    public class DataItemObserver : IDataItemObserver, IDataItemSubject
    {
        public event Action<DataItem>? ItemAdded;

        public void OnItemAdded(DataItem item)
        {
            ItemAdded?.Invoke(item);
        }
    }
}
