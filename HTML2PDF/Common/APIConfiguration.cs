namespace HTML2PDF
{
    public static class APIConfiguration
    {

        public static string GetLogPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["APILogFilePath"];
            }
        }


        public static string GetPDFPath
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["PDFPath"];
            }
        }



    }
}