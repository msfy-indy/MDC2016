using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace MDCTest2016
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            InitSoftware();
        }

        private void InitSoftware()
        {
            this.Icon = new BitmapImage(new Uri("./icon/MainWindowLogo.ico", UriKind.RelativeOrAbsolute));
            Sensor.InitMachine();
            Sensor.ShowValuePanel.SetValue(Grid.ColumnProperty, 0);
            Sensor.ShowValuePanel.SetValue(Grid.RowProperty, 2);
            this.BackGroundGrid.Children.Add(Sensor.ShowValuePanel);
            Communication.EstablishCommunication(Sensor.CommunicationMode);
            System.Threading.ThreadPool.QueueUserWorkItem(GetAndAnalysisData.GetData, null);
            switch (Sensor.CommunicationMode)
            {
                case 0:
                    {
                        this.CommunicationModeLabel.Content = "USB通讯";
                        break;
                    }
                case 1:
                    {
                        this.CommunicationModeLabel.Content = "HID通讯";
                        break;
                    }
                case 2:
                    {
                        this.CommunicationModeLabel.Content = "WIFI通讯";
                        break;
                    }
                default:
                    {
                        this.CommunicationModeLabel.Content = "USB通讯";
                        break;
                    }
            }
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += this.RefreshUI;
            timer.Start();
        }

        public void RefreshUI(object sender, EventArgs e)
        {
            this.ShowSystemInformation();
            this.ShowValues();
        }

        private void ShowSystemInformation()
        {
            if (!Datas.IsOnlie)
            {
                this.CommunicatingLabel.Visibility = System.Windows.Visibility.Visible;
                this.CommunicatingLabel.Content = "通讯断开";
                this.EnableLabel.Visibility = System.Windows.Visibility.Hidden;
                return;
            }
            else
            {
                this.CommunicatingLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            if (Datas.UpLimit)
            {
                this.UplimitLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.UplimitLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            if (Datas.LowLimit)
            {
                this.DownLimitLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.DownLimitLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            if (Datas.EmergencyStop)
            {
                this.EmergencyStopLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.EmergencyStopLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            if (Datas.IsOverLoad)
            {
                this.OverLoadLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.OverLoadLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            if (Datas.Day<0xffff)
            {
                if (Datas.Day>0)
                {
                    this.EnableLabel.Visibility = System.Windows.Visibility.Visible;
                    this.EnableLabel.Content = "可使用"+Datas.Day.ToString()+"天";
                }
                else
                {
                    this.EnableLabel.Visibility = System.Windows.Visibility.Visible;
                    this.EnableLabel.Content = "禁止使用";
                }
            }
            else
            {
                this.EnableLabel.Visibility= System.Windows.Visibility.Hidden;
            }
            if (Datas.IsCali)
            {
                this.CalibrationLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.CalibrationLabel.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        private void ShowValues()
        {
            foreach (var item in Sensor.ShowValuePanel.ValueGrid)
            {
                string Modi = "0.";
                for (int i = 0; i < Sensor.DecimalPoint[item.ChannelNum]; i++)
                {
                    Modi = Modi + "0";
                }
                item.ValueLabel.Content = Sensor.GetValue(item.ChannelNum).ToString(Modi);
            }
            if (Datas.IsRunning)
            {
                this.PosiSpeedLabel.Content = double.Parse(Math.Abs(Datas.SpeedCode[1] * Sensor.ScaleValue[1]).ToString("G3")).ToString();
            }
            else
            {
                this.PosiSpeedLabel.Content = "0";
            }
            
        }

        private void ReInitUI()
        {
            this.TestTabBtn.Width = (this.MainGrid.ActualWidth - 10) / 3;
            this.ReportTabBtn.Width = (this.MainGrid.ActualWidth - 10) / 3;
            this.MachineTabBtn.Width = (this.MainGrid.ActualWidth - 10) / 3;
            this.TestingTabBtn.Height = (this.MainGrid.ActualHeight - 46) / 2;
            this.TestProjectTabBtn.Height = (this.MainGrid.ActualHeight - 46) / 2;
            this.TestingTabBtn.Header = "试\n验\n管\n理";
            this.TestProjectTabBtn.Header = "试\n验\n方\n案";
            this.TestProjectTabItem1.Width = (this.MainGrid.ActualWidth - 60) / 4;
            this.TestProjectTabItem2.Width = (this.MainGrid.ActualWidth - 60) / 4;
            this.TestProjectTabItem3.Width = (this.MainGrid.ActualWidth - 60) / 4;
            this.TestProjectTabItem4.Width = (this.MainGrid.ActualWidth - 60) / 4;
            this.ReportTabControlItem1.Height = (this.MainGrid.ActualHeight - 46) / 2;
            this.ReportTabControlItem2.Height = (this.MainGrid.ActualHeight - 46) / 2;
            this.ReportTabControlItem1.Header = "报\n告\n管\n理";
            this.ReportTabControlItem2.Header = "曲\n线\n分\n析";
            this.MachineControlItem1.Height = (this.MainGrid.ActualHeight - 46) / 5;
            this.MachineControlItem2.Height = (this.MainGrid.ActualHeight - 46) / 5;
            this.MachineControlItem3.Height = (this.MainGrid.ActualHeight - 46) / 5;
            this.MachineControlItem4.Height = (this.MainGrid.ActualHeight - 46) / 5;
            this.MachineControlItem5.Height = (this.MainGrid.ActualHeight - 46) / 5;
            this.MachineControlItem1.Header = "设\n备";
            this.MachineControlItem2.Header = "校\n准";
            this.MachineControlItem3.Header = "检\n定";
            this.MachineControlItem4.Header = "测\n控\n器";
            this.MachineControlItem5.Header = "附\n件";
            this.CaliTabItem1.Width = (this.MainGrid.ActualWidth - 60) / 5;
            this.CaliTabItem2.Width = (this.MainGrid.ActualWidth - 60) / 5;
            this.CaliTabItem3.Width = (this.MainGrid.ActualWidth - 60) / 5;
            this.CaliTabItem4.Width = (this.MainGrid.ActualWidth - 60) / 5;
            this.CaliTabItem5.Width = (this.MainGrid.ActualWidth - 60) / 5;
            System.Threading.ThreadPool.QueueUserWorkItem(InitPaint, null);
        }

        private void InitPaint(object o)
        {
            while (true)
            {
                if (CurvePictureBoxGrid.ActualHeight > 0)
                {
                    CurvePictureBox.Image = new Bitmap((int)CurvePictureBoxGrid.ActualWidth, (int)CurvePictureBoxGrid.ActualHeight);
                    paint.xIndex[0] = 1;
                    paint.yIndex[0] = 0;
                    paint.xDeviceOrg[0] = 24;
                    paint.yDeviceOrg[0] = 24;
                    paint.p[0] = this.CurvePictureBox;
                    paint.StaticG[0] = paint.p[0].CreateGraphics();
                    paint.StaticImgG[0] = System.Drawing.Graphics.FromImage(paint.p[0].Image);
                    paint.initPicture(0, false, (float)0.01, 1, true);
                    break;
                }
                else
                {
                    System.Threading.Thread.Sleep(200);
                }
            }
            
        }

        private void InitFlexGrids()
        {
            this.TestStepFlexGrid.Column(0).Width = 42;
            this.TestStepFlexGrid.Column(1).Width = (short)(this.TestStepFlexGrid.Width - 62);
            this.TestStepFlexGrid.Cell(0, 0).Text = "步骤号";
            this.TestStepFlexGrid.Cell(0, 1).Text = "内容";

            this.InputGrid.Column(0).Width = 100;
            this.InputGrid.Column(1).Width = (short)(this.TestStepFlexGrid.Width - 120);
            this.InputGrid.Cell(0, 0).Text = "输入项";
            this.InputGrid.Cell(0, 1).Text = "内容";

            this.ResultGrid.Column(0).Width = 100;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReInitUI();
            InitFlexGrids();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ReInitUI();
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Datas.IsCali = !Datas.IsCali;
            
            //System.Environment.Exit(0);
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Cmd.StopRun();
        }
    }
}
