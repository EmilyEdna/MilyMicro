using Mily.Forms.DataModel;
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
            Json = Read();
            Type = Help.Boxs;
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

        private Dictionary<string,int> _Type;
        public Dictionary<string, int> Type
        {
            get
            {
                return _Type;
            }
            set
            {
                _Type = value;
                OnPropertyChanged("Type");
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
                return new Commands<string>((str) => CreateConfig(), () => true);
            }
        }
        public Commands<string> Remove
        {
            get
            {
                return new Commands<string>((str) => RemoveConfig(str), () => true);
            }
        }
        #endregion

        /// <summary>
        /// 创建配置
        /// </summary>
        private void CreateConfig()
        {
            if (Key.IsNullOrEmpty() || Val.IsNullOrEmpty()) return;
            List<CustomerTag> Datas;
            Datas = Read();
            Datas.Add(new CustomerTag { Key = Key, Value = Val, AddTime = DateTime.Now });
            Help.Write(Help.Config_cof, Datas.Count == 0 ? "" : Datas.ToJson().ToLzStringEnc());
            Json = Datas;
            //konachanMainView.Ioc.Values.FirstOrDefault().Json = Datas;
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        private void RemoveConfig(string key)
        {
            List<CustomerTag> Datas;
            Datas = Read();
            Datas.RemoveAll(t => t.Key.Equals(key));
            Help.Write(Help.Config_cof, Datas.Count == 0 ? "" : Datas.ToJson().ToLzStringEnc());
            Json = Datas;
            //konachanMainView.Ioc.Values.FirstOrDefault().Json = Datas;
        }

        #region 读
        private List<CustomerTag> Read()
        {
            Help.FileCreater(Help.Config_cof);
            var res = Help.Read(Help.Config_cof);
            return !res.IsNullOrEmpty() ? res.ToLzStringDec().ToModel<List<CustomerTag>>() : new List<CustomerTag>();
        }
        #endregion
    }
}
