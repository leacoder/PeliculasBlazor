﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlazorApp1.Shared;

namespace BlazorApp1.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FilesaveController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly ILogger<FilesaveController> logger;

        public FilesaveController(IWebHostEnvironment env, ILogger<FilesaveController> logger)
        {
            this.env = env;
            this.logger = logger;
        }

        [HttpPost("AppendFile/{fragment}")]
        public async Task<ActionResult> UploadFileChunk(int fragment, IFormFile file)
        {
            try
            {
                var fileName = @"C:\TEMP\" + file.FileName;

                if (fragment == 0 && System.IO.File.Exists(fileName))
                {
                    System.IO.File.Delete(fileName);
                }
                using (var fileStream = new FileStream(fileName, FileMode.Append, FileAccess.Write, FileShare.None))
                using (var bw = new BinaryWriter(fileStream))
                {
                    await file.CopyToAsync(fileStream);
                }
                return Ok(true);
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception: {0}", exception.Message);
                return Ok(false);
            }
        }

        [HttpPost]
        public async Task<ActionResult<IList<UploadResult>>> PostFile(
            [FromForm] IEnumerable<IFormFile> files)
        {
            var maxAllowedFiles = 3;
            long maxFileSize = 1024 * 2000000;
            var filesProcessed = 0;
            var resourcePath = new Uri($"{Request.Scheme}://{Request.Host}/");
            List<UploadResult> uploadResults = new();

            foreach (var file in files)
            {
                var uploadResult = new UploadResult();
                string trustedFileNameForFileStorage;
                var untrustedFileName = file.FileName;
                uploadResult.FileName = untrustedFileName;
                var trustedFileNameForDisplay =
                    WebUtility.HtmlEncode(untrustedFileName);

                if (filesProcessed < maxAllowedFiles)
                {
                    if (file.Length == 0)
                    {
                        logger.LogInformation("{FileName} length is 0 (Err: 1)",
                            trustedFileNameForDisplay);
                        uploadResult.ErrorCode = 1;
                    }
                    else if (file.Length > maxFileSize)
                    {
                        logger.LogInformation("{FileName} of {Length} bytes is " +
                            "larger than the limit of {Limit} bytes (Err: 2)",
                            trustedFileNameForDisplay, file.Length, maxFileSize);
                        uploadResult.ErrorCode = 2;
                    }
                    else
                    {
                        try
                        {
                            trustedFileNameForFileStorage = untrustedFileName;
                            var path = Path.Combine(env.ContentRootPath,
                                env.EnvironmentName, "unsafe_uploads",
                                trustedFileNameForFileStorage);

                            await using FileStream fs = new(path, FileMode.Create);
                            await file.CopyToAsync(fs);

                            logger.LogInformation("{FileName} saved at {Path}",
                                trustedFileNameForDisplay, path);
                            uploadResult.Uploaded = true;
                            uploadResult.StoredFileName = trustedFileNameForFileStorage;
                        }
                        catch (IOException ex)
                        {
                            logger.LogError("{FileName} error on upload (Err: 3): {Message}",
                                trustedFileNameForDisplay, ex.Message);
                            uploadResult.ErrorCode = 3;
                        }
                    }

                    filesProcessed++;
                }
                else
                {
                    logger.LogInformation("{FileName} not uploaded because the " +
                        "request exceeded the allowed {Count} of files (Err: 4)",
                        trustedFileNameForDisplay, maxAllowedFiles);
                    uploadResult.ErrorCode = 4;
                }

                uploadResults.Add(uploadResult);
            }

            return new CreatedResult(resourcePath, uploadResults);
        }
    
    }
}
