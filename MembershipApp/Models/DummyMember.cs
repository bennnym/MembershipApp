namespace MembershipApp.Models
{
    public class DummyMember : IMember
    {
        public DummyMember()
        {
            Id = 1;
            FirstName = "Ben";
            LastName = "Muller";
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}