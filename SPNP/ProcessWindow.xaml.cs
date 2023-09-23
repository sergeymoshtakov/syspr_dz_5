using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;

namespace SPNP
{
    /// <summary>
    /// Логика взаимодействия для ProcessWindow.xaml
    /// </summary>
    public partial class ProcessWindow : Window
    {
        public ProcessWindow()
        {
            InitializeComponent();
        }

        private void ShowProcesses_Click(object sender, RoutedEventArgs e)
        {
            Process[] processes = Process.GetProcesses();
            // ProcTextBlock.Text = "";
            // foreach (Process process in processes)
            // {
            // ProcTextBlock.Text += String.Format("{0} {1}\n", process.Id, process.ProcessName);
            // }
            String prevName = "";
            TreeViewItem? item = null;
            foreach (Process process in processes.OrderBy(p => p.ProcessName))
            {
                if(prevName != process.ProcessName)
                {
                    prevName = process.ProcessName;
                    item = new TreeViewItem() { Header = prevName };
                    ProcTreeBlock.Items.Add(item);
                }
                var subItem = new TreeViewItem()
                {
                    Header = String.Format("{0} {1}", process.Id, process.ProcessName),
                    Tag = process
                };
                subItem.MouseDoubleClick += TreeViewItem_MouseDoubleClick;
                item?.Items.Add(subItem);
            }
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if(sender is TreeViewItem item)
            {
                String message = "";
                if(item.Tag is Process process)
                {
                    TimeSpan cpuTime = process.TotalProcessorTime;
                    double cpuTimeMilliseconds = cpuTime.TotalMilliseconds;
                    long memoryUsage = process.WorkingSet64;
                    double memoryUsageKB = memoryUsage / 1024.0;
                    double memoryUsageMB = memoryUsageKB / 1024.0;
                    int threadCount = process.Threads.Count;
                    message += "споживання процесорного часу: " + cpuTimeMilliseconds + " miliseconds\r\n";
                    message += "споживання оперативної пам'яті: " + memoryUsageMB.ToString("F2") + " MB\r\n";
                    message += "загальнa кількість потоків: " + threadCount + "\r\n";
                }
                else
                {
                    message = "No process in tag";
                }
                MessageBox.Show(message);
            }
        }
    }
}
