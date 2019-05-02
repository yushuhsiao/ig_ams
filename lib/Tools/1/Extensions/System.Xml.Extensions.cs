using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;

namespace System.Xml
{
    public static class Extensions
    {
        public static bool? GetBoolean(this XmlAttribute a)
        {
            if (a == null) return null; return a.Value.ToBoolean();
        }
        public static bool? GetBoolean(this XmlElement e, string name)
        {
            if (e == null) return null; return e.GetAttribute(name).ToBoolean();
        }
        
        //static bool getstr<T>(this XmlElement e, string name, out string attrstr, out T v)
        //{
        //    v = default(T);
        //    attrstr = default(string);
        //    XmlAttribute attr = e.Attributes[name];
        //    if (attr != null)
        //    {
        //        bool n = !string.IsNullOrEmpty(attr.Value);
        //        if (n) attrstr = attr.Value;
        //        return n;
        //    }
        //    return false;
        //}


        public static T GetAttribute<T>(this XmlElement e, string name)
        {
            T value;
            e.GetAttribute<T>(name, out value);
            return value;
        }
        public static T GetValue<T>(this XmlAttribute attr)
        {
            T value;
            attr.GetValue<T>(out value);
            return value;
        }

        public static T GetAttribute<T>(this XmlElement e, string name, T d)
        {
            T value;
            return e.GetAttribute<T>(name, out value) ? value : d;
        }
        public static T GetValue<T>(this XmlAttribute attr, T d)
        {
            T value;
            return attr.GetValue<T>(out value) ? value : d;
        }

        public static bool GetAttribute<T>(this XmlElement e, string name, out T value)
        {
            return e.GetAttributeNode(name).GetValue<T>(out value);
        }
        public static bool GetValue<T>(this XmlAttribute a, out T value)
        {
            object tmp;
            if (a.GetValue(typeof(T), out tmp))
            {
                if (tmp is T)
                {
                    value = (T)tmp;
                    return true;
                }
            }
            value = default(T);
            return false;
        }

        public static bool GetAttribute(this XmlElement e, string name, Type type, out object value)
        {
            return e.GetAttributeNode(name).GetValue(type, out value);
        }
        public static bool GetValue(this XmlAttribute a, Type type, out object value)
        {
            if ((a != null) && !string.IsNullOrEmpty(a.Value))
            {
                TypeConverter c = TypeDescriptor.GetConverter(type);
                if (c.CanConvertFrom(typeof(string)))
                {
                    value = c.ConvertFromString(a.Value);
                    if (value != null)
                    {
                        if (value.GetType() == type)
                        {
                            return true;
                        }
                    }
                }
            }
            value = null;
            return false;
        }


        //public static Boolean GetBoolean(this XmlElement e, string name) /*******************/ { return e.GetAttributeNode(name).GetBoolean().GetValueOrDefault(); }                        // { return Boolean.Parse(e.Attributes[name].Value); }
        public static Boolean GetBoolean(this XmlElement e, string name, Boolean d) /********/ { return e.GetAttributeNode(name).GetBoolean().GetValueOrDefault(d); }                       // { Boolean r; return e.GetAttribute(name, out r) ? r : d; }
        public static Boolean GetAttribute(this XmlElement e, string name, Boolean d) /******/ { return e.GetAttributeNode(name).GetBoolean().GetValueOrDefault(d); }                       // Boolean r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetBoolean(this XmlElement e, string name, out Boolean v) /*******/ { return e.GetAttributeNode(name).GetBoolean(out v); }                                       // { string s; if (e.getstr<Boolean>(name, out s, out v)) return Boolean.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Boolean v) /*****/ { return e.GetAttributeNode(name).GetBoolean(out v); }                                       // string s; if (e.getstr<Boolean>(name, out s, out v)) return Boolean.TryParse(s, out v); return false; }
        public static Boolean? GetBooleanN(this XmlElement e, string name) /*****************/ { return e.GetAttributeNode(name).GetBoolean(); }                                            // { if (e != null) return e.Attributes[name].ToBoolean(); return null; }

        //public static Boolean? GetBoolean(this XmlAttribute a) /******************************/ { return a.GetString().ToBoolean(); }
        public static bool GetBoolean(this XmlAttribute a, out Boolean value) /***************/ { return a.GetString().ToBoolean(out value); }



