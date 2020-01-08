using Mily.DbCore.Caches;
using Mily.DbCore.Model.SystemModel;
#if RELEASE
using Mily.Extension.LoggerFactory;
#endif
using Mily.DbCore.Model;
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
using XExten.XPlus;
using XCache = XExten.CacheFactory;
using Mily.Extension.Attributes;

namespace Mily.DbCore
{
    /// <summary>
    /// 获取SQLDB上下文
    /// </summary>
    public class SugerDbContext : BasicEvent
    {
        public static readonly IDictionary<string, string> AdoSQL = MilyConfig.XmlSQL;

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
                    ConnectionString =string.Format(MilyConfig.ConnectionString1,TargetDbName),
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
                    ConnectionString = string.Format(MilyConfig.ConnectionString2,TargetDbName),
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
        /// 目标库
        /// </summary>
        private static string TargetDbName { get; set; }

        /// <summary>
        /// 数据库类型切换
        /// </summary>
        public static DBType TypeAttrbuite { get; set; }

        /// <summary>
        /// 数据库类型上下文
        /// </summary>
        /// <param name="InitDbTable"></param>
        /// <returns></returns>
        public static SqlSugarClient DbContext(string DbName = null, bool InitDbTable = false)
        {
            if (DbName.IsNullOrEmpty())
                TargetDbName = MilyConfig.Default;
            else
                TargetDbName = MilyConfig.DbName.Where(Name => Name.Equals(DbName)).FirstOrDefault().IsNullOrEmpty() ? MilyConfig.Default : MilyConfig.DbName.Where(Name => Name.Equals(DbName)).FirstOrDefault();
            return TypeAttrbuite switch
            {
                DBType.MSSQL => DB_MSSQL(InitDbTable),
                DBType.MYSQL => DB_MYSQL(InitDbTable),
                _ => DB_MSSQL(InitDbTable)
            };
        }

        /// <summary>
        /// 切换数据为MYSQL
        /// </summary>
        /// <param name="InitDbTable"></param>
        /// <returns></returns>
        private static SqlSugarClient DB_MYSQL(bool InitDbTable)
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
            #endif
            if (InitDbTable)
            {
                Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
                if (TargetDbName.Equals(MilyConfig.Default))
                    Emily.CodeFirst.InitTables(ModelTypes);
                else
                {
                    List<Type> Condition = ModelTypes.Where(Item => Item.CustomAttributes.Any(Attr => Attr.AttributeType == typeof(DataBaseNameAttribute))).ToList();
                    Type[] Complete = Condition.Where(Item => (Item.GetCustomAttributes(typeof(DataBaseNameAttribute), false).FirstOrDefault() as DataBaseNameAttribute).DbHostAttr.Contains(TargetDbName)).ToArray();
                    Emily.CodeFirst.InitTables(Complete);
                }
            }
            return Emily;
        }

