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
    }

    public enum Currency
    {
        AED, AFN, ALL, AMD, ANG, AOA, ARS, AUD, AWG, AZN,
        BAM, BBD, BDT, BGN, BHD, BIF, BMD, BND, BOB, BRL,
        BSD, BTN, BWP, BYN, BZD, CAD, CDF, CHF, CLP, COP,
        CRC, CUC, CUP, CVE, CZK, DJF, DKK, DOP, DZD, EGP,
        ERN, ETB, EUR, FJD, FKP, GBP, GEL, GHS, GIP, GMD,
        GNF, GTQ, GYD, HKD, HNL, HRK, HTG, HUF, IDR, ILS,
        INR, IQD, IRR, ISK, JMD, JOD, JPY,
        KES, KGS, KHR, KMF, KPW, KRW, KWD, KYD, KZT,
        LAK, LBP, LKR, LRD, LSL, LYD,
        MAD, MDL, MGA, MKD, MMK, MNT, MOP, MRU, MUR, MVR,
        MWK, MXN, MYR, MZN,
        NAD, NGN, NIO, NOK, NPR, NZD,
        OMR,
        PAB, PEN, PGK, PHP, PKR, PLN, PYG,
        QAR,
        RON, RSD, RUB, RWF,
        SAR, SBD, SCR, SDG, SEK, SGD, SHP, SLE, SLL, SOS,
        SRD, SSP, STD, STN, SVC, SYE, SZL,
        THB, TJS, TMT, TND, TOP, TRY, TTD, TWD, TZS,
        UAH, UGX, USD, UYU, UZS,
        VES, VND, VUV,
        WST,
        XAF, XCD, XOF, XPF,
        YER,
        ZAR, ZMW, ZWL
    }


    public enum AccountType
    {
        Current = 0,
        Savings = 1 << 0
    }
}
