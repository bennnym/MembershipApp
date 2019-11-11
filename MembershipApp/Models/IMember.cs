namespace MembershipApp.Models
{
    public interface IMember
    {
        int Id { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string FullName { get; set; }
    }
}