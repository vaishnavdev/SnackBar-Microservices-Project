namespace SnackBar.Services.ProductAPI.Model.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } 
        public IList<string> Errors { get; set; }
    }
}
