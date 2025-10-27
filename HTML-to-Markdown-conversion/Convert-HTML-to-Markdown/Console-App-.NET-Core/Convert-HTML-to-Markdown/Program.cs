﻿using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace Convert_HTML_to_Markdown
{
    class Program
    {
        static void Main(string[] args)
        {
            //Open a file as a stream.
            using (FileStream fileStreamPath = new FileStream(Path.GetFullPath(@"input.html"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Load an existing HTML file.
                using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Html))
                {
                    //Create a file stream.
                    using (FileStream outputFileStream = new FileStream(Path.GetFullPath(@"../../../HTMLToMarkdown.md"), FileMode.Create, FileAccess.ReadWrite))
                    {
                        //Save the Word document in Markdown Format.
                        document.Save(outputFileStream, FormatType.Markdown);
                    }
                }
            }
        }
    }
}
