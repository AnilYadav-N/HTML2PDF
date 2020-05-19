using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using ZXing;

namespace HTML2PDF.Models
{
    public class BarcodeRequest
    {

        [Required]
        public string BarcodeString { get; set; }
        public string NoOfBarcodes { get; set; }
        public string BarcodeType { get; set; }

        public string BarcodeHeader { get; set; }
        public string CIFInformation { get; set; }
        public bool BarcodePerPage { get; set; }

        public bool ValidRequest()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(BarcodeString) && string.IsNullOrWhiteSpace(BarcodeType))
            {
                isValid = false;
            }
            return isValid;
        }

        public int CreateBarcode(string strBarcode) { 

            //initiate BarcodeWriter
            var barcodeWriter = new BarcodeWriter();
            //set barcode format
            barcodeWriter.Format = BarcodeFormat.CODE_128;
            int PageNo = 0;
            string imagefile = null;

            imagefile = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\Barcodes\barcode-";

            // Taking a string 
            String str = strBarcode;

            char[] spearator = { ',' };

            String[] strlist = str.Split(spearator, StringSplitOptions.None);
            int totalBarcode = strlist.Count() - 1;// removed last comma ,
            PageNo = totalBarcode ;

            int i = 1;
            //to create barcode images in template
            foreach (String s in strlist)
            {
                if (string.IsNullOrWhiteSpace(s) )
                {
                    //do nothing
                }
                else
                {
                    barcodeWriter
                    .Write(s)
                    .Save(imagefile + i + ".bmp");

                    i = i +33;
                }
            }

            return PageNo;
        }
            public int CreateBarcode(string strBarcode, string strBarcodeHeader)
        {
            //initiate BarcodeWriter
            var barcodeWriter = new BarcodeWriter();
            //set barcode format
            barcodeWriter.Format = BarcodeFormat.CODE_128;
            int PageNo = 0;
            string imagefile = null;

            imagefile = AppDomain.CurrentDomain.BaseDirectory + @"\Utilities\HTML\Barcodes\barcode-";

            if (BarcodeType.ToUpper() == "TXN")
            {
                // Taking a string 
                String str = strBarcode;

                char[] spearator = { ',' };

                String[] strlist = str.Split(spearator, StringSplitOptions.None);
                int totalBarcode = strlist.Count() - 1;// removed last comma ,
                int mod = totalBarcode % 33;
                if (mod == 0)
                {
                    PageNo = totalBarcode / 33;
                }
                else
                    PageNo = totalBarcode / 33 + 1;

                int i = 1;
                //to create barcode images in template
                foreach (String s in strlist)
                {
                    if (string.IsNullOrWhiteSpace(s) )
                    {
                    //do nothing
                    }
                    else
                    {

                        barcodeWriter
                        .Write(s)
                        .Save(imagefile + i + ".bmp");

                        i++;
                    }
                }
                //to create blank images for remaining barcodes in template of 99 barcodes
                for (; i <= PageNo * 33; i++)
                {
                    CreateHeaderFile("", imagefile + i + ".bmp");
                }
            }
            else if (BarcodeType.ToUpper() == "EOS")
            {
                barcodeWriter
                .Write(strBarcode)
                .Save(imagefile + "EOS.bmp");

            }
            else if (BarcodeType.ToUpper() == "DOC" || BarcodeType.ToUpper() == "APP")
            {
                barcodeWriter
                .Write(strBarcode)
                .Save(imagefile + "DOC.bmp");

                CreateHeaderFile(strBarcodeHeader, "");
            }
            else if (BarcodeType.ToUpper() == "CIF")
            {
                barcodeWriter
                .Write("BE" + strBarcode)
                .Save(imagefile + "CIFBarcode.bmp");

                // Taking a string 
                String strCIFInfo = CIFInformation;

                char[] spearator = { '#' };

                String[] strCIFlist = strCIFInfo.Split(spearator, StringSplitOptions.None);

                int i = 1;
                //to create barcode images in template
                foreach (String s in strCIFlist)
                {
                    CreateHeaderFile(s, imagefile + "CIF" + i + ".bmp");
                    i++;
                }


            }

            return PageNo;
        }

        protected string CreateHeaderFile(string strFileContents, string strFileName)
        {
            string strfile = AppDomain.CurrentDomain.BaseDirectory + @"Utilities\HTML\Barcodes\barcode-Header.bmp";

            if (strFileName.Length > 1) { strfile = strFileName; }

            string textToWrite = strFileContents;

            Bitmap image = new Bitmap(400, 30);
            Graphics g = null;
            try
            {
                g = Graphics.FromImage(image);
                Font f = new Font("Arial", 10, FontStyle.Regular);
                SolidBrush b = new SolidBrush(Color.White);
                g.FillRectangle(b, 0, 0, 400, 30);
                g.DrawString(textToWrite, f, Brushes.Black, 2, 5);

                f.Dispose();
                image.Save(strfile, System.Drawing.Imaging.ImageFormat.Bmp);

            }
            catch (Exception e)
            {
                Console.Write(e.Message.ToString());
            }
            finally
            {
                image.Dispose();
                g.Dispose();
            }


            return strfile;


        }
    }
}