# Create PDF document for barcode label sheet #

We are looking for the solution to create the PDF file to generate browser independent barcode document. The document need to align all barcode images to the specified position accordingly based on the specification of "LDW33C" label sheet (refer to LDW33C_Specification.pdf).

## Available Options ##

There are several options to create PDF file but they are mostly need to specify the position to allocate the images by hand and write the position parameters in the source code by ourselves. In the Java world, we have Jasper Report and iReport to create the report includint PDF format and it is easy to design the report by iReport.

However, we do not have those sort of tools in the .Net world. So, we can chose one of the following options to create PDF document for barcode generation.

1. Use commercial version of PDF tools that has nice desiner tools, and create the document template by designer tool.
1. Use OSS PDF library like PDFSharp and write the position paramter in source code to place the contents by hand.
1. Use wkhtmltopdf or HtmlRender.PDFSharp to convert HTML convert the HTML to PDF.

In case of No.1 and 2, we do not need any further research. We simply just do it. However, No.1 requires license fee, No.2 need much work effort. We need much cost anyway.

## Configuration ##

So, there is No.3 option. We can create simple HTML page that has 3 columns and 11 rows per page. It is the template to allocate all barcode images. After embedding the barcode images in the page ("img" tag as you know), we simply convert the HTML to PDF.

![](./doc_resources/ConversionFlow.png)

We have 2 options to convert the HTML to PDF. The first one is the command line tool and we can simply call the command line from the program. The second one is the DLL. We need to import the library in our code and use the API to convert.

## Limitations ##

I have no Windows development environment now. So, I just tried the command line version of convesion. And it works fine in my Mac environment even though it has some limitation.

1. CSS does not work as expected in my Mac environment.
1. Only supported HTML4.1, not HTML5

However, we only need the simple table layout and no need to write Japanese text inside. So, those above limitation seems OK.

## Performance ##

The current version takes around 14 sec. to display 99 barcodes (3 pages) on the screen. Hence, wkhtmltopdf need only 3 sec. to convert 99 barcode images to PDF. I am not sure how long does it take to generate 99 barcode images but I hope this performace is not so bad compare to the current version.

## Materisls ##
This folder contains the following materials.

1. LDW33C_Specification.pdf<br/>
This is the specification for the label sheet that ROD scan team now using.

1. html2pdf.sh<br/>
This is the shell script to run the wkhtmltopdf command line tool.

1. ldw33c.css<br/>
This is the css file for the ldw33c.html but it does not work as expected.

1. ldw33c.pdf<br/>
This is the PDF file that is converted from the HTML file (ldw33c.html)

1. wkhtmltopdf-help.txt<br/>
The manual for the wkhtmltopdf command line tool.

## URL and license ##
wkhtmltopdf : LGPLv3<br/>
https://wkhtmltopdf.org

htmlrenderer.pdfsharp : BSD 3-Clause "New" or "Revised" License
https://github.com/ArthurHub/HTML-Renderer

