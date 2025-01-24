namespace Shared.Dtos;
public class KeywordAnalyzer
{
    public class FileAnalysis
    {
        public string FileName { get; set; } =default!;
        public FileMetadata Metadata { get; set; } = default!;
        public Dictionary<string, int> KeywordCounts { get; set; } = default!;
    }

    public class FileMetadata
    {
        public string Title { get; set; } = default!;
        public string Author { get; set; } = default!;
        public int PageCount { get; set; }
    }

    public class AnalysisResult
    {
        public List<FileAnalysis> Files { get; set; } = default!;
        public Dictionary<string, int> TotalKeywordCounts { get; set; } = default!;
    }

}
