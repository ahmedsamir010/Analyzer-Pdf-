using Application.Interfaces;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using static Shared.Dtos.KeywordAnalyzer;
namespace Infrastructre.Implementation;
public class FileProcessingService : IFileProcessingService
{
    public Task<FileAnalysis> AnalyzePdfFileAsync(Stream pdfStream, string fileName, List<string> keywords)
    {
        return Task.Run(() =>
        {
            var keywordCounts = keywords.ToDictionary(k => k, k => 0);
            string title , author ;
            int pageCount = 0;

            using (var pdfReader = new PdfReader(pdfStream))
            using (var pdfDocument = new PdfDocument(pdfReader))
            {
                pageCount = pdfDocument.GetNumberOfPages();

                var info = pdfDocument.GetDocumentInfo();
                title = info.GetTitle();
                author = info.GetAuthor();

                for (int i = 1; i <= pageCount; i++)
                {
                    var page = pdfDocument.GetPage(i);
                    var text = PdfTextExtractor.GetTextFromPage(page);

                    foreach (var keyword in keywords)
                    {
                        keywordCounts[keyword] += CountOccurrences(text, keyword);
                    }
                }
            }

            return new FileAnalysis
            {
                FileName = fileName,
                Metadata = new FileMetadata
                {
                    Title = title ?? "Unknown",
                    Author = author ?? "Unknown",
                    PageCount = pageCount
                },
                KeywordCounts = keywordCounts
            };
        });
    }

    public int CountOccurrences(string text, string keyword)
    {
        return text.Split(new[] { keyword }, StringSplitOptions.None).Length - 1;
    }
}



