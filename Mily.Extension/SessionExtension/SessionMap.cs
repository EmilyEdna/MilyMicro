﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using XExten.XCore;

namespace Mily.Extension.SessionExtension
{
    public static class SessionMap
    {
        /// <summary>
        ///添加Session
        /// </summary>
        /// <param name="value"></param>
        public static void SetSession<T>(this ISession session, String key, T value)
        {
            session.SetString(key, value.ToJson());
        }

        /// <summary>
        /// 取出Session
        /// </summary>
        /// <returns></returns>
        public static T GetSession<T>(this ISession session, String key)
        {
            return session.GetString(key) == null ? default : session.GetString(key).ToModel<T>();
        }

        /// <summary>
        /// 删除Session
        /// </summary>
        /// <param name="session"></param>
        /// <param name="Key"></param>
        public static void DeleteSession(this ISession session, String Key)
        {
            session.Remove(Key);
        }

        /// <summary>
        /// 清空Session
        /// </summary>
        /// <param name="session"></param>
        public static void ClearSession(this ISession session)
        {
            session.Clear();
        }
    }
}