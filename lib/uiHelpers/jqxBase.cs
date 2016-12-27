using ams;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using Tools;

namespace jqx
{
    public enum alignment { left, center, right }
    public enum verticalPosition { top, bottom, both }
    public enum horizontalAlignment { left, right }
    public enum verticalAlignment { top, bottom }


    [JsonConverter(typeof(_jqxBase.JsonConverter))]
    public abstract class _jqxBase
    {
        #region public class _event
        public class _event
        {
            public string name;

            public static implicit operator _event(string n)
            {
                return new _event() { name = n };
            }
            public static implicit operator string(_event n)
            {
                return n.name;
            }
            public override string ToString()
            {
                if (string.IsNullOrEmpty(name))
                    return null;
                return "jqx" + name + "(event)";
            }
        }
        #endregion
        #region public class _function
        public class _function
        {
            public string name;
            public static implicit operator _function(string n)
            {
                return new _function() { name = n };
            }
            public static implicit operator string(_function n)
            {
                return n.name;
            }
            public override string ToString()
            {
                return name;
            }
        }
        #endregion
        #region values

        internal Dictionary<string, object> _values = new Dictionary<string, object>();

        protected void _set<T>(T value, [CallerMemberName] string name = null, bool quote = true)
        {
            if (typeof(T).IsEnum)
                _values[name] = value.ToString();
            else if (value is string)
                _values[name] = (value as string).Trim(false);
            else
                _values[name] = value;
        }

        protected void _set2(string value, [CallerMemberName] string name = null, bool quote = true)
        {
            this._set((SqlBuilder.str)value, name, quote);
        }
        protected string _get2([CallerMemberName] string name = null)
        {
            return (string)this._get<SqlBuilder.str>(name);
        }

        protected bool _get<T>(out T result, [CallerMemberName] string name = null)
        {
            object _result;
            if (_values.TryGetValue(name, out _result))
            {
                if (_result is T)
                    return _null.result(true, result = (T)_result);
            }
            return _null.noop(false, out result);
        }

        protected T _get<T>([CallerMemberName] string name = null, Func<T> create = null)
        {
            T result;
            if (!_get<T>(out result, name) && (create != null))
                _set<T>(result = create(), name);
            return result;
        }

        protected bool _exists([CallerMemberName] string name = null)
        {
            return _values.ContainsKey(name);
        }

        protected void _del([CallerMemberName] string name = null)
        {
            if (_values.ContainsKey(name))
                _values.Remove(name);
        }

        #endregion

