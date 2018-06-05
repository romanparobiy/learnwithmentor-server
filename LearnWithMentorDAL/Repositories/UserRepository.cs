﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LearnWithMentorDAL.Entities;
using System.Data.Entity;
using LearnWithMentorDTO;

namespace LearnWithMentorDAL.Repositories
{
    public class UserRepository: BaseRepository<User>, IUserRepository
    {
        public UserRepository(LearnWithMentor_DBEntities _context) : base(_context)
        {
        }
        public User Get(int id)
        {
            return context.Users.Where(u => u.Id == id).FirstOrDefault();
        }
        public void RemoveById(int id)
        {
            var item = context.Users.Where(u => u.Id == id);
            if (item != null)
            {
                context.Users.RemoveRange(item);
            }
        }
        public void UpdateById(int id, UserDTO user)
        {
            var item = context.Users.Where(u => u.Id == id);
            if (item != null)
            {
                User toUpdate = item.First();
                toUpdate.FirstName = user.FirstName;
                toUpdate.LastName = user.LastName;
                if (context.Roles.Where(r => r.Name == user.Role) != null)
                {
                    toUpdate.Role_Id = context.Roles.Where(r => r.Name == user.Role).FirstOrDefault().Id;
                }
                Update(toUpdate);
            }
        }
        public void Add(UserDTO userDTO, string password)
        {
            User toAdd = new User();
            toAdd.Email = userDTO.Email;
            //add hashing
            toAdd.Password = password;
            toAdd.Role_Id = context.Roles.Where(r => r.Name == userDTO.Role) != null ?
                context.Roles.Where(r => r.Name == userDTO.Role).First().Id : context.Roles.Where(r => r.Name == "Student").FirstOrDefault().Id;
            toAdd.FirstName = userDTO.FirstName;
            toAdd.LastName = userDTO.LastName;
            context.Users.Add(toAdd);
        }
        public IEnumerable<User> Search(string[] str)
        {
            List<User> ret = new List<User>();
            foreach (var s in str)
            {
                var found = context.Users.Where(u => u.FirstName.Contains(s) || u.LastName.Contains(s));
                foreach (var f in found)
                {
                    if (!ret.Contains(f))
                    {
                        ret.Add(f);
                    }
                }
            }
            return ret;
        }

        public string ExtractFullName(Nullable<int> id)
        {
            if (id == null)
                return null;
            User currentUser = context.Users.Where(u => u.Id == id).FirstOrDefault();
            string fullName = null;
            if (currentUser!=null)
                fullName=string.Concat(currentUser.FirstName, " ", currentUser.LastName);
            return fullName;
        }
    }
}
