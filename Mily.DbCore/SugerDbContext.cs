using Mily.DbCore.Caches;
using Mily.DbCore.Model;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XExten.XCore;
using XExten.XExpres;

namespace Mily.DbCore
{
    /// <summary>
    /// 获取SQLDB上下文
    /// </summary>
    public class SugerDbContext
    {
        /// <summary>
        /// 获取连接客服端
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient Emily
        {
            get
            {
                List<ConnectionConfig> Configs = new List<ConnectionConfig> {
                    new ConnectionConfig() {
                    ConfigId="MSSQL",
                    ConnectionString = MilyConfig.ConnectionString1,
                    DbType = DbType.SqlServer,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { new SlaveConnectionConfig()  {
                        HitRate =100, ConnectionString=MilyConfig.ConnectionStringSlave}
                    }},
                    #if RELEASE
                    new ConnectionConfig() {
                    ConfigId="MYSQL",
                    ConnectionString = MilyConfig.ConnectionString2,
                    DbType = DbType.MySql,
                    IsAutoCloseConnection = true,
                    InitKeyType = InitKeyType.Attribute,
                    SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { new SlaveConnectionConfig()  {
                        HitRate =100, ConnectionString=MilyConfig.ConnectionStringSlave}
                    }},
                    #endif
                };
                SqlSugarClient Db = new SqlSugarClient(Configs);
                return Db;
            }
        }
        /// <summary>
        /// 切换数据为MYSQL
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient DB_MYSQL()
        {
            Emily.ChangeDatabase("MYSQL");
            Emily.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices
            {
                DataInfoCacheService = new DbCache()
            };
            #if RELEASE
            Emily.Aop.OnError = (Ex) =>
            {
                var Logs = $"SQL语句：{Ex.Sql}[SQL参数：{Ex.Parametres}]";
                //启用NLog
                LogFactoryExtension.WriteSqlError(Logs);
            };
            Emily.Aop.OnLogExecuted = (Sql, Args) =>
            {
                var Logs = $"SQL语句：{Sql}[SQL参数：{Emily.Utilities.SerializeObject(Args.ToDictionary(t => t.ParameterName, t => t.Value))}][SQL执行时间：{Emily.Ado.SqlExecutionTime.TotalMilliseconds}毫秒]";
                Emily.MappingTables.ToList().ForEach(t =>
                {
                    if (Sql.Contains($"[{t.DbTableName}]") && !Sql.Contains("ALTER TABLE"))
                        //启用NLog
                        LogFactoryExtension.WriteSqlWarn(Logs);
                });
            };
            #endif
            //Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
            //Emily.CodeFirst.InitTables(ModelTypes);
            return Emily;
        }
        /// <summary>
        /// 切换数据库为MSSQL
        /// </summary>
        /// <returns></returns>
        public static SqlSugarClient DB_MSSQL()
        {
            Emily.ChangeDatabase("MSSQL");
            Emily.CurrentConnectionConfig.ConfigureExternalServices = new ConfigureExternalServices
            {
                DataInfoCacheService = new DbCache()
            };
            #if RELEASE
            Emily.Aop.OnError = (Ex) =>
            {
                var Logs = $"SQL语句：{Ex.Sql}[SQL参数：{Ex.Parametres}]";
                //启用NLog
                LogFactoryExtension.WriteSqlError(Logs);
            };
            Emily.Aop.OnLogExecuted = (Sql, Args) =>
            {
                var Logs = $"SQL语句：{Sql}[SQL参数：{Emily.Utilities.SerializeObject(Args.ToDictionary(t => t.ParameterName, t => t.Value))}][SQL执行时间：{Emily.Ado.SqlExecutionTime.TotalMilliseconds}毫秒]";
                Emily.MappingTables.ToList().ForEach(t =>
                {
                    if (Sql.Contains($"[{t.DbTableName}]") && !Sql.Contains("ALTER TABLE"))
                        //启用NLog
                        LogFactoryExtension.WriteSqlWarn(Logs);
                });
            };
            #endif
            //Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
            //Emily.CodeFirst.InitTables(ModelTypes);
            return Emily;
        }
        /// <summary>
        /// 新增数据通用
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<Object> InsertData<Entity>(Entity entity, List<Entity> entities = null, DBType db = DBType.MSSQL, DbReturnTypes type = DbReturnTypes.InsertDefault) where Entity : class, new()
        {
            IInsertable<Entity> Insert = null;
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "KeyId", Guid.NewGuid() },
                         { "CreateUser", null },
                         { "CreateUserId", null },
                         { "CreateTime", DateTime.Now },
                         { "IsDelete", false }
                    };
                    XExp.SetProptertiesValue(DataValue, t);
                });
                Insert = (db == DBType.MSSQL ? DB_MSSQL().Insertable(entities) : DB_MYSQL().Insertable(entities));
            }
            else
            {
                Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                {
                       { "KeyId", Guid.NewGuid() },
                       { "CreateUser", null },
                       { "CreateUserId", null },
                       { "CreateTime", DateTime.Now },
                       { "IsDelete", false }
                };
                XExp.SetProptertiesValue(DataValue, entity);
                Insert = (db == DBType.MSSQL ? DB_MSSQL().Insertable(entity) : DB_MYSQL().Insertable(entity));
            }
            switch (type)
            {
                case DbReturnTypes.Rowspan:
                    return await Insert.ExecuteCommandAsync();
                case DbReturnTypes.Integer:
                    return await Insert.ExecuteReturnIdentityAsync();
                case DbReturnTypes.BigInteger:
                    return await Insert.ExecuteReturnBigIdentityAsync();
                case DbReturnTypes.Model:
                    return await Insert.ExecuteReturnEntityAsync();
                default:
                    return await Insert.ExecuteCommandIdentityIntoEntityAsync();
            }
        }
        /// <summary>
        /// 更新数据通用
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="Del"></param>
        /// <param name="ObjExp"></param>
        /// <param name="BoolExp"></param>
        /// <returns></returns>
        public virtual async Task<Object> AlterData<Entity>(Entity entity, List<Entity> entities = null, DBType db = DBType.MSSQL, DbReturnTypes type = DbReturnTypes.AlterDefault,
            Boolean? Del = true, Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            IUpdateable<Entity> Update = null;
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    if (type != DbReturnTypes.AlterSoft)
                    {
                        Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                        {
                         { "UpdateUser", null },
                         { "UpdateUserId", null },
                         { "UpdateTime", DateTime.Now }
                        };
                        XExp.SetProptertiesValue(DataValue, t);
                    }
                    else
                    {
                        Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                        {
                         { "UpdateUser", null },
                         { "UpdateUserId", null },
                         { "UpdateTime", DateTime.Now },
                         { "IsDelete", Del }
                        };
                        XExp.SetProptertiesValue(DataValue, t);
                    }
                });
                Update = (db == DBType.MSSQL ? DB_MSSQL().Updateable(entities) : DB_MYSQL().Updateable(entities));
            }
            else
            {
                if (type != DbReturnTypes.AlterSoft)
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "UpdateUser", null },
                         { "UpdateUserId", null },
                         { "UpdateTime", DateTime.Now }
                    };
                    XExp.SetProptertiesValue(DataValue, entity);
                }
                else
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "UpdateUser", null },
                         { "UpdateUserId", null },
                         { "UpdateTime", DateTime.Now },
                         { "IsDelete", Del }
                    };
                    XExp.SetProptertiesValue(DataValue, entity);
                }
                Update = (db == DBType.MSSQL ? DB_MSSQL().Updateable(entity) : DB_MYSQL().Updateable(entity));
            }
            switch (type)
            {
                case DbReturnTypes.AlterEntity:
                    return await Update.Where(BoolExp).ExecuteCommandAsync();
                case DbReturnTypes.AlterCols:
                    return await Update.UpdateColumns(ObjExp).Where(BoolExp).ExecuteCommandAsync();
                case DbReturnTypes.AlterSoft:
                    return await Update.UpdateColumns(ObjExp).Where(BoolExp).ExecuteCommandAsync();
                default:
                    return await Update.Where(BoolExp).ExecuteCommandAsync();
            }
        }
        /// <summary>
        /// 删除数据通用
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="BoolExp"></param>
        /// <param name="ObjExp"></param>
        /// <returns></returns>
        public virtual async Task<Object> RemoveData<Entity>(Entity entity, List<Entity> entities = null, DBType db = DBType.MSSQL, DbReturnTypes type = DbReturnTypes.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            IDeleteable<Entity> Delete = null;
            List<Guid> Ids = new List<Guid>();
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    var Map = t.ToDic();
                    Ids.Add(Guid.Parse(Map["KeyId"].ToString()));
                });
                Delete = (db == DBType.MSSQL ? DB_MSSQL().Deleteable(entities) : DB_MYSQL().Deleteable(entities));
            }
            else
            {
                Delete = (db == DBType.MSSQL ? DB_MSSQL().Deleteable(entity) : DB_MYSQL().Deleteable(entity));
                Ids.Add(Guid.Parse(entity.ToDic()["KeyId"].ToString()));
            }
            switch (type)
            {
                case DbReturnTypes.RemoveEntity:
                    return await Delete.Where(entity).ExecuteCommandAsync();
                case DbReturnTypes.WithNoId:
                    return await Delete.In(ObjExp, Ids).ExecuteCommandAsync();
                case DbReturnTypes.WithWhere:
                    return await Delete.Where(BoolExp).ExecuteCommandAsync();
                default:
                    return await Delete.In(Ids).ExecuteCommandAsync();
            }

        }
    }
}
