using me.cqp.luohuaming.iKun.PublicInfos;
using me.cqp.luohuaming.iKun.PublicInfos.Items;
using me.cqp.luohuaming.iKun.PublicInfos.Models;
using me.cqp.luohuaming.iKun.PublicInfos.Models.Results;
using me.cqp.luohuaming.iKun.Sdk.Cqp;
using me.cqp.luohuaming.iKun.Sdk.Cqp.EventArgs;
using me.cqp.luohuaming.iKun.Sdk.Cqp.Interface;
using System;
using System.IO;
using System.Reflection;

namespace me.cqp.luohuaming.iKun.Code
{
    public class Event_StartUp : ICQStartup
    {
        public void CQStartup(object sender, CQStartupEventArgs e)
        {
            MainSave.AppDirectory = e.CQApi.AppDirectory;
            MainSave.CQApi = e.CQApi;
            MainSave.CQLog = e.CQLog;
            MainSave.ImageDirectory = CommonHelper.GetAppImageDirectory();
            foreach (var item in Assembly.GetAssembly(typeof(Event_GroupMessage)).GetTypes())
            {
                if (item.IsInterface)
                    continue;
                foreach (var instance in item.GetInterfaces())
                {
                    if (instance == typeof(IOrderModel))
                    {
                        IOrderModel obj = (IOrderModel)Activator.CreateInstance(item);
                        if (obj.ImplementFlag == false)
                            break;
                        MainSave.Instances.Add(obj);
                    }
                }
            }

            e.CQLog.Info("初始化", "加载配置");
            AppConfig appConfig = new(Path.Combine(MainSave.AppDirectory, "Config.json"));
            appConfig.LoadConfig();
            appConfig.EnableAutoReload();

            ItemConfig itemConfig = new(Path.Combine(MainSave.AppDirectory, "Items.json"));
            itemConfig.LoadConfig();
            itemConfig.EnableAutoReload();

            e.CQLog.Info("初始化", "初始化日志");
            Logger.Init();

            e.CQLog.Info("初始化", "创建数据库");
            SQLHelper.CreateDB();
            Kun.InitiazlizeRandom();

            e.CQLog.Info("初始化", "加载挂机列表");
            AutoPlay.AutoPlayFinished -= AutoPlay_AutoPlayFinished;
            AutoPlay.AutoPlayFinished += AutoPlay_AutoPlayFinished;
            AutoPlay.LoadAutoPlays();

            e.CQLog.Info("初始化", "启动天罚线程");
            new RandomPunish().Start();

            e.CQLog.Info("初始化", "初始化完成");
        }

        private void AutoPlay_AutoPlayFinished(AutoPlay autoPlay, AutoPlayResult autoPlayResult, Kun kun)
        {
            try
            {
                if (autoPlayResult == null || kun.Level <= 0 || kun.Weight <= 0 || kun.Abandoned)
                {
                    return;
                }
                if (AppConfig.Groups.Contains(autoPlay.GroupId))
                {
                    string msg = "";
                    switch (autoPlay.AutoPlayType)
                    {
                        case PublicInfos.Enums.AutoPlayType.Exp:
                            if (autoPlayResult.Dead)
                            {
                                msg = string.Format(CQApi.CQCode_At(kun.PlayerID) + AppConfig.ReplyAutoPlayFinishedButDead, kun.ToString(), autoPlayResult.Duration.TotalHours, autoPlayResult.Increment.ToShortNumber());
                            }
                            else
                            {
                                msg = string.Format(CQApi.CQCode_At(kun.PlayerID) + AppConfig.ReplyAutoPlayFinished, kun.ToString(), autoPlayResult.Duration.TotalHours, autoPlayResult.Increment.ToShortNumber(), kun.Weight.ToShortNumber());
                                if (autoPlayResult.WeightLimit)
                                {
                                    msg += $"\n{AppConfig.ReplyWeightLimit}";
                                }
                            }
                            break;
                        case PublicInfos.Enums.AutoPlayType.Coin:
                            if (!kun.Alive)
                            {
                                return;
                            }
                            msg = string.Format(CQApi.CQCode_At(kun.PlayerID) + AppConfig.ReplyWorkingFinished, kun.ToString(), autoPlayResult.Duration.TotalHours, (int)autoPlayResult.Increment, autoPlayResult.CurrentCoin);
                            break;
                        default:
                            break;
                    }
                    if (!string.IsNullOrEmpty(msg))
                    {
                        MainSave.CQApi.SendGroupMessage(autoPlay.GroupId, msg);
                    }
                }
            }
            catch (Exception ex)
            {
                MainSave.CQLog.Error("挂机结算", $"异常发生：{ex.Message}\n{ex.StackTrace}");
            }
        }
    }
}