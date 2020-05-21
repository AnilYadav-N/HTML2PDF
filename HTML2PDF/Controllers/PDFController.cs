using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using System.IO;
using HTML2PDF.Models;

namespace HTML2PDF.Controllers
{
    public class PDFController : ApiController
    {
        // GET: api/PDF
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PDF/5
        public string Get(int id)
        {
            return "Not Implemented";
        }

        // POST: api/PDF
        //public void Post([FromBody]string value)
        //{

        //}

        // POST: api/PDF
        
        public string Post([FromBody]PDFRequest objPDFRequest)
        {
                        
            if (objPDFRequest == null)
            {
                return "Web API Request Exception:- Bad Request";
            }

            string strResult = null;
            try
            {
                if (!objPDFRequest.ValidRequest())
                {
                    return "Web API Request Exception:- Bad Request(Invalid)";
                }
                
                HTML2PDF obj = new HTML2PDF();
                string strHTMLURL = objPDFRequest.HTMLURL;
                string fileName = obj.CreateHTML2PDF(strHTMLURL,objPDFRequest.HTMLContents, objPDFRequest.PageSize);

                if (!string.IsNullOrEmpty(fileName))
                {
                    string file = fileName;
                    if (File.Exists(file))
                    {
                        strResult = fileName;

                        var openFile = File.OpenRead(file);
                        // copy the stream (thanks to http://stackoverflow.com/questions/230128/best-way-to-copy-between-two-stream-instances-c)
                        byte[] buffer = new byte[32768];
                        while (true)
                        {
                            int read = openFile.Read(buffer, 0, buffer.Length);
                            if (read <= 0)
                            {
                                break;
                            }
                            //Response.OutputStream.Write(buffer, 0, read);

                        }
                        openFile.Close();
                        openFile.Dispose();

                        // File.Delete(file);
                    }
                }
                
                return strResult;
            }
            catch (Exception ex)
            {
               return "Web API Request Exception:- " + ex.Message;
            }

        }
        
        // PUT: api/PDF/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PDF/5
        public void Delete(int id)
        {
        }
    }
}
