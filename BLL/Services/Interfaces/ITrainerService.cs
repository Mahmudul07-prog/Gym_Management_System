using BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainer();

        bool CreateTrainer(CreateTrainerViewModel createdTrainer);

        TrainerViewModel? GetTrainerDetails(int trainerId);

        #region Update
        UpdatedTrainerViewModel? GetTrainerToUpdate(int trainerId);
        bool UpdateTrainer(int trainerId, UpdatedTrainerViewModel updatedTrainer); 
        #endregion

        bool DeleteTrainer(int trainerId);
    }
}