        class JsonConverter : Newtonsoft.Json.JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                throw new NotImplementedException();
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is _jqxBase)
                    serializer.Serialize(writer, ((_jqxBase)value)._values);
                else
                    serializer.Serialize(writer, value);
            }
        }
    }

    public abstract class jqxBase : _jqxBase, IHtmlString
    {
        public string element { get; set; }

        public jqxBase()
        {
            //this.width = "100%";
            //this.height = "100%";
        }

        #region properties

        public string settings
        {
            get { return _get2(); }
            set { _set2(value); }
        }

        public string instance
        {
            get { return _get2(); }
            set { _set2(value); }
        }

        //public object source
        //{
        //    get { return _get<object>(); }
        //    set { _set(value); }
        //}

        /// <summary>
        /// Sets or gets a value indicating whether widget's elements are aligned to support locales using right-to-left fonts.
        /// </summary>
        public bool rtl
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Sets the widget's theme.
        /// </summary>
        public string theme
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        /// <summary>
        /// Set the disabled property.
        /// (false)
        /// </summary>
        public bool disabled
        {
            get { return _get<bool>(); }
            set { _set(value); }
        }

        public string watch
        {
            get { return _get2(); }
            set { _set2(value); }
        }

        public string watchSettings
        {
            get { return _get2(); }
            set { _set2(value); }
        }

        public string ngModel
        {
            get { return _get2(); }
            set { _set2(value); }
        }

        //return JSON.stringify(data);


        public object width
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        public object height
        {
            get { return _get<object>(); }
            set { _set(value); }
        }

        #endregion

        protected void ng_name(StringBuilder s, string name)
        {
            char c;
            if (name.Length > 0)
            {
                c = name[0];
                s.Append(c);
            }
            for (int i = 1, n = name.Length; i < n; i++)
            {
                c = name[i];
                if (char.IsUpper(c))
                {
                    s.Append('-');
                    s.Append(char.ToLower(c));
                }
                else s.Append(c);
            }
        }

        protected virtual string ToHtmlString(string name, object value)
        {
            return value.ToString().Trim(false);
        }

        string IHtmlString.ToHtmlString()
        {
            StringBuilder s = new StringBuilder();
            if (!string.IsNullOrEmpty(this.element))
                s.AppendFormat("<{0} ", this.element);

            ng_name(s, this.GetType().Name);

            foreach (var p in this._values)
            {
                if (p.Value == null) continue;
                Type valueType = p.Value.GetType();
                s.Append(" jqx-");
                ng_name(s, p.Key);
                if (p.Value == null) continue;
                s.Append('=');
                if (p.Value is bool)
                    s.Append(p.Value.ToString().ToLower());
                else if (valueType.IsArray || (p.Value is IList))
                    s.Append(json.SerializeObject(p.Value, quoteName: true, quoteChar: '\''));
                else
                {
                    Type t = p.Value.GetType();
                    bool quote = t.IsEnum || p.Value is string;
                    string _value = this.ToHtmlString(p.Key, p.Value).Trim();
                    if (_value.Length == 0) continue;
                    if (_value[0] == '@')
                    {
                        if (_value.Length == 1) continue;
                        _value = _value.Substring(1);
                    }
                    if (quote)
                        s.AppendFormat(@"""'{0}'""", _value);
                    else
                        s.AppendFormat(@"""{0}""", _value);
                }
            }
            if (!string.IsNullOrEmpty(this.element))
                s.AppendFormat("></{0}>", this.element);
            return s.ToString();
        }
    }

    //public abstract class jqxBase : _jqxBase, IHtmlString
    //{
    //    protected void _name2(StringBuilder s, string value)
    //    {
    //        foreach (char c in value)
    //        {
    //            if (char.IsUpper(c))
    //            {
    //                s.Append('-');
    //                s.Append(char.ToLower(c));
    //            }
    //            else s.Append(c);
    //        }
    //    }

    //    protected virtual void TagName(StringBuilder s)
    //    {
    //        this.TagName(s, this.GetType());
    //    }
    //    protected void TagName(StringBuilder s, params Type[] t)
    //    {
    //        for (int i = 0; i < t.Length; i++)
    //        {
    //            s.Append("-");
    //            _name2(s, t[i].Name);
    //        }
    //    }

    //    protected virtual string ToHtmlString(string name, object value)
    //    {
    //        return value.ToString().Trim(false);
    //    }

    //    string IHtmlString.ToHtmlString()
    //    {
    //        StringBuilder s = new StringBuilder();
    //        if (!string.IsNullOrEmpty(this.element))
    //            s.AppendFormat("<{0} ", this.element);
    //        s.Append("ui");
    //        this.TagName(s);
    //        foreach (var p in this.values)
    //        {
    //            s.Append(" data-");
    //            s.Append(p.Key);
    //            //_name2(s, p.Key);
    //            if (p.Value == null) continue;
    //            string _value;
    //            if (p.Value is bool)
    //            {
    //                _value = p.Value.ToString().ToLower();
    //            }
    //            else if (p.Value.GetType().IsArray)
    //            {
    //                _value = json.SerializeObject(p.Value, quoteName: true, quoteChar: '\'');
    //            }
    //            else
    //            {
    //                _value = this.ToHtmlString(p.Key, p.Value).Trim();
    //                if (_value.Length == 0) continue;
    //                if (_value[0] == '@')
    //                {
    //                    if (_value.Length == 1) continue;
    //                    _value = _value.Substring(1);
    //                }
    //            }
    //            s.AppendFormat("=\"{0}\"", _value);
    //        }
    //        if (!string.IsNullOrEmpty(this.element))
    //            s.AppendFormat("></{0}>", this.element);
    //        return s.ToString();
    //    }

    //    public string element { get; set; }
    //}

    //public abstract class jqxBaseWidget : jqxBase
    //{
    //    public string settings
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    public string instance
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    //public object source
    //    //{
    //    //    get { return _get<object>(); }
    //    //    set { _set(value); }
    //    //}

    //    /// <summary>
    //    /// Sets or gets a value indicating whether widget's elements are aligned to support locales using right-to-left fonts.
    //    /// </summary>
    //    public bool rtl
    //    {
    //        get { return _get<bool>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// Sets the widget's theme.
    //    /// </summary>
    //    public string theme
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// Set the disabled property.
    //    /// (false)
    //    /// </summary>
    //    public bool disabled
    //    {
    //        get { return _get<bool>(); }
    //        set { _set(value); }
    //    }

    //    public object width
    //    {
    //        get { return _get<object>(); }
    //        set { _set(value); }
    //    }

    //    public object height
    //    {
    //        get { return _get<object>(); }
    //        set { _set(value); }
    //    }
    //}
    //public static class _jqx
    //{
    //    public static IHtmlString jqx(this HtmlHelper htmlHelper, jqxBase obj, string s1 = null)
    //    {
    //        string jqx_name = obj.GetType().Name;
    //        StringBuilder s = new StringBuilder(s1);
    //        s.AppendFormat("$('{0}').{1}(", obj.selector, jqx_name);
    //        json.SerializeObject(s, obj.values, Formatting.Indented, true, '\'');
    //        s.AppendFormat(").{0}('getInstance');", jqx_name);
    //        return htmlHelper.Raw(s);
    //    }
    //}

    //public class jqx_angular : DynamicObject, IHtmlString
    //{
    //    string tagName;
    //    public bool inline = true;
    //    public jqx_angular(string tagName)
    //    {
    //        this.tagName = tagName;
    //        if (this.tagName == null)
    //        {
    //            StringBuilder s = new StringBuilder();
    //            _name2(s, this.GetType().Name);
    //            this.tagName = s.ToString();
    //        }
    //    }

    //    Dictionary<string, object> values = new Dictionary<string, object>();

    //    protected virtual string _name(string name)
    //    {
    //        if (name[0] == '@')
    //            return name.Substring(1);
    //        return name;
    //    }

    //    protected void _set<T>(T value, [CallerMemberName] string name = null)
    //    {
    //        values[_name(name)] = value;
    //    }

    //    protected bool _get<T>(out T result, [CallerMemberName] string name = null)
    //    {
    //        object _result;
    //        if (values.TryGetValue(_name(name), out _result))
    //        {
    //            if (_result is T)
    //            {
    //                result = (T)_result;
    //                return true;
    //            }
    //        }
    //        result = default(T);
    //        return false;
    //    }

    //    protected T _get<T>([CallerMemberName] string name = null)
    //    {
    //        T result; _get<T>(out result, _name(name)); return result;
    //    }

    //    protected bool _exists([CallerMemberName] string name = null)
    //    {
    //        return values.ContainsKey(_name(name));
    //    }

    //    protected void _del([CallerMemberName] string name = null)
    //    {
    //        name = _name(name);
    //        if (values.ContainsKey(name))
    //            values.Remove(name);
    //    }

    //    public override bool TrySetMember(SetMemberBinder binder, object value)
    //    {
    //        values[binder.Name] = value;
    //        return true;
    //    }

    //    public override bool TryGetMember(GetMemberBinder binder, out object result)
    //    {
    //        return values.TryGetValue(binder.Name, out result);
    //    }

    //    protected virtual string ToHtmlString(string name, object value)
    //    {
    //        return value.ToString();
    //    }

    //    void _name2(StringBuilder s, string value)
    //    {
    //        foreach (char c in value)
    //        {
    //            if (char.IsUpper(c))
    //            {
    //                s.Append('-');
    //                s.Append(char.ToLower(c));
    //            }
    //            else s.Append(c);
    //        }
    //    }

    //    string IHtmlString.ToHtmlString()
    //    {
    //        StringBuilder s = new StringBuilder();
    //        if (!inline)
    //            s.Append('<');
    //        s.Append(tagName);
    //        foreach (var p in this.values)
    //        {
    //            s.Append(' ');
    //            _name2(s, p.Key);
    //            if (p.Value == null) continue;
    //            string _value;
    //            if (p.Value is bool)
    //            {
    //                _value = p.Value.ToString().ToLower();
    //            }
    //            else
    //            {
    //                _value = this.ToHtmlString(p.Key, p.Value).Trim();
    //                if (_value.Length == 0) continue;
    //                if (_value[0] == '@')
    //                {
    //                    if (_value.Length == 1) continue;
    //                    _value = _value.Substring(1);
    //                }
    //            }
    //            s.AppendFormat("=\"{0}\"", _value);
    //        }
    //        if (!inline)
    //            s.AppendFormat("></{0}>", tagName);
    //        return s.ToString();
    //    }
    //}
    //public abstract class jqxBase : jqx_angular
    //{
    //    #region public struct _event
    //    public struct _event
    //    {
    //        public string name;

    //        public static implicit operator _event(string n)
    //        {
    //            return new _event() { name = n };
    //        }
    //        public static implicit operator string(_event n)
    //        {
    //            return n.name;
    //        }
    //        public override string ToString()
    //        {
    //            if (string.IsNullOrEmpty(name))
    //                return null;
    //            return name + "(event)";
    //        }
    //    }
    //    #endregion
    //    #region public struct _function
    //    public class _function
    //    {
    //        public string name;
    //        public static implicit operator _function(string n)
    //        {
    //            return new _function() { name = n };
    //        }
    //        public static implicit operator string(_function n)
    //        {
    //            return n.name;
    //        }
    //        public override string ToString()
    //        {
    //            return name;
    //        }
    //    }
    //    #endregion
    //    public enum _alignment { left, center, right }

    //    public jqxBase() : this(null) { }
    //    public jqxBase(string tagName) : base(tagName)
    //    {
    //        //this.watchSettings = true;
    //    }

    //    public dynamic _extend
    //    {
    //        get { return this; }
    //    }

    //    #region jqx get/set

    //    protected override string _name(string name)
    //    {
    //        name = base._name(name);
    //        return "jqx" + char.ToUpper(name[0]) + name.Substring(1);
    //    }

    //    string _jqx_name<T>(string name)
    //    {
    //        return "jqx" + char.ToUpper(name[0]) + name.Substring(1);
    //    }

    //    #endregion

    //    #region Properties

    //    #region angular

    //    /// <summary>
    //    /// The "jqx-source" attribute is a special attribute which can be used for setting the widget's data source to an object or property defined in the Controller. 
    //    /// </summary>
    //    //public string source
    //    //{
    //    //    get { return jqx_get<string>(); }
    //    //    set { jqx_set(value); }
    //    //}

    //    /// <summary>
    //    /// The "jqx-settings" attribute can be used for setting the widget properties through an object defined in the Controller.
    //    /// </summary>
    //    public string settings
    //    {
    //        get { return _get<string>(); }
    //        set
    //        {
    //            _set(value);
    //            //this.instance = this.instance ?? value + ".instance";
    //            if (value != null)
    //            {
    //                if (value.Contains('.'))
    //                    _set(value);
    //                else
    //                    _set(string.Format("jqxSettings.{0}", value));
    //            }
    //        }
    //    }
    //    public string settings_str
    //    {
    //        get { return _get<string>("settings"); }
    //        set { _set(value, "settings"); }
    //    }

    //    /// <summary>
    //    /// For automatic refreshes, you can add the "jqx-watch-settings" attribute to your HTML Element. By doing that the plugin will watch for changes in the settings object and will refresh the widget if necessary.
    //    /// </summary>
    //    public bool watchSettings
    //    {
    //        get { return _exists(); }
    //        set { if (value) _set<string>(null); else _del(); }
    //    }

    //    /// <summary>
    //    /// The "jqx-watch" attribute can be used for performing an object equality watch. When there is a change in the watched property's value or any of its inner properties, the widget will perform an automatic update. The attribute can be used for watching changes in multiple properties. Example: jqx-watch="[calendarSettings.firstDayOfWeek, calendarSettings.width]"
    //    /// </summary>
    //    public string watch
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// The "jqx-create" attribute can be used for creating the widget on demand or with some delay. For example, you may want the widget to be created after some other action like when an Ajax call is completed or when the widget's settings object is available.
    //    /// </summary>
    //    public string create
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// The "jqx-instance" attribute can be used for setting the widget instance in the Controller. You will have to initialize an empty object in the Controller and then set the "jqx-instance" attribute to point to it. The instance will allow you to easily invoke widget methods.
    //    /// </summary>
    //    public string instance
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    #endregion

    //    /// <summary>
    //    /// Sets or gets a value indicating whether widget's elements are aligned to support locales using right-to-left fonts.
    //    /// </summary>
    //    public bool rtl
    //    {
    //        get { return _get<bool>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// Sets the widget's theme.
    //    /// </summary>
    //    public string theme
    //    {
    //        get { return _get<string>(); }
    //        set { _set(value); }
    //    }

    //    /// <summary>
    //    /// Set the disabled property.
    //    /// (false)
    //    /// </summary>
    //    public bool disabled
    //    {
    //        get { return _get<bool>(); }
    //        set { _set(value); }
    //    }

    //    public int width
    //    {
    //        get { return _get<int>(); }
    //        set { _set(value); }
    //    }

    //    public int height
    //    {
    //        get { return _get<int>(); }
    //        set { _set(value); }
    //    }

    //    #endregion
    //}
    public class jqxWindow : jqxBase { }
    public class jqxKnob : jqxBase { }
    public class jqxResponsivePanel : jqxBase { }
    public class jqxChart : jqxBase { }
    public class jqxMenu : jqxBase { }
    public class jqxDocking : jqxBase { }
    public class jqxGauge : jqxBase { }
    public class jqxNavBar : jqxBase { }
    public class jqxScheduler : jqxBase { }
    public class jqxNotification : jqxBase { }
    public class jqxPanel : jqxBase { }
    public class jqxTagCloud : jqxBase { }
    public class jqxNavigationBar : jqxBase { }
    public class jqxPopover : jqxBase { }
    public class jqxExpander : jqxBase { }
    public class jqxTooltip : jqxBase { }
    public class jqxRating : jqxBase { }
    public class jqxDraw : jqxBase { }
    public class jqxTreeMap : jqxBase { }
    public class jqxToolBar : jqxBase { }
    public class jqxColorPicker : jqxBase { }
    public class jqxRangeSelector : jqxBase { }
    public class jqxDragDrop : jqxBase { }
    public class jqxEditor : jqxBase { }
    public class jqxScrollView : jqxBase { }
    public class jqxDataAdapter : jqxBase { }
    public class jqxRibbon : jqxBase { }
    public class jqxProgressBar : jqxBase { }
    public class jqxScrollBar : jqxBase { }
    public class jqxResponse : jqxBase { }
    public class jqxListMenu : jqxBase { }
    public class jqxFileUpload : jqxBase { }
    public class jqxValidator : jqxBase { }
    public class jqxButtons : jqxBase { }
    public class jqxKanban : jqxBase { }
    public class jqxBulletChart : jqxBase { }
    public class jqxSortable : jqxBase { }
}