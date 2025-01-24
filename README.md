# PDF Keyword Analysis API
# Link Swagger Documentation : https://analyzepdf.runasp.net/swagger/index.html

## Overview

This project provides an API that analyzes PDF files contained within a ZIP archive. It counts the occurrences of specified keywords within each PDF file and returns the analysis results, including metadata about the PDFs and the total keyword counts across all files.

## Features

- Upload a ZIP file containing multiple PDF files.
- Specify a list of keywords to search for within the PDF files.
- The API analyzes each PDF file, counts the occurrences of each keyword, and returns detailed results.
- The results include metadata for each PDF (title, author, page count) and keyword occurrence counts.

## Technology Stack

- **Backend**: ASP.NET Core (C#)
- **Libraries**:
  - `iTextSharp` for PDF text extraction
  - `System.IO.Compression` for handling ZIP files
- **Frontend**: Angular (for interacting with the API)

## API Endpoints

### `POST /api/files/analyze`

This endpoint accepts a ZIP file and a list of keywords, processes the PDFs inside the ZIP file, and returns the analysis results.

#### Request

- **Content-Type**: `multipart/form-data`
- **Parameters**:
  - `ZipFile`: The ZIP file containing the PDF files to analyze.
  - `Keywords`: A list of keywords to search for within the PDF files.

#### Response

- **200 OK**: Returns a JSON object with the analysis results, including metadata and keyword counts for each PDF file.
- **400 Bad Request**: If the ZIP file or keywords are missing or invalid.
- **500 Internal Server Error**: If an error occurs while processing the request.

#### Example Request

```bash
POST /api/files/analyze
Content-Type: multipart/form-data
{
  "ZipFile": <file>,
  "Keywords": ["Mvc", "Backend", "Task"]
}
