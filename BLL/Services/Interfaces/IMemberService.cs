using BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createdMember);

        MemberViewModel? GetMemberDetails(int MemberId);

        HealthViewModel? GetMemberHealthRecordDetails(int MemberId);

        // For Update: 
        MemberToUpdateViewModel GetMemberToUpdate(int MemberId);
        bool UpdateMemberDetails(int id, MemberToUpdateViewModel UpdatedMember);

        bool RemoveMember(int MemberId);
    }
}
