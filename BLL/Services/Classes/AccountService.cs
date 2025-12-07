using BLL.Services.Interfaces;
using BLL.ViewModels;
using DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Classes
{
    public class AccountService(UserManager<ApplicationUser> _userManager) : IAccountService
    {
        public ApplicationUser? ValidateUser(LoginViewModel loginVM)
        {
            var User = _userManager.FindByEmailAsync(loginVM.Email).Result;
            if (User == null) return null;
            var IsPassValide = _userManager.CheckPasswordAsync(User, loginVM.Password).Result;

            return IsPassValide ? User : null;
        }
    }
}
