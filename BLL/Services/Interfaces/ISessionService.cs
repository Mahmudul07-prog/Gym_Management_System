using BLL.ViewModels.SessionsViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();
        SessionViewModel? SessionById(int sessionId);

        bool CreateSession(CreateSessionViewModel createdSession);

        // Update
        UpdateSessionViewModel? GetSessionToUpdate(int sessionId);
        bool UpdateSession(UpdateSessionViewModel updateSession, int sessionId);

        bool DeleteSession(int sessionId);

        IEnumerable<TrainerAndCategorySelectViewModel> GetTrainers();
        IEnumerable<TrainerAndCategorySelectViewModel> GetCategories();

    }
}
