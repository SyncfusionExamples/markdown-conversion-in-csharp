using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using System.Net;

namespace Customize_image_data
{
    class Program
    {
        static void Main(string[] args)
        {
            //Open a file as a stream.
            using (FileStream fileStreamPath = new FileStream(Path.GetFullPath(@"../../../Data/Input.md"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Create a Word document instance.
                using (WordDocument document = new WordDocument())
                {
                    //Hook the event to customize the image while importing Markdown.
                    document.MdImportSettings.ImageNodeVisited += MdImportSettings_ImageNodeVisited;
                    //Open an existing Markdown file.
                    document.Open(fileStreamPath, FormatType.Markdown);
                    //Create a file stream.
                    using (FileStream outputFileStream = new FileStream(Path.GetFullPath(@"../../../MarkdownToHTML.html"), FileMode.Create, FileAccess.ReadWrite))
                    {
                        //Save the Word document in HTML Format.
                        document.Save(outputFileStream, FormatType.Html);
                    }
                }
            }
        }
        /// <summary>
        /// Event handler to import the images during Markdown to Word conversion.
        /// </summary>
        private static void MdImportSettings_ImageNodeVisited(object sender, Syncfusion.Office.Markdown.MdImageNodeVisitedEventArgs args)
        {
            //Retrieve the image from the local machine file path and use it.
            if (args.Uri == "Road-550.png")
                args.ImageStream = new FileStream(Path.GetFullPath(@"../../../Data/" + args.Uri), FileMode.Open);
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
            //Retrieve the image from the base64 string and use it.
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