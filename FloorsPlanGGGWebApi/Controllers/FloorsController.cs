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
    public class FloorsController : ApiController
    {
        private readonly GenericRepository<Room> m_roomsRepo;
        private readonly GenericRepository<Floor> m_floorsRepo;

        public FloorsController()
        {
            UnitOfWork uok = new UnitOfWork();
            m_roomsRepo = new GenericRepository<Room>(uok);
            m_floorsRepo = new GenericRepository<Floor>(uok);
        }

        // GET api/floors
        public IEnumerable<FloorDto> Get()
        {
            try
            {
                var floorsList = m_floorsRepo.GetAll();
                return floorsList.Select(x => new FloorDto
                {
                    Id = x.Id,
                    FloorPhotoUrl = x.FloorPhotoUrl,
                    DisplayName = x.DisplayName
                });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // GET api/floors/5
        public FloorDto Get(int id)
        {
            try
            {
                var floorsModel = m_floorsRepo.Get(id);
                return new FloorDto
                {
                    Id = floorsModel.Id,
                    FloorPhotoUrl = floorsModel.FloorPhotoUrl,
                    DisplayName = floorsModel.DisplayName
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        // POST api/floors
        public async Task<HttpResponseMessage> Post([FromBody]FloorDto floor)
        {
            try
            {
                var floorsModelAlreadyExists = m_floorsRepo.Get(floor.Id) != null;
                if (floorsModelAlreadyExists)
                {
                    // we already have a floor with this ID
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                    {
                        ReasonPhrase = "Floor with this ID already exists."
                    };
                }

                var roomsOnFloor = m_roomsRepo.GetAll().Where(x => x.FloorId == floor.Id);
                await m_floorsRepo.Add(new Floor { Id = floor.Id, FloorPhotoUrl = floor.FloorPhotoUrl, DisplayName = floor.DisplayName, Rooms = roomsOnFloor.ToList() });
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

        // PUT api/floors/5
        public async Task<HttpResponseMessage> Put(int id, [FromBody]FloorDto floor)
        {
            try
            {
                var floorToUpdate = m_floorsRepo.Get(floor.Id);
                if (floorToUpdate == null)
                {
                    // we already have a floor with this ID
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

        // DELETE api/floors/5
        public async Task<HttpResponseMessage> Delete(int id)
        {
            try
            {
                var floorToDelete = m_floorsRepo.Get(id);
                if (floorToDelete == null)
                {
                    // we already have a floor with this ID
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
    }
}
