using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

namespace TK.BaseLib.Tags
{
    public class TagManager
    {
        private TagManager()
        {
            serializer = new XmlSerializer(typeof(TagList));
        }

        XmlSerializer serializer;

        private static TagManager instance = null;

        public static TagManager GetInstance()
        {
            if (instance == null)
                instance = new TagManager();

            return instance;
        }

        public static string Serialize(TagList taglist)
        {
            MemoryStream stream = new MemoryStream();
            GetInstance().serializer.Serialize(stream,taglist);
            return TypesHelper.StringFromStream(stream);
        }

        public static TagList DeSerialize(string serialized)
        {
            MemoryStream stream = new MemoryStream();
            return (TagList)GetInstance().serializer.Deserialize(TypesHelper.StringToStream(serialized));
        }
    }
}
