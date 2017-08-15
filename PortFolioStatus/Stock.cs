using SQLite;
using System;

namespace PortFolioStatus
{
    public class Stock
    {
        [AutoIncrement, PrimaryKey]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Ticker { get; set; }
        public string Exchange { get; set; }
        public int Qty { get; set; }
        public decimal UnitCost { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool Short { get; set; }
        [Ignore]
        public decimal CurrentUnitCost { get; set; }
        [Ignore]
        public DateTime CurrentDateTime { get; set; }
        public override string ToString()
        {
            return string.Format("[Stock: ID={0}, Name={1}, Ticker={2}, Exchange={3}, Qty={4}, UnitCost={5}, PurchaseDate={6}, Short={7}, " +
                "CurrentUnitCost={8}, CurrentDateTime={9}]",
                ID, Name, Ticker, Exchange, Qty, UnitCost, PurchaseDate, Short, CurrentUnitCost, CurrentDateTime);
        }
    }
}