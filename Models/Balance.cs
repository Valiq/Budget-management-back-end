namespace Budget_management_back_end.Models
{
    public class Balance
    {
        public long Id { get; set; }
        public long FinanceEntityId { get; set; }
        public long CurrencyId { get; set; }
        public decimal Sum { get; set; }
    }
}
