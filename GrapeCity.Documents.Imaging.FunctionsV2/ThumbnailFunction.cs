using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace GrapeCity.Documents.Imaging.FunctionsV2
{
    public static class ThumbnailFunction
    {
        [FunctionName("ThumbnailFunction")]
        public static void Run(
            [BlobTrigger("images/{name}", Connection = "AzureWebJobsStorage")]byte[] myBlob,
            [Blob("thumbs/{name}", Connection = "AzureWebJobsStorage")] out byte[] myOutputBlob,
            string name,
            ILogger log)
        {
            log.LogInformation($"ThumbnailFunction function started for \n Name:{name} \n Size: {myBlob.Length} Bytes");
            myOutputBlob = GcImagingOperations.GetConvertedImage(myBlob);
            log.LogInformation($"ThumbnailFunction function finished");
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[32768];
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, read);
            }
        }
    }
}
