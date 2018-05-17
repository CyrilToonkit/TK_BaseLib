using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace TK.BaseLib
{
    public class Mapper
    {
        public Mapper(string inPrefix, string inRefPrefix)
        {
            refPrefix = inRefPrefix;
            prefix = inPrefix;
        }

        protected Dictionary<string, string> translator = new Dictionary<string, string>();
        public Dictionary<string, string> Translator
        {
            get
            {
                return translator;
            }
            set
            {
                translator = value;
            }
        }

        protected virtual string GetHimself(string inTarget)
        {
            return inTarget;
        }

        string prefix = string.Empty;
        public string Prefix
        {
            get
            {
                return prefix;
            }
            set
            {
                prefix = value;
            }
        }

        string refPrefix = string.Empty;
        public string RefPrefix
        {
            get
            {
                return refPrefix;
            }
            set
            {
                refPrefix = value;
            }
        }

        /// <summary>
        /// Get ref objects by priority order
        /// </summary>
        /// <param name="inTarget"></param>
        /// <param name="inRefs"></param>
        /// <returns></returns>
        public virtual List<string> GetRefs(string inTarget, string inRefs)
        {
            List<string> refs = TypesHelper.StringSplit(inRefs, ";", true, true);

            //If the list contains None, don't add itself to possible matches
            if (refs.Contains("None"))
            {
                refs.Remove("None");
            }
            else //Add itself to possible matches
            {
                bool offsettedSelf = false;
                for (int i = 0; i < refs.Count; i++)
                {
                    if (refs[i].StartsWith("+"))
                    {
                        refs[i] = GetHimself(inTarget) + refs[i];
                        offsettedSelf = true;
                        break;
                    }
                }

                if (!offsettedSelf)
                {
                    refs.Add(GetHimself(inTarget));
                }
            }

            return refs;
        }

        /// <summary>
        /// Clean optional match helpers after matching
        /// </summary>
        /// <param name="inTarget"></param>
        public virtual void Matched(string inTarget)
        {
        }
    }
}
