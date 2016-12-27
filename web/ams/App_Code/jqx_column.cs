using ams;
using ams.Data;
using ASP;
using jqx;
using System.Text;
using System.Web.Mvc;
using datafield = jqx.jqxGrid.datafield;

public class jqx_column
{
    static void set_datafield(datafield value, dataFieldType type)
    {
        if (value != null) value.type = type;
    }

    public class DateTime : jqxGrid._column
    {
        public DateTime(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new jqxGrid.datafield() { name = name ?? this.GetType().Name };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.editable = false;
            this.ColumnDefine = "DateTime";
        }
    }
    public class Op_User : jqxGrid._column
    {
        public Op_User(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.width = 100;
            this.editable = false;
        }
        public override datafield _datafield
        {
            get { return base._datafield; }
            set { set_datafield(base._datafield = value, dataFieldType._number); }
        }
    }

    public class CreateTime : DateTime { public CreateTime(_WebViewPage page) : base(page) { } }
    public class CreateUser : Op_User { public CreateUser(_WebViewPage page) : base(page) { } }
    public class ModifyTime : DateTime { public ModifyTime(_WebViewPage page) : base(page) { } }
    public class ModifyUser : Op_User { public ModifyUser(_WebViewPage page) : base(page) { } }

    public class _string : jqxGrid._column
    {
        public _string(_WebViewPage page, string name, string text = null)
        {
            this._datafield = new datafield { name = name, type = dataFieldType._string };
            this.text = text ?? page.lang[name];
            this.width = 100;
            this.columntype = columntype.textbox;
        }
    }
    public class _number : jqxGrid._column
    {
        public _number(_WebViewPage page, string name, string text = null)
        {
            this._datafield = new datafield { name = name, type = dataFieldType._number };
            this.text = text ?? page.lang[name];
            this.width = 100;
        }
    }
    public class _boolean : jqxGrid._column
    {
        public _boolean(_WebViewPage page, string name, string text = null)
        {
            this._datafield = new datafield { name = name, type = dataFieldType._bool__ };
            this.text = text ?? page.lang[name];
            this.width = 100;
            this.columntype = columntype.textbox;
        }
    }
    public class Amount : jqxGrid._column
    {
        public Amount(_WebViewPage page, string name, string text = null)
        {
            this._datafield = new datafield { name = name };
            this.text = text ?? page.lang[name];
            this.ColumnDefine = "Amount";
        }
    }

    public class CorpID : jqxGrid._column
    {
        public CorpID(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang["Corp ID"];
            this.width = 100;
            this.hidden = true;
            //this.filtertype = filtertype.list;
            //CorpInfo corp;
            //this.filteritems = langHelper.CorpList(out corp);
            //this.filterdefault = corp.UserName;
        }
    }
    public class CorpIDx : jqxGrid._column
    {
        public bool root;

