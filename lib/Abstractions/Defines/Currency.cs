//using Newtonsoft.Json;
using System;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Globalization;
//using System.Text;
using _DebuggerStepThrough = System.Diagnostics.FakeDebuggerStepThroughAttribute;

namespace InnateGlory
{
    /// <summary>
    /// ISO 4217
    /// <see cref="https://en.wikipedia.org/wiki/ISO_4217"/>
    /// </summary>
    public enum CurrencyCode : Int16
    {
        Default = 000,
        //IG_Point = 001,
        //IG_ECoin = 002,
        AED = 784, //	2	United Arab Emirates dirham	 United Arab Emirates
        AFN = 971, //	2	Afghan afghani	 Afghanistan
        ALL = 008, //	2	Albanian lek	 Albania
        AMD = 051, //	2	Armenian dram	 Armenia
        ANG = 532, //	2	Netherlands Antillean guilder	 Curaçao,  Sint Maarten
        AOA = 973, //	2	Angolan kwanza	 Angola
        ARS = 032, //	2	Argentine peso	 Argentina
        AUD = 036, //	2	Australian dollar	 Australia, Australian Antarctic Territory,  Christmas Island,  Cocos (Keeling) Islands,  Heard and McDonald Islands,  Kiribati,  Nauru,  Norfolk Island,  Tuvalu
        AWG = 533, //	2	Aruban florin	 Aruba
        AZN = 944, //	2	Azerbaijani manat	 Azerbaijan
        BAM = 977, //	2	Bosnia and Herzegovina convertible mark	 Bosnia and Herzegovina
        BBD = 052, //	2	Barbados dollar	 Barbados
        BDT = 050, //	2	Bangladeshi taka	 Bangladesh
        BGN = 975, //	2	Bulgarian lev	 Bulgaria
        BHD = 048, //	3	Bahraini dinar	 Bahrain
        BIF = 108, //	0	Burundian franc	 Burundi
        BMD = 060, //	2	Bermudian dollar (customarily known as Bermuda dollar)	 Bermuda
        BND = 096, //	2	Brunei dollar	 Brunei,  Singapore
        BOB = 068, //	2	Boliviano	 Bolivia
        BOV = 984, //	2	Bolivian Mvdol (funds code)	 Bolivia
        BRL = 986, //	2	Brazilian real	 Brazil
        BSD = 044, //	2	Bahamian dollar	 Bahamas
        BTN = 064, //	2	Bhutanese ngultrum	 Bhutan
        BWP = 072, //	2	Botswana pula	 Botswana
        BYR = 974, //	0	Belarusian ruble	 Belarus
        BZD = 084, //	2	Belize dollar	 Belize
        CAD = 124, //	2	Canadian dollar	 Canada,  Saint Pierre and Miquelon
        CDF = 976, //	2	Congolese franc	 Democratic Republic of Congo
        CHE = 947, //	2	WIR Euro (complementary currency)	 Switzerland
        CHF = 756, //	2	Swiss franc	 Switzerland,  Liechtenstein
        CHW = 948, //	2	WIR Franc (complementary currency)	 Switzerland
        CLF = 990, //	0	Unidad de Fomento (funds code)	 Chile
        CLP = 152, //	0	Chilean peso	 Chile
        CNY = 156, //	2	Chinese yuan	 China
        COP = 170, //	2	Colombian peso	 Colombia
        COU = 970, //	2	Unidad de Valor Real	 Colombia
        CRC = 188, //	2	Costa Rican colon	 Costa Rica
        CUC = 931, //	2	Cuban convertible peso	 Cuba
        CUP = 192, //	2	Cuban peso	 Cuba
        CVE = 132, //	0	Cape Verde escudo	 Cape Verde
        CZK = 203, //	2	Czech koruna	 Czech Republic
        DJF = 262, //	0	Djiboutian franc	 Djibouti
        DKK = 208, //	2	Danish krone	 Denmark,  Faroe Islands,  Greenland
        DOP = 214, //	2	Dominican peso	 Dominican Republic
        DZD = 012, //	2	Algerian dinar	 Algeria
        EGP = 818, //	2	Egyptian pound	 Egypt
        ERN = 232, //	2	Eritrean nakfa	 Eritrea
        ETB = 230, //	2	Ethiopian birr	 Ethiopia
        EUR = 978, //	2	Euro	 Andorra,  Austria,  Belgium,  Cyprus,  Estonia,  Finland,  France,  Germany,  Greece,  Ireland,  Italy,  Kosovo,  Luxembourg,  Malta,  Monaco,  Montenegro,  Netherlands,  Portugal,  San Marino,  Slovakia,  Slovenia,  Spain,  Vatican City; see eurozone
        FJD = 242, //	2	Fiji dollar	 Fiji
        FKP = 238, //	2	Falkland Islands pound	 Falkland Islands
        GBP = 826, //	2	Pound sterling	 United Kingdom, British Crown dependencies (the  Isle of Man and the Channel Islands), certain British Overseas Territories ( South Georgia and the South Sandwich Islands,  British Antarctic Territory and  British Indian Ocean Territory)
        GEL = 981, //	2	Georgian lari	 Georgia (country)
        GHS = 936, //	2	Ghanaian cedi	 Ghana
        GIP = 292, //	2	Gibraltar pound	 Gibraltar
        GMD = 270, //	2	Gambian dalasi	 Gambia
        GNF = 324, //	0	Guinean franc	 Guinea
        GTQ = 320, //	2	Guatemalan quetzal	 Guatemala
        GYD = 328, //	2	Guyanese dollar	 Guyana
        HKD = 344, //	2	Hong Kong dollar	 Hong Kong,  Macao
        HNL = 340, //	2	Honduran lempira	 Honduras
        HRK = 191, //	2	Croatian kuna	 Croatia
        HTG = 332, //	2	Haitian gourde	 Haiti
        HUF = 348, //	2	Hungarian forint	 Hungary
        IDR = 360, //	2	Indonesian rupiah	 Indonesia
        ILS = 376, //	2	Israeli new shekel	 Israel,  Palestinian territories[7]
        INR = 356, //	2	Indian rupee	 India
        IQD = 368, //	3	Iraqi dinar	 Iraq
        IRR = 364, //	0	Iranian rial	 Iran
        ISK = 352, //	0	Icelandic króna	 Iceland
        JMD = 388, //	2	Jamaican dollar	 Jamaica
        JOD = 400, //	3	Jordanian dinar	 Jordan
        JPY = 392, //	0	Japanese yen	 Japan
        KES = 404, //	2	Kenyan shilling	 Kenya
        KGS = 417, //	2	Kyrgyzstani som	 Kyrgyzstan
        KHR = 116, //	2	Cambodian riel	 Cambodia
        KMF = 174, //	0	Comoro franc	 Comoros
        KPW = 408, //	0	North Korean won	 North Korea
        KRW = 410, //	0	South Korean won	 South Korea
        KWD = 414, //	3	Kuwaiti dinar	 Kuwait
        KYD = 136, //	2	Cayman Islands dollar	 Cayman Islands
        KZT = 398, //	2	Kazakhstani tenge	 Kazakhstan
        LAK = 418, //	0	Lao kip	 Laos
        LBP = 422, //	0	Lebanese pound	 Lebanon
        LKR = 144, //	2	Sri Lankan rupee	 Sri Lanka
        LRD = 430, //	2	Liberian dollar	 Liberia
        LSL = 426, //	2	Lesotho loti	 Lesotho
        LTL = 440, //	2	Lithuanian litas	 Lithuania
        LVL = 428, //	2	Latvian lats	 Latvia
        LYD = 434, //	3	Libyan dinar	 Libya
        MAD = 504, //	2	Moroccan dirham	 Morocco
        MDL = 498, //	2	Moldovan leu	 Moldova (except  Transnistria)
        MGA = 969, //	0*[8]	Malagasy ariary	 Madagascar
        MKD = 807, //	0	Macedonian denar	 Macedonia
        MMK = 104, //	0	Myanma kyat	 Myanmar
        MNT = 496, //	2	Mongolian tugrik	 Mongolia
        MOP = 446, //	2	Macanese pataca	 Macao
        MRO = 478, //	0*[8]	Mauritanian ouguiya	 Mauritania
        MUR = 480, //	2	Mauritian rupee	 Mauritius
        MVR = 462, //	2	Maldivian rufiyaa	 Maldives
        MWK = 454, //	2	Malawian kwacha	 Malawi
        MXN = 484, //	2	Mexican peso	 Mexico
        MXV = 979, //	2	Mexican Unidad de Inversion (UDI) (funds code)	 Mexico
        MYR = 458, //	2	Malaysian ringgit	 Malaysia
        MZN = 943, //	2	Mozambican metical	 Mozambique
        NAD = 516, //	2	Namibian dollar	 Namibia
        NGN = 566, //	2	Nigerian naira	 Nigeria
        NIO = 558, //	2	Nicaraguan córdoba	 Nicaragua
        NOK = 578, //	2	Norwegian krone	 Norway,  Svalbard,  Jan Mayen,  Bouvet Island, Queen Maud Land, Peter I Island
        NPR = 524, //	2	Nepalese rupee	   Nepal
        NZD = 554, //	2	New Zealand dollar	 Cook Islands,  New Zealand,  Niue,  Pitcairn,  Tokelau, Ross Dependency
        OMR = 512, //	3	Omani rial	 Oman
        PAB = 590, //	2	Panamanian balboa	 Panama
        PEN = 604, //	2	Peruvian nuevo sol	 Peru
        PGK = 598, //	2	Papua New Guinean kina	 Papua New Guinea
        PHP = 608, //	2	Philippine peso	 Philippines
        PKR = 586, //	2	Pakistani rupee	 Pakistan
        PLN = 985, //	2	Polish złoty	 Poland
        PYG = 600, //	0	Paraguayan guaraní	 Paraguay
        QAR = 634, //	2	Qatari riyal	 Qatar
        RON = 946, //	2	Romanian new leu	 Romania
        RSD = 941, //	2	Serbian dinar	 Serbia
        RUB = 643, //	2	Russian rouble	 Russia,  Abkhazia,  South Ossetia
        RWF = 646, //	0	Rwandan franc	 Rwanda
        SAR = 682, //	2	Saudi riyal	 Saudi Arabia
        SBD = 090, //	2	Solomon Islands dollar	 Solomon Islands
        SCR = 690, //	2	Seychelles rupee	 Seychelles
        SDG = 938, //	2	Sudanese pound	 Sudan
        SEK = 752, //	2	Swedish krona/kronor	 Sweden
        SGD = 702, //	2	Singapore dollar	 Singapore,  Brunei
        SHP = 654, //	2	Saint Helena pound	 Saint Helena
        SLL = 694, //	0	Sierra Leonean leone	 Sierra Leone
        SOS = 706, //	2	Somali shilling	 Somalia (except  Somaliland)
        SRD = 968, //	2	Surinamese dollar	 Suriname
        SSP = 728, //	2	South Sudanese pound	 South Sudan
        STD = 678, //	0	São Tomé and Príncipe dobra	 São Tomé and Príncipe
        SYP = 760, //	2	Syrian pound	 Syria
        SZL = 748, //	2	Swazi lilangeni	 Swaziland
        THB = 764, //	2	Thai baht	 Thailand
        TJS = 972, //	2	Tajikistani somoni	 Tajikistan
        TMT = 934, //	2	Turkmenistani manat	 Turkmenistan
        TND = 788, //	3	Tunisian dinar	 Tunisia
        TOP = 776, //	2	Tongan paʻanga	 Tonga
        TRY = 949, //	2	Turkish lira	 Turkey,  Northern Cyprus
        TTD = 780, //	2	Trinidad and Tobago dollar	 Trinidad and Tobago
        TWD = 901, //	2	New Taiwan dollar	 Republic of China (Taiwan)
        TZS = 834, //	2	Tanzanian shilling	 Tanzania
        UAH = 980, //	2	Ukrainian hryvnia	 Ukraine
        UGX = 800, //	2	Ugandan shilling	 Uganda
        USD = 840, //	2	United States dollar	 American Samoa,  Barbados (as well as Barbados Dollar),  Bermuda (as well as Bermudian Dollar),  British Indian Ocean Territory,  British Virgin Islands, Caribbean Netherlands,  Ecuador,  El Salvador,  Guam,  Haiti,  Marshall Islands,  Federated States of Micronesia,  Northern Mariana Islands,  Palau,  Panama,  Puerto Rico,  Timor-Leste,  Turks and Caicos Islands,  United States,  U.S. Virgin Islands,  Zimbabwe
        USN = 997, //	2	United States dollar (next day) (funds code)	 United States
        USS = 998, //	2	United States dollar (same day) (funds code) (one source[who?] claims it is no longer used, but it is still on the ISO 4217-MA list)	 United States
        UYI = 940, //	0	Uruguay Peso en Unidades Indexadas (URUIURUI) (funds code)	 Uruguay
        UYU = 858, //	2	Uruguayan peso	 Uruguay
        UZS = 860, //	2	Uzbekistan som	 Uzbekistan
        VEF = 937, //	2	Venezuelan bolívar fuerte	 Venezuela
        VND = 704, //	0	Vietnamese dong	 Vietnam
        VUV = 548, //	0	Vanuatu vatu	 Vanuatu
        WST = 882, //	2	Samoan tala	 Samoa
        XAF = 950, //	0	CFA franc BEAC	 Cameroon,  Central African Republic,  Republic of the Congo,  Chad,  Equatorial Guinea,  Gabon
        XAG = 961, //	.	Silver (one troy ounce)	
        XAU = 959, //	.	Gold (one troy ounce)	
        XBA = 955, //	.	European Composite Unit (EURCO) (bond market unit)	
        XBB = 956, //	.	European Monetary Unit (E.M.U.-6) (bond market unit)	
        XBC = 957, //	.	European Unit of Account 9 (E.U.A.-9) (bond market unit)	
        XBD = 958, //	.	European Unit of Account 17 (E.U.A.-17) (bond market unit)	
        XCD = 951, //	2	East Caribbean dollar	 Anguilla,  Antigua and Barbuda,  Dominica,  Grenada,  Montserrat,  Saint Kitts and Nevis,  Saint Lucia,  Saint Vincent and the Grenadines
        XDR = 960, //	.	Special drawing rights	International Monetary Fund
                   //XFU = Nil, //	.	UIC franc (special settlement currency)	International Union of Railways
        XOF = 952, //	0	CFA franc BCEAO	 Benin,  Burkina Faso,  Côte d'Ivoire,  Guinea-Bissau,  Mali,  Niger,  Senegal,  Togo
        XPD = 964, //	.	Palladium (one troy ounce)	
        XPF = 953, //	0	CFP franc (Franc du Pacifique)	French territories of the Pacific Ocean:  French Polynesia,  New Caledonia,  Wallis and Futuna
        XPT = 962, //	.	Platinum (one troy ounce)	
        XTS = 963, //	.	Code reserved for testing purposes	
        XXX = 999, //	.	No currency	
        YER = 886, //	2	Yemeni rial	 Yemen
        ZAR = 710, //	2	South African rand	 South Africa
        ZMW = 967, //	2	Zambian kwacha	 Zambia
    }

