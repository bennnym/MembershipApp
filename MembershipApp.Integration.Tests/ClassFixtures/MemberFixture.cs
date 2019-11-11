using MembershipApp.Models;

namespace MembershipApp.Tests.Integration.ClassFixtures
{
    public class MemberFixture
    {
        public MemberFixture()
        {
            FirstTestMember = new Member
            {
                FirstName = "Ben",
                LastName = "Muller"
            };
            
            SecondTestMember = new Member
            {
                FirstName = "Leah",
                LastName = "Hou"
            };
        }

        public Member SecondTestMember { get; }
        public Member FirstTestMember { get; }
    }
}