namespace Budget_management_back_end.Models
{
    public class TransactionAudit
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public long BalanceFromId { get; set; }
        public long BalanceToId { get; set; }
        public decimal Sum { get; set; }
        public string DateTime { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
    }
}
