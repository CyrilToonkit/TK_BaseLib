using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.CustomData
{
    /// <summary>
    /// Custom Node class to be used by the TGV
    /// </summary>
    public interface ITGVNode
    {
        string ITGV_UniqueName
        {
            get;
        }

        string ITGV_Name
        {
            get;
            set;
        }

        string ITGV_Description
        {
            get;
        }

        string ITGV_Type
        {
            get;
        }

        bool ITGV_Expanded
        {
            get;
            set;
        }

        bool ITGV_HasChildren();

        List<ITGVNode> ITGV_GetChildren();

        List<string> ITGV_GetFields();

        object ITGV_GetFieldValue(string field);

        void ITGV_SetFieldValue(string field, object value);
    }
}
