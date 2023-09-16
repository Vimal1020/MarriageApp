namespace API.DTOs
{
    public class PhotoForApprovalDto
    {
        public string Username { get; set; }
        public int id { get; set; }
        public string Url { get; set; }
        public bool isApproved { get; set; }
    }
}
