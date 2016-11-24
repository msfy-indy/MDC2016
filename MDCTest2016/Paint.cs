using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MDCTest2016
{
    class paint
    {
        //画线用工具，不释放，一直使用？！
        public static Graphics[] StaticG = new Graphics[4];//画在窗体上面的
        public static Graphics[] StaticImgG = new Graphics[4];//画在窗体里面的IMG上的
        public static Color DrawColor = Color.Black;
        //画线特征点
        public static bool IsNeedDrawPoints = false;
        public static bool IsNeedDrawRp = false;
        public static bool IsNeedDrawRt = false;
        public static bool IsNeedDrawEstart = false;
        public static bool IsNeedDrawEend = false;
        public static bool IsNeedDrawMaxLoad = false;
        public static bool IsNeedDrawUpYield = false;
        public static bool IsNeedDrawDownYield = false;
        public static bool IsNeedDrawPointsMsg = false;
        //是否需要自动适配大小
        public static bool IsNeedJudges = false;
        //是否用户绘制报告
        public static bool IsReportDraw = false;
        //需要绘制的pictureBox对象列表
        public static PictureBox[] p = new PictureBox[4];
        //需要还原的曲线的序号
        public static int NeedReductionNum = 0;
        //坐标原点的位置（相对于左下角，y轴正方向朝上）,单位为像素点
        public static int[] xDeviceOrg = new int[4];
        public static int[] yDeviceOrg = new int[4];
        //坐标的显示方式，从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6，试验已运行时间，应力，应变
        public static int[] xIndex = new int[4];
        public static int[] yIndex = new int[4];
        //一个像素点代表多少个单位
        public static float[] xPixelLength = new float[4];
        public static float[] yPixelLength = new float[4];
        //绘图区域被按钮按下的坐标
        public static int[] xPosition = new int[4];
        public static int[] yPosition = new int[4];
        //点击图片之后记录相应的坐标点
        public static void SetXY1stPostion(int index, int x, int y)
        {
            xPosition[index] = x;
            yPosition[index] = y;
        }
        //移动原点的位置并且重新绘制曲线区域
        public static void ChangeDeviceOrg(int index, int x, int y)
        {
            if (Math.Abs(x - xPosition[index]) < 15 && Math.Abs(y - yPosition[index]) < 15)
            {
                return;
            }
            IsNeedDrawPoints = true;
            IsReportDraw = true;
            xDeviceOrg[index] = xDeviceOrg[index] - xPosition[index] + x;
            yDeviceOrg[index] = yDeviceOrg[index] + yPosition[index] - y;
            initPicture(index, true, 0, 0, true);



            //DrawCurveFromList(index);



            IsReportDraw = false;
        }
        //放大或者缩小曲线
        public static void ZoomInOrOutP(int index, int zoom, int x, int y)
        {
            if (zoom < 0)
            {
                xPixelLength[index] = xPixelLength[index] * 2;
                yPixelLength[index] = yPixelLength[index] * 2;
                xDeviceOrg[index] = x + (xDeviceOrg[index] - x) / 2;
                yDeviceOrg[index] = p[index].Height - y + (yDeviceOrg[index] - p[index].Height + y) / 2;
            }
            if (zoom > 0)
            {
                xPixelLength[index] = xPixelLength[index] / 2;
                yPixelLength[index] = yPixelLength[index] / 2;
                xDeviceOrg[index] = x + (xDeviceOrg[index] - x) * 2;
                yDeviceOrg[index] = p[index].Height - y + (yDeviceOrg[index] - p[index].Height + y) * 2;
            }
            IsNeedDrawPoints = true;
            IsReportDraw = true;
            initPicture(index, true, 0, 0, true);



            //DrawCurveFromList(index);



            IsReportDraw = false;
        }
        //还原曲线
        public static void ReductionPicture()
        {
            xDeviceOrg[NeedReductionNum] = 24;
            yDeviceOrg[NeedReductionNum] = 24;
            initPicture(0, true, 0, 0, true);
            //paint.DrawCurveFromList(NeedReductionNum);
        }

        //清空一个绘图区域
        //初始化一个绘图区域
        public static void initPicture(int index, bool IsAutoDraw, float xPL, float yPL, bool IsDrawOnImage)
        {
            try
            {
                Graphics g;
                if (IsDrawOnImage)
                {
                    g = StaticImgG[index];
                }
                else
                {
                    g = StaticG[index];
                }
                lock (g)
                {
                    g.Clear(Color.FromArgb(199, 237, 204));
                    if (!IsAutoDraw)
                    {
                        xPixelLength[index] = xPL;
                        yPixelLength[index] = yPL;
                    }
                    Pen t = new Pen(Color.White);
                    int l = 0;
                    double coordinatetmp = 0;
                    while (l < 10)
                    {
                        g.DrawLine(t, 0, (p[index].Height - 24) * (l + 1) / 10, p[index].Width, (p[index].Height - 24) * (l + 1) / 10);
                        g.DrawLine(t, (p[index].Width - 24) * l / 10 + 24, 0, (p[index].Width - 24) * l / 10 + 24, p[index].Height);
                        coordinatetmp = (p[index].Height - (p[index].Height - 24) * (l + 1) / 10 - yDeviceOrg[index]) * yPixelLength[index];
                        coordinatetmp = coordinatetmp * Sensor.UnitCoefficient[yIndex[index]];
                        if (coordinatetmp == 0)
                        {
                            g.DrawString(coordinatetmp.ToString("0"), new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), 0, (p[index].Height - 24) * (l + 1) / 10);
                        }
                        else
                        {
                            g.DrawString(coordinatetmp.ToString("0.00"), new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), 0, (p[index].Height - 24) * (l + 1) / 10);
                        }
                        coordinatetmp = ((p[index].Width - 24) * l / 10 + 24 - xDeviceOrg[index]) * xPixelLength[index];
                        coordinatetmp = coordinatetmp * Sensor.UnitCoefficient[xIndex[index]];
                        if (coordinatetmp == 0)
                        {
                            g.DrawString(coordinatetmp.ToString("0"), new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), (p[index].Width - 24) * l / 10 + 30, p[index].Height - 24);
                        }
                        else
                        {
                            g.DrawString(coordinatetmp.ToString("0.00"), new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), (p[index].Width - 24) * l / 10 + 30, p[index].Height - 24);
                        }
                        l = l + 1;
                    }
                    t = new Pen(Color.Black);
                    g.DrawLine(t, xDeviceOrg[index], p[index].Height, xDeviceOrg[index], 0);
                    g.DrawLine(t, 0, p[index].Height - yDeviceOrg[index], p[index].Width, p[index].Height - yDeviceOrg[index]);
                    if (xDeviceOrg[index] != 24 && yDeviceOrg[0] != 24)
                    {
                        g.DrawString("0", new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), xDeviceOrg[index], p[index].Height - yDeviceOrg[index]);
                    }
                    g.DrawString(Sensor.Channel[yIndex[index]] + "\n" + "(" + Sensor.UnitName[yIndex[index]] + ")", new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), 0, 0);
                    g.DrawString(Sensor.Channel[xIndex[index]] + "\n" + "(" + Sensor.UnitName[xIndex[index]] + ")", new System.Drawing.Font("仿宋", 9F), new SolidBrush(Color.Black), (p[index].Width - 24) * 9 / 10 + 24, p[index].Height - 48);
                }
                p[index].Refresh();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        /*
        //画一个点
        public static void DrawOneDot(int index, double[] Lastdata, double[] Nextdata, bool DrawInImage, Color c)
        {
            double LastxValue = 0, LastyValue = 0, NextxValue = 0, NextyValue = 0;
            if (xIndex[index] < 13)
            {
                LastxValue = Lastdata[xIndex[index]];
                NextxValue = Nextdata[xIndex[index]];
            }
            if (yIndex[index] < 13)
            {
                LastyValue = Lastdata[yIndex[index]];
                NextyValue = Nextdata[yIndex[index]];

            }
            if (xIndex[index] == 13)
            {
                LastxValue = Lastdata[0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                NextxValue = Nextdata[0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
            }
            if (xIndex[index] == 14)
            {
                switch (TestRunCtrl.MainTestProject.DeformSensorName)
                {
                    case 0:
                        {
                            LastxValue = Lastdata[1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextxValue = Nextdata[1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 1:
                        {
                            LastxValue = Lastdata[2] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextxValue = Nextdata[2] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 2:
                        {
                            LastxValue = (Lastdata[3] + Lastdata[4]) * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextxValue = (Nextdata[3] + Nextdata[4]) * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 3:
                        {
                            LastxValue = Lastdata[3] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextxValue = Nextdata[3] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 4:
                        {
                            LastxValue = Lastdata[4] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextxValue = Nextdata[4] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            if (yIndex[index] == 13)
            {
                LastyValue = Lastdata[0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                NextyValue = Nextdata[0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
            }
            if (yIndex[index] == 14)
            {
                switch (TestRunCtrl.MainTestProject.DeformSensorName)
                {
                    case 0:
                        {
                            LastyValue = Lastdata[1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextyValue = Nextdata[1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 1:
                        {
                            LastyValue = Lastdata[2] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextyValue = Nextdata[2] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 2:
                        {
                            LastyValue = (Lastdata[3] + Lastdata[4]) * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextyValue = (Nextdata[3] + Nextdata[4]) * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 3:
                        {
                            LastyValue = Lastdata[3] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextyValue = Nextdata[3] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    case 4:
                        {
                            LastyValue = Lastdata[4] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            NextyValue = Nextdata[4] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            //在这里继续写
            try
            {
                Pen t = new Pen(c);
                float x1, y1, x2, y2;
                bool NeedRedraw1 = false;
                if (xPixelLength[index] == 0)
                {
                    x1 = xDeviceOrg[index];
                    x2 = xDeviceOrg[index];
                }
                else
                {
                    x1 = (float)(LastxValue / xPixelLength[index] + xDeviceOrg[index]);
                    x2 = (float)(NextxValue / xPixelLength[index] + xDeviceOrg[index]);
                }
                if (yPixelLength[index] == 0)
                {
                    y1 = p[index].Height - yDeviceOrg[index];
                    y2 = p[index].Height - yDeviceOrg[index];
                }
                else
                {
                    y1 = (float)(p[index].Height - LastyValue / yPixelLength[index] - yDeviceOrg[index]);
                    y2 = (float)(p[index].Height - NextyValue / yPixelLength[index] - yDeviceOrg[index]);
                }



                if (TestRunCtrl.IsOnTest)
                {
                    if (x2 < 0 || x2 > p[index].Width * 0.9)
                    {
                        xPixelLength[index] = (float)(xPixelLength[index] * 2);
                        NeedRedraw1 = true;
                    }
                    if (y2 > p[index].Height || y2 < p[index].Height * 0.1)
                    {
                        yPixelLength[index] = (float)(yPixelLength[index] * 2);

                        NeedRedraw1 = true;
                    }
                }
                if (NeedRedraw1)
                {
                    ThreadPool.QueueUserWorkItem(DrawCurveFromList, index);
                }
                if (IsReportDraw)
                {
                    lock (StaticImgG[index])
                    {
                        StaticImgG[index].DrawLine(t, x1, y1, x2, y2);
                    }
                }
                else
                {
                    lock (StaticG[index])
                    {
                        StaticG[index].DrawLine(t, x1, y1, x2, y2);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        
        /// <summary>
        /// 根据数据链表来画曲线
        /// </summary>
        /// <param name="o">需要绘制的链表序号</param>
        public static void DrawCurveFromList(object o)
        {
            int index = (int)o;
            if (TestRunCtrl.IsOnTest)
            {
                initPicture(index, true, 0, 0, true);
            }
            Queue<double[]> dataqueue = new Queue<double[]>();
            int i = 0, j = 0;
            double[] MaxValues = new double[15];
            while (i < TestRunCtrl.MainTestProject.DataList.Count)
            {
                dataqueue.Enqueue(TestRunCtrl.MainTestProject.DataList[i]);
                j = 0;
                while (j < 13)
                {
                    if (MaxValues[j] < Math.Abs(TestRunCtrl.MainTestProject.DataList[i][j]))
                    {
                        MaxValues[j] = Math.Abs(TestRunCtrl.MainTestProject.DataList[i][j]);
                    }
                    j = j + 1;
                }
                if (MaxValues[13] < Math.Abs(TestRunCtrl.MainTestProject.DataList[i][0] / TestRunCtrl.MainTestProject.BasicInformation.Area))
                {
                    MaxValues[13] = Math.Abs(TestRunCtrl.MainTestProject.DataList[i][0] / TestRunCtrl.MainTestProject.BasicInformation.Area);
                }
                if (MaxValues[14] < Math.Abs(TestRunCtrl.MainTestProject.DataList[i][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength))
                {
                    MaxValues[14] = Math.Abs(TestRunCtrl.MainTestProject.DataList[i][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength);
                }
                i = i + 1;
            }
            if (IsNeedJudges)
            {
                if (TestRunCtrl.IsOnTest)
                {
                    xPixelLength[index] = (float)(MaxValues[xIndex[index]] * 2 / (p[index].Width - xDeviceOrg[index]));
                    yPixelLength[index] = (float)(MaxValues[yIndex[index]] * 2 / (p[index].Height - yDeviceOrg[index]));
                }
                else
                {
                    xPixelLength[index] = (float)(MaxValues[xIndex[index]] * 1.2 / (p[index].Width - xDeviceOrg[index]));
                    yPixelLength[index] = (float)(MaxValues[yIndex[index]] * 1.2 / (p[index].Height - yDeviceOrg[index]));
                }
                if (xPixelLength[index] == 0)
                {
                    xPixelLength[index] = (float)0.01;
                }
                if (yPixelLength[index] == 0)
                {
                    yPixelLength[index] = (float)0.01;
                }
                initPicture(index, true, 0, 0, true);
                IsNeedJudges = false;
            }
            double[] LastPoint = new double[13], NextPoint = new double[13];
            if (dataqueue.Count > 0)
            {
                LastPoint = dataqueue.First();
            }
            while (dataqueue.Count != 0)
            {
                NextPoint = dataqueue.Dequeue();
                DrawOneDot(index, LastPoint, NextPoint, IsReportDraw, DrawColor);
                LastPoint = NextPoint;
            }
            if (IsNeedDrawPoints)
            {
                DrawPoints(index);
            }
            IsNeedDrawPoints = false;
            if (IsReportDraw)
            {
                p[index].Refresh();
            }
        }

        public static void DrawPoints(int index)
        {
            try
            {
                double Xzb = 0, Yzb = 0;
                if (IsNeedDrawRp && TestRunCtrl.MainTestProject.BasicInformation.RpPos >= TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RpPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.Brown);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "Rp0.2" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.Brown);
                    }
                }
                if (IsNeedDrawRt && TestRunCtrl.MainTestProject.BasicInformation.RtPos >= TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.RtPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.Violet);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "Rt0.5" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.Violet);
                    }
                }
                if (IsNeedDrawEstart)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticBeginDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.Green);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "弹性起点" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.Green);
                    }
                }
                if (IsNeedDrawEend)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.ElasticEndDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.DarkGreen);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "弹性终点" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.DarkGreen);
                    }
                }
                if (IsNeedDrawMaxLoad)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.MaxLoadDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.Red);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "最大力" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.Red);
                    }
                }
                if (IsNeedDrawUpYield)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.UpYieldDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.Blue);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "上屈服点" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.Blue);
                    }
                }
                if (IsNeedDrawDownYield)
                {
                    if (xIndex[index] < 13)
                    {
                        Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][xIndex[index]];
                    }
                    else
                    {
                        if (xIndex[index] == 13)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (xIndex[index] == 14)
                        {
                            Xzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    if (yIndex[index] < 13)
                    {
                        Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][yIndex[index]];
                    }
                    else
                    {
                        if (yIndex[index] == 13)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][0] / TestRunCtrl.MainTestProject.BasicInformation.Area;
                        }
                        if (yIndex[index] == 14)
                        {
                            Yzb = TestRunCtrl.MainTestProject.DataList[TestRunCtrl.MainTestProject.BasicInformation.DownYieldDotPos][1] * 100 / TestRunCtrl.MainTestProject.BasicInformation.GaugeLength;
                        }
                    }
                    DrawOneCircles(index, Xzb, Yzb, true, Color.DarkBlue);
                    if (IsNeedDrawPointsMsg)
                    {
                        DrawStringFormValue(index, Xzb, Yzb, "下屈服点" + "\nX坐标：" + Xzb.ToString("0.00") + "\nY坐标：" + Yzb.ToString("0.00"), true, Color.DarkBlue);
                    }
                }
                IsNeedDrawPoints = false;
            }
            catch (Exception ex)
            {
                string path = "log.txt";//文件的路径，保证文件存在。
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("在进行特征点显示时，发生异常，异常时间：" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "\n异常信息:" + ex.Message);
                sw.Close();
                fs.Close();
            }
        }
        */
        public static void DrawStringFormValue(int index, double x, double y, string s, bool DrawInImage, Color color)
        {
            try
            {
                float yxx = 0, yxy = 0;
                yxx = (float)(x / xPixelLength[index] + xDeviceOrg[index]);
                yxy = (float)(p[index].Height - y / yPixelLength[index] - yDeviceOrg[index]);
                if (IsReportDraw)
                {
                    lock (StaticImgG[index])
                    {
                        StaticImgG[index].DrawString(s, new System.Drawing.Font("仿宋", 9F), new SolidBrush(color), yxx + 5, yxy + 5);
                    }
                }
                else
                {
                    lock (StaticG[index])
                    {
                        StaticG[index].DrawString(s, new System.Drawing.Font("仿宋", 9F), new SolidBrush(color), yxx + 5, yxy + 5);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 绘制一个圆圈
        /// </summary>
        /// <param name="index">需要绘制到的图片序号</param>
        /// <param name="x">x值</param>
        /// <param name="y">y值</param>
        /// <param name="DrawInImage">是否画在img上</param>
        /// <param name="color">绘制的颜色</param>
        public static void DrawOneCircles(int index, double x, double y, bool DrawInImage, Color color)
        {
            try
            {
                System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(color);
                float yxx = 0, yxy = 0;
                yxx = (float)(x / xPixelLength[index] + xDeviceOrg[index]);
                //y2 = (float)(p[index].Height - NextyValue / yPixelLength[index] - yDeviceOrg[index]);
                yxy = (float)(p[index].Height - y / yPixelLength[index] - yDeviceOrg[index]);
                if (IsReportDraw)
                {
                    lock (StaticImgG[index])
                    {
                        StaticImgG[index].FillEllipse(myBrush, yxx - 5, yxy - 5, 10, 10);
                    }
                }
                else
                {
                    lock (StaticG[index])
                    {
                        StaticG[index].FillEllipse(myBrush, yxx - 5, yxy - 5, 10, 10);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
