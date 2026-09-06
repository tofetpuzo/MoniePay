namespace MoniePay.src
{
    [Flags]
    public enum Status
    {
        SUCCESS = 1 << 3,
        FAIL = 1 << 4,
        PROCESSING = 1 << 5,
        TRYING = 1 << 6,
    }

    [Flags]
    public enum Channel
    {
        ATM = 1 << 1,
        WEB = 1 << 2,
        POS = 1 << 3,
        MOBILE = 1 << 7,
        API = 1 << 8,
        COUNTER = 1 << 9,
    }

    public enum Currency
    {
        AED = 0, AFN = 1, ALL = 2, AMD = 3, ANG = 4,
        AOA = 5, ARS = 6, AUD = 7, AWG = 8, AZN = 9,

        BAM = 10, BBD = 11, BDT = 12, BGN = 13, BHD = 14,
        BIF = 15, BMD = 16, BND = 17, BOB = 18, BRL = 19,

        BSD = 20, BTN = 21, BWP = 22, BYN = 23, BZD = 24,
        CAD = 25, CDF = 26, CHF = 27, CLP = 28, COP = 29,

        CRC = 30, CUC = 31, CUP = 32, CVE = 33, CZK = 34,
        DJF = 35, DKK = 36, DOP = 37, DZD = 38, EGP = 39,

        ERN = 40, ETB = 41, EUR = 42, FJD = 43, FKP = 44,
        GBP = 45, GEL = 46, GHS = 47, GIP = 48, GMD = 49,

        GNF = 50, GTQ = 51, GYD = 52, HKD = 53, HNL = 54,
        HRK = 55, HTG = 56, HUF = 57, IDR = 58, ILS = 59,

        INR = 60, IQD = 61, IRR = 62, ISK = 63, JMD = 64,
        JOD = 65, JPY = 66,

        KES = 67, KGS = 68, KHR = 69, KMF = 70, KPW = 71,
        KRW = 72, KWD = 73, KYD = 74, KZT = 75,

        LAK = 76, LBP = 77, LKR = 78, LRD = 79, LSL = 80,
        LYD = 81,

        MAD = 82, MDL = 83, MGA = 84, MKD = 85, MMK = 86,
        MNT = 87, MOP = 88, MRU = 89, MUR = 90, MVR = 91,

        MWK = 92, MXN = 93, MYR = 94, MZN = 95,

        NAD = 96, NGN = 97, NIO = 98, NOK = 99, NPR = 100,
        NZD = 101,

        OMR = 102,

        PAB = 103, PEN = 104, PGK = 105, PHP = 106, PKR = 107,
        PLN = 108, PYG = 109,

        QAR = 110,

        RON = 111, RSD = 112, RUB = 113, RWF = 114,

        SAR = 115, SBD = 116, SCR = 117, SDG = 118, SEK = 119,
        SGD = 120, SHP = 121, SLE = 122, SLL = 123, SOS = 124,

        SRD = 125, SSP = 126, STD = 127, STN = 128, SVC = 129,
        SYE = 130, SZL = 131,

        THB = 132, TJS = 133, TMT = 134, TND = 135, TOP = 136,
        TRY = 137, TTD = 138, TWD = 139, TZS = 140,

        UAH = 141, UGX = 142, USD = 143, UYU = 144, UZS = 145,

        VES = 146, VND = 147, VUV = 148,

        WST = 149,

        XAF = 150, XCD = 151, XOF = 152, XPF = 153,

        YER = 154,

        ZAR = 155, ZMW = 156, ZWL = 157
    }



    public enum AccountType
    {
        Current = 0,
        Savings = 1 << 0
    }
}
