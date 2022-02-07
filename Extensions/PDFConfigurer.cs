using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PolicyUploader.Extensions
{
    public static class PDFConfigurer
    {
        public static string ExtractPdf(this HttpPostedFileBase file,string path)
        {
            StringBuilder sb = new StringBuilder();
            using (PdfReader reader = new PdfReader(path))
            {
                ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

                for (int page = 0; page < reader.NumberOfPages; page++)
                {
                    string text = PdfTextExtractor.GetTextFromPage(reader, page + 1, strategy);
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        sb.Append(Encoding.UTF8.GetString(Encoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(text))));
                    }
                }
            }
            return sb.ToString();
        }

        public static List<string> GetLinesBySearchTexts(this string content,params string[] searchTexts)
        {
            List<string> sentences = new List<string>();
            if (string.IsNullOrEmpty(content))
                throw new NullReferenceException("Content cannot be empty.");
            List<string> lines = content.Split('\n').ToList();

            foreach (var text in searchTexts)
            {
                string line = lines.Where(a => a.Contains(text)).FirstOrDefault();
                sentences.Add(line);
            }
            
            return sentences;
        }
    }
}