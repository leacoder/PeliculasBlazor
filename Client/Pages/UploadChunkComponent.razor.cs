using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Razor;

namespace BlazorApp1.Client.Pages
{
    public partial class UploadChunkComponent
    {
        [Inject] ILogger<UploadChunkComponent> Logger { get; set; }
        //[Inject] IWebHostEnvironment Environment { get; set; }

        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 15000;
        private int maxAllowedFiles = 3;
        private bool isLoading;
        private decimal progressPercent;

        private async Task LoadFiles(InputFileChangeEventArgs e)
        {
            isLoading = true;
            loadedFiles.Clear();
            progressPercent = 0;

            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    var trustedFileName = file.Name;
                    //var path = Path.Combine("File",
                    //    "Development", "unsafe_uploads", trustedFileName);

                    var path = Path.Combine("Client\\uploadfiles\\", trustedFileName);
                    string filename = @"C:\folder\" + trustedFileName;
                    await using FileStream writeStream = new(filename, FileMode.Create);
                    using var readStream = file.OpenReadStream(maxFileSize);
                    var bytesRead = 0;
                    var totalRead = 0;
                    var buffer = new byte[1024 * 10];

                    while ((bytesRead = await readStream.ReadAsync(buffer)) != 0)
                    {
                        totalRead += bytesRead;

                        await writeStream.WriteAsync(buffer, 0, bytesRead);

                        progressPercent = Decimal.Divide(totalRead, file.Size);

                        StateHasChanged();
                    }

                    loadedFiles.Add(file);
                }
                catch (Exception ex)
                {
                    Logger.LogError("File: {Filename} Error: {Error}",
                        file.Name, ex.Message);
                }
            }

            isLoading = false;
        }

    }
}
