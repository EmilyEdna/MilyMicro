using Mily.Setting.ModelEnum;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using XExten.XCore;
using XExten.XExpres;

namespace Mily.DbCore
{
    public abstract class BasicEvent
    {
        #region 新增
        /// <summary>
        /// 新增前组装基础数据
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        internal virtual void InsertDataEvent<Entity>(params Entity[] entity) where Entity : class, new()
        {
            foreach (var Item in entity)
            {
                Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                {
                     { "Created",DateTime.Now },
                     { "Deleted", false }
                 };
                XExp.SetProptertiesValue(DataValue, Item);
            }
        }
        /// <summary>
        /// 执行插入
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Insert"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        internal virtual async Task<Object> ExecuteInsert<Entity>(IInsertable<Entity> Insert, DbReturnEnum type) where Entity : class, new()
        {
            return type switch
            {
                DbReturnEnum.Rowspan => await Insert.ExecuteCommandAsync(),
                DbReturnEnum.Integer => await Insert.ExecuteReturnIdentityAsync(),
                DbReturnEnum.BigInteger => await Insert.ExecuteReturnBigIdentityAsync(),
                DbReturnEnum.Model => await Insert.ExecuteReturnEntityAsync(),
                _ => await Insert.ExecuteCommandIdentityIntoEntityAsync(),
            };
        }
        #endregion

        #region 更新
        /// <summary>
        /// 执行更新
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Update"></param>
        /// <param name="type"></param>
        /// <param name="ObjExp"></param>
        /// <param name="BoolExp"></param>
        /// <returns></returns>
        internal virtual async Task<Object> ExecuteAlter<Entity>(IUpdateable<Entity> Update, DbReturnEnum type, Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            IUpdateable<Entity> Updateable = Update;
            if (BoolExp != null)
                Updateable = Updateable.Where(BoolExp);
            return type switch
            {
                DbReturnEnum.AlterEntity => await Updateable.ExecuteCommandAsync(),
                DbReturnEnum.AlterCols => await Updateable.UpdateColumnsIF(ObjExp != null, ObjExp).ExecuteCommandAsync(),
                _ => await Updateable.ExecuteCommandAsync(),
            };
        }
        #endregion

        #region 逻辑删除
        /// <summary>
        /// 逻辑删除组装数据
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Delete"></param>
        /// <param name="entity"></param>
        internal virtual void LogicDeleteOrRecoveryEvent<Entity>(bool Delete, params Entity[] entity) where Entity : class, new()
        {
            foreach (var Item in entity)
            {
                Dictionary<String, Object> DataValue = new Dictionary<String, Object>
                {
                     { "Deleted",Delete }
                 };
                XExp.SetProptertiesValue(DataValue, Item);
            }
        }
        /// <summary>
        /// 执行逻辑删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Update"></param>
        /// <param name="ObjExp"></param>
        /// <param name="BoolExp"></param>
        /// <returns></returns>
        internal virtual async Task<Object> ExecuteLogicDeleteOrRecovery<Entity>(IUpdateable<Entity> Update, Expression<Func<Entity, Object>> ObjExp = null, Expression<Func<Entity, bool>> BoolExp = null) where Entity : class, new()
        {
            IUpdateable<Entity> Updateable = Update.UpdateColumnsIF(ObjExp != null, ObjExp);
            if (BoolExp != null)
                Updateable = Updateable.Where(BoolExp);
            return await Updateable.ExecuteCommandAsync();
        }
        #endregion

        #region 删除
        /// <summary>
        /// 组装删除数数据
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        internal virtual List<int> RemoveDataEvent<Entity>(params Entity[] entity) where Entity : class, new()
        {
            List<int> Id = new List<int>();
            foreach (var Item in entity)
            {
                var EntityMap = Item.ToDic();
                Id.Add(EntityMap["Id"].ObjToInt());
            }
            return Id;
        }
        /// <summary>
        /// 执行删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Delete"></param>
        /// <param name="Keys"></param>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <param name="BoolExp"></param>
        /// <param name="ObjExp"></param>
        /// <returns></returns>
        internal virtual async Task<Object> ExecuteRemove<Entity>(IDeleteable<Entity> Delete,List<int> Keys, Entity entity, DbReturnEnum type,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            return type switch
            {
                DbReturnEnum.RemoveEntity => await Delete.Where(entity).ExecuteCommandAsync(),
                DbReturnEnum.WithNoId => await Delete.In(ObjExp, Keys).ExecuteCommandAsync(),
                DbReturnEnum.WithWhere => await Delete.Where(BoolExp).ExecuteCommandAsync(),
                _ => await Delete.In(Keys).ExecuteCommandAsync(),
            };
        }
        /// <summary>
        /// 执行删除
        /// </summary>
        /// <typeparam name="Entity"></typeparam>
        /// <param name="Delete"></param>
        /// <param name="Keys"></param>
        /// <param name="entities"></param>
        /// <param name="type"></param>
        /// <param name="BoolExp"></param>
        /// <param name="ObjExp"></param>
        /// <returns></returns>
        internal virtual async Task<Object> ExecuteRemove<Entity>(IDeleteable<Entity> Delete, List<int> Keys, List<Entity> entities, DbReturnEnum type,
            Expression<Func<Entity, bool>> BoolExp = null, Expression<Func<Entity, Object>> ObjExp = null) where Entity : class, new()
        {
            return type switch
            {
                DbReturnEnum.RemoveEntities => await Delete.Where(entities).ExecuteCommandAsync(),
                DbReturnEnum.WithNoId => await Delete.In(ObjExp, Keys).ExecuteCommandAsync(),
                DbReturnEnum.WithWhere => await Delete.Where(BoolExp).ExecuteCommandAsync(),
                _ => await Delete.In(Keys).ExecuteCommandAsync(),
            };
        }
        #endregion
    }
}
