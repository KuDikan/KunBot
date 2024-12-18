﻿using me.cqp.luohuaming.iKun.PublicInfos.Models;
using SqlSugar;
using System;
using System.IO;

namespace me.cqp.luohuaming.iKun.PublicInfos
{
    public static class SQLHelper
    {
        public static string DBPath => Path.Combine(MainSave.AppDirectory, "data.db");

        public static SqlSugarClient GetInstance()
        {
            return new(new ConnectionConfig()
            {
                ConnectionString = $"data source={DBPath}",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = false,
                InitKeyType = InitKeyType.Attribute,
            });
        }

        public static bool CreateDB()
        {
            try
            {
                using var db = GetInstance();
                db.DbMaintenance.CreateDatabase(DBPath);
                db.CodeFirst.InitTables(typeof(InventoryItem));
                db.CodeFirst.InitTables(typeof(Kun));
                db.CodeFirst.InitTables(typeof(Player));
                db.CodeFirst.InitTables(typeof(AutoPlay));
                db.CodeFirst.InitTables(typeof(Record));
                return true;
            }
            catch (Exception e)
            {
                MainSave.CQLog.Error("创建数据库", "创建数据库过程发生异常：" + e.Message + e.StackTrace);
                return false;
            }
        }
    }
}