        public static Byte GetByte(this XmlElement e, string name) /*************************/ { return e.GetAttributeNode(name).GetByte().GetValueOrDefault(); }                           // { return Byte.Parse(e.Attributes[name].Value); }
        public static Byte GetByte(this XmlElement e, string name, Byte d) /*****************/ { return e.GetAttributeNode(name).GetByte().GetValueOrDefault(d); }                          // { Byte r; return e.GetAttribute(name, out r) ? r : d; }
        public static Byte GetAttribute(this XmlElement e, string name, Byte d) /************/ { return e.GetAttributeNode(name).GetByte().GetValueOrDefault(d); }                          // Byte r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetAttribute(this XmlElement e, string name, out Byte v) /********/ { return e.GetAttributeNode(name).GetByte(out v); }                                          // string s; if (e.getstr<Byte>(name, out s, out v)) return Byte.TryParse(s, out v); return false; }
        public static bool GetByte(this XmlElement e, string name, out Byte v) /*************/ { return e.GetAttributeNode(name).GetByte(out v); }                                          // string s; if (e.getstr<Byte>(name, out s, out v)) return Byte.TryParse(s, out v); return false; }
        public static Byte? GetByteN(this XmlElement e, string name) /***********************/ { return e.GetAttributeNode(name).GetByte(); }                                               //if (e != null) return e.Attributes[name].ToByte(); return null; }

        public static Byte? GetByte(this XmlAttribute a) /************************************/ { return a.GetString().ToByte(); }
        public static bool GetByte(this XmlAttribute a, out Byte value) /*********************/ { return a.GetString().ToByte(out value); }



        public static SByte GetSByte(this XmlElement e, string name) /************************/ { return e.GetAttributeNode(name).GetSByte().GetValueOrDefault(); }                         // { return SByte.Parse(e.Attributes[name].Value); }
        public static SByte GetSByte(this XmlElement e, string name, SByte d) /***************/ { return e.GetAttributeNode(name).GetSByte().GetValueOrDefault(d); }                        // { SByte r; return e.GetAttribute(name, out r) ? r : d; }
        public static SByte GetAttribute(this XmlElement e, string name, SByte d) /***********/ { return e.GetAttributeNode(name).GetSByte().GetValueOrDefault(d); }                        // SByte r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetSByte(this XmlElement e, string name, out SByte v) /************/ { return e.GetAttributeNode(name).GetSByte(out v); }                                        // string s; if (e.getstr<SByte>(name, out s, out v)) return SByte.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out SByte v) /********/ { return e.GetAttributeNode(name).GetSByte(out v); }                                        // string s; if (e.getstr<SByte>(name, out s, out v)) return SByte.TryParse(s, out v); return false; }
        public static SByte? GetSByteN(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetSByte(); }                                             // if (e != null) return e.Attributes[name].ToSByte(); return null; }

        public static SByte? GetSByte(this XmlAttribute a) /***********************************/ { return a.GetString().ToSByte(); }
        public static bool GetSByte(this XmlAttribute a, out SByte value) /********************/ { return a.GetString().ToSByte(out value); }



        public static Int16 GetInt16(this XmlElement e, string name) /************************/ { return e.GetAttributeNode(name).GetInt16().GetValueOrDefault(); }                         // { return Int16.Parse(e.Attributes[name].Value); }
        public static Int16 GetInt16(this XmlElement e, string name, Int16 d) /***************/ { return e.GetAttributeNode(name).GetInt16().GetValueOrDefault(d); }                        // { Int16 r; return e.GetAttribute(name, out r) ? r : d; }
        public static Int16 GetAttribute(this XmlElement e, string name, Int16 d) /***********/ { return e.GetAttributeNode(name).GetInt16().GetValueOrDefault(d); }                        //Int16 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetInt16(this XmlElement e, string name, out Int16 v) /************/ { return e.GetAttributeNode(name).GetInt16(out v); }                                        //  string s; if (e.getstr<Int16>(name, out s, out v)) return Int16.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Int16 v) /********/ { return e.GetAttributeNode(name).GetInt16(out v); }                                        // string s; if (e.getstr<Int16>(name, out s, out v)) return Int16.TryParse(s, out v); return false; }
        public static Int16? GetInt16N(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetInt16(); }                                             // if (e != null) return e.Attributes[name].ToInt16(); return null; }

        public static Int16? GetInt16(this XmlAttribute a) /***********************************/ { return a.GetString().ToInt16(); }
        public static bool GetInt16(this XmlAttribute a, out Int16 value) /********************/ { return a.GetString().ToInt16(out value); }



