using iText.Kernel.XMP;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICEAPP.Utility
{
    public class MetaData
    {
        private Hashtable info = new Hashtable();

        public string Author { get => (string)info["Author"]; set => info["Author"] = value; }
        public string Title { get => (string)info["Title"]; set => info["Title"] = value; }
        public string Subject { get => (string)info["Subject"]; set => info["Subject"] = value; }
        public string Keywords { get => (string)info["Keywords"]; set => info["Keywords"] = value; }
        public string Producer { get => (string)info["Producer"]; set => info["Producer"] = value; }
        public string Creator { get => (string)info["Creator"]; set => info["Creator"] = value; }

        public Hashtable getMetaData() => info;

        public byte[] getStreamedMetaData()
        {
            using MemoryStream os = new MemoryStream();
            XMPMeta meta = XMPMetaFactory.Create();
            foreach (DictionaryEntry item in info)
            {
                meta.SetProperty(XMPConst.NS_DC, item.Key.ToString(), item.Value.ToString());
            }
            XMPMetaFactory.Serialize(meta, os);
            return os.ToArray();
        }
    }
}
