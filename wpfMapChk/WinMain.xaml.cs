using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MapTimerChk
{
    /// <summary>
    /// Window1.xaml 的互動邏輯
    /// </summary>
    public partial class WinMain : Window
    {
        private int old_Height;
        private int old_Width;
        private int x;
        private int y;
        private int sum;
        private bool Ftop = false;
        private bool bType = false;
        private bool bScreenSaverFlag = false;

        public WinMain()
        {
            InitializeComponent();
        }

        private void btnChk_Click(object sender, RoutedEventArgs e)
        {
            IntPtr POEhwnd = FindWindow(null, "Path of Exile");
            if (POEhwnd != IntPtr.Zero)
            {
                Rect POERect = new Rect();
                GetWindowRect(POEhwnd.ToInt32(), ref POERect);
                y = POERect.Bottom - 50;
                x = POERect.Right - 50;
                foreach (Control i in this.Controls)
                {
                    i.Enabled = true;
                }
                //tmrCount.Enabled = true;
                //btnAction.Text = "停止";
            }
            else
            {
                this.Content = "偵測不到POE";
                foreach (Control i in this.Controls)
                {
                    i.Enabled = false;
                }
                btnChk.Enabled = true;
                tmrCount.Enabled = false;
            }
        }

        private void btnTop_Click(object sender, RoutedEventArgs e)
        {
            if (Ftop)
            {
                this.TopMost = false;
                btnTop.Content = "置頂";
            }
            else
            {
                this.TopMost = true;
                btnTop.Content = "非置頂";
            }
            Ftop = !Ftop;
        }

        private void btnAction_Click(object sender, RoutedEventArgs e)
        {
            if (tmrCount.Enabled)
            {
                btnAction.Content = "啟動";
                picbx.BackColor = Color.LawnGreen;
                chkbxScrSaver.Checked = true;
            }
            else
            {
                btnAction.Content = "停止";
                picbx.BackColor = Color.LawnGreen;
                chkbxScrSaver.Checked = false;
            }
            sum = 0;
            tmrCount.Enabled = !tmrCount.Enabled;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (bType)
            {
                bType = false;
                this.Size = new Size(old_Width, old_Height);
                btnMinimize.Content = "最小化";
                foreach (Control i in this.Controls)
                {
                    i.Enabled = true;
                }
            }
            else
            {
                bType = true;
                this.Size = new Size(85, 52);
                this.Left = 10;
                this.Top = 65;
                foreach (Control i in this.Controls)
                {
                    i.Enabled = false;
                }
                btnMinimize.Enabled = true;
                this.ActiveControl = btnMinimize;
                btnMinimize.Content = "恢復";
                this.Content = "地圖切換計時器";
            }
        }

        private void chkbxScrSaver_CheckedChanged(object sender, RoutedEventArgs e)
        {
            CheckBox cb = (sender as CheckBox);
            if (cb.Checked)
                ScreenSaver.Enable();
            else
                ScreenSaver.Disable();
        }

        private void tmrCount_Tick(object sender, RoutedEventArgs e)
        {
            int hdc = 0;

            try
            {
                hdc = GetDC(0);
                if (GetPixel(hdc, x, y) == 0)   //black color
                {
                    sum = sum + 1;
                    this.Text = sum.ToString();
                }
                else
                {
                    sum = 0;
                }
                if (!bType) // normal 時顯示
                {
                    picbx.BackColor = (picbx.BackColor == Color.LawnGreen) ?
                                        SystemColors.Control : Color.LawnGreen;
                }
            }
            finally
            {
                ReleaseDC(IntPtr.Zero.ToInt32(), hdc);
            }
        }

        private void grpBtn_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (radioButton1.Checked)
            {
                tmrCount.Interval = 100;
            }
            else if (radioButton2.Checked)
            {
                tmrCount.Interval = 200;
            }
            else if (radioButton3.Checked)
            {
                tmrCount.Interval = 300;
            }
            else if (radioButton4.Checked)
            {
                tmrCount.Interval = 500;
            }
            else if (radioButton5.Checked)
            {
                tmrCount.Interval = 50;
            }
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            foreach (Control i in this.Controls)
            {
                i.Enabled = false;
            }
            btnChk.Enabled = true;
            chkbxScrSaver.Enabled = true;
            this.old_Height = this.Height;
            this.old_Width = this.Width;

            this.Ftop = false;
            this.bType = false;

            bScreenSaverFlag = ScreenSaver.Check();
            chkbxScrSaver.Checked = bScreenSaverFlag;

            grpBtn_CheckedChanged(this, e);
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (bScreenSaverFlag) ScreenSaver.Enable();
        }
    }
}
