using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FloorsPlanGGGWebApi.DataModels;
using FloorsPlanGGGWebApi.DTOs;

namespace FloorsPlanGGGWebApi.Controllers
{
    public class UsersController : ApiController
    {
        private readonly GenericRepository<User> m_userRepository;

        public UsersController()
        {
            m_userRepository = new GenericRepository<User>();
        }

        // GET api/users
        public IEnumerable<UserDto> Get()
        {
            try
            {
                var userList = m_userRepository.GetAll();
                return userList.Select(x => new UserDto
                {
                    Id = x.Id,
                    FullName = x.Name,
                    PhotoUrl = x.PhotoUrl
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET api/users/5
        public UserDto Get(int id)
        {
            try
            {
                var userModel = m_userRepository.Get(id);
                return new UserDto
                {
                    Id = userModel.Id,
                    FullName = userModel.Name,
                    PhotoUrl = userModel.PhotoUrl
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // POST api/users
        public async Task<HttpResponseMessage> Post([FromBody]UserDto user)
        {
            try
            {
                var userModelAlreadyExists = m_userRepository.Get(user.Id) != null;
                if (userModelAlreadyExists)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "User with this ID already exists."
                    };
                }

                await m_userRepository.Add(new User { Id = user.Id, PhotoUrl = user.PhotoUrl, Name = user.FullName, Place = 1});
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to add the user. ({ex.Message})"
                };
            }
        }

        // PUT api/users/5
        public async Task<HttpResponseMessage> Put(int id, [FromBody]UserDto user)
        {
            try
            {
                var userToUpdate = m_userRepository.Get(user.Id);
                if (userToUpdate == null)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "User with this ID doesn't exist."
                    };
                }
            
                //userToUpdate.Id = user.Id;
                userToUpdate.Name = user.FullName;
                userToUpdate.PhotoUrl = user.PhotoUrl;
                await m_userRepository.Update(userToUpdate);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to add the user. ({ex.Message})"
                };
            }
        }

        // DELETE api/users/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                var userToDelete = m_userRepository.Get(id);
                if (userToDelete == null)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "User with this ID doesn't exist."
                    };
                }
            
                await m_userRepository.DeleteById(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to remove the user. ({ex.Message})"
                };
            }
        }
    }
}
