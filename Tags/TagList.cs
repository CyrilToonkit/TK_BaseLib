using System;
using System.Collections.Generic;
using System.Text;

namespace TK.BaseLib.Tags
{
    public class TagList
    {
        List<Tag> mTags = new List<Tag>();
        public List<Tag> Tags
        {
            get { return mTags; }
            set { mTags = value; }
        }

        public bool HasTag(string Key)
        {
            foreach (Tag tag in mTags)
            {
                if (tag.Key == Key)
                {
                    return true;
                }
            }

            return false;
        }

        public Tag GetTag(string Key)
        {
            foreach (Tag tag in mTags)
            {
                if (tag.Key == Key)
                {
                    return tag;
                }
            }

            return null;
        }
    }
}
