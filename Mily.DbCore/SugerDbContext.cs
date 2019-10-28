using Mily.DbCore.Caches;
using Mily.DbCore.Model.SystemModel;
#if RELEASE
using Mily.Extension.LoggerFactory;
#elif DEBUG
using Mily.DbCore.Model;
#endif
using Mily.Setting;
using Mily.Setting.DbTypes;
using Mily.Setting.ModelEnum;
using Newtonsoft.Json.Linq;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XExten.XCore;
using XExten.XExpres;
using XCache = XExten.CacheFactory;

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
        private static SqlSugarClient Emily
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
                    IsShardSameThread=true,
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
                    IsShardSameThread=true,
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
        /// 数据库类型切换
        /// </summary>
        public static DBType TypeAttrbuite { get; set; }

        /// <summary>
        /// 数据库类型上下文
        /// </summary>
        /// <param name="Type"></param>
        /// <returns></returns>
        public static SqlSugarClient DbContext()
        {
            return TypeAttrbuite switch
            {
                DBType.MSSQL => DB_MSSQL(),
                DBType.MYSQL => DB_MYSQL(),
                _ => DB_MSSQL()
            };
        }

        /// <summary>
        /// 切换数据为MYSQL
        /// </summary>
        /// <returns></returns>
        private static SqlSugarClient DB_MYSQL()
        {
            Emily.ChangeDatabase("MYSQL");
            Emily.DbMaintenance.CreateDatabase();
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
            #elif DEBUG
            //Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
            //Emily.CodeFirst.InitTables(ModelTypes);
           #endif
            return Emily;
        }

        /// <summary>
        /// 切换数据库为MSSQL
        /// </summary>
        /// <returns></returns>
        private static SqlSugarClient DB_MSSQL()
        {
            Emily.ChangeDatabase("MSSQL");
            Emily.DbMaintenance.CreateDatabase();
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
            #elif DEBUG
            //Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
            //Emily.CodeFirst.InitTables(ModelTypes);
            #endif
            return Emily;
        }

        /// <summary>
        /// 新增数据通用
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<Object> InsertData<Entity>(Entity entity, List<Entity> entities = null, DbReturnTypes type = DbReturnTypes.InsertDefault) where Entity : class, new()
        {
            IInsertable<Entity> Insert;
            SqlSugarClient Client = DbContext();
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "KeyId", Guid.NewGuid() },
                         { "Deleted", false }
                    };
                    XExp.SetProptertiesValue(DataValue, t);
                });
                Insert = Client.Insertable(entities);
                await AddSystemLog(entities.FirstOrDefault().GetType().Name, HandleEnum.Add); ;
            }
            else
            {
                Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                 {
                       { "KeyId", Guid.NewGuid() },
                       { "Deleted", false }
                 };
                XExp.SetProptertiesValue(DataValue, entity);
                Insert = Client.Insertable(entity);
                await AddSystemLog(typeof(Entity).Name, HandleEnum.Add);
            }
            return type switch
            {
                DbReturnTypes.Rowspan => await Insert.ExecuteCommandAsync(),
                DbReturnTypes.Integer => await Insert.ExecuteReturnIdentityAsync(),
                DbReturnTypes.BigInteger => await Insert.ExecuteReturnBigIdentityAsync(),
                DbReturnTypes.Model => await Insert.ExecuteReturnEntityAsync(),
                _ => await Insert.ExecuteCommandIdentityIntoEntityAsync(),
            };
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
        public virtual async Task<Object> AlterData<Entity>(Entity entity, List<Entity> entities = null, DbReturnTypes type = DbReturnTypes.AlterDefault,
            Boolean? Del = true, Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            IUpdateable<Entity> Update = null;
            SqlSugarClient Client = DbContext();
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    if (type == DbReturnTypes.AlterSoft)
                    {
                        Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                        {
                         { "Deleted", Del }
                        };
                        XExp.SetProptertiesValue(DataValue, t);
                    }
                });
                await AddSystemLog(entities.FirstOrDefault().GetType().Name, HandleEnum.Edit);
                Update = Client.Updateable(entities);
            }
            else
            {
                if (type == DbReturnTypes.AlterSoft)
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "Deleted", Del }
                    };
                    XExp.SetProptertiesValue(DataValue, entity);
                }
                Update = Client.Updateable(entity);
                await AddSystemLog(typeof(Entity).Name, HandleEnum.Edit);
            }
            return type switch
            {
                DbReturnTypes.AlterEntity => await Update.Where(BoolExp).ExecuteCommandAsync(),
                DbReturnTypes.AlterCols => await Update.UpdateColumns(ObjExp).Where(BoolExp).ExecuteCommandAsync(),
                DbReturnTypes.AlterSoft => await Update.UpdateColumns(ObjExp).Where(BoolExp).ExecuteCommandAsync(),
                _ => await Update.Where(BoolExp).ExecuteCommandAsync(),
            };
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
        public virtual async Task<Object> RemoveData<Entity>(Entity entity, List<Entity> entities = null, DbReturnTypes type = DbReturnTypes.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            IDeleteable<Entity> Delete = null;
            List<Guid> Ids = new List<Guid>();
            SqlSugarClient Client = DbContext();
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    var Map = t.ToDic();
                    Ids.Add(Guid.Parse(Map["KeyId"].ToString()));
                });
                Delete = Client.Deleteable(entities);
                await AddSystemLog(entities.FirstOrDefault().GetType().Name, HandleEnum.Remove);
            }
            else
            {
                Delete = Client.Deleteable(entity);
                Ids.Add(Guid.Parse(entity.ToDic()["KeyId"].ToString()));
                await AddSystemLog(typeof(Entity).Name, HandleEnum.Remove);
            }

            return type switch
            {
                DbReturnTypes.RemoveEntity => await Delete.Where(entity).ExecuteCommandAsync(),
                DbReturnTypes.RemoveEntities => await Delete.Where(entities).ExecuteCommandAsync(),
                DbReturnTypes.WithNoId => await Delete.In(ObjExp, Ids).ExecuteCommandAsync(),
                DbReturnTypes.WithWhere => await Delete.Where(BoolExp).ExecuteCommandAsync(),
                _ => await Delete.In(Ids).ExecuteCommandAsync(),
            };
        }

        /// <summary>
        /// 统一数据操作日志
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="db"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        private async Task<Object> AddSystemLog(string entity, HandleEnum handle)
        {
            SystemhandleLog Log = new SystemhandleLog
            {
                KeyId = Guid.NewGuid(),
                Deleted = false,
                HandleTime = DateTime.Now,
                Hnadler = SearchCache(),
                HandleObject = entity,
                HandleType = handle,
                HandleName = handle.ToSelectDes()
            };
            Log.HandleObvious = $"【{Log.Hnadler}】对【{entity}】表进行了【{Log.HandleName}】，操作时间为：【{Log.HandleTime}】";
            return await DbContext().Insertable(Log).ExecuteCommandAsync();
        }

        /// <summary>
        /// 查询缓存
        /// </summary>
        /// <returns></returns>
        private String SearchCache()
        {
            Object View = XCache.Caches.RedisCacheGet<Object>(MilyConfig.CacheKey);
            return View == null ? "*" : JToken.FromObject(XCache.Caches.RedisCacheGet<Object>(MilyConfig.CacheKey)).SelectToken("AdminName").ToString();
        }
    }
}