﻿using System;
using System.Collections.Generic;
using System.Web.Http;
using LearnWithMentorDAL.Entities;
using LearnWithMentorDAL.UnitOfWork;
using LearnWithMentorDTO;

namespace LearnWithMentor.Controllers
{
    public class UserController : ApiController
    {
        private IUnitOfWork UoW;
        public UserController()
        {
            UoW = new UnitOfWork(new LearnWithMentor_DBEntities());
        }
        // GET: api/User
        public IEnumerable<UserDTO> Get()
        {
            List<UserDTO> dto = new List<UserDTO>();
            foreach (var u in UoW.Users.GetAll())
            {
                dto.Add(new UserDTO(u.Id, u.FirstName, u.LastName, u.Email, u.Roles.Name));
            }
            return dto;
        }
        // GET: api/User/5
        public UserDTO Get(int id)
        {
            User u = UoW.Users.Get(id);
            return new UserDTO(u.Id, u.FirstName, u.LastName, u.Email, u.Roles.Name);
        }

        // POST: api/User
        public IHttpActionResult Post([FromBody]UserDTO value)
        {
            bool success = UoW.Users.Add(value, "123");
            if (success)
            {
                try
                {
                    UoW.Save();
                    return Ok(String.Format("Succesfully created user: {0}.", value.Email));
                }
                catch (Exception exception)
                {
                    return InternalServerError(exception);
                }
            }
            else
            {
                var message = "Incorrect request syntax.";
                return BadRequest(message);
            }
        }

        // PUT: api/User/5
        public IHttpActionResult Put(int id, [FromBody]UserDTO value)
        {
            bool success = UoW.Users.UpdateById(id, value);
            if (success)
            {
                try
                {
                    UoW.Save();
                    return Ok(String.Format("Succesfully updated user id: {0}.", id));
                }
                catch (Exception exception)
                {
                    return InternalServerError(exception);
                }
            }
            else
            {
                var message = "Incorrect request syntax or user does not exist.";
                return BadRequest(message);
            }
        }

        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {
            bool success = UoW.Users.RemoveById(id);
            if (success)
            {
                try
                {
                    UoW.Save();
                    return Ok(String.Format("Succesfully deleted user id: {0}.", id));
                }
                catch (Exception exception)
                {
                    return InternalServerError(exception);
                }
            }
            else
            {
                var message = $"Not existing user with id: {id}.";
                return BadRequest(message);
            }
        }

        [HttpGet]
        [Route("api/user/search")]
        public IEnumerable<UserDTO> Search(string q, string role)
        {
            if (q == null)
            {
                return Get();
            }
            else
            {
                Role criteria;
                bool existsRole = UoW.Roles.TryGetByName(role, out criteria);
                string[] lines = q.Split(' ');
                List<UserDTO> dto = new List<UserDTO>();
                foreach (var u in existsRole ? UoW.Users.Search(lines,criteria.Id) : UoW.Users.Search(lines, null))
                {
                    dto.Add(new UserDTO(u.Id, u.FirstName, u.LastName, u.Email, u.Roles.Name));
                }
                return dto;
            }
        }

        [Route("api/user/roles")]
        public IEnumerable<RoleDTO> GetRoles()
        {
            var dtos = new List<RoleDTO>();
            foreach(var role in UoW.Roles.GetAll())
            {
                dtos.Add(new RoleDTO(role.Id, role.Name));
            }
            return dtos;
        }
    }
}
