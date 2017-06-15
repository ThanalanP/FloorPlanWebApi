namespace FloorsPlanGGGWebApi.DTOs
{
    public class UserLocationDetailsDto
    {
        public UserDto User { get; set; }

        public RoomDto Room { get; set; }

        public FloorDto Floor { get; set; }

        public CoordinatesDto Coordinates { get; set; }

    }
}