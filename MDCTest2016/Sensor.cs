using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;

namespace MDCTest2016
{
    class Sensor
    {
        #region 试验机参数
        public static string MachineName = "";

        public static int CommunicationMode = 0;
        //示值栏本体
        public static MDCTest2016.ShowValueGrid ShowValuePanel;
        //示值栏的内容
        public static List<int> ChannelOfShowValuePanel=new List<int>();
        //厂家名
        public static string ManufactorName = "";

        //当前使用的负荷传感器和变形传感器
        public static int LoadSensorNum = 0;//当前负荷传感器的序号
        public static int LoadSelectedWorkSpace = 0;//上次负荷传感器的使用空间,0-上横梁，1-中上，2-中下，3-下横梁,4-拉压同向
        public static int DeformationNum = 0;//当前变形传感器的序号
        public static int DeformationSelectedWorkSpace = 0;

        #region 负荷传感器相关
        //使用空间能否使用.若为拉压同向时，以中横梁上空间为参考
        //0-上横梁，1-中上，2-中下，3-下横梁
        public static bool WorkSpaceTop;
        public static bool WorkSpaceMidUp;
        public static bool WorkSpaceMidDown;
        public static bool WorkSpaceBottom;
        #endregion

        //引伸计标距
        public static float Gauge;
        //变形传感器名
        public static int WorkSpaceUpPositive = 0;
        public static int WorkSpaceUpNegative = 0;
        public static int WorkSpaceDownPositive = 0;
        public static int WorkSpaceDownNegative = 0;

        //单位系数，即每个单位等于多少个基本单位，用于之后修改单位时使用
        //其中，力的基本单位为N，位移等基本单位为mm
        //从0开始，分别是力，位移，变形，大变形上，大变形下，扩展1-6，应力，应变，时间
        public static double[] UnitCoefficient = new double[14];
        /*
        try
            {
                string connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Machine.accdb;Persist Security Info=False;";
                string sql;
                OleDbCommand mycom;
                OleDbDataReader myReader;
                OleDbConnection mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from machine";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    MessageBox.Show(myReader["FullCode"].ToString());
                }
                mycon.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("读取配置文件Machine.accdb失败！\n错误信息：" + ex.Message, "警告");
            }
            */
        #endregion
            
        //单位名、通道名和设备名 从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static string[] UnitName = new string[11];
        public static string[] DevName = new string[15];
        public static string[] Channel = new string[15];

        //小数点位数 从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static int[] DecimalPoint = new int[11];

        //分辨率 从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static int[] FullCode = new int[11];

        //满量程 从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6,
        public static float[] FullScale = new float[11];

        //灵敏度 从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static float[] Sensitivity = new float[11];

        //校准表，以及校准方式从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static List<float[]>[] CaliList= new List<float[]>[11];

        //校准方式,0-缺省校准，1-物理校准
        public static int[] CaliMode=new int[11];

        //缺省校准系数从0开始分别是力，位移，变形，大变形上，大变形下，扩展1-6
        public static double[] ScaleValue=new double[11];

