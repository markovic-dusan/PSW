using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace PSW_Dusan_Markovic.resources.model
{
    public class User : IdentityUser
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public User(string UserName, string password, string name, string lastName, string email, UserType userType)
        {
            this.UserName = UserName;
            Password = password;
            UserType = userType;
            Name = name;
            LastName = lastName;
            Email = email;
        }

        public void UpdateUser(User user)
        {
            UserName = user.UserName;
            Password = user.Password;
            Name = user.Name;
            LastName= user.LastName;
            Email = user.Email;
        }
    }
}
