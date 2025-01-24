using Microsoft.AspNetCore.Http;
using static Shared.Dtos.KeywordAnalyzer;
namespace Application.Interfaces;
public interface IFileProcessingService
{
    Task<FileAnalysis> AnalyzePdfFileAsync(Stream pdfStream, string fileName, List<string> keywords);
    int CountOccurrences(string text, string keyword);
}
