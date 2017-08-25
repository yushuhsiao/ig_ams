using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using mshtml;
using SHDocVw;

namespace mshtml
{
    public static class mshtml_extensions
    {
        /// <summary>
        /// get frame by name
        /// </summary>
        public static HTMLWindow2 frames(this HTMLDocument doc, string name)
        {
            for (int i = 0; i < doc.frames.length; i++)
            {
                HTMLWindow2 w = doc.frames(i);
                if (w.name == name) return w;
            }
            return null;
        }
        /// <summary>
        /// get frame by index
        /// </summary>
        public static HTMLWindow2 frames(this HTMLDocument doc, int index)
        {
            object o = index;
            return doc.frames.item(ref o) as HTMLWindow2;
        }
        /// <summary>
        /// enum frames
        /// </summary>
        public static IEnumerable<HTMLWindow2> frames(this HTMLDocument doc)
        {
            for (int i = 0; i < doc.frames.length; i++)
                yield return doc.frames(i);
        }
        public static IEnumerable<HTMLWindow2> frames(this IHTMLDocument2 doc)
        {
            return frames((HTMLDocument)doc);
        }
        public static HTMLDocument document(this HTMLWindow2 w)
        {
            return (HTMLDocument)w.document;
        }
    }
}
namespace SHDocVw
{
    public static class SHDocVw_Extensions
    {
        public static HTMLDocument Document(this InternetExplorer ie)
        {
            return (HTMLDocument)ie.Document;
        }
    }
}