    //[_DebuggerStepThrough]
    //[TypeConverter(typeof(CurrencyCode._TypeConverter))]
    //[JsonConverter(typeof(CurrencyCode._JsonConverter))]
    //public struct CurrencyCode
    //{
    //    public readonly object OrginalValue;
    //    public readonly Values Code;
    //    public bool HasValue
    //    {
    //        get { return this.Code != 0; }
    //    }
    //    public CurrencyCode(Values value)
    //    {
    //        this.OrginalValue = value;
    //        this.Code = value;
    //    }
    //    public CurrencyCode(object value)
    //    {
    //        this.OrginalValue = value;
    //        if (value is Int16)
    //            this.Code = (Values)(Int16)value;
    //        else if ((value is Int16?) && ((Int16?)value).HasValue)
    //            this.Code = (Values)((Int16?)value).Value;
    //        else if (value is string)
    //            this.Code = ((string)value).ToEnum<Values>() ?? 0;
    //        else
    //            this.Code = 0;
    //    }

    //    public static implicit operator CurrencyCode? (Int16? value)
    //    {
    //        if (value.HasValue) return (CurrencyCode)value.Value; return null;
    //    }
    //    public static implicit operator Int16? (CurrencyCode? value)
    //    {
    //        if (value.HasValue) return (Int16)value.Value.Code; return null;
    //    }
    //    public static implicit operator CurrencyCode(Int16 value)
    //    {
    //        return new CurrencyCode((Values)value);
    //    }
    //    public static implicit operator Int16(CurrencyCode value)
    //    {
    //        return (Int16)value.Code;
    //    }

