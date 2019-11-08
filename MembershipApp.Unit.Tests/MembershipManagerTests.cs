using Xunit;

namespace MembershipApp.Tests
{
    public class MembershipManagerTests
    {
        private readonly MembershipManager _membershipManager;

        public MembershipManagerTests()
        {
            _membershipManager = new MembershipManager();
        }
        
        [Fact]
        public void Should_CreateNewMember_WithNameAndId()
        {
            // Act
            var newMember = _membershipManager.CreateNewMember("Test", "Member");
            
            // Assert
            Assert.Equal(1,newMember.Id);
            Assert.Equal("Test", newMember.FirstName);
            Assert.Equal("Member", newMember.LastName);
        }
        
        [Fact]
        public void Should_CreateNewMemberWithNextId_WhenMembershipAppHasNoMembers()
        {
            // Arrange
            var newMember = _membershipManager.CreateNewMember("Test", "Member");
            var nextNewMember = _membershipManager.CreateNewMember("Other", "Member");
            
            // Assert
            Assert.Equal(2,nextNewMember.Id);
        }

        [Fact]
        public void Should_RemoveMember_WhenValidIdProvided()
        {
            // Arrange
            var newMember = _membershipManager.CreateNewMember("Test", "Member");

            // Act
            _membershipManager.DeleteMember(1);
            
            // Assert
            Assert.Empty(_membershipManager.GetAllMembers());
        }

        [Fact]
        public void Should_Return_Null_When_No_Member_Is_Present()
        {
            // Act
            var member = _membershipManager.GetMember(1);
            
            // Assert
            Assert.Null(member);
        }
        
        
    }
}