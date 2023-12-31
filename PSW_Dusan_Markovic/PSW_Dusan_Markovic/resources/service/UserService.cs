﻿using Microsoft.AspNetCore.Mvc;
using PSW_Dusan_Markovic.resources.model;

namespace PSW_Dusan_Markovic.resources.service
{
    public class UserService
    {
        private readonly YourDbContext _context; 

        public UserService(YourDbContext context)
        {
            _context = context;
        }

        //za testove
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

        public User getUserById(int id)
        {
            return _context.Users.Find(id);
        }

        public bool registerUser(User user)
        {
            bool successfullyRegistered = true;
            bool userExists = _context.Users.Any(u => u.Email == user.Email);
            if (!userExists){
                try
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
                catch(Exception e){
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
            var userToUpdate = _context.Users.Find(user.UserId);
            if (userToUpdate == null)
            {
                return false;
            }
            userToUpdate.Username = user.Username;
            userToUpdate.Password = user.Password;
            userToUpdate.UserType = user.UserType;
            userToUpdate.Name = user.Name;
            userToUpdate.LastName = user.LastName;
            userToUpdate.Email = user.Email;
            _context.SaveChanges();
            return true;
        }

        public bool deleteUser(int id)
        {
            var userToDelete = _context.Users.Find(id);
            if(userToDelete == null)
            {
                return false;
            }
            _context.Remove(userToDelete);
            _context.SaveChanges();
            return true;
        }
    }
}
