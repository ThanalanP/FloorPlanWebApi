using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FloorsPlanGGGWebApi.DataModels
{
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        public string PhotoUrl { get; set; }

        public int RoomId { get; set; }

        public int Place { get; set; }

        public int? CoordX { get; set; }

        public int? CoordY { get; set; }

        [StringLength(50)]
        public string SpecialCoord { get; set; }

        public virtual Room Room { get; set; }
    }
}
