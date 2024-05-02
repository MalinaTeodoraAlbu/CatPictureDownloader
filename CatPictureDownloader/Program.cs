using System;
using System.IO;
using System.Net.Http;

namespace CatPictureDownloader
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string urlAPI = "https://cataas.com/cat";
            string textOption = "/says/";
            string colorOptions = "?fontSize=50&fontColor=white";

            string fileName = "";
            string text = "";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-o" && i + 1 < args.Length)
                {
                    fileName = args[i + 1];
                    i++;
                }
                else if (args[i] == "-t" && i + 1 < args.Length)
                {
                    text = args[i + 1];
                    i++;
                }
            }

            if (!string.IsNullOrEmpty(text))
            {
                urlAPI += textOption + text + colorOptions;
            }

            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    Console.WriteLine("Error: Filename is empty");
                    return;
                }
                else if (!Path.GetExtension(fileName).Equals(".jpg", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Error: Filename must have the extension '.jpg'.");
                    return;
                }

                using (var client = new HttpClient())
                {
                    var res = client.GetAsync(urlAPI).Result;
                    if (res.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var content = res.Content.ReadAsByteArrayAsync().Result;
                        File.WriteAllBytes(fileName, content);
                        Console.WriteLine("Image downloaded successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Failed to retrieve the image. Status code: " + res.StatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);

            }
        }
    }
}