    //    public static bool operator ==(CurrencyCode? src, object obj)
    //    {
    //        if (src.HasValue)
    //            return src.Value.Equals(obj);
    //        return object.ReferenceEquals(obj, null);
    //    }
    //    public static bool operator !=(CurrencyCode? src, object obj)
    //    {
    //        return !(src == obj);
    //    }
    //    public static bool operator ==(CurrencyCode src, object obj)
    //    {
    //        return src.Equals(obj);
    //    }
    //    public static bool operator !=(CurrencyCode src, object obj)
    //    {
    //        return !(src == obj);
    //    }

    //    public override bool Equals(object obj)
    //    {
    //        if (obj is CurrencyCode)
    //            return this.Code == ((CurrencyCode)obj).Code;
    //        else if (obj is CurrencyCode.Values)
    //            return this.Code == (CurrencyCode.Values)obj;
    //        else if (obj is Int16)
    //            return this.Code == (CurrencyCode.Values)(Int16)obj;
    //        else
    //            return false;
    //    }

    //    public override int GetHashCode()
    //    {
    //        return base.GetHashCode();
    //    }

    //    public override string ToString()
    //    {
    //        return this.Code.ToString();
    //    }

    //    [_DebuggerStepThrough]
    //    class _TypeConverter : TypeConverter
    //    {
    //        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    //        {
    //            if (sourceType == typeof(Int16))
    //                return true;
    //            if (sourceType == typeof(Int16?))
    //                return true;
    //            if (sourceType == typeof(string))
    //                return true;
    //            return base.CanConvertFrom(context, sourceType);
    //        }
    //        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    //        {
    //            return new CurrencyCode(value);
    //        }