        public static UInt16 GetUInt16(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetUInt16().GetValueOrDefault(); }                        // { return UInt16.Parse(e.Attributes[name].Value); }
        public static UInt16 GetUInt16(this XmlElement e, string name, UInt16 d) /************/ { return e.GetAttributeNode(name).GetUInt16().GetValueOrDefault(d); }                       // { UInt16 r; return e.GetAttribute(name, out r) ? r : d; }
        public static UInt16 GetAttribute(this XmlElement e, string name, UInt16 d) /*********/ { return e.GetAttributeNode(name).GetUInt16().GetValueOrDefault(d); }                       // UInt16 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetUInt16(this XmlElement e, string name, out UInt16 v) /**********/ { return e.GetAttributeNode(name).GetUInt16(out v); }                                       // string s; if (e.getstr<UInt16>(name, out s, out v)) return UInt16.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out UInt16 v) /*******/ { return e.GetAttributeNode(name).GetUInt16(out v); }                                       // string s; if (e.getstr<UInt16>(name, out s, out v)) return UInt16.TryParse(s, out v); return false; }
        public static UInt16? GetUInt16N(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetUInt16(); }                                            // if (e != null) return e.Attributes[name].ToUInt16(); return null; }

        public static UInt16? GetUInt16(this XmlAttribute a) /*********************************/ { return a.GetString().ToUInt16(); }
        public static bool GetUInt16(this XmlAttribute a, out UInt16 value) /******************/ { return a.GetString().ToUInt16(out value); }



        public static Int32 GetInt32(this XmlElement e, string name) /************************/ { return e.GetAttributeNode(name).GetInt32().GetValueOrDefault(); }                         // { return Int32.Parse(e.Attributes[name].Value); }
        public static Int32 GetInt32(this XmlElement e, string name, Int32 d) /***************/ { return e.GetAttributeNode(name).GetInt32().GetValueOrDefault(d); }                        // { Int32 r; return e.GetAttribute(name, out r) ? r : d; }
        public static Int32 GetAttribute(this XmlElement e, string name, Int32 d) /***********/ { return e.GetAttributeNode(name).GetInt32().GetValueOrDefault(d); }                        // Int32 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetInt32(this XmlElement e, string name, out Int32 v) /************/ { return e.GetAttributeNode(name).GetInt32(out v); }                                        // string s; if (e.getstr<Int32>(name, out s, out v)) return Int32.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Int32 v) /********/ { return e.GetAttributeNode(name).GetInt32(out v); }                                        // string s; if (e.getstr<Int32>(name, out s, out v)) return Int32.TryParse(s, out v); return false; }
        public static Int32? GetInt32N(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetInt32(); }                                             // if (e != null) return e.Attributes[name].ToInt32(); return null; }

        public static Int32? GetInt32(this XmlAttribute a) /***********************************/ { return a.GetString().ToInt32(); }
        public static bool GetInt32(this XmlAttribute a, out Int32 value) /********************/ { return a.GetString().ToInt32(out value); }



        public static UInt32 GetUInt32(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetUInt32().GetValueOrDefault(); }                        // { return UInt32.Parse(e.Attributes[name].Value); }
        public static UInt32 GetUInt32(this XmlElement e, string name, UInt32 d) /************/ { return e.GetAttributeNode(name).GetUInt32().GetValueOrDefault(d); }                       // { UInt32 r; return e.GetAttribute(name, out r) ? r : d; }
        public static UInt32 GetAttribute(this XmlElement e, string name, UInt32 d) /*********/ { return e.GetAttributeNode(name).GetUInt32().GetValueOrDefault(d); }                       // UInt32 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetUInt32(this XmlElement e, string name, out UInt32 v) /**********/ { return e.GetAttributeNode(name).GetUInt32(out v); }                                       // string s; if (e.getstr<UInt32>(name, out s, out v)) return UInt32.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out UInt32 v) /*******/ { return e.GetAttributeNode(name).GetUInt32(out v); }                                       // string s; if (e.getstr<UInt32>(name, out s, out v)) return UInt32.TryParse(s, out v); return false; }
        public static UInt32? GetUInt32N(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetUInt32(); }                                            // if (e != null) return e.Attributes[name].ToUInt32(); return null; }

        public static UInt32? GetUInt32(this XmlAttribute a) /*********************************/ { return a.GetString().ToUInt32(); }
        public static bool GetUInt32(this XmlAttribute a, out UInt32 value) /******************/ { return a.GetString().ToUInt32(out value); }



