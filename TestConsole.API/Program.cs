using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;
using System.Net.Http.Headers;
//using System.Net.Http.Formatting;
using System.IO;
using System.Net;

namespace TestConsole.API
{
    public class Class1
    {

        private const string URL = "http://localhost:60920/api/Barcode";

        private static string urlParameters = "?strRequestJSON=10001";

        static void Main(string[] args)
        {
            string strRequestJSON = string.Empty;

            strRequestJSON = @"{""HTMLURL"":""C:\\Users\\960985\\Desktop\\BinderHanko\\NewCoorporateHankoEbinder\\Nucleus.IS.ScanVerification\\PrintBarcode.html"",""PageSize"":""A4""}";
            // strRequestJSON = @"{""HTMLURL"":"""",""PageSize"":""A4"",""HTMLContents"":""Test Page of A4 size""}";

            strRequestJSON = @"{""HTMLURL"":""C:\\Users\\960985\\Desktop\\BinderHanko\\HTML2PDF\\HTML2PDF\\Utilities\\HTML\\TXN.html"",""PageSize"":""A4"",""HTMLContents"":""}";

            strRequestJSON = @"{""BarcodeString"":""IS20200422,IS20200423,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424,IS20200424"",""BarcodeType"":""TXN"",""BarcodePerPage"":""}";

            strRequestJSON = @"{""BarcodeString"":""IS20200422,IS20200423,IS20200424,"",""BarcodeType"":""TXN"",""BarcodePerPage"":""1""}";

            // strRequestJSON = @"{""BarcodeString"":""Hanko Document"",""BarcodeType"":""DOC"",""NoOfBarcodes"":""}";
             //strRequestJSON = @"{""BarcodeString"":""BE20070401022"",""BarcodeType"":""CIF"",""NoOfBarcodes"":"""",""CIFInformation"":""20200520#Test Subject#Shinsei Taro#Shinsei Taro JP#800029112""}";

            webResponse("", URL, strRequestJSON);

          
        }


        private static string webResponse(string _strHeader, string strurl, string RequestJson)
        {
            try
            {
                ASCIIEncoding encoding = new ASCIIEncoding();
                byte[] data = Encoding.UTF8.GetBytes(RequestJson);
                string strResult = string.Empty;
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(strurl);
                webrequest.Method = "POST";
                webrequest.ContentType = "application/json;charset=UTF-8";
                webrequest.ContentLength = data.Length;
                Stream newStream = webrequest.GetRequestStream();
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                WebResponse response = webrequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader sr = new StreamReader(responseStream);
                string s = sr.ReadToEnd();
                return s;
                
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public class DataObject
        {
            public string Name { get; set; }

        }

    }
}
