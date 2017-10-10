using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace SkillDataFileEditor
{
    static class  Tools
    {
        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 合并数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static IEnumerable<T> Combine<T>(this IEnumerable<IEnumerable<T>> values)
        {
            List<T> result = new List<T>();
            foreach (IEnumerable<T> item in values)
            {
                result.AddRange(item);
            }
            return result;
        }

        public static Delegate[] GetObjectEventList(Control p_Control, string p_EventName)
        {
            PropertyInfo _PropertyInfo = p_Control.GetType().GetProperty("Events", BindingFlags.Instance | BindingFlags.NonPublic);
            if (_PropertyInfo != null)
            {
                object _EventList = _PropertyInfo.GetValue(p_Control, null);
                if (_EventList != null && _EventList is EventHandlerList)
                {
                    EventHandlerList _List = (EventHandlerList)_EventList;
                    FieldInfo _FieldInfo = (typeof(Control)).GetField(p_EventName, BindingFlags.Static | BindingFlags.NonPublic);
                    if (_FieldInfo == null) return null;
                    Delegate _ObjectDelegate = _List[_FieldInfo.GetValue(p_Control)];
                    if (_ObjectDelegate == null) return null;
                    return _ObjectDelegate.GetInvocationList();
                }
            }
            return null;
        }
    }
}
