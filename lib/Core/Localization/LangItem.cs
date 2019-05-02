using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Web;
using LCID = System.Int32;

namespace InnateGlory
{
    //using dict1 = Dictionary<int, LangItem._row>;
    //using dict2 = Dictionary<string, Dictionary<int, LangItem._row>>;
    //using dict3 = Dictionary<string, Dictionary<string, Dictionary<int, LangItem._row>>>;

    internal class LangItem : PathNode<LangItem>
    {
        class _row
        {
            public Entity.Lang src;
            private HtmlString _html;
            public HtmlString html => _html = _html ?? new HtmlString(src.Text);
            public bool exists;
            private long count;

            public long Count => Interlocked.Read(ref count);
            public long IncrementCount() => Interlocked.Increment(ref count);
        }

        private Dictionary<string, Dictionary<string, Dictionary<int, _row>>> _datas =
            new Dictionary<string, Dictionary<string, Dictionary<int, _row>>>(StringComparer.OrdinalIgnoreCase);
        private SyncList<_row> _rows = new SyncList<_row>();

        public PlatformId PlatformId { get; }

        public LangItem() { }
        public LangItem(PlatformId platformId)
        {
            this.PlatformId = platformId;
        }

        public void AddData(Entity.Lang data) => AddData(data, out _row row);
        private void AddData(Entity.Lang data, out _row row)
        {
            lock (_rows)
            {
                row = new _row() { src = data, exists = true };
                _datas
                    .GetValue(data.Type.Trim(false) ?? "", () => new Dictionary<string, Dictionary<int, _row>>(StringComparer.OrdinalIgnoreCase))
                    .GetValue(data.Key.Trim(false) ?? "", () => new Dictionary<int, _row>())
                    [data.LCID] = row;
                _rows.Add(row);
            }
        }

        private bool _GetText(PlatformId platformId, string type, string key, LCID? lcid, string defaultText, out HtmlString html)
        {
            if (platformId == this.PlatformId)
            {
                lock (_rows)
                {
                    foreach (var node in this.GetParents())
                    {
                        if (node._datas.TryGetValue(type, out var list2) && list2.TryGetValue(key, out var list1))
                        {
                            foreach (CultureInfo c in GetCultureInfo(lcid).GetParents())
                            {
                                LCID _lcid = c.LCID == 127 ? 0 : c.LCID;

                                if (list1.TryGetValue(_lcid, out var data))
                                {
                                    html = data.html;
                                    data.IncrementCount();
                                    return data.exists;
                                }
                            }
                        }
                    }
                    AddData(new Entity.Lang()
                    {
                        Path = this.FullPath.Substring(2),
                        Type = type,
                        Key = key,
                        LCID = 0,
                        PlatformId = platformId,
                        Text = defaultText
                    }, out var row);
                    html = row.html;
                    row.IncrementCount();
                    return row.exists = false;
                }
            }
            html = new HtmlString(defaultText);
            return false;
        }

        public bool GetText(PlatformId platformId, string key, LCID? lcid, string defaultText, out HtmlString html)
        {
            string _type = "";
            string _key = key.Trim(false) ?? "";
            return _GetText(platformId, _type, _key, lcid, defaultText, out html);
        }

        public bool GetEnum(PlatformId platformId, object key, LCID? lcid, string defaultText, out HtmlString html)
        {
            if (key is string)
                return GetText(platformId, (string)key, lcid, defaultText, out html);
            if (key == null)
                return _null.noop(false, out html);
            Type type = key.GetType();
            string _type = type.Name;
            string _key = key.ToString();
            return _GetText(platformId, _type, _key, lcid, defaultText, out html);
        }

        public IEnumerable<Entity.Lang> GetRows(Predicate<Entity.Lang> match = null, bool? exists = null)
        {
            foreach (var row in _rows)
            {
                if (exists.HasValue && exists.Value != row.exists)
                    continue;
                if (match != null && match(row.src) == false)
                    continue;
                yield return row.src;
            }
        }

        private static CultureInfo GetCultureInfo(LCID? lcid)
        {
            if (lcid.HasValue)
            {
                try { return CultureInfo.GetCultureInfo(lcid.Value); }
                catch { }
            }
            return CultureInfo.CurrentUICulture;
        }
    }
}