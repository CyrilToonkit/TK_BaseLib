using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Tags
{
    public class Tag
    {
        string mKey;
        public string Key
        {
            get { return mKey; }
            set { mKey = value; }
        }

        List<string> mValues = new List<string>();
        public List<string> Values
        {
            get { return mValues; }
            set { mValues = value; }
        }
    }
}
