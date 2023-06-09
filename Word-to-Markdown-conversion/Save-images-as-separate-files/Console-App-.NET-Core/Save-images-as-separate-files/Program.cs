using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;

namespace Save_images_as_separate_files
{
    class Program
    {
		static int imageCount = 0;
        static void Main(string[] args)
        {
            //Open a file as a stream.
            using (FileStream fileStreamPath = new FileStream(Path.GetFullPath(@"../../../Input.docx"), FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //Load an existing Word document.
                using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Docx))
                {
                    //Hook the event to customize the image. 
                    document.SaveOptions.ImageNodeVisited += SaveImage;
                    //Create a file stream.
                    using (FileStream outputFileStream = new FileStream(Path.GetFullPath(@"../../../WordToMarkdown.md"), FileMode.Create, FileAccess.ReadWrite))
                    {                       
                        //Save a Markdown file to the file stream.
                        document.Save(outputFileStream, FormatType.Markdown);
                    }
                }
            }
        }

        /// <summary>
        /// Event handler to save the images in an external folder during Word to Markdown conversion.
        /// </summary>
        static void SaveImage(object sender, ImageNodeVisitedEventArgs args)
        {
            //Image path to save the images in an external folder.
            string imagepath = @"E:\WordToMD\Image_" + imageCount + ".png";
            //Save the image stream as a file. 
            using (FileStream fileStreamOutput = File.Create(imagepath))
                args.ImageStream.CopyTo(fileStreamOutput);
            //Set the image URI to be used in the output markdown.
            args.Uri = imagepath;
            imageCount++;
        }
    }
}
