using MahApps.Metro;
using MahApps.Metro.Controls;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace wpfMapChk
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : MetroWindow   //Window
    {
        private double old_Height;
        private double old_Width;
        private int x;
        private int y;
        private int sum;
        private bool bTop = false;
        private bool bType = false;
        private bool bTest = false;
        private DispatcherTimer tmrCount;
        private List<Control> myControls;
        private vmChk objChk;

        public MainWindow()
        {
            InitializeComponent();
        }

        void radioButton1_Checked(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void btnChk_Click(object sender, RoutedEventArgs e)
        {
            IntPtr POEhwnd = Win32.FindWindow(null, "Path of Exile");
            if (bTest || (POEhwnd != IntPtr.Zero)) // find or test
            {
                Rect POERect = new Rect();
                Win32.GetWindowRect(POEhwnd.ToInt32(), ref POERect);
                y = POERect.Bottom - 50;
                x = POERect.Right - 50;

                InitContorls(true);

                //this.Title = "地圖切換計時器";
                ICommand cmd = objChk.UpdateTitle;
                cmd.Execute("地圖切換計時器v");
            }

            if (!bTest && (POEhwnd == IntPtr.Zero))   // not found and not test
            {
                //this.Title = "偵測不到POE";
                ICommand cmd = objChk.UpdateTitle;
                cmd.Execute("偵測不到POE");

                InitContorls(false);
                AlwaysEnableCtrl();
            }
        }

        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            if (bTop)
            {
                this.Topmost = false;
                btnTop.Content = "置頂";
            }
            else
            {
                this.Topmost = true;
                btnTop.Content = "非置頂";
            }
            bTop = !bTop;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            sum = 0;
            if (tmrCount.IsEnabled)
            {
                btnAction.Content = "啟動";
                lblColor.Background = Brushes.DarkGreen;
                objChk.ScrSaver = true;
                chkbxScrSaver.IsChecked = true;
                tmrCount.Stop();
            }
            else
            {
                btnAction.Content = "停止";
                lblColor.Background = Brushes.DarkGreen;
                objChk.ScrSaver = false;
                chkbxScrSaver.IsChecked = false;
                tmrCount.Start();
            }
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (bType)
            {
                bType = false;
                //this.WindowStyle = System.Windows.WindowStyle.SingleBorderWindow;
                this.Height = old_Height;
                this.Width = old_Width;
                btnMinimize.Content = "最小化";

                InitContorls(true);
            }
            else
            {
                bType = true;
                //this.WindowStyle = System.Windows.WindowStyle.ToolWindow;
                this.Left = 10;
                this.Top = 65;
                this.Height = 80;
                this.Width = 110;

                InitContorls(false);

                btnMinimize.IsEnabled = true;
                btnMinimize.Content = "恢復";
                this.Title = "地圖切換計時器";
            }
        }


        private void cmbAccents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ??? 只會顯示上一次選擇的值

            //ThemeManager.ChangeAppStyle(this, ThemeManager.Accents.First(a => a.Name == "Red"),
            //                                ThemeManager.AppThemes.First(a => a.Name == "BaseDark"));

            string sAccent = cmbAccents.Text == "" ? "Blue" : cmbAccents.Text;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/" + sAccent + ".xaml")
            });
        }
        
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //ThemeManager.ChangeAppStyle(this, ThemeManager.Accents.First(a => a.Name == "Red"),
            //                                ThemeManager.AppThemes.First(a => a.Name == "BaseDark"));

            string sAccent = cmbAccents.Text == "" ? "Blue" : cmbAccents.Text; 
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary()
            {
                Source = new Uri("pack://application:,,,/MahApps.Metro;component/Styles/Accents/" + sAccent + ".xaml")
            });
        }

        private void chkbxScrSaver_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            objChk.ScrSaver = (cb.IsChecked == true) ? true : false;
        }

        private void tmrCount_Tick(object sender, EventArgs e)
        {
            int hdc = 0;

            try
            {
                hdc = Win32.GetDC(0);
                if (Win32.GetPixel(hdc, x, y) == 0)   //black color
                {
                    sum = sum + 1;
                    this.Content = sum.ToString();
                }
                else
                {
                    sum = 0;
                }
                if (!bType) // normal 時顯示
                {
                    lblColor.Background = (lblColor.Background == Brushes.DarkGreen) ?
                                            Brushes.White : Brushes.DarkGreen;
                }
            }
            finally
            {
                Win32.ReleaseDC(IntPtr.Zero.ToInt32(), hdc);
            }
        }

        private void grpBtn_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 100);
            }
            else if (radioButton2.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 200);
            }
            else if (radioButton3.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 300);
            }
            else if (radioButton4.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 500);
            }
            else if (radioButton5.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 50);
            }
            else if (radioButton6.IsChecked == true)
            {
                tmrCount.Interval = new TimeSpan(0, 0, 0, 1, 00);
            }
            else  // default
            {
                radioButton4.IsChecked = true;
                tmrCount.Interval = new TimeSpan(0, 0, 0, 0, 500);
            }
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //List all the UIElement in the VisualTree
            myControls = new List<Control>();
            //Walk through the VisualTree
            FindControl(this.mygrid, myControls);

            InitContorls(false);
            AlwaysEnableCtrl();

            this.old_Height = this.Height;
            this.old_Width = this.Width;

            this.bTop = false;
            this.bType = false;

            String[] arguments = Environment.GetCommandLineArgs();
            string sCmd = (arguments.Count() > 1) ? arguments[1] : "none";
            if (sCmd == "test")
            {
                this.bTest = true;
            }

            //  DispatcherTimer setup
            tmrCount = new System.Windows.Threading.DispatcherTimer();
            tmrCount.Tick += new EventHandler(tmrCount_Tick);
            //tmrCount.Interval = new TimeSpan(0, 0, 0, 1, 0);
            grpBtn_CheckedChanged(this, e);
            //tmrCount.Start();

            objChk = new vmChk();
            chkbxScrSaver.IsChecked = objChk.ScrSaver;

            // 手動將 accents 加到 combolist
            //foreach (var acc in ThemeManager.Accents)
            //{
            //    cmbAccents.Items.Add(acc.Name);
            //}

            System.Windows.Data.CollectionViewSource accentViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("accentViewSource")));
            // 透過設定 CollectionViewSource.Source 屬性載入資料: 
            accentViewSource.Source = ThemeManager.Accents;
        }

        private void AlwaysEnableCtrl()
        {
            // always enabled
            btnChk.IsEnabled = true;
            cmbAccents.IsEnabled = true;
            btnTest.IsEnabled = true;
            chkbxScrSaver.IsEnabled = true;
        }

        private void InitContorls(bool bEnable)
        {
            foreach (Control i in this.myControls)
            {
                i.IsEnabled = bEnable;
            }
        }

        public void FindControl(DependencyObject root, List<Control> controls)
        {
            controls.Clear();
            var queue = new Queue<DependencyObject>();
            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var control = current as Control;
                if (control != null && control.IsTabStop)
                {
                    controls.Add(control);
                }

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(current); i++)
                {
                    var child = VisualTreeHelper.GetChild(current, i);
                    if (child != null)
                    {
                        queue.Enqueue(child);
                    }
                }
            }
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            objChk.Recovery();
        }


    }
}
