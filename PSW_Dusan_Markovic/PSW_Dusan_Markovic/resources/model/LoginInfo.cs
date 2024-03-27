using System.Net.Http.Headers;

namespace PSW_Dusan_Markovic.resources.model
{
    public class LoginInfo
    {
        public string Role { get; set; }
        public string Token { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }

        public LoginInfo(string role, string token, string username, string email, string userId, string name, string surname)
        {
            this.Role = role;
            this.Token = token;
            this.Username = username;
            this.Email = email;
            this.UserId = userId;
            this.Name = name;
            this.Surname = surname;
        }

        public LoginInfo(string token, User user)
        {
            this.Token = token;
            this.Email = user.Email;
            this.Username = user.UserName;
            this.UserId = user.Id;
            this.Name = user.Name;
            this.Surname = user.LastName;
            switch (user.UserType)
            {
                case UserType.AUTHOR:
                    this.Role = "author";
                    break;
                case UserType.ADMIN:
                    this.Role = "admin";
                    break;
                default:
                case UserType.TOURIST:
                    this.Role = "tourist";
                    break;
            };
              
        }
    }
}
