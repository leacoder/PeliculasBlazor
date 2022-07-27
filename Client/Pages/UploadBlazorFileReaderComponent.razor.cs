using Microsoft.AspNetCore.Components;
using Tewr.Blazor.FileReader;
using System.Net.Http.Headers;
using System.Net;
using System.IO;

namespace BlazorApp1.Client.Pages
{
    public partial class UploadBlazorFileReaderComponent
    {
        [Inject] private IFileReaderService fileReaderService { get; set; }
        private ElementReference inputTypeFileElement;
        private List<File> files;
        //public byte[] Buffer { get; set; }
        public System.Threading.CancellationToken cancellationToken;
        private HttpWebRequest webRequest = null;
        private FileStream fileReader = null;
        private Stream requestStream = null;
        private const int MAX_CHUNK_SIZE = (1024 * 5000);


        public async void ReadFile(string uri, string file)
        {
            byte[] fileData;
            fileReader = new FileStream(file, FileMode.Open, FileAccess.Read);
            webRequest = (HttpWebRequest)WebRequest.Create(uri);
            webRequest.Method = "POST";
            webRequest.ContentLength = fileReader.Length;
            webRequest.Timeout = 600000;
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            webRequest.AllowWriteStreamBuffering = false;
            requestStream = webRequest.GetRequestStream();

            long fileSize = fileReader.Length;
            long remainingBytes = fileSize;
            int numberOfBytesRead = 0, done = 0;

            while (numberOfBytesRead < fileSize)
            {
                SetByteArray(out fileData, remainingBytes);
                done = WriteFileToStream(fileData, requestStream);
                numberOfBytesRead += done;
                remainingBytes -= done;
            }
            fileReader.Close();

            //return true;
        }

        public int WriteFileToStream(byte[] fileData, Stream requestStream)
        {
            int done = fileReader.Read(fileData, 0, fileData.Length);
            requestStream.Write(fileData, 0, fileData.Length);

            return done;
        }

        private void SetByteArray(out byte[] fileData, long bytesLeft)
        {
            fileData = bytesLeft < MAX_CHUNK_SIZE ? new byte[bytesLeft] : new byte[MAX_CHUNK_SIZE];
        }

    
        
    }


    public class File
    {
        public string? Name { get; set; }
    }
}

