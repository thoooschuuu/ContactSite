using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;

namespace TSITSolutions.ImageResizer;

public class ImageUploaded
{
    [FunctionName("ResizeImage")]
    public void Run([BlobTrigger("originals/{name}", Connection = "")]Stream myBlob, string blobName, 
        [Blob("results-large/{name}", FileAccess.Write)] Stream imageLarge,
        [Blob("results-middle/{name}", FileAccess.Write)] Stream imageMiddle,
        [Blob("results-small/{name}", FileAccess.Write)] Stream imageSmall,
        [Blob("results-extra-small/{name}", FileAccess.Write)] Stream imageExtraSmall,
        ILogger log)
    {
        log.LogInformation("C# Blob trigger function Processed blob\\n Name:{BlobNamee} \\n Size:{BlobLengthth} Bytes", blobName, myBlob.Length);
        
        using var image = Image.Load(myBlob, out var format);
        Resize(image, 2, imageLarge, format);
        Resize(image, 4, imageMiddle, format);
        Resize(image, 8, imageSmall, format);
        Resize(image, 16, imageExtraSmall, format);
    }

    private static void Resize(Image originalImage, int dimension, Stream output, IImageFormat format)
    {
        using var clonedImage = originalImage.Clone(x => x.Resize(originalImage.Width / dimension, originalImage.Height / dimension, KnownResamplers.Bicubic));
        clonedImage.Save(output, format);
    }
}