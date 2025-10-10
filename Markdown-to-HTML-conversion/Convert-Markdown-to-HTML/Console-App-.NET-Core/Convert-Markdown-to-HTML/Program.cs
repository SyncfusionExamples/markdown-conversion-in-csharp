using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;

namespace Convert_Markdown_to_Word
{
    class Program
    {
        static void Main(string[] args)
        {
            //Open a file as a stream.
            using (FileStream fileStreamPath = new FileStream(Path.GetFullPath(@"../../../Input.md"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Load an existing Markdown file.
                using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Markdown))
                {
                    //Create a file stream.
                    using (FileStream outputFileStream = new FileStream(Path.GetFullPath(@"../../../MarkdownToHTML.html"), FileMode.Create, FileAccess.ReadWrite))
                    {
                        //Save the Word document in HTML Format.
                        document.Save(outputFileStream, FormatType.Html);
                    }
                }
            }
        }
    }
}
