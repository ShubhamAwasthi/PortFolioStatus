namespace PortFolioStatus
{
    public class StockAdapterListItem
    {
        public string Name { get; set; }
        public string Exchange { get; set; }
        public string Ticker { get; set; }
        public string Qty { get; set; }
        public string IsShort { get; set; }
        public string OriginalPrice { get; set; }
        public string OriginalDate { get; set; }
        public string CurrentPrice { get; set; }
        public string CurrentDate { get; set; }
        public string ChangeFromLastTrade { get; set; }
        public string ChangePctFromLastTrade { get; set; }
        public string TotalCost { get; set; }
        public string TotalCurrentCost { get; set; }
        public string TotalChange { get; set; }
        public string TotalChangePct { get; set; }
    }
}