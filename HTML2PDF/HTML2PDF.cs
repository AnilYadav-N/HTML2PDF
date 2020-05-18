using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Web.UI;

namespace HTML2PDF
{


    public partial class HTML2PDF : Page
    {
        public string CreateHTML2PDF(string strHTMLURL, string strHTMLContents, string strPageSize)
        {
            if (string.IsNullOrEmpty(strHTMLURL) && !string.IsNullOrEmpty(strHTMLContents))
            {
                strHTMLURL = CreateHTMLFile(strHTMLContents);
            }
            string strfileNamePDF = WKHtmlToPdf(strHTMLURL);

            if (!string.IsNullOrEmpty(strfileNamePDF))
            {
                string file = Server.MapPath("~\\utilities\\GeneratedPDFs\\" + strfileNamePDF);
                //if (File.Exists(file))
                //    // strfileNamePDF = file;
                //    File.Copy(file, @"\\SSOST8768\BarcodePDF$\"+ strfileNamePDF);
            }

            return strfileNamePDF;

            //if (!string.IsNullOrEmpty(fileName))
            //{
            //    string file = Server.MapPath("~\\utilities\\GeneratedPDFs\\" + fileName);
            //    if (File.Exists(file))
            //    {
            //        var openFile = File.OpenRead(file);
            //        // copy the stream (thanks to http://stackoverflow.com/questions/230128/best-way-to-copy-between-two-stream-instances-c)
            //        byte[] buffer = new byte[32768];
            //        while (true)
            //        {
            //            int read = openFile.Read(buffer, 0, buffer.Length);
            //            if (read <= 0)
            //            {
            //                break;
            //            }
            //            Response.OutputStream.Write(buffer, 0, read);
            //        }
            //        openFile.Close();
            //        openFile.Dispose();

            //        File.Delete(file);
            //    }
            //}

        }

        public string WKHtmlToPdf(string Url)
        {
            var p = new Process();

            string switches = "";
            //switches += "--print-media-type ";
            // switches += "--margin-top 10mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm ";
            switches += "--page-size A4 ";
            // waits for a javascript redirect it there is one
            ////switches += "--redirect-delay 100";

            // basically set a filename and prepends a GUID to it to keep it unique
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            LogRequestData("", "Before Process for PDF generation");
            string strExecutableName = Server.MapPath("~\\utilities\\PDF\\wkhtmltopdf.exe");
            var startInfo = new ProcessStartInfo
            {
                FileName = Server.MapPath("~\\utilities\\PDF\\wkhtmltopdf.exe"),
                Arguments = switches + " " + Url + " \"" +
                                            "../GeneratedPDFs/" + fileName
                                            + "\"",
                UseShellExecute = false, // needs to be false in order to redirect output
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true, // redirect all 3, as it should be all 3 or none
                WorkingDirectory = Server.MapPath("~\\utilities\\PDF")

            };
            LogRequestData("", "wkhtmltopdf path is" + strExecutableName);
            LogRequestData("", "Before Process.start() for PDF generation");

            p.StartInfo = startInfo;
            p.Start();


            // doesn't work correctly...
            // read the output here...
            // string output = p.StandardOutput.ReadToEnd();

            //  wait n milliseconds for exit (as after exit, it can't read the output)
            p.WaitForExit(60000);

            // read the exit code, close process
            int returnCode = p.ExitCode;
            p.Close();
            LogRequestData("", "PDF generation exit code: " + returnCode);
            // if 0, it worked
            return (returnCode == 0) ? fileName : null;
        }

        public static void LogRequestData(string LogName, string strMessage)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";
            LogName = "abc.txt";
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logPath + "\\" + LogName, true))
            {
                writer.WriteLine(strMessage);
            }

        }

        /// <summary>
        /// Create HTML file based on passed contents
        /// </summary>
        /// <param name="strHTMLContents"></param>
        /// <returns></returns>
        protected string CreateHTMLFile(string strHTMLContents)
        {

            //string path = @"C:\Users\960985\Desktop\BinderHanko\HTML2PDF\HTML2PDF\Utilities\HTML\MyTest.html";
            string strHTMLfile = Server.MapPath("~\\utilities\\HTML\\" + Guid.NewGuid().ToString() + ".html");

            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(strHTMLfile))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(strHTMLContents);
                    // Add some information to the file.
                    fs.Write(info, 0, info.Length);
                }

                // Open the stream and read it back.
                using (StreamReader sr = File.OpenText(strHTMLfile))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine(ex.ToString());
                throw;
            }

            return strHTMLfile;

        }

    }


}