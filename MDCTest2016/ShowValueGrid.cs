using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MDCTest2016
{
    class ShowValueGrid:System.Windows.Controls.Grid
    {
        public int _NumOfValueGird;
        public ValueGrid[] ValueGrid;
        /// <summary>
        /// 初始化示值区域时，必须要带区中显示区域的个数
        /// </summary>
        /// <param name="NumOfValueGird">一共有几个显示区域</param>
        public ShowValueGrid(int NumOfValueGird,string[] ChannelName,string[] ChannelUnit,int[] ChannelNum)
        {
            _NumOfValueGird = NumOfValueGird;
            ValueGrid = new MDCTest2016.ValueGrid[_NumOfValueGird];
            //return;
            for (int i = 0; i < ValueGrid.Length; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition());
                this.ColumnDefinitions[i].Width = new System.Windows.GridLength(1, System.Windows.GridUnitType.Star);
                ValueGrid[i] = new ValueGrid(ChannelName[i], ChannelUnit[i]);
                ValueGrid[i].SetValue(Grid.ColumnProperty, i);
                ValueGrid[i].SetValue(Grid.RowProperty, 2);
                ValueGrid[i].ChannelNum = ChannelNum[i];
                this.Children.Add(ValueGrid[i]);
            }
        }
    }

    class ValueGrid : System.Windows.Controls.Grid
    {
        public Label ValueLabel;
        public Label MaxValueLabel;
        public Label MiniValueLabel;
        public Label ChannelNameLabel;
        public Label ChannelUnitLabel;
        public Button ZeroBtn;
        public string _ChannelName;
        public string _ChannelUnit;
        Grid ValueBack;
        public int ChannelNum;

        public ValueGrid(string ChannelName,string ChannelUnit)
        {
            _ChannelName = ChannelName;
            _ChannelUnit = ChannelUnit;
            this.ValueLabel = new Label();
            this.ValueLabel.Margin = new Thickness(4, 4, 4, 4);
            this.ValueLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.ValueLabel.VerticalAlignment = VerticalAlignment.Center;
            this.MaxValueLabel = new Label();
            this.MaxValueLabel.Margin = new Thickness(4, 4, 4, 4);
            this.MaxValueLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.MaxValueLabel.VerticalAlignment = VerticalAlignment.Center;
            this.MiniValueLabel = new Label();
            this.MiniValueLabel.Margin = new Thickness(4, 4, 4, 4);
            this.MiniValueLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.MiniValueLabel.VerticalAlignment = VerticalAlignment.Center;
            this.ChannelNameLabel = new Label();
            this.ChannelNameLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.ChannelNameLabel.VerticalAlignment = VerticalAlignment.Center;
            this.ChannelUnitLabel = new Label();
            this.ChannelUnitLabel.HorizontalAlignment = HorizontalAlignment.Center;
            this.ChannelUnitLabel.VerticalAlignment = VerticalAlignment.Center;
            this.ZeroBtn = new Button();
            this.ZeroBtn.Margin = new Thickness(4,4,4,4);

            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions.Add(new ColumnDefinition());
            this.ColumnDefinitions[0].Width = new System.Windows.GridLength(70, System.Windows.GridUnitType.Star);
            this.ColumnDefinitions[1].Width = new System.Windows.GridLength(15, System.Windows.GridUnitType.Star);
            this.ColumnDefinitions[2].Width = new System.Windows.GridLength(15, System.Windows.GridUnitType.Star);

            

            ValueBack = new Grid();
            ValueBack.Margin = new Thickness(4, 4, 4, 4);
            ValueBack.Background= new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xf2,0xf2,0xf2));
            ValueBack.Children.Add(ValueLabel);
            ValueBack.SetValue(Grid.ColumnProperty, 0);
            ValueBack.SetValue(Grid.RowProperty, 0);
            ValueLabel.Content = "0";
            ValueLabel.FontSize = 24;
            this.Children.Add(ValueBack);

            Grid ng = new Grid();
            ng.SetValue(Grid.ColumnProperty, 1);
            ng.SetValue(Grid.RowProperty, 0);
            ng.RowDefinitions.Add(new RowDefinition());
            ng.RowDefinitions.Add(new RowDefinition());

            Grid tmg;
            tmg = new Grid();
            tmg.Margin = new Thickness(4, 4, 4, 4);
            tmg.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xd1, 0xee, 0xee));
            tmg.Children.Add(ChannelNameLabel);
            tmg.SetValue(Grid.ColumnProperty, 0);
            tmg.SetValue(Grid.RowProperty, 0);
            ChannelNameLabel.Content = _ChannelName;
            ng.Children.Add(tmg);

            tmg = new Grid();
            tmg.Margin = new Thickness(4, 4, 4, 4);
            tmg.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromRgb(0xd1, 0xee, 0xee));
            tmg.SetValue(Grid.ColumnProperty, 0);
            tmg.SetValue(Grid.RowProperty, 1);
            tmg.Children.Add(ChannelUnitLabel);
            ChannelUnitLabel.Content = _ChannelUnit;
            ng.Children.Add(tmg);

            this.Children.Add(ng);

            ZeroBtn.SetValue(Grid.ColumnProperty, 2);
            ZeroBtn.SetValue(Grid.RowProperty, 0);
            ZeroBtn.Content = "清零";
            this.Children.Add(ZeroBtn);
            this.ZeroBtn.Click += ZeroBtn_Click;
        }

        private void ZeroBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show("清零通道：" + ChannelNum);
        }
    }
}
