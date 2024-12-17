using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;


namespace BlazorEcommerce.Client.Utilities
{
    public static class ImageHelper
    {
        /// <summary>
        /// fills in extra space with transparent background, to make any image a square image
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public static byte[]? MakeSquare(this byte[] imageBytes)
        {
            // Validate input
            if (imageBytes == null || imageBytes.Length == 0)
                return null;

            using (var originalImage = Image.Load(imageBytes))
            {
                // Determine the size of the square image (max of width or height)
                int squareSize = Math.Max(originalImage.Width, originalImage.Height);

                // Create a new image with transparent background
                using (var squareImage = new Image<Rgba32>(squareSize, squareSize, Color.Transparent))
                {
                    // Calculate positioning to center the original image
                    int x = (squareSize - originalImage.Width) / 2;
                    int y = (squareSize - originalImage.Height) / 2;

                    // Mutate the image by drawing the original image
                    squareImage.Mutate(ctx => ctx
                        .DrawImage(originalImage, new Point(x, y), 1f)
                    );

                    // Save to a memory stream
                    using (var outputMs = new MemoryStream())
                    {
                        squareImage.Save(outputMs, new PngEncoder());
                        return outputMs.ToArray();
                    }
                }
            }
        }
    }
}
