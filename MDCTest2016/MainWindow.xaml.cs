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
            this.Icon = new BitmapImage(new Uri("./icon/MainWindowLogo.ico", UriKind.RelativeOrAbsolute));

            Communication.EstablishCommunication(0);

            Sensor.InitMachine();



            System.Threading.ThreadPool.QueueUserWorkItem(GetAndAnalysisData.GetData, null);
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += RefreshUI;
            timer.Start();

            //Sensor.ShowValuePanel = new MDCTest2016.ShowValueGrid(4, null, null);
            Sensor.ShowValuePanel.SetValue(Grid.ColumnProperty, 0);
            Sensor.ShowValuePanel.SetValue(Grid.RowProperty, 2);
            this.BackGroundGrid.Children.Add(Sensor.ShowValuePanel);


        }

        public void RefreshUI(object sender, EventArgs e)
        {
            if (!Datas.IsOnlie)
            {
                return;
            }
            if (Datas.UpLimit)
            {
                this.UplimitLabel.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                this.UplimitLabel.Visibility = System.Windows.Visibility.Hidden;
            }
            foreach (var item in Sensor.ShowValuePanel.ValueGrid)
            {
                string Modi = "0.";
                for (int i = 0; i < Sensor.DecimalPoint[item.ChannelNum]; i++)
                {
                    Modi = Modi + "0";
                }
                item.ValueLabel.Content = Sensor.GetValue(item.ChannelNum).ToString(Modi);
            }
            this.ShowTestTimeLabel.Content = Datas.Code[0].ToString();
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

            this.InputGrid.Column(0).Width = 130;
            this.InputGrid.Column(1).Width = (short)(this.TestStepFlexGrid.Width - 150);
            this.InputGrid.Cell(0, 0).Text = "输入项";
            this.InputGrid.Cell(0, 1).Text = "内容";

            this.ResultGrid.Column(0).Width = 130;
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
            Cmd.StopRun();
            MessageBox.Show(CurvePictureBoxGrid.ActualHeight.ToString(""));
            //System.Environment.Exit(0);
        }
    }
}
