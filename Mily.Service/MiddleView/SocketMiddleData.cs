﻿using Mily.Service.MiddleView.ViewEnum;
using Mily.Service.MiddleView.ViewInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleView
{
    public class SocketMiddleData
    {
        public SendTypeEnum SendType { get; set; }
        public ISocketSession Session { get; set; }
        public ISocketResult MiddleResult { get; set; }
        /// <summary>
        /// 传输数据
        /// </summary>
        /// <param name="SendType"></param>
        /// <param name="Data"></param>
        /// <param name="Session"></param>
        /// <returns></returns>
        public static SocketMiddleData Middle(SendTypeEnum SendType, ISocketResult Result, ISocketSession Session = null)
        {
            return new SocketMiddleData()
            {
                SendType = SendType,
                MiddleResult = Result,
                Session = Session
            };
        }
    }
}
