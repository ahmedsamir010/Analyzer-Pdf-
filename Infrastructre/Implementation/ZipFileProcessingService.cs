using Application.Interfaces;
using Shared.Dtos;
using System.IO.Compression;
using static Shared.Dtos.KeywordAnalyzer;

namespace Infrastructre.Implementation;
public class ZipFileProcessingService(IFileProcessingService fileProcessingService) : IZipFileProcessingService
{
    private readonly IFileProcessingService _fileProcessingService = fileProcessingService;
    public async Task<AnalysisResult> AnalyzeZipFileAsync(AnalyzeZipRequest request)
    {
        var result = new AnalysisResult
        {
            Files = [],
            TotalKeywordCounts = request.Keywords.ToDictionary(k => k, k => 0)
        };
        using var zipStream = request.ZipFile.OpenReadStream();
        using var archive = new ZipArchive(zipStream);

        foreach (var entry in archive.Entries)
        {
            if (!entry.FullName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                continue;

            var fileAnalysis = await ProcessPdfFileAsync(entry, request.Keywords);
            result.Files.Add(fileAnalysis);

            foreach (var keyword in request.Keywords)
            {
                result.TotalKeywordCounts[keyword] += fileAnalysis.KeywordCounts[keyword];
            }
        }

        return result;
    }
    private async Task<FileAnalysis> ProcessPdfFileAsync(ZipArchiveEntry entry, List<string> keywords)
    {
        using var pdfStream = entry.Open();
        using var memoryStream = new MemoryStream();
        await pdfStream.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return await _fileProcessingService.AnalyzePdfFileAsync(memoryStream, entry.Name, keywords);
    }
}

