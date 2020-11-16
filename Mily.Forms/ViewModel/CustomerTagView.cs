using Mily.Forms.Core.Sql;
using Mily.Forms.DataModel.Konochan;
using Mily.Forms.DataModel.SqlModel;
using Mily.Forms.Utils;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using XExten.XCore;
using XExten.XPlus;

namespace Mily.Forms.ViewModel
{
    public class CustomerTagView : BaseView
    {
        public CustomerTagView()
        {
            Json = ReadTag();
        }

        #region Property
        private string _Key;
        public string Key
        {
            get
            {
                return _Key;
            }
            set
            {
                _Key = value;
                OnPropertyChanged("Key");
            }
        }

        private string _Val;
        public string Val
        {
            get
            {
                return _Val;
            }
            set
            {
                _Val = value;
                OnPropertyChanged("Val");
            }
        }

        private List<CustomerTag> _Json;
        public List<CustomerTag> Json
        {
            get
            {
                return _Json.OrderByDescending(t => t.AddTime).ToList();
            }
            set
            {
                _Json = value;
                OnPropertyChanged("Json");
            }
        }
        #endregion

        #region Command
        public Commands<string> Add
        {
            get
            {
                return new Commands<string>((str) => AddTag(), () => true);
            }
        }
        public Commands<string> Remove
        {
            get
            {
                return new Commands<string>((str) => RemoveTag(str), () => true);
            }
        }
        #endregion

        #region 新操作
        /// <summary>
        /// 创建配置
        /// </summary>
        private void AddTag()
        {
           var data = DbContext.Db().Queryable<UserTag>().Where(t => t.Key.Equals(Key)).First();
            if (data == null)
            {
                UserTag Tag = new UserTag { Key = Key, Value = Val, AddTime = DateTime.Now };
                DbContext.Db().Insertable(Tag).ExecuteCommand();
                var res = DbContext.Db().Queryable<UserTag>().ToList().ToAutoMapper<UserTag, CustomerTag>();
                HomeView.Ioc.Values.FirstOrDefault().Json = res;
                Json = res;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="key"></param>
        private void RemoveTag(string key)
        {
            DbContext.Db().Deleteable<UserTag>().Where(t => t.Key == key).ExecuteCommand();
            var res = DbContext.Db().Queryable<UserTag>().ToList().ToAutoMapper<UserTag, CustomerTag>();
            HomeView.Ioc.Values.FirstOrDefault().Json = res;
            Json = res;
        }
        private List<CustomerTag> ReadTag()
        {
            return DbContext.Db().Queryable<UserTag>().ToList().ToAutoMapper<UserTag, CustomerTag>();
        }
        #endregion

        #region 弃用
        /// <summary>
        /// 创建配置
        /// </summary>
        [Obsolete("使用AddTag替代")]
        private void CreateConfig()
        {
            if (Key.IsNullOrEmpty() || Val.IsNullOrEmpty()) return;
            List<CustomerTag> Datas;
            Datas = Read();
            Datas.Add(new CustomerTag { Key = Key, Value = Val, AddTime = DateTime.Now });
            Help.Write(Help.Config_cof, Datas.Count == 0 ? "" : Datas.ToJson().ToLzStringEnc());
            Json = Datas;
            HomeView.Ioc.Values.FirstOrDefault().Json = Datas;
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        [Obsolete]
        private void RemoveConfig(string key)
        {
            List<CustomerTag> Datas;
            Datas = Read();
            Datas.RemoveAll(t => t.Key.Equals(key));
            Help.Write(Help.Config_cof, Datas.Count == 0 ? "" : Datas.ToJson().ToLzStringEnc());
            Json = Datas;
            HomeView.Ioc.Values.FirstOrDefault().Json = Datas;
        }
       
        [Obsolete]
        private List<CustomerTag> Read()
        {
            Help.FileCreater(Help.Config_cof);
            var res = Help.Read(Help.Config_cof);
            return !res.IsNullOrEmpty() ? res.ToLzStringDec().ToModel<List<CustomerTag>>() : new List<CustomerTag>();
        }
        #endregion
    }
}
