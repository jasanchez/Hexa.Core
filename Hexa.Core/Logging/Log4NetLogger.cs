﻿#region License

//===================================================================================
//Copyright 2010 HexaSystems Corporation
//===================================================================================
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//http://www.apache.org/licenses/LICENSE-2.0
//===================================================================================
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
//===================================================================================

#endregion

using System;
using System.Globalization;

using log4net;

namespace Hexa.Core.Logging
{
    class Log4NetLogger : ILogger
    {
        private ILog _Log;

        public Log4NetLogger(Type type)
        {
            _Log = LogManager.GetLogger(type.FullName);
        }

        public Log4NetLogger(string typeName)
        {
            _Log = LogManager.GetLogger(typeName);
        }

        public void Debug(object message)
        {
            _Log.Debug(message);
        }
        public void Debug(object message, Exception exception)
        {
            _Log.Debug(message, exception);
        }
        public void DebugFormat(string format, params object[] args)
        {
            _Log.DebugFormat(CultureInfo.InvariantCulture, format, args);
        }
        public void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.DebugFormat(provider, format, args);
        }
        public void Error(object message)
        {
            _Log.Error(message);
        }
        public void Error(object message, Exception exception)
        {
            _Log.Error(message, exception);
        }
        public void ErrorFormat(string format, params object[] args)
        {
            _Log.ErrorFormat(CultureInfo.InvariantCulture, format, args);
        }
        public void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.ErrorFormat(provider, format, args);
        }
        public void Fatal(object message)
        {
            _Log.Fatal(message);
        }
        public void Fatal(object message, Exception exception)
        {
            _Log.Fatal(message, exception);
        }
        public void FatalFormat(string format, params object[] args)
        {
            _Log.FatalFormat(CultureInfo.InvariantCulture, format, args);
        }
        public void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.FatalFormat(provider, format, args);
        }
        public void Info(object message)
        {
            _Log.Info(message);
        }
        public void Info(object message, Exception exception)
        {
            _Log.Info(message, exception);
        }
        public void InfoFormat(string format, params object[] args)
        {
            _Log.InfoFormat(CultureInfo.InvariantCulture, format, args);
        }
        public void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.InfoFormat(provider, format, args);
        }
        public void Warn(object message)
        {
            _Log.Warn(message);
        }
        public void Warn(object message, Exception exception)
        {
            _Log.Warn(message, exception);
        }
        public void WarnFormat(string format, params object[] args)
        {
            _Log.WarnFormat(CultureInfo.InvariantCulture, format, args);
        }
        public void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            _Log.WarnFormat(provider, format, args);
        }
    }

}