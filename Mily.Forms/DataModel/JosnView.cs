using Mily.Forms.DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XExten.XCore;
using System.Linq;
using XExten.XPlus;

namespace Mily.Forms.DataModel
{
    public class JosnView : BaseView
    {
        public JosnView()
        {
            Json = Read(out string BasePath);
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

        private List<JsonTag> _Json;
        public List<JsonTag> Json
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
            List<JsonTag> Datas;
            Datas = Read(out string BasePath);
            Datas.Add(new JsonTag { Key = Key, Value = Val, AddTime = DateTime.Now });
            Write(Datas, BasePath);
            ModelView.Ioc.Values.FirstOrDefault().Json = Datas;
        }

        /// <summary>
        /// 删除配置
        /// </summary>
        private void RemoveConfig(string key)
        {
            List<JsonTag> Datas;
            Datas = Read(out string BasePath);
            Datas.RemoveAll(t => t.Key.Equals(key));
            Write(Datas, BasePath);
            ModelView.Ioc.Values.FirstOrDefault().Json = Datas;
        }

        #region 读写
        private List<JsonTag> Read(out string Path)
        {
            var BasePath = AppDomain.CurrentDomain.BaseDirectory + "config.cof";
            Path = BasePath;
            if (!File.Exists(BasePath))
                File.Create(BasePath).Dispose();
            using StreamReader reader = new StreamReader(BasePath);
            var res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return !res.IsNullOrEmpty() ? res.ToLzStringDec().ToModel<List<JsonTag>>() : new List<JsonTag>();
        }
        private void Write(List<JsonTag> Datas, string BasePath)
        {
            using StreamWriter writer = new StreamWriter(BasePath, false);
            XPlusEx.XTry(() =>
            {
                if (Datas.Count == 0)
                    writer.Write("");
                else
                {
                    var encRes = Datas.ToJson().ToLzStringEnc();
                    writer.Write(encRes);
                }
                Json = Datas;
            }, ex => throw ex, () =>
            {
                writer.Close();
                writer.Dispose();
            });
        }
        #endregion
    }
}