        /// <summary>
        /// 切换数据库为MSSQL
        /// </summary>
        /// <param name="InitDbTable"></param>
        /// <returns></returns>
        private static SqlSugarClient DB_MSSQL(bool InitDbTable)
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
            #endif
            if (InitDbTable)
            {
                Type[] ModelTypes = typeof(SugerDbContext).GetTypeInfo().Assembly.GetTypes().Where(t => t.BaseType == typeof(BaseModel)).ToArray();
                if (TargetDbName.Equals(MilyConfig.Default))
                    Emily.CodeFirst.InitTables(ModelTypes);
                else
                {
                    List<Type> Condition = ModelTypes.Where(Item => Item.CustomAttributes.Any(Attr => Attr.AttributeType == typeof(DataBaseNameAttribute))).ToList();
                    Type[] Complete = Condition.Where(Item => (Item.GetCustomAttributes(typeof(DataBaseNameAttribute), false).FirstOrDefault() as DataBaseNameAttribute).DbHostAttr.Contains(TargetDbName)).ToArray();
                    Emily.CodeFirst.InitTables(Complete);
                }
            }
            return Emily;
        }

        /// <summary>
        /// 单条数据插入
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="DbName"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual async Task<Object> InsertData<Entity>(Entity entity, String DbName = null, DbReturnTypes type = DbReturnTypes.InsertDefault) where Entity : class, new()
        {
            base.InsertDataEvent(entity);
            IInsertable<Entity> Insert;
            SqlSugarClient Client = DbContext(DbName);
            Insert = Client.Insertable(entity);
            return await XPlusEx.XTry<Object>(async () =>
             {
                 Client.BeginTran();
                 await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Add);
                 Task<Object> ExecuteResult = base.ExecuteInsert(Insert, type);
                 Client.CommitTran();
                 return ExecuteResult;
             }, async Ex =>
             {
                 Client.RollbackTran();
                 return await Task.FromResult(false);
             });
        }

        /// <summary>
        /// 批量数据插入
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities">多实体</param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="type">执行类型</param>
        /// <returns></returns>
        public virtual async Task<Object> InsertData<Entity>(List<Entity> entities, String DbName = null, DbReturnTypes type = DbReturnTypes.InsertDefault) where Entity : class, new()
        {
            base.InsertDataEvent(entities);
            IInsertable<Entity> Insert;
            SqlSugarClient Client = DbContext(DbName);
            Insert = Client.Insertable(entities);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Add);
                Task<Object> ExecuteResult = base.ExecuteInsert(Insert, type);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 单条数据更新
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> AlterData<Entity>(Entity entity, String DbName = null, DbReturnTypes type = DbReturnTypes.AlterDefault,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entity);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Edit);
                Task<Object> ExecuteResult = base.ExecuteAlter(Update, type, ObjExp, BoolExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 批量更新
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities"></param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> AlterData<Entity>(List<Entity> entities, String DbName = null, DbReturnTypes type = DbReturnTypes.AlterDefault,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entities);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Edit);
                Task<Object> ExecuteResult = base.ExecuteAlter(Update, type, ObjExp, BoolExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 逻辑删除或逻辑恢复
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> LogicDeleteOrRecovery<Entity>(Entity entity, bool Delete, String DbName = null,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            base.LogicDeleteOrRecoveryEvent(Delete, entity);
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entity);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
                Task<Object> ExecuteResult = base.ExecuteLogicDeleteOrRecovery(Update, ObjExp, BoolExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 批量逻辑删除或逻辑恢复
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> LogicDeleteOrRecovery<Entity>(List<Entity> entities, bool Delete, String DbName = null,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            base.LogicDeleteOrRecoveryEvent(Delete, entities);
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entities);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
                Task<Object> ExecuteResult = base.ExecuteLogicDeleteOrRecovery(Update, ObjExp, BoolExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 单条删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> RemoveData<Entity>(Entity entity, String DbName = null, DbReturnTypes type = DbReturnTypes.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            if (type == DbReturnTypes.RemoveEntities) return await Task.FromResult(false);
            List<Guid> Ids = base.RemoveDataEvent(entity);
            SqlSugarClient Client = DbContext(DbName);
            IDeleteable<Entity> Delete = Client.Deleteable(entity);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
                Task<Object> ExecuteResult = base.ExecuteRemove(Delete, Ids, entity, type, BoolExp, ObjExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entities">实体</param>
        /// <param name="DbName">数据库名称</param>
        /// <param name="type">执行类型</param>
        /// <param name="BoolExp">条件表达式</param>
        /// <param name="ObjExp">对象表达式</param>
        /// <returns></returns>
        public virtual async Task<Object> RemoveData<Entity>(List<Entity> entities, String DbName = null, DbReturnTypes type = DbReturnTypes.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            if (type == DbReturnTypes.RemoveEntity) return await Task.FromResult(false);
            List<Guid> Ids = base.RemoveDataEvent(entities);
            SqlSugarClient Client = DbContext(DbName);
            IDeleteable<Entity> Delete = Client.Deleteable(entities);
            return await  XPlusEx.XTry<Object>(async () =>
            {
                Client.BeginTran();
                await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
                Task<Object> ExecuteResult = base.ExecuteRemove(Delete, Ids, entities, type, BoolExp, ObjExp);
                Client.CommitTran();
                return ExecuteResult;
            }, async Ex =>
            {
                Client.RollbackTran();
                return await Task.FromResult(false);
            });
        }

        /// <summary>
        /// 统一数据操作日志
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="entity"></param>
        /// <param name="handle"></param>
        /// <returns></returns>
        private async Task<Object> AddSystemLog(SqlSugarClient Client, string entity, HandleEnum handle)
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
            return await Client.Insertable(Log).ExecuteCommandAsync();
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