    //        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
    //        {
    //            return destinationType == typeof(string);
    //        }
    //        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
    //        {
    //            if ((value is CurrencyCode) && (destinationType == typeof(string)))
    //                return ((CurrencyCode)value).ToString();
    //            return base.ConvertTo(context, culture, value, destinationType);
    //        }
    //    }

    //    [_DebuggerStepThrough]
    //    class _JsonConverter : JsonConverter
    //    {
    //        public override bool CanConvert(Type objectType)
    //        {
    //            return true;
    //        }

    //        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    //        {
    //            return (reader.Value as string).ToEnum<CurrencyCode>();
    //        }

    //        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    //        {
    //            if (value is CurrencyCode)
    //                serializer.Serialize(writer, ((CurrencyCode)value).ToString());
    //            else if (value is CurrencyCode?)
    //                serializer.Serialize(writer, ((CurrencyCode?)value).Value.ToString());
    //            else
    //                serializer.Serialize(writer, value);
    //        }
    //    }

    //    /// <summary>
    //    /// ISO 4217
    //    /// <see cref="https://en.wikipedia.org/wiki/ISO_4217"/>
    //    /// </summary>
    //    public enum Values : Int16
    //    {
    //        AED = 784, //	2	United Arab Emirates dirham	 United Arab Emirates
    //        AFN = 971, //	2	Afghan afghani	 Afghanistan
    //        ALL = 008, //	2	Albanian lek	 Albania
    //        AMD = 051, //	2	Armenian dram	 Armenia
    //        ANG = 532, //	2	Netherlands Antillean guilder	 Curaçao,  Sint Maarten
    //        AOA = 973, //	2	Angolan kwanza	 Angola
    //        ARS = 032, //	2	Argentine peso	 Argentina
    //        AUD = 036, //	2	Australian dollar	 Australia, Australian Antarctic Territory,  Christmas Island,  Cocos (Keeling) Islands,  Heard and McDonald Islands,  Kiribati,  Nauru,  Norfolk Island,  Tuvalu
    //        AWG = 533, //	2	Aruban florin	 Aruba
    //        AZN = 944, //	2	Azerbaijani manat	 Azerbaijan
    //        BAM = 977, //	2	Bosnia and Herzegovina convertible mark	 Bosnia and Herzegovina
    //        BBD = 052, //	2	Barbados dollar	 Barbados
    //        BDT = 050, //	2	Bangladeshi taka	 Bangladesh
    //        BGN = 975, //	2	Bulgarian lev	 Bulgaria
    //        BHD = 048, //	3	Bahraini dinar	 Bahrain
    //        BIF = 108, //	0	Burundian franc	 Burundi
    //        BMD = 060, //	2	Bermudian dollar (customarily known as Bermuda dollar)	 Bermuda
    //        BND = 096, //	2	Brunei dollar	 Brunei,  Singapore
    //        BOB = 068, //	2	Boliviano	 Bolivia
    //        BOV = 984, //	2	Bolivian Mvdol (funds code)	 Bolivia
    //        BRL = 986, //	2	Brazilian real	 Brazil
    //        BSD = 044, //	2	Bahamian dollar	 Bahamas
    //        BTN = 064, //	2	Bhutanese ngultrum	 Bhutan
    //        BWP = 072, //	2	Botswana pula	 Botswana
    //        BYR = 974, //	0	Belarusian ruble	 Belarus
    //        BZD = 084, //	2	Belize dollar	 Belize
    //        CAD = 124, //	2	Canadian dollar	 Canada,  Saint Pierre and Miquelon
    //        CDF = 976, //	2	Congolese franc	 Democratic Republic of Congo
    //        CHE = 947, //	2	WIR Euro (complementary currency)	 Switzerland
    //        CHF = 756, //	2	Swiss franc	 Switzerland,  Liechtenstein
    //        CHW = 948, //	2	WIR Franc (complementary currency)	 Switzerland
    //        CLF = 990, //	0	Unidad de Fomento (funds code)	 Chile
    //        CLP = 152, //	0	Chilean peso	 Chile
    //        CNY = 156, //	2	Chinese yuan	 China
    //        COP = 170, //	2	Colombian peso	 Colombia
    //        COU = 970, //	2	Unidad de Valor Real	 Colombia
    //        CRC = 188, //	2	Costa Rican colon	 Costa Rica
    //        CUC = 931, //	2	Cuban convertible peso	 Cuba
    //        CUP = 192, //	2	Cuban peso	 Cuba
    //        CVE = 132, //	0	Cape Verde escudo	 Cape Verde
    //        CZK = 203, //	2	Czech koruna	 Czech Republic
    //        DJF = 262, //	0	Djiboutian franc	 Djibouti
    //        DKK = 208, //	2	Danish krone	 Denmark,  Faroe Islands,  Greenland
    //        DOP = 214, //	2	Dominican peso	 Dominican Republic
    //        DZD = 012, //	2	Algerian dinar	 Algeria
    //        EGP = 818, //	2	Egyptian pound	 Egypt
    //        ERN = 232, //	2	Eritrean nakfa	 Eritrea
    //        ETB = 230, //	2	Ethiopian birr	 Ethiopia
    //        EUR = 978, //	2	Euro	 Andorra,  Austria,  Belgium,  Cyprus,  Estonia,  Finland,  France,  Germany,  Greece,  Ireland,  Italy,  Kosovo,  Luxembourg,  Malta,  Monaco,  Montenegro,  Netherlands,  Portugal,  San Marino,  Slovakia,  Slovenia,  Spain,  Vatican City; see eurozone
    //        FJD = 242, //	2	Fiji dollar	 Fiji
    //        FKP = 238, //	2	Falkland Islands pound	 Falkland Islands
    //        GBP = 826, //	2	Pound sterling	 United Kingdom, British Crown dependencies (the  Isle of Man and the Channel Islands), certain British Overseas Territories ( South Georgia and the South Sandwich Islands,  British Antarctic Territory and  British Indian Ocean Territory)
    //        GEL = 981, //	2	Georgian lari	 Georgia (country)
    //        GHS = 936, //	2	Ghanaian cedi	 Ghana
    //        GIP = 292, //	2	Gibraltar pound	 Gibraltar
    //        GMD = 270, //	2	Gambian dalasi	 Gambia
    //        GNF = 324, //	0	Guinean franc	 Guinea
    //        GTQ = 320, //	2	Guatemalan quetzal	 Guatemala
    //        GYD = 328, //	2	Guyanese dollar	 Guyana
    //        HKD = 344, //	2	Hong Kong dollar	 Hong Kong,  Macao
    //        HNL = 340, //	2	Honduran lempira	 Honduras
    //        HRK = 191, //	2	Croatian kuna	 Croatia
    //        HTG = 332, //	2	Haitian gourde	 Haiti
    //        HUF = 348, //	2	Hungarian forint	 Hungary
    //        IDR = 360, //	2	Indonesian rupiah	 Indonesia
    //        ILS = 376, //	2	Israeli new shekel	 Israel,  Palestinian territories[7]
    //        INR = 356, //	2	Indian rupee	 India
    //        IQD = 368, //	3	Iraqi dinar	 Iraq
    //        IRR = 364, //	0	Iranian rial	 Iran
    //        ISK = 352, //	0	Icelandic króna	 Iceland
    //        JMD = 388, //	2	Jamaican dollar	 Jamaica
    //        JOD = 400, //	3	Jordanian dinar	 Jordan
    //        JPY = 392, //	0	Japanese yen	 Japan
    //        KES = 404, //	2	Kenyan shilling	 Kenya
    //        KGS = 417, //	2	Kyrgyzstani som	 Kyrgyzstan
    //        KHR = 116, //	2	Cambodian riel	 Cambodia
    //        KMF = 174, //	0	Comoro franc	 Comoros
    //        KPW = 408, //	0	North Korean won	 North Korea
    //        KRW = 410, //	0	South Korean won	 South Korea
    //        KWD = 414, //	3	Kuwaiti dinar	 Kuwait
    //        KYD = 136, //	2	Cayman Islands dollar	 Cayman Islands
    //        KZT = 398, //	2	Kazakhstani tenge	 Kazakhstan
    //        LAK = 418, //	0	Lao kip	 Laos
    //        LBP = 422, //	0	Lebanese pound	 Lebanon
    //        LKR = 144, //	2	Sri Lankan rupee	 Sri Lanka
    //        LRD = 430, //	2	Liberian dollar	 Liberia
    //        LSL = 426, //	2	Lesotho loti	 Lesotho
    //        LTL = 440, //	2	Lithuanian litas	 Lithuania
    //        LVL = 428, //	2	Latvian lats	 Latvia
    //        LYD = 434, //	3	Libyan dinar	 Libya
    //        MAD = 504, //	2	Moroccan dirham	 Morocco
    //        MDL = 498, //	2	Moldovan leu	 Moldova (except  Transnistria)
    //        MGA = 969, //	0*[8]	Malagasy ariary	 Madagascar
    //        MKD = 807, //	0	Macedonian denar	 Macedonia
    //        MMK = 104, //	0	Myanma kyat	 Myanmar
    //        MNT = 496, //	2	Mongolian tugrik	 Mongolia
    //        MOP = 446, //	2	Macanese pataca	 Macao
    //        MRO = 478, //	0*[8]	Mauritanian ouguiya	 Mauritania
    //        MUR = 480, //	2	Mauritian rupee	 Mauritius
    //        MVR = 462, //	2	Maldivian rufiyaa	 Maldives
    //        MWK = 454, //	2	Malawian kwacha	 Malawi
    //        MXN = 484, //	2	Mexican peso	 Mexico
    //        MXV = 979, //	2	Mexican Unidad de Inversion (UDI) (funds code)	 Mexico
    //        MYR = 458, //	2	Malaysian ringgit	 Malaysia
    //        MZN = 943, //	2	Mozambican metical	 Mozambique
    //        NAD = 516, //	2	Namibian dollar	 Namibia
    //        NGN = 566, //	2	Nigerian naira	 Nigeria
    //        NIO = 558, //	2	Nicaraguan córdoba	 Nicaragua
    //        NOK = 578, //	2	Norwegian krone	 Norway,  Svalbard,  Jan Mayen,  Bouvet Island, Queen Maud Land, Peter I Island
    //        NPR = 524, //	2	Nepalese rupee	   Nepal
    //        NZD = 554, //	2	New Zealand dollar	 Cook Islands,  New Zealand,  Niue,  Pitcairn,  Tokelau, Ross Dependency
    //        OMR = 512, //	3	Omani rial	 Oman
    //        PAB = 590, //	2	Panamanian balboa	 Panama
    //        PEN = 604, //	2	Peruvian nuevo sol	 Peru
    //        PGK = 598, //	2	Papua New Guinean kina	 Papua New Guinea
    //        PHP = 608, //	2	Philippine peso	 Philippines
    //        PKR = 586, //	2	Pakistani rupee	 Pakistan
    //        PLN = 985, //	2	Polish złoty	 Poland
    //        PYG = 600, //	0	Paraguayan guaraní	 Paraguay
    //        QAR = 634, //	2	Qatari riyal	 Qatar
    //        RON = 946, //	2	Romanian new leu	 Romania
    //        RSD = 941, //	2	Serbian dinar	 Serbia
    //        RUB = 643, //	2	Russian rouble	 Russia,  Abkhazia,  South Ossetia
    //        RWF = 646, //	0	Rwandan franc	 Rwanda
    //        SAR = 682, //	2	Saudi riyal	 Saudi Arabia
    //        SBD = 090, //	2	Solomon Islands dollar	 Solomon Islands
    //        SCR = 690, //	2	Seychelles rupee	 Seychelles
    //        SDG = 938, //	2	Sudanese pound	 Sudan
    //        SEK = 752, //	2	Swedish krona/kronor	 Sweden
    //        SGD = 702, //	2	Singapore dollar	 Singapore,  Brunei
    //        SHP = 654, //	2	Saint Helena pound	 Saint Helena
    //        SLL = 694, //	0	Sierra Leonean leone	 Sierra Leone
    //        SOS = 706, //	2	Somali shilling	 Somalia (except  Somaliland)
    //        SRD = 968, //	2	Surinamese dollar	 Suriname
    //        SSP = 728, //	2	South Sudanese pound	 South Sudan
    //        STD = 678, //	0	São Tomé and Príncipe dobra	 São Tomé and Príncipe
    //        SYP = 760, //	2	Syrian pound	 Syria
    //        SZL = 748, //	2	Swazi lilangeni	 Swaziland
    //        THB = 764, //	2	Thai baht	 Thailand
    //        TJS = 972, //	2	Tajikistani somoni	 Tajikistan
    //        TMT = 934, //	2	Turkmenistani manat	 Turkmenistan
    //        TND = 788, //	3	Tunisian dinar	 Tunisia
    //        TOP = 776, //	2	Tongan paʻanga	 Tonga
    //        TRY = 949, //	2	Turkish lira	 Turkey,  Northern Cyprus
    //        TTD = 780, //	2	Trinidad and Tobago dollar	 Trinidad and Tobago
    //        TWD = 901, //	2	New Taiwan dollar	 Republic of China (Taiwan)
    //        TZS = 834, //	2	Tanzanian shilling	 Tanzania
    //        UAH = 980, //	2	Ukrainian hryvnia	 Ukraine
    //        UGX = 800, //	2	Ugandan shilling	 Uganda
    //        USD = 840, //	2	United States dollar	 American Samoa,  Barbados (as well as Barbados Dollar),  Bermuda (as well as Bermudian Dollar),  British Indian Ocean Territory,  British Virgin Islands, Caribbean Netherlands,  Ecuador,  El Salvador,  Guam,  Haiti,  Marshall Islands,  Federated States of Micronesia,  Northern Mariana Islands,  Palau,  Panama,  Puerto Rico,  Timor-Leste,  Turks and Caicos Islands,  United States,  U.S. Virgin Islands,  Zimbabwe
    //        USN = 997, //	2	United States dollar (next day) (funds code)	 United States
    //        USS = 998, //	2	United States dollar (same day) (funds code) (one source[who?] claims it is no longer used, but it is still on the ISO 4217-MA list)	 United States
    //        UYI = 940, //	0	Uruguay Peso en Unidades Indexadas (URUIURUI) (funds code)	 Uruguay
    //        UYU = 858, //	2	Uruguayan peso	 Uruguay
    //        UZS = 860, //	2	Uzbekistan som	 Uzbekistan
    //        VEF = 937, //	2	Venezuelan bolívar fuerte	 Venezuela
    //        VND = 704, //	0	Vietnamese dong	 Vietnam
    //        VUV = 548, //	0	Vanuatu vatu	 Vanuatu
    //        WST = 882, //	2	Samoan tala	 Samoa
    //        XAF = 950, //	0	CFA franc BEAC	 Cameroon,  Central African Republic,  Republic of the Congo,  Chad,  Equatorial Guinea,  Gabon
    //        XAG = 961, //	.	Silver (one troy ounce)	
    //        XAU = 959, //	.	Gold (one troy ounce)	
    //        XBA = 955, //	.	European Composite Unit (EURCO) (bond market unit)	
    //        XBB = 956, //	.	European Monetary Unit (E.M.U.-6) (bond market unit)	
    //        XBC = 957, //	.	European Unit of Account 9 (E.U.A.-9) (bond market unit)	
    //        XBD = 958, //	.	European Unit of Account 17 (E.U.A.-17) (bond market unit)	
    //        XCD = 951, //	2	East Caribbean dollar	 Anguilla,  Antigua and Barbuda,  Dominica,  Grenada,  Montserrat,  Saint Kitts and Nevis,  Saint Lucia,  Saint Vincent and the Grenadines
    //        XDR = 960, //	.	Special drawing rights	International Monetary Fund
    //                   //XFU = Nil, //	.	UIC franc (special settlement currency)	International Union of Railways
    //        XOF = 952, //	0	CFA franc BCEAO	 Benin,  Burkina Faso,  Côte d'Ivoire,  Guinea-Bissau,  Mali,  Niger,  Senegal,  Togo
    //        XPD = 964, //	.	Palladium (one troy ounce)	
    //        XPF = 953, //	0	CFP franc (Franc du Pacifique)	French territories of the Pacific Ocean:  French Polynesia,  New Caledonia,  Wallis and Futuna
    //        XPT = 962, //	.	Platinum (one troy ounce)	
    //        XTS = 963, //	.	Code reserved for testing purposes	
    //        XXX = 999, //	.	No currency	
    //        YER = 886, //	2	Yemeni rial	 Yemen
    //        ZAR = 710, //	2	South African rand	 South Africa
    //        ZMW = 967, //	2	Zambian kwacha	 Zambia
    //    }
    //}
}