namespace Budget_management_back_end.Models
{
    public class FinanceEntity
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
