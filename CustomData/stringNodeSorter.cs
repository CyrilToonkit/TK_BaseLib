using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.CustomData
{
class stringNodeSorter : IComparer<stringNode>
{
    List<string> namesRef = new List<string>();

    /// <summary> 
    /// constructor to set the sort column and sort order. 
    /// </summary> 
    /// <param name="strMemberName"></param> 
    /// <param name="sortingOrder"></param> 
    public stringNodeSorter(List<string> inRef)
    {
        namesRef = inRef;
    }

    public int Compare(stringNode sN1, stringNode sN2)
    {
        int value1 = namesRef.IndexOf(sN1.Name);
        int value2 = namesRef.IndexOf(sN2.Name);

        if (value1 == -1)
        {
            value1 = int.MaxValue;
        }

        if (value2 == -1)
        {
            value2 = int.MaxValue;
        }

        return value1.CompareTo(value2);
    }
} 
}