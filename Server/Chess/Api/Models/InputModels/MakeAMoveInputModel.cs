using System.ComponentModel.DataAnnotations;

namespace Api.Models.InputModels
{
    public class MakeAMoveInputModel
    {
        [Required]
        public Guid BoardId { get; set; }
        [Required]
        [Range(0,7)]
        public int StartRow { get; set; }
        [Required]
        [Range(0, 7)]
        public int StartCol { get; set; }
        [Required]
        [Range(0, 7)]
        public int EndRow { get; set; }
        [Required]
        [Range(0, 7)]
        public int EndCol { get; set; }
    }
}
