using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using FloorsPlanGGGWebApi.DataModels;
using FloorsPlanGGGWebApi.DTOs;
using FloorsPlanGGGWebApi.Misc;

namespace FloorsPlanGGGWebApi.Controllers
{
    public class UserLocationDetailsController : ApiController
    {
        private readonly GenericRepository<User> m_userRepository;
        private readonly GenericRepository<Room> m_roomRepository;

        public UserLocationDetailsController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            m_userRepository = new GenericRepository<User>(unitOfWork);
            m_roomRepository = new GenericRepository<Room>(unitOfWork);
        }

        // GET api/UserLocationDetailsDto
        public IEnumerable<UserLocationDetailsDto> Get()
        {
            try
            {
                var userList = m_userRepository.GetAll();
                return userList.Select(x => new UserLocationDetailsDto
                {
                    User = new UserDto
                    {
                        Id = x.Id,
                        FullName = x.Name,
                        PhotoUrl = x.PhotoUrl
                    },
                    Coordinates = new CoordinatesDto
                    {
                        Id = x.RoomId,
                        X = x.CoordX,
                        Y = x.CoordY,
                        ComplexIdBased = $"R{x.Room.Floor.Id}_{x.RoomId}_{x.Id}"
                    },
                    Floor = new FloorDto
                    {
                        Id = x.Room.Floor.Id,
                        DisplayName = x.Room.Floor.DisplayName,
                        FloorPhotoUrl = x.Room.Floor.FloorPhotoUrl
                    },
                    Room = new RoomDto
                    {
                        Id = x.RoomId,
                        DisplayName = x.Name
                    }
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET api/UserLocationDetailsDto/5
        public UserLocationDetailsDto Get(int id)
        {
            try
            {
                var userModel = m_userRepository.Get(id);

                var img = userModel.Room.Floor.FloorPhotoUrl.GetImageFromFileName();
                img?.PlacePinOnPhotoWithCoordinates(userModel.CoordX ?? 0, userModel.CoordY ?? 0);
                var newFileName = $"{userModel.Room.Floor.FloorPhotoUrl.Split('.')[0]}_{id}.png";
                img?.SaveImageToFile(newFileName);

                return new UserLocationDetailsDto
                {
                    User = new UserDto
                    {
                        Id = userModel.Id,
                        FullName = userModel.Name,
                        PhotoUrl = userModel.PhotoUrl
                    },
                    Coordinates = new CoordinatesDto
                    {
                        Id = userModel.RoomId,
                        X = userModel.CoordX,
                        Y = userModel.CoordY,
                        ComplexIdBased = $"R{userModel.Room.Floor.Id}_{userModel.RoomId}_{userModel.Id}"
                    },
                    Floor = new FloorDto
                    {
                        Id = userModel.Room.Floor.Id,
                        DisplayName = userModel.Room.Floor.DisplayName,
                        FloorPhotoUrl = newFileName
                    },
                    Room = new RoomDto
                    {
                        Id = userModel.RoomId,
                        DisplayName = userModel.Name
                    }
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // POST api/UserLocationDetailsDto
        public async Task<HttpResponseMessage> Post([FromBody]UserLocationDetailsDto fullUserLocationDetails)
        {
            try
            {
                var userModelAlreadyExists = m_userRepository.Get(fullUserLocationDetails.User.Id) != null;
                if (userModelAlreadyExists)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "User with this ID already exists."
                    };
                }

                var assignedRoom = m_roomRepository.Get(fullUserLocationDetails.Room.Id);
                if (assignedRoom == null)
                {
                    // we don't have a room with the data specified in the request
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "No room with the data specified in the request seems to exist."
                    };
                }

                await m_userRepository.Add(new User
                {
                    Id = fullUserLocationDetails.User.Id,
                    Name = fullUserLocationDetails.User.FullName,
                    PhotoUrl = fullUserLocationDetails.User.PhotoUrl,
                    CoordX = fullUserLocationDetails.Coordinates.X,
                    CoordY = fullUserLocationDetails.Coordinates.Y,
                    SpecialCoord = fullUserLocationDetails.Coordinates.ComplexIdBased,
                    Place = fullUserLocationDetails.Coordinates.Id ?? -1,
                    Room = assignedRoom,
                    RoomId = assignedRoom.Id
                });

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

        // PUT api/UserLocationDetailsDto/5
        public async Task<HttpResponseMessage> Put(int id, [FromBody]UserLocationDetailsDto fullUserLocationDetails)
        {
            try
            {
                var userToUpdate = m_userRepository.Get(fullUserLocationDetails.User.Id);
                if (userToUpdate == null)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "User with this ID doesn't exist."
                    };
                }

                var assignedRoom = m_roomRepository.Get(fullUserLocationDetails.Room.Id);
                if (assignedRoom == null)
                {
                    // we don't have a room with the data specified in the request
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "No room with the data specified in the request seems to exist."
                    };
                }

                userToUpdate.Name = fullUserLocationDetails.User.FullName;
                userToUpdate.PhotoUrl = fullUserLocationDetails.User.PhotoUrl;
                userToUpdate.CoordX = fullUserLocationDetails.Coordinates.X;
                userToUpdate.CoordY = fullUserLocationDetails.Coordinates.Y;
                userToUpdate.SpecialCoord = fullUserLocationDetails.Coordinates.ComplexIdBased;
                userToUpdate.Place = fullUserLocationDetails.Coordinates.Id ?? -1;
                userToUpdate.Room = assignedRoom;
                userToUpdate.RoomId = assignedRoom.Id;

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

        // DELETE api/UserLocationDetailsDto/5
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