        public static Int64 GetInt64(this XmlElement e, string name) /************************/ { return e.GetAttributeNode(name).GetInt64().GetValueOrDefault(); }                         // { return Int64.Parse(e.Attributes[name].Value); }
        public static Int64 GetInt64(this XmlElement e, string name, Int64 d) /***************/ { return e.GetAttributeNode(name).GetInt64().GetValueOrDefault(d); }                        // { Int64 r; return e.GetAttribute(name, out r) ? r : d; }
        public static Int64 GetAttribute(this XmlElement e, string name, Int64 d) /***********/ { return e.GetAttributeNode(name).GetInt64().GetValueOrDefault(d); }                        // Int64 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetInt64(this XmlElement e, string name, out Int64 v) /************/ { return e.GetAttributeNode(name).GetInt64(out v); }                                        // string s; if (e.getstr<Int64>(name, out s, out v)) return Int64.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Int64 v) /********/ { return e.GetAttributeNode(name).GetInt64(out v); }                                        //string s; if (e.getstr<Int64>(name, out s, out v)) return Int64.TryParse(s, out v); return false; }
        public static Int64? GetInt64N(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetInt64(); }                                             //if (e != null) return e.Attributes[name].ToInt64(); return null; }

        public static Int64? GetInt64(this XmlAttribute a) /***********************************/ { return a.GetString().ToInt64(); }
        public static bool GetInt64(this XmlAttribute a, out Int64 value) /********************/ { return a.GetString().ToInt64(out value); }



        public static UInt64 GetUInt64(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetUInt64().GetValueOrDefault(); }                        // { return UInt64.Parse(e.Attributes[name].Value); }
        public static UInt64 GetUInt64(this XmlElement e, string name, UInt64 d) /************/ { return e.GetAttributeNode(name).GetUInt64().GetValueOrDefault(d); }                       // { UInt64 r; return e.GetAttribute(name, out r) ? r : d; }
        public static UInt64 GetAttribute(this XmlElement e, string name, UInt64 d) /*********/ { return e.GetAttributeNode(name).GetUInt64().GetValueOrDefault(d); }                       // UInt64 r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetUInt64(this XmlElement e, string name, out UInt64 v) /**********/ { return e.GetAttributeNode(name).GetUInt64(out v); }                                       //string s; if (e.getstr<UInt64>(name, out s, out v)) return UInt64.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out UInt64 v) /*******/ { return e.GetAttributeNode(name).GetUInt64(out v); }                                       // string s; if (e.getstr<UInt64>(name, out s, out v)) return UInt64.TryParse(s, out v); return false; }
        public static UInt64? GetUInt64N(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetUInt64(); }                                            // if (e != null) return e.Attributes[name].ToUInt64(); return null; }

        public static UInt64? GetUInt64(this XmlAttribute a) /*********************************/ { return a.GetString().ToUInt64(); }
        public static bool GetUInt64(this XmlAttribute a, out UInt64 value) /******************/ { return a.GetString().ToUInt64(out value); }



        public static Single GetSingle(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetSingle().GetValueOrDefault(); }                        // { return Single.Parse(e.Attributes[name].Value); }
        public static Single GetSingle(this XmlElement e, string name, Single d) /************/ { return e.GetAttributeNode(name).GetSingle().GetValueOrDefault(d); }                       // { Single r; return e.GetAttribute(name, out r) ? r : d; }
        public static Single GetAttribute(this XmlElement e, string name, Single d) /*********/ { return e.GetAttributeNode(name).GetSingle().GetValueOrDefault(d); }                       //Single r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetSingle(this XmlElement e, string name, out Single v) /**********/ { return e.GetAttributeNode(name).GetSingle(out v); }                                       //string s; if (e.getstr<Single>(name, out s, out v)) return Single.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Single v) /*******/ { return e.GetAttributeNode(name).GetSingle(out v); }                                       //string s; if (e.getstr<Single>(name, out s, out v)) return Single.TryParse(s, out v); return false; }
        public static Single? GetSingleN(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetSingle(); }                                            //if (e != null) return e.Attributes[name].ToSingle(); return null; }

        public static Single? GetSingle(this XmlAttribute a) /*********************************/ { return a.GetString().ToSingle(); }
        public static bool GetSingle(this XmlAttribute a, out Single value) /******************/ { return a.GetString().ToSingle(out value); }



        public static Double GetDouble(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetDouble().GetValueOrDefault(); }                        // { return Double.Parse(e.Attributes[name].Value); }
        public static Double GetDouble(this XmlElement e, string name, Double d) /************/ { return e.GetAttributeNode(name).GetDouble().GetValueOrDefault(d); }                       // { Double r; return e.GetAttribute(name, out r) ? r : d; }
        public static Double GetAttribute(this XmlElement e, string name, Double d) /*********/ { return e.GetAttributeNode(name).GetDouble().GetValueOrDefault(d); }                       //Double r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetDouble(this XmlElement e, string name, out Double v) /**********/ { return e.GetAttributeNode(name).GetDouble(out v); }                                       //string s; if (e.getstr<Double>(name, out s, out v)) return Double.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Double v) /*******/ { return e.GetAttributeNode(name).GetDouble(out v); }                                       //string s; if (e.getstr<Double>(name, out s, out v)) return Double.TryParse(s, out v); return false; }
        public static Double? GetDoubleN(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetDouble(); }                                            //if (e != null) return e.Attributes[name].ToDouble(); return null; }

