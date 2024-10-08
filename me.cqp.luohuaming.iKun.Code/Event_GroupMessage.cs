﻿using me.cqp.luohuaming.iKun.PublicInfos;
using me.cqp.luohuaming.iKun.Sdk.Cqp.EventArgs;
using System;
using System.Linq;

namespace me.cqp.luohuaming.iKun.Code
{
    public class Event_GroupMessage
    {
        public static FunctionResult GroupMessage(CQGroupMessageEventArgs e)
        {
            FunctionResult result = new FunctionResult()
            {
                SendFlag = false
            };
            try
            {
                if (AppConfig.Groups.Any(x => x == e.FromGroup) is false)
                {
                    return result;
                }
                foreach (var item in MainSave.Instances.Where(item => item.Judge(e.Message.Text)))
                {
                    return item.Execute(e);
                }
                return result;
            }
            catch (Exception exc)
            {
                MainSave.CQLog.Info("异常抛出", exc.Message + exc.StackTrace);
                return result;
            }
        }
    }
}