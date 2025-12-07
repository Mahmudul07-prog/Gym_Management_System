using BLL.ViewModels;
using BLL.ViewModels.SessionsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ISessionScheduleService
    {
        IEnumerable<SessionScheduleViewModel> GetAll();
        IEnumerable<GetMembersToNewBooking> membersToNewBookings(int SessionId);
        bool AddNewBooking(CreatedSessionScheduleViewModel viewModel);
        bool updateIsAttend(CreatedSessionScheduleViewModel viewModel);

        IEnumerable<MembersViewModel?> GetMembersForUpcomingSession(int sessionId);
        IEnumerable<MembersViewModel?> GetMembersForOngoingSessions(int sessionId);

        bool CancelBooking(CancelMembersBookingViewModel viewModel);
    }
}
