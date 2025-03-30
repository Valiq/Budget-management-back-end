namespace Budget_management_back_end.Models
{
    public class UserAudit
    {
        public long Id { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public long AccountId { get; set; }
        public string LoginDate { get; set; }
    }
}
