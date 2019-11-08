using System;
using System.Collections.Generic;
using System.Linq;
using MembershipApp.Models;

namespace MembershipApp
{
    public class MembershipManager
    {
        private readonly List<IMember> _membersDataBase;
        
        private static MembershipManager _instance;
        
        public static MembershipManager Instance 
        {
            get {
                if (_instance == null)
                {
                    _instance = new MembershipManager();
                }
                return _instance;
            }
        }
        public MembershipManager(List<IMember> members = null)
        {
            _membersDataBase = members ?? new List<IMember>();
        }
        
        public IMember CreateNewMember(string firstName, string lastName)
        {
            var newMember = new Member
            {
                Id = GetNextValidId(),
                FirstName = firstName,
                LastName = lastName,
            };
            
            _membersDataBase.Add(newMember);

            return newMember;
        }

        public IMember GetMember(int memberId)
        {
            return _membersDataBase.FirstOrDefault(m => m.Id == memberId);
        }

        public List<IMember> GetAllMembers()
        {
            return _membersDataBase;
        }

        public void DeleteMember(int memberId)
        {
            var memberToDelete = GetMember(memberId);
            _membersDataBase.Remove(memberToDelete);
        }

        private int GetNextValidId()
        {
            try
            { 
                return _membersDataBase.Max(m => m.Id) + 1;
            }
            catch (InvalidOperationException e)
            {
                return 1;
            }
            
        }
    }
}