using HTML2PDF.Models;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace HTML2PDF.Controllers
{
    public class BarcodeController : ApiController
    {

        public static string logPath = null;
        //Log file name is unique for each request
        public static string logFileName = DateTime.Now.ToString("yyyymmdd") + "_" + Guid.NewGuid().ToString().Split(new char[] { '-' })[0] + ".txt";
        public static string PDFDirectory;
        // GET: api/Barcode
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Barcode/5
        public string Get(int id)
        {
            //BarcodeRequest obj = new BarcodeRequest();
            //obj.BarcodeType = "APP";
            //obj.CreateBarcode("NOAH-HL,IS2020042200005,IS2020042200006,IS2020042200007");

            return "value";
        }

        // POST: api/Barcode

        public string Post([FromBody] BarcodeRequest objBarcodeRequest)
        {
            //BarcodeRequest obj = new BarcodeRequest();
            //obj.BarcodeType = "APP";
            //obj.CreateBarcode("IS2020042200004,IS2020042200005,IS2020042200006,IS2020042200007");

            LoadSettingsFromConfig();
            LogRequestData(logFileName, DateTime.Now + " API Configuration loaded");


            if (objBarcodeRequest == null)
            {
                LogRequestData(logFileName, DateTime.Now + " Web API Request Exception:- Bad Request");
                return "Web API Request Exception:- Bad Request";
            }

            string strResult = null;
            try
            {
                if (!objBarcodeRequest.ValidRequest())
                {
                    LogRequestData(logFileName, DateTime.Now + " Web API Request Exception:- Bad Request(Invalid)");
                    return "Web API Request Exception:- Bad Request(Invalid)";
                }
                LogRequestData(logFileName, DateTime.Now + " BarcodeType:- " + objBarcodeRequest.BarcodeType);
                LogRequestData(logFileName, DateTime.Now + " Barcode:- " + objBarcodeRequest.BarcodeString);

                BarcodeRequest objBarcode = new BarcodeRequest();
                objBarcode.BarcodeType = objBarcodeRequest.BarcodeType;
                objBarcode.CIFInformation = objBarcodeRequest.CIFInformation;

                objBarcode.BarcodePerPage = true;
                int pageno=1;

                LogRequestData(logFileName, DateTime.Now + " CreateBarcode() started");
                if (objBarcode.BarcodePerPage)
                {
                    pageno = objBarcode.CreateBarcode(objBarcodeRequest.BarcodeString);
                }
                else
                {
                    pageno = objBarcode.CreateBarcode(objBarcodeRequest.BarcodeString, objBarcodeRequest.BarcodeHeader);
                }
                LogRequestData(logFileName, DateTime.Now + " CreateBarcode() Finished," + " Total Pages:-" + pageno);

                string strHTMLTemplate = "";

                if (objBarcode.BarcodeType.ToUpper() == "TXN")
                    strHTMLTemplate = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\" + objBarcode.BarcodeType + pageno + ".html";
                else if (objBarcode.BarcodeType.ToUpper() == "EOS")
                    strHTMLTemplate = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\" + objBarcode.BarcodeType + ".html";
                else if (objBarcode.BarcodeType.ToUpper() == "APP" || objBarcode.BarcodeType.ToUpper() == "DOC")
                    strHTMLTemplate = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\DOC.html";
                else if (objBarcode.BarcodeType.ToUpper() == "CIF")
                    strHTMLTemplate = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\CIF.html";


                LogRequestData(logFileName, DateTime.Now + " Template Name:- " + strHTMLTemplate);

                //Make request to create PDF from selected HTML template
                LogRequestData(logFileName, DateTime.Now + " Call to CreateHTML2PDF() is to be started");
                HTML2PDF objPDFRequest = new HTML2PDF();
                string fileName = objPDFRequest.CreateHTML2PDF(strHTMLTemplate, "", "");
                strResult = fileName;

                LogRequestData(logFileName, DateTime.Now + " Call to CreateHTML2PDF() Finished");
                if (!string.IsNullOrEmpty(fileName))
                {
                    string file = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\GeneratedPDFs\" + fileName;

                    if (System.IO.File.Exists(file))
                    {
                        LogRequestData(logFileName, DateTime.Now + " Copy file to Big Storage started");
                        System.IO.File.Copy(file, PDFDirectory + fileName);
                        LogRequestData(logFileName, DateTime.Now + " Copy file to Big Storage finished");
                        System.IO.File.Delete(file);
                    }
                }
            }


            catch (Exception ex)
            {
                LogRequestData(logFileName, DateTime.Now + "Web API Request Exception:- " + ex.Message);
                return "Web API Request Exception:- " + ex.Message;
            }


            APIResponse objresponse = new APIResponse();
            objresponse.ResultFilePath = strResult;
            if (string.IsNullOrWhiteSpace(strResult))
                objresponse.ResponseMessage = "NG";
            else
                objresponse.ResponseMessage = "OK";

            System.Web.Script.Serialization.JavaScriptSerializer js = new System.Web.Script.Serialization.JavaScriptSerializer();
            var strResultJSON = js.Serialize(objresponse);

            LogRequestData(logFileName, DateTime.Now + " Serialization completed, ready to send response");
            LogRequestData(logFileName, DateTime.Now + " Result JSON:- " + strResultJSON);

            return strResultJSON;


        }

        /*
        public JsonResult Post([FromBody] BarcodeRequest objBarcodeRequest)
        {
            JsonResult jr = new JsonResult();
            string _res = "";
            try
            {
                _res = "ok";
                   
                if (_res == "ok")
                    jr.Data = "Valid";
                else
                    jr.Data = "Not Valid";
                jr.ContentType = "JSON";
                jr.ContentEncoding = System.Text.Encoding.UTF8;
            }
            catch (Exception ex)
            {
                //Common.Controllers.ErrorLogging.FetchErrorData(ex);
                //Response.Write(ex.Message);
                jr.Data = "Error";
            }
            finally
            {

            }
            jr.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return jr;
        }
        */

        // PUT: api/Barcode/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Barcode/5
        public void Delete(int id)
        {
        }

        /// <summary>
        /// Load settings from configuration file
        /// </summary>
        [System.Web.Http.NonAction]
        public void LoadSettingsFromConfig()
        {
            try
            {
                logPath = APIConfiguration.GetLogPath;
                PDFDirectory = APIConfiguration.GetPDFPath;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Purpose is to log messages in text file. File name is unique for each request
        /// </summary>
        /// <param name="LogName"></param>
        /// <param name="strMessage"></param>
        public static void LogRequestData(string LogName, string strMessage)
        {
            if (logPath.Length > 1 && !System.IO.Directory.Exists(logPath))
                System.IO.Directory.CreateDirectory(logPath);
            else
                logPath = AppDomain.CurrentDomain.BaseDirectory + "\\logs\\";

            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(logPath + "\\" + LogName, true))
            {
                writer.WriteLine(strMessage);
            }

        }

    }
}
