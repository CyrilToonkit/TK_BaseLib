using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.CustomData
{
    public class SceneUserData
    {
        private SceneUserData()
        {

        }

        private static SceneUserData instance = null;

        public static SceneUserData GetInstance()
        {
            if (instance == null)
                instance = new SceneUserData();

            return instance;
        }

        Dictionary<string, object> Values = new Dictionary<string, object>();

        public static void PutValue(string Name, object Value)
        {
            SceneUserData i = GetInstance();

            if (i.Values.ContainsKey(Name))
            {
                i.Values[Name] = Value;
            }
            else
            {
                i.Values.Add(Name, Value);
            }
        }

        public static object GetValue(string Name)
        {
            SceneUserData i = GetInstance();

            if (i.Values.ContainsKey(Name))
            {
                return i.Values[Name];
            }

            return null;
        }
    }
}
