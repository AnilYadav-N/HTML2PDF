namespace HTML2PDF.Models
{
    public class APIResponse
    {
        public string ResponseMessage { get; set; }

        public string ResultFilePath { get; set; }

        //public APIResponseMessage Error { get; set; }
    }

    public class APIResponseMessage
    {
        public string ERR_CD { get; set; }
        public string ERR_MSG { get; set; }
    }
}