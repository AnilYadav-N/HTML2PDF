using System.ComponentModel.DataAnnotations;


namespace HTML2PDF.Models
{
    public class PDFRequest
    {

        [Required]
        public string HTMLURL { get; set; }
        public string PageSize { get; set; }
        public string HTMLContents { get; set; }

        public bool ValidRequest()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(HTMLURL) && string.IsNullOrWhiteSpace(HTMLContents))
            {
                isValid = false;
            }
            return isValid;
        }

    }
}