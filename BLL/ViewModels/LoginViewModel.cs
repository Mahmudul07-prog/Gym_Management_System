using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Email is Requierd !")]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage ="Password is Requierd !")]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }
}
