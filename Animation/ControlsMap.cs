using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation
{
    public class ControlsMap
    {
        List<List<string>> maps = new List<List<string>>();
        public List<List<string>> ControlMaps
        {
            get { return maps; }
            set { maps = value; }
        }

        public List<string> GetPossibleRemaps(string controlName)
        {
            List<string> possRemaps = new List<string>();

            foreach (List<string> remaps in maps)
            {
                if (remaps.Contains(controlName))
                {
                    foreach (string map in remaps)
                    {
                        if (!map.Equals(controlName))
                        {
                            possRemaps.Add(map);
                        }
                    }

                    return possRemaps;
                }
            }

            return possRemaps;
        }

        public void Add(string controlName, string refControl)
        {
            int index = GetRemapIndex(controlName);

            if (refControl != "")
            {
                int refIndex = GetRemapIndex(controlName);
                if (refIndex != -1)
                {
                    if (index != -1)
                    {
                        maps[index].Remove(controlName);
                    }

                    maps[refIndex].Add(controlName);
                }
            }
            else
            {
                if (index == -1)
                {
                    List<string> newRemaps = new List<string>();
                    newRemaps.Add(controlName);
                    maps.Add(newRemaps);
                }
            }
        }

        public int GetRemapIndex(string controlName)
        {
            int counter = 0;
            foreach (List<string> remaps in maps)
            {
                if (remaps.Contains(controlName))
                {
                    return counter;
                }
                counter++;
            }

            return -1;
        }

        public bool Initialize()
        {
            if (maps.Count == 0)
            {
                //SPINEIKTOP
                List<string> spineMap = new List<string>();
                spineMap.Add("#SPINEIKTOP");
                spineMap.Add("UpperBody");
                maps.Add(spineMap);

                //SPINEIKBOTTOM
                spineMap = new List<string>();
                spineMap.Add("#SPINEIKBOTTOM");
                spineMap.Add("LowerBody");
                maps.Add(spineMap);

                return false;
            }

            return true;
        }
    }
}
