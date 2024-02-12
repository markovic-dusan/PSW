using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;

namespace PSW_Dusan_Markovic.resources.service
{
    public class UserService
    {
        private readonly YourDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserService(YourDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //konstruktor koji se koristi za testove
        public UserService(YourDbContext context, List<User> initialData)
        {
            _context = context;
            _context.Users.AddRange(initialData);
            _context.SaveChanges();
        }

        public List<User> getAllUsers()
        {
            return _context.Users.ToList();
        }

        public User getUserById(string id)
        {
            return _context.Users.Find(id);
        }

        public async Task<bool> registerUser(User user)
        {
            bool successfullyRegistered = true;
            bool userExists = _context.Users.Any(u => u.Email == user.Email || u.UserName == user.UserName);

            if (!userExists)
            {
                foreach (Interest i in user.Interests)
                {
                    _context.UserInterests.Add(new UserInterest(user.Id, i.InterestValue));
                }
                _context.SaveChanges();

                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded && _userManager != null)
                {
                    await _userManager.AddToRoleAsync(user, user.UserType.ToString());
                }
                else
                {
                    successfullyRegistered = false;
                }
            }
            else
            {
                successfullyRegistered = false;
            }

            return successfullyRegistered;
        }

        public bool updateUser(User user)
        {
            var userToUpdate = _context.Users.Find(user.Id);
            if (userToUpdate == null)
            {
                return false;
            }
            userToUpdate.updateUser(user);
            _context.SaveChanges();
            return true;
        }

        public bool deleteUser(string id)
        {
            var userToDelete = _context.Users.Find(id);
            if(userToDelete == null)
            {
                return false;
            }
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return true;
        }
    }
}