        public CorpIDx(_WebViewPage page, string name = "CorpID")
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang["Corp Name"];
            this.width = 100;
            this.filtertype = filtertype.list;
            CorpInfo corp;
            this.filteritems = langHelper.CorpList(out corp, root);
            if (corp != null) this.filterdefault = corp.UserName;
        }
    }
    public class CorpName : jqxGrid._column
    {
        public bool root;

        public CorpName(_WebViewPage page, string name = null, bool root = false)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang["Corp Name"];
            this.width = 100;
            this.filtertype = filtertype.list;
            CorpInfo corp;
            this.filteritems = langHelper.CorpList(out corp, root);
            if (corp != null) this.filterdefault = corp.UserName;
        }
    }
    /// <summary>
    /// ID for int
    /// </summary>
    public class ID1 : jqxGrid._column
    {
        public ID1(_WebViewPage page, string text = null)
        {
            this._datafield = new datafield { name = "ID", type = dataFieldType._number };
            this.text = text ?? page.lang["ID"];
            this.width = 100;
        }
    }
    /// <summary>
    /// ID for GUID
    /// </summary>
    public class ID2 : jqxGrid._column
    {
        public ID2(_WebViewPage page, string name = "ID", string text = null)
        {
            this._datafield = new datafield { name = name, type = dataFieldType._string };
            this.text = text ?? page.lang["ID"];
            this.width = 100;
        }
    }
    public class ver : jqxGrid._column
    {
        public ver(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.hidden = true;
        }
    }
    public class ParentID : jqxGrid._column
    {
        public ParentID(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang["Parent ID"];
            this.width = 100;
        }
    }
    public class ParentName : jqxGrid._column
    {
        public ParentName(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._string };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.width = 100;
        }
    }
    public class AgentID : jqxGrid._column
    {
        public AgentID(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang["Agent ID"];
            this.width = 100;
        }
    }
    public class AgentName : jqxGrid._column
    {
        public AgentName(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._string };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.width = 100;
        }
    }
    public class UserID : jqxGrid._column
    {
        public UserID(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._number };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.width = 100;
            this.hidden = true;
        }
    }
    public class UserName : jqxGrid._column
    {
        public UserName(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._string };
            this.text = text ?? page.lang[name ?? this.GetType().Name];
            this.width = 100;
        }
    }
    public class NickName : jqxGrid._column
    {
        public NickName(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
        }
    }
    public class Depth : jqxGrid._column
    {
        public Depth(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.hidden = true;
        }
    }

    public class AdminActive : jqxGrid._column
    {
        public AdminActive(_WebViewPage page)
        {
            this._datafield = new datafield { name = "Active", type = dataFieldType._number };
            this.text = page.lang["State"];
            this.width = 050;
            this.columntype = columntype.checkbox;
        }
    }
    public class Active : jqxGrid._column
    {
        public Active(_WebViewPage page)
        {
            this._datafield = new datafield { name = "Active", type = dataFieldType._number };
            this.text = page.lang["Status"];
            this.width = 050;
            this.columntype = columntype.checkbox;
        }
    }
    public class Currency : jqxGrid._column
    {
        public Currency(_WebViewPage page, string name = null, string text = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._number };
            this.text = text ?? page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.GetEnumsIn(CurrencyCode.IG_Point, CurrencyCode.IG_ECoin, CurrencyCode.TWD, CurrencyCode.CNY, CurrencyCode.HKD, CurrencyCode.USD, CurrencyCode.EUR);
        }
        public override datafield _datafield
        {
            get { return base._datafield; }
            set { set_datafield(base._datafield = value, dataFieldType._string); }
        }
    }
    public class LogType : jqxGrid._column
    {
        public LogType(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang[this.GetType().Name];
            this.width = 150;
            this.hidden = true;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.LogTypes();
        }
    }
    public class PaymentType : jqxGrid._column
    {
        public PaymentType(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            //this.hidden = true;
        }
    }
    public class PaymentProvider : jqxGrid._column
    {
        public PaymentProvider(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            //this.hidden = true;
        }
    }
    public class PlatformID : jqxGrid._column
    {
        public PlatformID(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.PlatformIDs();
            this.hidden = true;
        }
    }
    public class PlatformName : jqxGrid._column
    {
        public PlatformName(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.PlatformNames();
        }
    }
    public class PlatformType : jqxGrid._column
    {
        public PlatformType(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.PlatformTypes();
        }
        public override datafield _datafield
        {
            get { return base._datafield; }
            set { set_datafield(base._datafield = value, dataFieldType._string); }
        }
    }
    public class GameID : jqxGrid._column
    {
        public GameID(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._number };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.hidden = true;
            //this.filtertype = filtertype.checkedlist;
        }
    }
    public class GameName : jqxGrid._column
    {
        public GameName(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.GameNames();
        }
    }
    public class GameClass : jqxGrid._column
    {
        public GameClass(_WebViewPage page)
        {
            this._datafield = new datafield { name = this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.GetEnums(ams.GameClass.Others);
        }
        public override datafield _datafield
        {
            get { return base._datafield; }
            set { set_datafield(base._datafield = value, dataFieldType._string); }
        }
    }
    public class AppealState : jqxGrid._column
    {
        public AppealState(_WebViewPage page, string name = null)
        {
            this._datafield = new datafield { name = name ?? this.GetType().Name, type = dataFieldType._string };
            this.text = page.lang[name ?? this.GetType().Name];
            this.width = 100;
            this.filtertype = filtertype.checkedlist;
            this.filteritems = langHelper.GetEnums<ams.Data.AppealState>();
        }
    }
    public class ActionPopup : jqxGrid._column
    {
        public ActionPopup(_WebViewPage page, string name, string text = "")
        {
            this.name = name;
            this.text = text;
            this.width = 70;
            this.ColumnDefine = this.GetType().Name;
        }

        public string ButtonText
        {
            get { return _get<string>(); }
            set { _set(value); }
        }

        popups.dialog _modal;
        public popups.dialog modal
        {
            get { return _modal; }
            set
            {
                _modal = value;
                if (value != null)
                { this.ModalSelector = value.Selector; }
                else
                { _del("ModalSelector"); }
            }
        }
        public string ModalSelector
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
    }
    public class ActionButton : jqxGrid._column
    {
        public ActionButton(_WebViewPage page, string name, string text = "")
        {
            this.name = name;
            this.text = text;
            this.width = 60;
            this.ColumnDefine = this.GetType().Name;
        }

        public string ButtonText
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
    }
    public class ActionDropdown : jqxGrid._column
    {
        public ActionDropdown(_WebViewPage page, string name, string text = "")
        {
            this.name = name;
            this.text = text;
            this.width = 60;
            this.ColumnDefine = this.GetType().Name;
        }

        public string ButtonText
        {
            get { return _get<string>(); }
            set { _set(value); }
        }
    }
    public class ActionEdit : jqxGrid._column
    {
        public ActionEdit(_WebViewPage page, string name = "_action_inline_edit", string text = "")
        {
            this.name = name;
            this.text = text;
            this.ColumnDefine = this.GetType().Name;
        }
    }
}