        public static Double? GetDouble(this XmlAttribute a) /*********************************/ { return a.GetString().ToDouble(); }
        public static bool GetDouble(this XmlAttribute a, out Double value) /******************/ { return a.GetString().ToDouble(out value); }



        public static Decimal GetDecimal(this XmlElement e, string name) /********************/ { return e.GetAttributeNode(name).GetDecimal().GetValueOrDefault(); }                       // { return Decimal.Parse(e.Attributes[name].Value); }
        public static Decimal GetDecimal(this XmlElement e, string name, Decimal d) /*********/ { return e.GetAttributeNode(name).GetDecimal().GetValueOrDefault(d); }                      // { Decimal r; return e.GetAttribute(name, out r) ? r : d; }
        public static Decimal GetAttribute(this XmlElement e, string name, Decimal d) /*******/ { return e.GetAttributeNode(name).GetDecimal().GetValueOrDefault(d); }                      //Decimal r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetDecimal(this XmlElement e, string name, out Decimal v) /********/ { return e.GetAttributeNode(name).GetDecimal(out v); }                                      //string s; if (e.getstr<Decimal>(name, out s, out v)) return Decimal.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out Decimal v) /******/ { return e.GetAttributeNode(name).GetDecimal(out v); }                                      //string s; if (e.getstr<Decimal>(name, out s, out v)) return Decimal.TryParse(s, out v); return false; }
        public static Decimal? GetDecimalN(this XmlElement e, string name) /******************/ { return e.GetAttributeNode(name).GetDecimal(); }                                           //if (e != null) return e.Attributes[name].ToDecimal(); return null; }

        public static Decimal? GetDecimal(this XmlAttribute a) /*******************************/ { return a.GetString().ToDecimal(); }
        public static bool GetDecimal(this XmlAttribute a, out Decimal value) /****************/ { return a.GetString().ToDecimal(out value); }



        public static IPAddress GetIPAddress(this XmlElement e, string name) /****************/ { return e.GetAttributeNode(name).GetIPAddress(); }                                         // return IPAddress.Parse(e.Attributes[name].Value); }
        public static IPAddress GetIPAddress(this XmlElement e, string name, IPAddress d) /***/ { IPAddress ip; if (e.GetAttributeNode(name).GetIPAddress(out ip)) return ip; return d; }   // IPAddress r; return e.GetAttribute(name, out r) ? r : d; }
        public static IPAddress GetAttribute(this XmlElement e, string name, IPAddress d) /***/ { IPAddress ip; if (e.GetAttributeNode(name).GetIPAddress(out ip)) return ip; return d; }   // IPAddress r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetIPAddress(this XmlElement e, string name, out IPAddress v) /****/ { return e.GetAttributeNode(name).GetIPAddress(out v); }                                    // string s; if (e.getstr<IPAddress>(name, out s, out v)) return IPAddress.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out IPAddress v) /****/ { return e.GetAttributeNode(name).GetIPAddress(out v); }                                    // string s; if (e.getstr<IPAddress>(name, out s, out v)) return IPAddress.TryParse(s, out v); return false; }
        public static IPAddress GetIPAddressN(this XmlElement e, string name) /***************/ { return e.GetAttributeNode(name).GetIPAddress(); }                                         // if (e != null) return e.Attributes[name].ToIPAddress(); return null; }

        public static IPAddress GetIPAddress(this XmlAttribute a) /****************************/ { return a.GetString().ToIPAddress(); }
        public static bool GetIPAddress(this XmlAttribute a, out IPAddress value) /************/ { return a.GetString().ToIPAddress(out value); }



