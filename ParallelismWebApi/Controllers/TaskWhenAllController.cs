using Microsoft.AspNetCore.Mvc;
using ParallelismWebApi.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace ParallelismWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class TaskWhenAllController : ControllerBase
{
    HttpClient? httpClient;
    public TaskWhenAllController()
    {
        httpClient = new();
    }
    
    [HttpGet(Name = "GetTaskwhenAll/{iterationNumber}")]
    public async Task<ActionResult<string>> ProcessTaskWhenAll(int iterationNumber)
    {
        string text  = "";
   
        string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
        string destinationSequential = Path.Combine(currentDirectory, @"images\result-sequential");
        string destinationParallel = Path.Combine(currentDirectory, @"images\result-simultaneoous");

        text += "Prepare Folders\r\n\r\n";
        PrepareExecution(destinationParallel, destinationSequential);
        text += $"Begin {iterationNumber} ITERATIONS\r\n";

        List<ImageDTO> images = GetImages(iterationNumber);

        //Sequential
        Stopwatch stopwatch = new();
        stopwatch.Start();

        images.ToList().ForEach(async img => await ProcessImage(destinationSequential, img));
                
        double sequentialTime = stopwatch.ElapsedMilliseconds / 1000.0;

        text += $"Sequential - duration in seconds: {sequentialTime}\r\n";

        stopwatch.Restart();

        // Parallel part
        var tasks = images.Select(async image => await ProcessImage(destinationParallel, image));

        await Task.WhenAll(tasks);

        double timeParallel = stopwatch.ElapsedMilliseconds / 1000.0;

        text += $"Parallel - duration in seconds: {timeParallel}\r\n";


        text +="End";

        
        return Ok(text);

    }

     private void PrepareExecution(string destinationParallel,
        string destinationSequential){
        if (!Directory.Exists(destinationParallel)){
            Directory.CreateDirectory(destinationParallel);
        }
        if (!Directory.Exists(destinationSequential)){
            Directory.CreateDirectory(destinationSequential);
        }
        if (!Directory.Exists(destinationParallel)){
            Directory.CreateDirectory(destinationParallel);
        }

        DeleteFiles(destinationParallel);
        DeleteFiles(destinationSequential);  
    }
    private async Task ProcessImage(string directory, ImageDTO image){
        HttpResponseMessage response = await httpClient!.GetAsync(image.URL);
        byte[] content = await response.Content.ReadAsByteArrayAsync();

        Bitmap bitmap;
        using (var ms = new MemoryStream(content)){
            bitmap = new Bitmap(ms);
        }

        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
        string destination = Path.Combine(directory, image.Name);
        bitmap.Save(destination);
    }

    private void DeleteFiles(string directory){
        var files = Directory.EnumerateFiles(directory);
        files.ToList().ForEach(f => System.IO.File.Delete(f));
    }
    private static List<ImageDTO> GetImages(int iterationNumber)
        {
            var images = new List<ImageDTO>();
            for (int i = 0; i < iterationNumber; i++)
            {
                {
                    images.Add(
                    new ImageDTO()
                    {
                        Name = $"Spider-Man Spider-Verse {i}.jpg",
                        URL = "https://m.media-amazon.com/images/M/MV5BMjMwNDkxMTgzOF5BMl5BanBnXkFtZTgwNTkwNTQ3NjM@._V1_UY863_.jpg"
                    });
                    images.Add(

                    new ImageDTO()
                    {
                        Name = $"Spider-Man Far From Home {i}.jpg",
                        URL = "https://m.media-amazon.com/images/M/MV5BMGZlNTY1ZWUtYTMzNC00ZjUyLWE0MjQtMTMxN2E3ODYxMWVmXkEyXkFqcGdeQXVyMDM2NDM2MQ@@._V1_UY863_.jpg"
                    });
                    images.Add(

                    new ImageDTO()
                    {
                        Name = $"Moana {i}.jpg",
                        URL = "https://lumiere-a.akamaihd.net/v1/images/r_moana_header_poststreet_mobile_bd574a31.jpeg?region=0,0,640,480"
                    });
                    images.Add(

                    new ImageDTO()
                    {
                        Name = $"Avengers Infinity War {i}.jpg",
                        URL = "https://img.redbull.com/images/c_crop,x_143,y_0,h_1080,w_1620/c_fill,w_1500,h_1000/q_auto,f_auto/redbullcom/2018/04/23/e4a3d8a5-2c44-480a-b300-1b2b03e205a5/avengers-infinity-war-poster"
                    });
                   
                }
            }

            return images;
        }
}