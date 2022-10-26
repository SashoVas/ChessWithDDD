namespace Api.Models.InputModels
{
    public class MakeAMoveInputModel
    {
        public Guid BoardId { get; set; }
        public int StartRow { get; set; }
        public int StartCol { get; set; }
        public int EndRow { get; set; }
        public int EndCol { get; set; }
    }
}
