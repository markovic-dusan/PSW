using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using System.Data;
using System.Text.Json.Serialization;

namespace PSW_Dusan_Markovic.resources.model
{
    public class User : IdentityUser
    {

        [Required]
        public string Password { get; set; }

        [Required]
        public UserType UserType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [NotMapped]
        public List<Interest> Interests { get; set; } 

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public User() { }

        public User(string UserName, string password, string name, string lastName, string email, UserType userType)
        {
            this.UserName = UserName;
            Password = password;
            UserType = userType;
            Name = name;
            LastName = lastName;
            Email = email;
            Interests = new List<Interest>();
        }

        [JsonConstructor]
        public User(string UserName, string password, string name, string lastName, string email, UserType userType, List<Interest> interests)
        {
            this.UserName = UserName;
            Password = password;
            UserType = userType;
            Name = name;
            LastName = lastName;
            Email = email;
            Interests = interests;
        }

        public void updateUser(User user)
        {
            UserName = user.UserName;
            Password = user.Password;
            Name = user.Name;
            LastName= user.LastName;
            Email = user.Email;
        }

    }
}
