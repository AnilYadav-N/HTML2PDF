#!/bin/sh
# This is the command script to convert HTML to PDF for Linux and Mac. 
wkhtmltopdf --page-size A4 --orientation Portrait --margin-left 5 --margin-top 8.5 --margin-right 10 --margin-bottom 8.5 --no-print-media-type ldw33c.html ldw33c.pdf
