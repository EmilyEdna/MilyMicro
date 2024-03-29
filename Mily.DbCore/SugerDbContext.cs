﻿using Mily.DbCore.Caches;

#if RELEASE
using Mily.Extension.LoggerFactory;
#endif

using Mily.Setting;
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
using Mily.DbEntity.SystemView;
using Mily.DbEntity;
using Mily.Extension.LoggerFactory;

namespace Mily.DbCore
{
    /// <summary>
    /// 获取SQLDB上下文
    /// </summary>
    public class SugerDbContext : BasicEvent
    {
        public static readonly IDictionary<string, string> AdoSQL = MilyConfig.XmlSQL;

        /// <summary>
        /// 数据库类型切换
        /// </summary>
        public static DBTypeEnum TypeAttrbuite { get; set; }

        /// <summary>
        /// 数据库类型上下文
        /// </summary>
        /// <param name="InitDbTable"></param>
        /// <returns></returns>
        public static SqlSugarClient DbContext(string TargetDbName = null, bool InitDbTable = false)
        {
            if (TargetDbName.IsNullOrEmpty())
                TargetDbName = MilyConfig.Default;
            List<ConnectionConfig> Configs = new List<ConnectionConfig> {
               new ConnectionConfig() {
               ConfigId="MSSQL",
               ConnectionString =string.Format(MilyConfig.ConnectionStrings.ConnectionString1,TargetDbName),
               DbType = DbType.SqlServer,
               IsAutoCloseConnection = true,
               InitKeyType = InitKeyType.Attribute,
               IsShardSameThread=true,
               AopEvents = new AopEvents(),
               SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { new SlaveConnectionConfig()  {
                   HitRate =100, ConnectionString=MilyConfig.ConnectionStrings.ConnectionStringSlave}
               }},
               #if RELEASE
               new ConnectionConfig() {
               ConfigId="MYSQL",
               ConnectionString = string.Format(MilyConfig.ConnectionStrings.ConnectionString2,TargetDbName),
               DbType = DbType.MySql,
               IsAutoCloseConnection = true,
               InitKeyType = InitKeyType.Attribute,
               IsShardSameThread=true,
               AopEvents = new AopEvents(),
               SlaveConnectionConfigs = new List<SlaveConnectionConfig>() { new SlaveConnectionConfig()  {
                   HitRate =100, ConnectionString=MilyConfig.ConnectionStrings.ConnectionStringSlave}
               }},
               #endif
            };
            SqlSugarClient Emily = new SqlSugarClient(Configs);
            //切换数据库
            switch (TypeAttrbuite)
            {
                case DBTypeEnum.MSSQL:
                    Emily.ChangeDatabase("MSSQL");
                    break;
                case DBTypeEnum.MYSQL:
                    Emily.ChangeDatabase("MYSQL");
                    break;
                default:
                    Emily.ChangeDatabase("MSSQL");
                    break;
            }
            //设置缓存
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
            //初始化表
            if (InitDbTable)
            {
                Type[] ModelTypes = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == typeof(BaseEntity))).ToArray();
                if (TargetDbName.Equals(MilyConfig.Default))
                    Emily.CodeFirst.InitTables(ModelTypes);
                else
                {
                    List<Type> Condition = ModelTypes.Where(Item => Item.CustomAttributes.Any(Attr => Attr.AttributeType == typeof(DataSliceAttribute))).ToList();
                    Type[] Complete = Condition.Where(Item => (Item.GetCustomAttributes(typeof(DataSliceAttribute), false).FirstOrDefault() as DataSliceAttribute).DBName.Contains(TargetDbName)).ToArray();
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
        public virtual async Task<Object> InsertData<Entity>(Entity entity, String DbName = null, DbReturnEnum type = DbReturnEnum.InsertDefault) where Entity : class, new()
        {
            base.InsertDataEvent(entity);
            IInsertable<Entity> Insert;
            SqlSugarClient Client = DbContext(DbName);
            Insert = Client.Insertable(entity);
            return await XPlusEx.XTry<Object>(async () =>
             {
                 Client.BeginTran();
                 await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Add);
                 Object ExecuteResult = await base.ExecuteInsert(Insert, type);
                 Client.CommitTran();
                 return await Task.FromResult(ExecuteResult);
             }, async Ex =>
             {
                 Client.RollbackTran();
                 return await Task.FromResult(-1);
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
        public virtual async Task<Object> InsertData<Entity>(List<Entity> entities, String DbName = null, DbReturnEnum type = DbReturnEnum.InsertDefault) where Entity : class, new()
        {
            base.InsertDataEvent(entities.ToArray());
            IInsertable<Entity> Insert;
            SqlSugarClient Client = DbContext(DbName);
            Insert = Client.Insertable(entities);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Add);
               Object ExecuteResult = await base.ExecuteInsert(Insert, type);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
        public virtual async Task<Object> AlterData<Entity>(Entity entity, String DbName = null, DbReturnEnum type = DbReturnEnum.AlterDefault,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entity);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Edit);
               Object ExecuteResult = await base.ExecuteAlter(Update, type, ObjExp, BoolExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
        public virtual async Task<Object> AlterData<Entity>(List<Entity> entities, String DbName = null, DbReturnEnum type = DbReturnEnum.AlterDefault,
            Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entities);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Edit);
               Object ExecuteResult = await base.ExecuteAlter(Update, type, ObjExp, BoolExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
               Object ExecuteResult = await base.ExecuteLogicDeleteOrRecovery(Update, ObjExp, BoolExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
            base.LogicDeleteOrRecoveryEvent(Delete, entities.ToArray());
            SqlSugarClient Client = DbContext(DbName);
            IUpdateable<Entity> Update = Client.Updateable(entities);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
               Object ExecuteResult = await base.ExecuteLogicDeleteOrRecovery(Update, ObjExp, BoolExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
        public virtual async Task<Object> RemoveData<Entity>(Entity entity, String DbName = null, DbReturnEnum type = DbReturnEnum.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            if (type == DbReturnEnum.RemoveEntities) return await Task.FromResult(false);
            List<int> Ids = base.RemoveDataEvent(entity);
            SqlSugarClient Client = DbContext(DbName);
            IDeleteable<Entity> Delete = Client.Deleteable(entity);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
               Object ExecuteResult = await base.ExecuteRemove(Delete, Ids, entity, type, BoolExp, ObjExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
        public virtual async Task<Object> RemoveData<Entity>(List<Entity> entities, String DbName = null, DbReturnEnum type = DbReturnEnum.RemoveDefault,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            if (type == DbReturnEnum.RemoveEntity) return await Task.FromResult(false);
            List<int> Ids = base.RemoveDataEvent(entities.ToArray());
            SqlSugarClient Client = DbContext(DbName);
            IDeleteable<Entity> Delete = Client.Deleteable(entities);
            return await XPlusEx.XTry<Object>(async () =>
           {
               Client.BeginTran();
               await AddSystemLog(Client, typeof(Entity).Name, HandleEnum.Remove);
               Object ExecuteResult = await base.ExecuteRemove(Delete, Ids, entities, type, BoolExp, ObjExp);
               Client.CommitTran();
               return await Task.FromResult(ExecuteResult);
           }, async Ex =>
           {
               Client.RollbackTran();
               return await Task.FromResult(-1);
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
                Deleted = false,
                HandleTime = DateTime.Now,
                Hnadler = SearchCache(),
                HandleObject = entity,
                HandleType = handle,
                HandleName = handle.ToDescription(),
                Created = DateTime.Now
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

        /// <summary>
        /// 获取缓存的用户 Session
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        internal static T GetCache<T>()
        {
            return XCache.Caches.RedisCacheGet<T>(MilyConfig.CacheKey);
        }
    }
}