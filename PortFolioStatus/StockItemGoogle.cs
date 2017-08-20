namespace PortFolioStatus
{
    public class StockItemGoogle
    {
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public decimal Price { get; set; }
        public System.DateTime Date { get; set; }
        public decimal Change { get; set; }
        public decimal ChangePct { get; set; }
    }
}