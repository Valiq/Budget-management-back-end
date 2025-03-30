namespace Budget_management_back_end.Models
{
    public class TransactionAudit
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long AccountId { get; set; }
        public long Finance_EntityFrom { get; set; }
        public long Finance_EntityTo { get; set; }
        public long CurrencyId { get; set; }
        public long BalanceId { get; set;}
        public decimal Sum { get; set; }
        public string DateTime { get; set; }
        public string Comment { get; set; }
    }
}
