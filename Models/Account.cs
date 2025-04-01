using static Budget_management_back_end.Records.Records;

namespace Budget_management_back_end.Models
{
    public class Account
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public List<UserResponse> Users { get; set;}
    }
}
