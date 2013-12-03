using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Animation
{
    public class TimeWarp
    {
        public double Length
        {
            get { return Max - Min + 1; }
        }

        public double Min
        {
            get { return Keys[0].RealKey; }
        }

        public double Max
        {
            get { return Keys[Keys.Count - 1].RealKey; }
        }

        public TimeWarp(double StartFrame, double EndFrame)
        {
            AddKey(StartFrame, StartFrame);
            AddKey(EndFrame, EndFrame);
        }

        public TimeWarp(object[] Keys)
        {
            for (int i = 0; i < Keys.Length; i=i+2)
            {
                AddKey(Convert.ToDouble(Keys[i]), Convert.ToDouble(Keys[i + 1]));
            }
        }

        List<KeyMap> mKeys = new List<KeyMap>();
        public List<KeyMap> Keys
        {
            get { return mKeys; }
            set { mKeys = value; }
        }

        public void Reset(bool RemoveKeys)
        {
            if (RemoveKeys)
            {
                int KeyCount = Keys.Count-2;
                for (int i = 0; i < KeyCount; i++)
                {
                    mKeys.RemoveAt(1);
                }
            }

            int index = 0;

            foreach (KeyMap map in mKeys)
            {
                map.WarpKey = map.RealKey;
                map.Index = index;
                index++;
            }
        }

        public KeyMap AddKey(double frame, double warpFrame)
        {
            KeyMap map = new KeyMap(frame, warpFrame);

            if (mKeys.Count == 0)
            {
                mKeys.Add(map);
            }
            else
            {
                KeyMap after = GetMapAfter(warpFrame);
                if (warpFrame < after.WarpKey)
                {
                    mKeys.Insert(after.Index, map);
                }
                else
                {
                    mKeys.Insert(after.Index + 1, map);
                }
            }

            SetIndices();

            return map;
        }

        public void RemoveKey(double warpFrame)
        {
            if (mKeys.Count > 2)
            {
                KeyMap after = GetClosest(warpFrame);
                mKeys.Remove(after);
            }

            SetIndices();
        }

        private void SetIndices()
        {
            int index = 0;

            foreach (KeyMap map in mKeys)
            {
                map.Index = index;
                index++;
            }
        }

        public void MoveKey(double frame, double warpFrame)
        {
            KeyMap after = GetClosest(frame);
            after.WarpKey = warpFrame;

            mKeys.Sort();
            SetIndices();
        }

        public double GetWarped(double key)
        {
            KeyMap before = GetMapBefore(key);

            if (before.WarpKey == key)
            {
                return before.RealKey;
            }

            KeyMap after = GetMapAfter(key);

            if (after.WarpKey == key)
            {
                return after.RealKey;
            }

            if (before == after) //Out of bounds
            {
                if (after.Index == (int)(mKeys.Count - 1))
                {
                    return before.RealKey + 1;
                }

                return before.RealKey;
            }

            return Blend(before.RealKey, after.RealKey, (key - before.WarpKey) / (after.WarpKey - before.WarpKey));
        }

        private double Blend(double d_before, double d_after, double value)
        {
            return d_before * (1 - value) + d_after * value;
        }

        public KeyMap GetClosest(double key)
        {
            KeyMap before = GetMapBefore(key);

            if (before.WarpKey == key)
            {
                return before;
            }

            KeyMap after = GetMapAfter(key);

            if (after.WarpKey == key)
            {
                return after;
            }

            if (key - before.WarpKey < after.WarpKey - key)
            {
                return before;
            }

            return after;
        }

        private KeyMap GetMapAfter(double key)
        {
            foreach (KeyMap map in mKeys)
            {
                if (map.WarpKey >= key)
                {
                    return map;
                }
            }

            return mKeys[mKeys.Count - 1];
        }

        private KeyMap GetMapBefore(double key)
        {
            KeyMap mapBefore = mKeys[0];

            foreach (KeyMap map in mKeys)
            {
                if (map.WarpKey > key)
                {
                    return mapBefore;
                }

                mapBefore = map;
            }

            return mapBefore;
        }

        public bool ContainsKey(double Frame)
        {
            foreach (KeyMap map in mKeys)
            {
                if (Math.Abs(map.WarpKey - Frame) <= 0.2)
                {
                    return true;
                }
            }

            return false;
        }

        public void Reverse()
        {
            foreach (KeyMap map in mKeys)
            {
                double warpKeeper = map.WarpKey;
                map.WarpKey = map.RealKey;
                map.RealKey = warpKeeper;
            }
        }
    }
}
