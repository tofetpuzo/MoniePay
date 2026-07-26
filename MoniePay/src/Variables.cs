namespace MoniePay.src
{
    [Flags]
    public enum Status
    {
        SUCCESS = 13,
        FAIL = 23,
        PROCESSING = 33,
        TRYING = 43
    }

    [Flags]
    public enum Channel
    {
        ATM = 10 << 3,
        WEB = 11 << 4,
        POS = 12 << 5,
        MOBILE = 13 << 6,
        API = 14 << 7,
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
}