        public static DateTime GetDateTime(this XmlElement e, string name) /******************/ { return e.GetAttributeNode(name).GetDateTime().GetValueOrDefault(); }                      // { return DateTime.Parse(e.Attributes[name].Value); }
        public static DateTime GetDateTime(this XmlElement e, string name, DateTime d) /******/ { return e.GetAttributeNode(name).GetDateTime().GetValueOrDefault(d); }                     // { DateTime r; return e.GetAttribute(name, out r) ? r : d; }
        public static DateTime GetAttribute(this XmlElement e, string name, DateTime d) /*****/ { return e.GetAttributeNode(name).GetDateTime().GetValueOrDefault(d); }                     // DateTime r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetDateTime(this XmlElement e, string name, out DateTime v) /******/ { return e.GetAttributeNode(name).GetDateTime(out v); }                                     // string s; if (e.getstr<DateTime>(name, out s, out v)) return DateTime.TryParse(s, out v); return false; }
        public static bool GetAttribute(this XmlElement e, string name, out DateTime v) /*****/ { return e.GetAttributeNode(name).GetDateTime(out v); }                                     //string s; if (e.getstr<DateTime>(name, out s, out v)) return DateTime.TryParse(s, out v); return false; }
        public static DateTime? GetDateTimeN(this XmlElement e, string name) /****************/ { return e.GetAttributeNode(name).GetDateTime(); }                                          //if (e != null) return e.Attributes[name].ToDateTime(); return null; }

        public static DateTime? GetDateTime(this XmlAttribute a) /*****************************/ { return a.GetString().ToDateTime(); }
        public static bool GetDateTime(this XmlAttribute a, out DateTime value) /**************/ { return a.GetString().ToDateTime(out value); }



        public static Guid GetGuid(this XmlElement e, string name) /**************************/ { return e.GetAttributeNode(name).GetGuid().GetValueOrDefault(); }                          // { return new Guid(e.Attributes[name].Value); }
        public static Guid GetGuid(this XmlElement e, string name, Guid d) /******************/ { return e.GetAttributeNode(name).GetGuid().GetValueOrDefault(d); }                         // { Guid r; return e.GetAttribute(name, out r) ? r : d; }
        public static Guid GetAttribute(this XmlElement e, string name, Guid d) /*************/ { return e.GetAttributeNode(name).GetGuid().GetValueOrDefault(d); }                         //Guid r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetGuid(this XmlElement e, string name, out Guid v) /**************/ { return e.GetAttributeNode(name).GetGuid(out v); }                                         //{string s;if (e.getstr<Guid>(name, out s, out v)){try { v = new Guid(s); }catch { }}return false;}
        public static bool GetAttribute(this XmlElement e, string name, out Guid v) /*********/ { return e.GetAttributeNode(name).GetGuid(out v); }                                         // { string s; if (e.getstr<Guid>(name, out s, out v)) { try { v = new Guid(s); } catch { } } return false; }
        public static Guid? GetGuidN(this XmlElement e, string name) /************************/ { return e.GetAttributeNode(name).GetGuid(); }                                              //if (e != null) return e.Attributes[name].ToGuid(); return null; }

        public static Guid? GetGuid(this XmlAttribute a) /*************************************/ { return a.GetString().ToGuid(); }
        public static bool GetGuid(this XmlAttribute a, out Guid guid) /***********************/ { return a.GetString().ToGuid(out guid); }




        //public static T GetAttribute<T>(this XmlElement e, string name)
        //{
        //    return (T)Convert.ChangeType(e.Attributes[name].Value, typeof(T));
        //}
        //public static T GetAttribute<T>(this XmlElement e, string name, T d)
        //{
        //    if (e.HasAttribute(name))
        //        return e.GetAttribute<T>(name);
        //    else
        //        return d;
        //}
        //public static bool GetAttribute<T>(this XmlElement e, string name, out T d)
        //{
        //    if (e.HasAttribute(name))
        //    {
        //        d = e.GetAttribute<T>(name);
        //        return true;
        //    }
        //    else
        //    {
        //        d = default(T);
        //        return false;
        //    }
        //}

        public static String GetString(this XmlElement e, string name) /***********************/ { return e.GetAttributeNode(name).GetString(); }                                           //return e.Attributes[name].Value; }
        public static String GetString(this XmlElement e, string name, String d) /*************/ { String r; return e.GetAttribute(name, out r) ? r : d; }
        public static bool GetString(this XmlElement e, string name, out String v) /***********/ { return e.GetAttributeNode(name).GetString(out v); }                                      //{ XmlAttribute a = e.Attributes[name]; if (a != null) { v = a.Value; return true; } else { v = String.Empty; return false; } }
        public static bool GetAttribute(this XmlElement e, string name, out String v) /********/ { return e.GetAttributeNode(name).GetString(out v); }                                      //{ XmlAttribute a = e.Attributes[name]; if (a != null) { v = a.Value; return true; } else { v = String.Empty; return false; } }
        public static String GetStringN(this XmlElement e, string name) /**********************/ { return e.GetAttributeNode(name).GetString(); }                                           // { XmlAttribute a = e.Attributes[name]; if (a == null) return null; else return a.Value; }

        public static String GetString(this XmlAttribute a) /**********************************/ { if (a == null) return null; else return a.Value; }
        public static bool GetString(this XmlAttribute a, out String value) /******************/ { if (a == null) { value = string.Empty; return false; } else { value = a.Value; return true; } }

