using Mily.DbCore.Caches;
using Mily.Setting;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
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
                Insert = (db == DBType.MSSQL ? DB_MSSQL().Insertable(entities) : DB_MYSQL().Insertable(entities));
            else
                Insert = (db == DBType.MSSQL ? DB_MSSQL().Insertable(entity) : DB_MYSQL().Insertable(entity));
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
        public virtual async Task<Object> AlterData<Entity>(Entity entity, List<Entity> entities = null, DBType db = DBType.MSSQL, DbReturnTypes type = DbReturnTypes.AlterDefault,
            Boolean? Del = true, Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            IUpdateable<Entity> Update = null;
            if (entities != null)
            {
                entities.ForEach(t =>
                {
                    if (type == DbReturnTypes.AlterSoft)
                    {
                        Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                        {
                         { "IsDelete", Del }
                        };
                        XExp.SetProptertiesValue(DataValue, t);
                    }
                });
                Update = (db == DBType.MSSQL ? DB_MSSQL().Updateable(entities) : DB_MYSQL().Updateable(entities));
            }
            else
            {
                if (type == DbReturnTypes.AlterSoft)
                {
                    Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                    {
                         { "IsDelete", Del }
                    };
                    XExp.SetProptertiesValue(DataValue, entity);
                }
                Update = (db == DBType.MSSQL ? DB_MSSQL().Updateable(entity) : DB_MYSQL().Updateable(entity));
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
            return type switch
            {
                DbReturnTypes.RemoveEntity => await Delete.Where(entity).ExecuteCommandAsync(),
                DbReturnTypes.WithNoId => await Delete.In(ObjExp, Ids).ExecuteCommandAsync(),
                DbReturnTypes.WithWhere => await Delete.Where(BoolExp).ExecuteCommandAsync(),
                _ => await Delete.In(Ids).ExecuteCommandAsync(),
            };
        }
    }
}