using Mily.Forms.DataModel.SqlModel;
using Mily.Forms.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.Core.Sql
{
    public class DbContext
    {
        public static SqlSugarClient Db()
        {
            Help.FileCreater(Help.SqlLite);
            var config = new ConnectionConfig()
            {
                DbType = DbType.Sqlite,
                InitKeyType = InitKeyType.Attribute,
                ConnectionString = $"DataSource={Help.SqlLite}",
                IsAutoCloseConnection = true,
                AopEvents = new AopEvents
                {
                    OnLogExecuting = (sql, p) =>
                    {
                        Console.WriteLine(sql);
                    }
                }
            };
            var db = new SqlSugarClient(config);
            Type[] type = new Type[] { typeof(KonaTag), typeof(UserTag) };
            db.CodeFirst.InitTables(type);
            return db;
        }
    }
}
