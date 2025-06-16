using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace sample_datagrid_many_data_WPF
{
    public partial class MainWindowViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<DataItem> m_DataItems  = new();
        private readonly object m_DataItemsLock = new();

        public IRelayCommand LoadedCommand { get; }
        public IRelayCommand UnloadedCommand { get; }
        public IRelayCommand StartCommand { get; }
        public IRelayCommand StopCommand { get; }

        [ObservableProperty] private int m_InitCount = 100000;

        public MainWindowViewModel(ILogger<MainWindowViewModel> logger, IDataItemObserver dataItemObserver, IDataItemSubject dataItemSubject)
        {
            m_DataItemSubject = dataItemSubject;
            m_DataItemObserver = dataItemObserver;
            m_Logger = logger;

            LoadedCommand = new RelayCommand(OnLoaded);
            UnloadedCommand = new RelayCommand(OnUnloaded);
            StartCommand = new RelayCommand(OnStart);
            StopCommand = new RelayCommand(OnStop);
            
        }

        private readonly ILogger<MainWindowViewModel> m_Logger;
        private readonly IDataItemObserver m_DataItemObserver;

        private CancellationTokenSource m_AddThreadCancel = new();
        private readonly IDataItemSubject m_DataItemSubject;

        private void OnLoaded()
        {
            m_DataItemObserver.ItemAdded += DataItemObserverOnItemAdded;
        }

        private void OnUnloaded()
        {
            m_DataItemObserver.ItemAdded -= DataItemObserverOnItemAdded;
            m_AddThreadCancel.Cancel();
        }


        private void DataItemObserverOnItemAdded(DataItem obj)
        {
            DataItems.Add(obj);
        }
        private void OnStart()
        {
            Interlocked.Exchange(ref m_AddThreadCancel, new CancellationTokenSource()).Dispose();

            var random = new Random();

            List<DataItem> buf = new();
            for (int i = 0; i < InitCount; i++)
            {
                buf.Add(CreateNewItem(random));
            }
            DataItems = new ObservableCollection<DataItem>(buf);
            int dataCount = InitCount;
            BindingOperations.EnableCollectionSynchronization(DataItems, m_DataItemsLock);

            _ = Task.Run(async () =>
            {
                while (!m_AddThreadCancel.IsCancellationRequested)
                {
                    dataCount++;
                    if (dataCount % 1000 == 0)
                    {
                        m_Logger.LogInformation($"Added Count={dataCount}");
                    }

                    await Task.Delay(TimeSpan.FromMilliseconds(1), m_AddThreadCancel.Token);
                    var item = CreateNewItem(random);
                    m_DataItemSubject.OnItemAdded(item);
                }
            }, m_AddThreadCancel.Token);

            DataItem CreateNewItem(Random random)
            {
                var item = new DataItem
                {
                    Name = $"Item {DateTime.Now.Ticks}",
                    Value = $"Value {random.Next(1000)}",
                    ValueInt = random.Next(1, 10000)
                };
                return item;
            }
        }
        private void OnStop()
        {
            m_AddThreadCancel.Cancel();
        }


    }
}
