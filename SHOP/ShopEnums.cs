namespace Game.Shop
{
    // Tipologia di oggetto/servizio acquistabile nello shop
    public enum ShopOfferType
    {
        DailyCard,     // Carta del giorno acquistabile in oro
        SingleCard,    // Carta singola acquistabile con gemme
        CardPack,      // Pacchetti di carte con rarità
        GoldPack,      // Pacchetti di oro
        GemPack,       // Pacchetti di gemme
        SpecialChest,  // Bauli speciali
        SpecialOffer   // Offerte miste: pacchetti + gemme + oro + euro
    }

    // Valuta con cui si compra un'offerta
    public enum ShopCurrency
    {
        Gold,        // Oro
        Gems,        // Gemme
        RealMoney    // Acquisto in euro
    }

    // Rarità delle offerte nello shop (diversa dalla rarità delle carte)
    public enum ShopItemRarity
    {
        Basic,       // Piccolo, economico
        Enhanced,    // Medio
        Grand,       // Grande
        Supreme,     // Premium
        Divine       // Esclusivo
    }
}
