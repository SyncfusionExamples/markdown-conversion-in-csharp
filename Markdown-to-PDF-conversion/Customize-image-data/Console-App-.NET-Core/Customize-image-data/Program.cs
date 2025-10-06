using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;
using System.Net;

namespace Customize_image_data
{
    class Program
    {
        static void Main(string[] args)
        {
            //Open a file as a stream.
            using (FileStream fileStreamPath = new FileStream(@"../../../Input.md", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Create a Word document instance.
                using (WordDocument document = new WordDocument())
                {
                    //Hook the event to customize the image while importing Markdown.
                    document.MdImportSettings.ImageNodeVisited += MdImportSettings_ImageNodeVisited;
                    //Open an existing Markdown file.
                    document.Open(fileStreamPath, FormatType.Markdown);
                    // Create an instance of DocIORenderer.
                    using (DocIORenderer renderer = new DocIORenderer())
                    {
                        // Convert Word document into PDF document.
                        using (PdfDocument pdfDocument = renderer.ConvertToPDF(document))
                        {
                            // Save the PDF file to the file system.
                            using (FileStream outputStream = new FileStream(@"../../../MarkdownToPDF.pdf", FileMode.Create, FileAccess.ReadWrite))
                            {
                                pdfDocument.Save(outputStream);
                            }
                        }
                    }
                }
            }
        }
        private static void MdImportSettings_ImageNodeVisited(object sender, Syncfusion.Office.Markdown.MdImageNodeVisitedEventArgs args)
        {
            //Retrieve the image from the local machine file path and use it.
            if (args.Uri == "Road-550.png")
                args.ImageStream = new FileStream(Path.GetFullPath(@"../../../" + args.Uri), FileMode.Open);
            //Retrieve the image from the website and use it.
            else if (args.Uri.StartsWith("https://"))
            {
                WebClient client = new WebClient();
                //Download the image as a stream.
                byte[] image = client.DownloadData(args.Uri);
                Stream stream = new MemoryStream(image);
                //Set the retrieved image from the input Markdown.
                args.ImageStream = stream;
            }
            //Retrieve the image from the Bbase64 string and use it.
            else if (args.Uri.StartsWith("data:image/"))
            {
                string src = args.Uri;
                int startIndex = src.IndexOf(",");
                src = src.Substring(startIndex + 1);
                byte[] image = System.Convert.FromBase64String(src);
                Stream stream = new MemoryStream(image);
                //Set the retrieved image from the input Markdown.
                args.ImageStream = stream;
            }
        }
    }
}

