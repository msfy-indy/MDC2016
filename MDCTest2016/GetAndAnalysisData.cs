using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDCTest2016
{
    class GetAndAnalysisData
    {
        public static void GetData(object state)
        {
            long OldTime = 0, TimeT = 0;
            Datas.IsOnlie = Communication.IsOpen();
            while (true)
            {
                lock (Datas.Original)
                {
                    System.Threading.Thread.Sleep(18);
                    //获取原始数据及解析，具体请参考通信协议
                    Communication.Read(Datas.Original);
                    Datas.Dwtime = (Datas.Original[0] << 24) | (Datas.Original[1] << 16) | (Datas.Original[2] << 8) | (Datas.Original[3]);
                    Datas.Day = (Datas.Original[4] << 8) | (Datas.Original[5]);
                    Datas.Code[1] = (Datas.Original[6] << 24) | (Datas.Original[7] << 16) | (Datas.Original[8] << 8) | (Datas.Original[9]);
                    Datas.Code[3] = (((short)(SByte)Datas.Original[10]) << 16) | (Datas.Original[11] << 8) | (Datas.Original[12]);
                    Datas.Code[4] = (((short)(SByte)Datas.Original[13]) << 16) | (Datas.Original[14] << 8) | (Datas.Original[15]);
                    Datas.Code[0] = (((short)(SByte)Datas.Original[16]) << 16) | (Datas.Original[17] << 8) | (Datas.Original[18]);
                    Datas.Code[2] = (((short)(SByte)Datas.Original[19]) << 16) | (Datas.Original[20] << 8) | (Datas.Original[21]);
                    Datas.Code[5] = (((short)(SByte)Datas.Original[22]) << 16) | (Datas.Original[23] << 8) | (Datas.Original[24]);
                    Datas.Code[6] = (((short)(SByte)Datas.Original[25]) << 16) | (Datas.Original[26] << 8) | (Datas.Original[27]);
                    Datas.Code[7] = (((short)(SByte)Datas.Original[28]) << 16) | (Datas.Original[29] << 8) | (Datas.Original[30]);
                    Datas.Code[8] = (((short)(SByte)Datas.Original[31]) << 16) | (Datas.Original[32] << 8) | (Datas.Original[33]);
                    Datas.Code[9] = (((short)(SByte)Datas.Original[34]) << 16) | (Datas.Original[35] << 8) | (Datas.Original[36]);
                    Datas.Code[10] = (((short)(SByte)Datas.Original[37]) << 16) | (Datas.Original[38] << 8) | (Datas.Original[39]);
                    Datas.Status0 = (Datas.Original[40] << 24) | (Datas.Original[41] << 16) | (Datas.Original[42] << 8) | (Datas.Original[43]);
                    Datas.SpeedCode[1] = (((short)(SByte)Datas.Original[44]) << 16) | (Datas.Original[45] << 8) | (Datas.Original[46]);
                    Datas.SpeedCode[0] = (((short)(SByte)Datas.Original[47]) << 16) | (Datas.Original[48] << 8) | (Datas.Original[49]);
                    Datas.SpeedCode[2] = (((short)(SByte)Datas.Original[50]) << 16) | (Datas.Original[51] << 8) | (Datas.Original[52]);
                    Datas.FeedBackNum = Datas.Original[53];
                    Datas.FeedBack = (Datas.Original[54] << 24) | (Datas.Original[55] << 16) | (Datas.Original[56] << 8) | (Datas.Original[57]);
                    Datas.OpenValue = (((int)(SByte)Datas.Original[58] << 8)) | (Datas.Original[59]);
                    Datas.Status1 = (Datas.Original[60] << 24) | (Datas.Original[61] << 16) | (Datas.Original[62] << 8) | (Datas.Original[63]);

                    //状态字0的解析
                    //是否在运行中
                    if ((Datas.Original[43] & 0x02) == 0x02)
                    {
                        Datas.IsRunning = true;
                    }
                    else
                    {
                        Datas.IsRunning = false;
                    }
                    if ((Datas.Original[43] & 0x01) == 0x01)
                    {
                        Datas.IsRunning = false;
                    }

                    //运行是否为试验运行
                    if ((Datas.Original[40] & 0x01) == 0x01)
                    {
                        Datas.IsOnTestingRunning = true;
                    }
                    else
                    {
                        Datas.IsOnTestingRunning = false;
                    }

                    //运行方向
                    if ((Datas.Original[41] & 0x40) == 0x40)
                    {
                        Datas.rundir = 1;
                    }
                    else
                    {
                        Datas.rundir = -1;
                    }

                    //是否为校准状态
                    if ((Datas.Original[43] & 0x80) == 0x80)
                    {
                        Datas.IsCali = true;
                    }
                    else
                    {
                        Datas.IsCali = false;
                    }

                    //控制方式
                    if ((Datas.Original[42] & 0x01) == 0x01)
                    {
                        Datas.CtrlMode = 1;
                    }
                    else
                    {
                        if ((Datas.Original[42] & 0x02) == 0x02)
                        {
                            Datas.CtrlMode = 3;
                        }
                        else
                        {
                            if ((Datas.Original[42] & 0x04) == 0x04)
                            {
                                Datas.CtrlMode = 2;
                            }
                            else
                            {
                                Datas.CtrlMode = 0;
                            }
                        }
                    }

                    //上下限位
                    if ((Datas.Original[42] & 0x08) == 0x08)
                    {
                        Datas.UpLimit = true;
                    }
                    else
                    {
                        Datas.UpLimit = false;
                    }
                    if ((Datas.Original[42] & 0x10) == 0x10)
                    {
                        Datas.LowLimit = true;
                    }
                    else
                    {
                        Datas.LowLimit = false;
                    }
                    //急停
                    if ((Datas.Original[62] & 0x08) == 0x08)
                    {
                        Datas.EmergencyStop = true;
                    }
                    else
                    {
                        Datas.EmergencyStop = false;
                    }

                    //获取反馈的参数
                    if (Datas.FeedBackNum > 127 && Datas.FeedBackNum < 176)
                    {
                        Datas.FeedBackParam[Datas.FeedBackNum] = Datas.FeedBack;
                    }

                    lock (Cmd.SendQueue)
                    {
                        if (Cmd.SendQueue.Count > 0)
                        {
                            Communication.Write(Cmd.SendQueue.Dequeue());
                        }
                    }
                }
            }

        }
    }
}


