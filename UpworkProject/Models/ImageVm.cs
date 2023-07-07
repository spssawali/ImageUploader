namespace UpworkProject.Models
{
    public class ImageVm
    {
        public string _id { get; set; }
        public string SessionId { get; set; } = default!;
        public IFormFile file { get; set; } = default!;
    }
}
