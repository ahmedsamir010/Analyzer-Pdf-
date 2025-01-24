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
        if (!Path.GetExtension(request.ZipFile.FileName).Equals(".zip", StringComparison.OrdinalIgnoreCase))
        {
            return BadRequest(new ApiResponse(400, "The uploaded file must be a ZIP archive."));
        }

        try
        {
            var result = await _zipFileProcessingService.AnalyzeZipFileAsync(request);

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new ApiResponse(400, ex.Message));
        }
    }

}
