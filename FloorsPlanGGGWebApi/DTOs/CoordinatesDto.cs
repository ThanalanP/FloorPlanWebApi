namespace FloorsPlanGGGWebApi.DTOs
{
    public class CoordinatesDto
    {
        public int? Id { get; set; }

        public int? X { get; set; }

        public int? Y { get; set; }

        // Vlad's implementation ID style
        // $"R{FLOOR_ID}_{ROOM_ID}_{COORD_ID}"
        public string ComplexIdBased { get; set; }
    }
}