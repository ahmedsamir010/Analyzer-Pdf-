using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Dtos.KeywordAnalyzer;

namespace Application.Interfaces;
public interface IZipFileProcessingService
{
    Task<AnalysisResult> AnalyzeZipFileAsync(AnalyzeZipRequest request);
}

