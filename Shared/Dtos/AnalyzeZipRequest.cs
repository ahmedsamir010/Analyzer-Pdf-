
using Microsoft.AspNetCore.Http;
namespace Shared.Dtos;

using System.ComponentModel.DataAnnotations;
public class AnalyzeZipRequest
{
    [Required(ErrorMessage = "ZIP file is required.")]
    [DataType(DataType.Upload)]
    public IFormFile ZipFile { get; set; } = default!;

    [Required(ErrorMessage = "Keywords are required.")]
    [MinLength(1, ErrorMessage = "At least one keyword is required.")]
    public List<string> Keywords { get; set; } = default!;
}
