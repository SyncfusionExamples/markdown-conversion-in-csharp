using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.Pdf;

// Open the Word document file stream.
using (FileStream inputStream = new FileStream(@"../../../Template.md", FileMode.Open, FileAccess.ReadWrite))
{
    // Load an existing Word document.
    using (WordDocument wordDocument = new WordDocument(inputStream, FormatType.Markdown))
    {
        // Create an instance of DocIORenderer.
        using (DocIORenderer renderer = new DocIORenderer())
        {
            // Convert Word document into PDF document.
            using (PdfDocument pdfDocument = renderer.ConvertToPDF(wordDocument))
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