        //public static bool GetValue(this XmlAttribute a, Type t, out object value) { return a.GetString().ToValue(t, out value); }

        public static bool GetValue(this XmlElement e, Type t, string name, out object result)
        {
            return e.GetAttributeNode(name).GetValue(t, out result);
        }

        //public static bool GetValue<T>(this XmlAttribute a, out T value, TryParseHandler<T> tryParse)
        //{
        //    if (a.TryGetValue<T>(out value, tryParse))
        //        return true;
        //    return false;
        //}

        //public static bool TryGetValue<T>(this XmlAttribute a, out T value, TryParseHandler<T> tryParse)
        //{
        //    try
        //    {
        //        if (a != null)
        //            if (tryParse != null)
        //                if (!string.IsNullOrEmpty(a.Value))
        //                    return tryParse(a.Value, out value);
        //    }
        //    catch
        //    {
        //    }
        //    value = default(T);
        //    return false;
        //}

        static bool IsValueType<T>(this Type t) where T : struct
        {
            return (t == typeof(T)) || (t == typeof(Nullable<T>));
        }

        public static void SetAttribute(this XmlElement e, string name, object value)
        {
            if (value == null)
            {
                XmlAttribute attr = e.GetAttributeNode(name);
                if (attr != null)
                    e.Attributes.Remove(attr);
            }
            else
                e.SetAttribute(name, value.ToString());
        }
        public static void SetAttribute(this XmlElement e, string name, string format, params object[] args)
        {
            e.SetAttribute(name, string.Format(format, args));
        }

        public static void Rename(this XmlAttribute attr, string name)
        {
            if (attr == null) return;
            if (attr.Name == name) return;
            XmlElement e = attr.OwnerElement;
            XmlAttribute attr_new = e.OwnerDocument.CreateAttribute(name);
            attr_new.Value = attr.Value;
            e.Attributes.Remove(attr);
            e.Attributes.Append(attr_new);
        }

        public static void MoveTo(this XmlAttribute attr, XmlElement e)
        {
            if (attr == null) return;
            if (e == null) return;
            if (attr.OwnerDocument == e.OwnerDocument)
            {
                attr.OwnerElement.Attributes.Remove(attr);
                e.Attributes.Append(attr);
            }
            else
            {
                XmlAttribute attr2 = e.OwnerDocument.CreateAttribute(attr.Name);
                attr2.Value = attr.Value;
                e.Attributes.Append(attr2);
                attr.OwnerElement.Attributes.Remove(attr);
            }
        }

        public static void CopyTo(this XmlAttribute attr, string name)
        {
            if (attr == null) return;
            if (string.IsNullOrEmpty(name)) return;
            attr.OwnerElement.SetAttribute(name, attr.Value);
        }

        public static XmlAttribute GetAttributeNode(this XmlElement e, MethodBase m)
        {
            PropertyInfo p = m.ToProperty();
            if (p != null)
                return e.Attributes[p.Name];
            return null;
        }


        public static string GetAttribute(this XmlElement e, MethodBase m)
        {
            PropertyInfo p = m.ToProperty();
            if (p != null)
            {
                XmlAttribute attr = e.Attributes[p.Name];
                if (attr != null)
                    return attr.Value;
            }
            return string.Empty;
        }
        public static void SetAttribute(this XmlElement e, MethodBase m, object value)
        {
            e.SetAttribute(m, Convert.ToString(value));
        }
        public static void SetAttribute(this XmlElement e, MethodBase m, string format, params object[] args)
        {
            e.SetAttribute(m, string.Format(format, args));
        }
        public static void SetAttribute(this XmlElement e, MethodBase m, string value)
        {
            PropertyInfo p = m.ToProperty();
            if (p != null)
                e.SetAttribute(p.Name, value);
        }

        public static bool IsNullOrEmpty(this XmlElement e, string name)
        {
            XmlAttribute attr = e.Attributes[name];
            if (attr == null)
                return true;
            return string.IsNullOrEmpty(attr.Value);
        }



        public static T AppendChild<T>(this XmlNode node) where T : XmlElement
        {
            return node.AppendChild<T>(typeof(T).Name);
        }
        public static T AppendChild<T>(this XmlNode node, string name) where T : XmlNode
        {
            return (T)node.AppendChild(node.OwnerDocument.CreateElement(name));
        }
        public static XmlElement AppendChild(this XmlElement e, string name)
        {
            return e.AppendChild<XmlElement>(name);
        }
        public static void SetAll(this XmlAttributeCollection src, string value)
        {
            foreach (XmlAttribute attr in src)
                attr.Value = value;
        }

