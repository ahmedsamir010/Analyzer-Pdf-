using API.Errors;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using System.Text.Json;
namespace API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PdfAnalyzerController(IZipFileProcessingService zipFileProcessingService) : ControllerBase
{
    private readonly IZipFileProcessingService _zipFileProcessingService = zipFileProcessingService;
    /// <summary>
    /// Analyzes a ZIP file containing PDF files for keyword occurrences.
    /// 
    /// This API endpoint accepts a ZIP file and a list of keywords. It processes each PDF file in the ZIP archive,
    /// counts the occurrences of each keyword in the PDF files, and returns the analysis results.
    /// </summary>
    /// <param name="request">The request containing the ZIP file and the list of keywords to search for.</param>
    /// <returns>A JSON response containing the analysis results, including keyword counts for each PDF file and total keyword counts.</returns>
    [HttpPost]
    public async Task<IActionResult> AnalyzePdf([FromForm] AnalyzeZipRequest request)
    {
        // Validate the request
        if (request.ZipFile == null || request.ZipFile.Length == 0)
        {
            return BadRequest(new ApiResponse(400, "ZIP file is required."));
        }

        if (request.Keywords == null || request.Keywords.Count == 0)
        {
            return BadRequest(new ApiResponse(400, "Keywords are required. Please provide a valid list of keywords."));
        }

        try
        {
            var result = await _zipFileProcessingService.AnalyzeZipFileAsync(request);

            // Rebuild keyword counts for each file correctly
            foreach (var file in result.Files)
            {
                var formattedKeywordCounts = new Dictionary<string, int>();

                foreach (var keyword in request.Keywords)
                {
                    // Count occurrences of each keyword in the file
                    formattedKeywordCounts[keyword] = file.KeywordCounts.ContainsKey(keyword)
                        ? file.KeywordCounts[keyword]
                        : 0;
                }

                file.KeywordCounts = formattedKeywordCounts;
            }

            // Rebuild total keyword counts correctly
            var formattedTotalKeywordCounts = new Dictionary<string, int>();
            foreach (var keyword in request.Keywords)
            {
                // Count total occurrences of each keyword across all files
                formattedTotalKeywordCounts[keyword] = result.TotalKeywordCounts.ContainsKey(keyword)
                    ? result.TotalKeywordCounts[keyword]
                    : 0;
            }

            result.TotalKeywordCounts = formattedTotalKeywordCounts;

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse(400, ex.Message));
        }
    }

}
