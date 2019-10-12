using XExten.XCore;

namespace Mily.Service.ViewSetting.SocketSetting
{
    public class ParseCmd
    {
        public static string HashData { get; set; }

        public static string Create(NetType Types, string Content = "")
        {
            switch (Types)
            {
                case NetType.Connect:
                    return "【注册服务】₩注册服务";

                case NetType.Listen:
                    return $"【监听服务】₩{Content}";

                case NetType.DisConnect:
                    return "【断开服务】₩断开服务";
            }
            return "【未知服务】₩未知服务";
        }

        public static Commanding Parse(string Content)
        {
            if (Content.IsNullOrEmpty())
                return new Commanding();
            string[] Contents = Content.Split('₩');
            switch (Contents[0])
            {
                case "【注册服务】":
                    return new Commanding(NetType.Connect, Contents[1]);

                case "【监听服务】":
                    return new Commanding(NetType.Listen, Contents[1]);

                case "【断开服务】":
                    return new Commanding(NetType.DisConnect, Contents[1]);
            }
            return new Commanding(NetType.Ohter, Contents[1]);
        }
    }
}