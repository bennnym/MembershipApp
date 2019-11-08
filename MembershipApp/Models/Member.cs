namespace MembershipApp.Models
{
    public class Member : IMember
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}