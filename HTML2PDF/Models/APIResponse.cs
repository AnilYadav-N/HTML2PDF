namespace HTML2PDF.Models
{
    public class ApiResponse
    {
        public string ResponseMessage { get; set; }

        public string ResultFilePath { get; set; }

       
    }

    public class ApiResponseMessage
    {
        public string ERR_CD { get; set; }
        public string ERR_MSG { get; set; }
    }
}