        //初始化机器
        public static void InitMachine()
        {
            for (int i = 0; i < 11; i++)
            {
                Sensor.CaliList[i] = new List<float[]>();
            }
            try
            {
                string connstr; string sql;
                OleDbCommand mycom;
                OleDbDataReader myReader;
                OleDbConnection mycon; 

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Machine.accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from machine";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    MachineName=myReader["MachineName"].ToString();
                    ManufactorName = myReader["ManufactorName"].ToString();
                    LoadSensorNum = int.Parse(myReader["LastLoadSensorNum"].ToString());
                    DeformationNum = int.Parse(myReader["LastDeformationNum"].ToString());
                    CommunicationMode= int.Parse(myReader["CommunicationMode"].ToString());
                }


                sql = "select * from ShowValuePanel";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    ChannelOfShowValuePanel.Add(int.Parse( myReader["channel"].ToString()));
                }

                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Channel0-"+ LoadSensorNum + ".accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    FullScale[0] = int.Parse(myReader["FullScale"].ToString());
                    Sensitivity[0] = float.Parse(myReader["Sensitivity"].ToString());
                    ScaleValue[0] = float.Parse(myReader["ScaleValue"].ToString());
                    CaliMode[0] = int.Parse(myReader["CaliMode"].ToString());
                    DevName[0] = myReader["DevName"].ToString();
                    Channel[0]= myReader["ChannelName"].ToString();
                    /*
                    WorkSpaceTop = int.Parse(myReader["WorkSpaceTop"].ToString());
                    WorkSpaceMidUp = int.Parse(myReader["WorkSpaceMidUp"].ToString());
                    WorkSpaceMidDown = int.Parse(myReader["WorkSpaceMidDown"].ToString());
                    WorkSpaceBottom = int.Parse(myReader["WorkSpaceBottom"].ToString());
                    */
                    UnitName[0]= myReader["UnitName"].ToString();
                    DecimalPoint[0] = int.Parse(myReader["DecimalPoint"].ToString());
                    UnitCoefficient[0] = 1;
                }
                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Channel2-" + DeformationNum + ".accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    FullScale[2] = int.Parse(myReader["FullScale"].ToString());
                    Sensitivity[2] = float.Parse(myReader["Sensitivity"].ToString());
                    ScaleValue[2] = float.Parse(myReader["ScaleValue"].ToString());
                    CaliMode[2] = int.Parse(myReader["CaliMode"].ToString());
                    DevName[2] = myReader["DevName"].ToString();
                    /*
                    WorkSpaceTop = int.Parse(myReader["WorkSpaceTop"].ToString());
                    WorkSpaceMidUp = int.Parse(myReader["WorkSpaceMidUp"].ToString());
                    WorkSpaceMidDown = int.Parse(myReader["WorkSpaceMidDown"].ToString());
                    WorkSpaceBottom = int.Parse(myReader["WorkSpaceBottom"].ToString());
                    */
                    UnitName[2] = myReader["UnitName"].ToString();
                    DecimalPoint[2] = int.Parse(myReader["DecimalPoint"].ToString());
                    Channel[2] = myReader["ChannelName"].ToString();
                    UnitCoefficient[2] = 1;
                }
                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Channel1.accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation ";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    DecimalPoint[1] = int.Parse(myReader["DecimalPoint"].ToString());
                    UnitCoefficient[1] = 1;
                    ScaleValue[1]= double.Parse(myReader["ScaleValue"].ToString());
                    DevName[1]= myReader["DevName"].ToString();
                    UnitName[1] = myReader["UnitName"].ToString();
                    Channel[1] = myReader["ChannelName"].ToString();
                }
                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Channel3.accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation ";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    DecimalPoint[3] = int.Parse(myReader["DecimalPoint"].ToString());
                    UnitCoefficient[3] = 1;
                    ScaleValue[3] = double.Parse(myReader["ScaleValue"].ToString());
                    DevName[3] = myReader["DevName"].ToString();
                    UnitName[3] = myReader["UnitName"].ToString();
                    Channel[3] = myReader["ChannelName"].ToString();
                }
                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Channel4.accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    DecimalPoint[4] = int.Parse(myReader["DecimalPoint"].ToString());
                    UnitCoefficient[4] = 1;
                    ScaleValue[4] = double.Parse(myReader["ScaleValue"].ToString());
                    DevName[4] = myReader["DevName"].ToString();
                    UnitName[4] = myReader["UnitName"].ToString();
                    Channel[4] = myReader["ChannelName"].ToString();
                }
                mycon.Close();

                connstr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=./Sensor/Ext.accdb;Persist Security Info=False;";
                mycon = new OleDbConnection(connstr);
                mycon.Open();
                sql = "select * from SensorInformation ";
                mycom = new OleDbCommand(sql, mycon);
                myReader = mycom.ExecuteReader();
                while (myReader.Read())
                {
                    int ExtNum = int.Parse(myReader["ID"].ToString());
                    ExtNum = ExtNum + 4;
                    ScaleValue[ExtNum]= double.Parse(myReader["ScaleValue"].ToString());
                    UnitName[ExtNum] = myReader["UnitName"].ToString();
                    DecimalPoint[ExtNum] = int.Parse(myReader["DecimalPoint"].ToString());
                    UnitCoefficient[ExtNum] = 1;
                    DevName[ExtNum] = myReader["DevName"].ToString();
                    UnitName[ExtNum] = myReader["UnitName"].ToString();
                    Channel[ExtNum] = myReader["ChannelName"].ToString();
                }
                mycon.Close();

                InitShowValuePanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("初始化设备时失败！\n错误信息：" + ex.Message, "警告");
            }
        }

        public static void InitShowValuePanel()
        {
            string[] s1 = new string[ChannelOfShowValuePanel.Count];
            string[] s2 = new string[ChannelOfShowValuePanel.Count];
            int[] cn = new int[ChannelOfShowValuePanel.Count];
            for (int i = 0; i < ChannelOfShowValuePanel.Count; i++)
            {
                s1[i] = Channel[ChannelOfShowValuePanel[i]];
                s2[i] = UnitName[ChannelOfShowValuePanel[i]];
                cn[i] = ChannelOfShowValuePanel[i];
            }
            ShowValuePanel = new ShowValueGrid(ChannelOfShowValuePanel.Count, s1, s2,cn);
        }

        public static double GetValue(int Channel)
        {
            switch (CaliMode[Channel])
            {
                case 0:
                    {
                        return (Datas.Code[Channel]-Datas.Zero[Channel])*Sensor.ScaleValue[Channel]*Math.Pow(-1,Datas.Symbol[Channel]);
                    }
                case 1:
                    {
                        return -1;
                    }
                default:
                    {
                        return -1;
                    }
            }
        }

        
        /// <summary>
        /// 由值得到码
        /// </summary>
        /// <param name="value">需要求码的值</param>
        /// <param name="ch">通道号</param>
        /// <returns></returns>
        public static int GetCode(double value,int ch)
        {
            if (CaliMode[ch] == 0)
            {
                return (int)(value / ScaleValue[ch] * Math.Pow(-1, Datas.Symbol[ch]));
            }
            else
            {
                return -1;
                //return GetCodeFromCaliList(v, ch);
            }
        }
    }
}
