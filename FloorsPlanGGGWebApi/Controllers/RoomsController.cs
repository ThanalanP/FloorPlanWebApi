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
    public class RoomsController : ApiController
    {
        private readonly GenericRepository<Room> m_roomsRepo;
        private readonly GenericRepository<Floor> m_floorsRepo;

        public RoomsController()
        {
            UnitOfWork uok = new UnitOfWork();
            m_roomsRepo = new GenericRepository<Room>(uok);
            m_floorsRepo = new GenericRepository<Floor>(uok);
        }

        // GET api/rooms
        public IEnumerable<RoomDto> Get()
        {
            try
            {
                var roomsList = m_roomsRepo.GetAll();
                return roomsList.Select(x => new RoomDto
                {
                    Id = x.Id,
                    DisplayName = x.DisplayName
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET api/rooms/5
        public FloorDto Get(int id)
        {
            try
            {
                var roomModel = m_roomsRepo.Get(id);
                return new FloorDto
                {
                    Id = roomModel.Id,
                    DisplayName = roomModel.DisplayName
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // POST api/rooms
        public async Task<HttpResponseMessage> Post([FromBody]RoomDto room)
        {
            try
            {
                var floorsModelAlreadyExists = m_roomsRepo.Get(room.Id) != null;
                if (floorsModelAlreadyExists)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Floor with this ID already exists."
                    };
                }

                // TODO: link floors with rooms ...
                await m_roomsRepo.Add(new Room { Id = room.Id, DisplayName = room.DisplayName });
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to add the floor. ({ex.Message})"
                };
            }
        }

        /*
         
        // PUT api/floors/5
        public async Task<HttpResponseMessage> Put(int id, [FromBody]FloorDto floor)
        {
            try
            {
                var floorToUpdate = m_floorsRepo.Get(floor.Id);
                if (floorToUpdate == null)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Floor with this ID doesn't exist."
                    };
                }

                //userToUpdate.Id = user.Id;
                floorToUpdate.DisplayName = floor.DisplayName;
                floorToUpdate.FloorPhotoUrl = floor.FloorPhotoUrl;
                await m_floorsRepo.Update(floorToUpdate);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to add the floor. ({ex.Message})"
                };
            }
        }

        // DELETE api/users/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                var floorToDelete = m_floorsRepo.Get(id);
                if (floorToDelete == null)
                {
                    // we already have a user with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Floor with this ID doesn't exist."
                    };
                }

                await m_floorsRepo.DeleteById(id);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = $"Something went wrong when trying to remove the floor. ({ex.Message})"
                };
            }
        }

        */
    }
}