        // isInclude : True - Include, False - Exclude
        public static void Import(this XmlElement e, XmlElement src, params string[] names)
        {
            e.Import(src, true, names);
        }
        public static void Import(this XmlElement e, XmlElement src, bool isInclude, params string[] names)
        {
            foreach (XmlAttribute a in src.Attributes)
            {
                string name = a.Name;
                bool import = !string.IsNullOrEmpty(a.Value);
                if (!import)
                {
                    if (names.Length > 0)
                        import = !(isInclude ^ names.Conatins(name));
                    //          conatins    import
                    // exclude  False       True
                    // exclude  True        False
                    // include  False       False
                    // include  True        True

                }
                if (import)
                    e.SetAttribute(name, a.Value);
            }
        }

        public static XmlNode SelectSingleNode(this XmlNode e, string format, params object[] args)
        {
            return e.SelectSingleNode(string.Format(format, args));
        }
        public static XmlNodeList SelectNodes(this XmlNode e, string format, params object[] args)
        {
            return e.SelectNodes(string.Format(format, args));
        }

        public static bool ContainsAttribute(this XmlElement e, params string[] names)
        {
            for (int i = 0; i < names.Length; i++)
                if (!e.HasAttribute(names[i]))
                    return false;
            return true;
        }

        // 檢查欄位是否存在, 如果不存在則 throw exception
        public static void ContainsAttributeEx(this XmlElement e, params string[] names)
        {
            StringBuilder sb = null;
            foreach (string s in names)
            {
                if (!e.HasAttribute(s))
                {
                    if (sb == null)
                        sb = new StringBuilder();
                    else
                        sb.Append(", ");
                    sb.Append(s);
                }
            }
            if (sb != null)
                throw new ArgumentNullException(sb.ToString());
        }

        public static bool GetAttributeNode(this XmlElement e, string name, out XmlAttribute attr)
        {
            attr = e.GetAttributeNode(name);
            return attr != null;
        }

        public static void WriteAttributeString(this XmlWriter w, string localName, object value)
        {
            w.WriteAttributeString(localName, Convert.ToString(value));
        }

        public static void WriteAttributeString(this XmlWriter w, string localName, string format, params object[] args)
        {
            w.WriteAttributeString(localName, string.Format(format, args));
        }

        class XmlWriterSegment : IDisposable
        {
            static Queue<XmlWriterSegment> p = new Queue<XmlWriterSegment>();
            public static XmlWriterSegment Alloc(System.Threading.ThreadStart end)
            {
                XmlWriterSegment t;
                lock (p)
                    if (p.Count > 0)
                        t = p.Dequeue();
                    else
                        t = new XmlWriterSegment();
                t.end = end;
                return t;
            }

            System.Threading.ThreadStart end;

            void IDisposable.Dispose()
            {
                end();
                end = null;
                lock (p) p.Enqueue(this);
            }
        }

        public static IDisposable WriteElement(this XmlWriter w, string localName)
        {
            w.WriteStartElement(localName);
            return XmlWriterSegment.Alloc(w.WriteEndElement);
        }
        public static IDisposable WriteElement(this XmlWriter w, string localName, string ns)
        {
            w.WriteStartElement(localName, ns);
            return XmlWriterSegment.Alloc(w.WriteEndElement);
        }
        public static IDisposable WriteElement(this XmlWriter w, string prefix, string localName, string ns)
        {
            w.WriteStartElement(prefix, localName, ns);
            return XmlWriterSegment.Alloc(w.WriteEndElement);
        }

        public static IDisposable WriteAttribute(this XmlWriter w, string localName)
        {
            w.WriteStartAttribute(localName);
            return XmlWriterSegment.Alloc(w.WriteEndAttribute);
        }
        public static IDisposable WriteAttribute(this XmlWriter w, string localName, string ns)
        {
            w.WriteStartAttribute(localName, ns);
            return XmlWriterSegment.Alloc(w.WriteEndAttribute);
        }
        public static IDisposable WriteAttribute(this XmlWriter w, string prefix, string localName, string ns)
        {
            w.WriteStartAttribute(prefix, localName, ns);
            return XmlWriterSegment.Alloc(w.WriteEndAttribute);
        }

        public static Nullable<T> GetValue<T>(this XmlTextReader r, string name, TryParseHandler<T> tryParse) where T : struct
        {
            if (r.MoveToAttribute(name))
                if (r.HasValue)
                {
                    T n;
                    if (tryParse(r.Value, out n))
                        return n;
                }
            return null;
        }

    }

}
