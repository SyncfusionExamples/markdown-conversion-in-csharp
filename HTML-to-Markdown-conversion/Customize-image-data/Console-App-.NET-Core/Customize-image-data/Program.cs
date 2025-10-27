﻿using Syncfusion.DocIO;
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
            using (FileStream fileStreamPath = new FileStream(Path.GetFullPath(@"../../../Data/Input.html"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Create a Word document instance.
                using (WordDocument document = new WordDocument())
                {
                    //Hooks the ImageNodeVisited event to open the image from a specific location
                    document.HTMLImportSettings.ImageNodeVisited += OpenImage;
                    //Open an existing HTML file.
                    document.Open(fileStreamPath, FormatType.Html);
                    //Unhooks the ImageNodeVisited event after loading HTML.
                    document.HTMLImportSettings.ImageNodeVisited -= OpenImage;
                    //Create a file stream.
                    using (FileStream outputFileStream = new FileStream(Path.GetFullPath(@"../../../HTMLToMarkdown.md"), FileMode.Create, FileAccess.ReadWrite))
                    {
                        //Save the Word document in Markdown Format.
                        document.Save(outputFileStream, FormatType.Markdown);
                    }
                }
            }
        }
        /// <summary>
        /// Event handler to import the images during HTML to Word conversion.
        /// </summary>
        private static void OpenImage(object sender, ImageNodeVisitedEventArgs args)
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
                //Set the retrieved image from the input HTML.
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
                //Set the retrieved image from the input HTML.
                args.ImageStream = stream;
            }
        }
    }
}