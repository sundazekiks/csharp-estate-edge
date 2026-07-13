using estate_edge.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;



namespace estate_edge.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IConfiguration configuration;
    public FileController(IConfiguration config)
    {
        configuration = config;
    }

    [HttpGet]
    [Route("test")]
    public ActionResult<object> Test()
    {
        string env = configuration["CustomKey"]!;
        Console.WriteLine(env);
        string name = "Travis";
        return name;
    }

    [HttpPost]
    [Route("upload")]
    public async Task<ActionResult<object>> FileUpload([FromForm] IFormFile file)
    {
        Console.WriteLine($"{file.GetType()} files");
        Console.WriteLine("Uploading");
        DateTime date = DateTime.Now;
  
        string newFilePath = $"{date:yyyy-MM-dd_HH-mm-ss} {file.Name}.csv";


        using (FileStream fs = System.IO.File.Create($"./Data/{newFilePath}"))
        {
            await file.CopyToAsync(fs);
        }

        var f = new FileInfo($"./Data/{newFilePath}");

        Console.WriteLine("Does it exist: " + System.IO.File.Exists($"./Data/{newFilePath}"));

        return new {dir = $"./Data/{newFilePath}"};
        
    }
    [HttpPost]
    [Route("upload/step")]
    public async Task<ActionResult<object>> SendHeaders([FromBody] UploadDir Dir)
    {

        Console.WriteLine(Dir.dir);
        string filePath = Dir.dir;
        List<string?> heads = new List<string?>();


        using (StreamReader sr = System.IO.File.OpenText(filePath))
        {
            string?[] headers = sr.ReadLine().Split(",");

            foreach (string? header in headers) {
                heads.Add(header);
            }
        }

        foreach (string? header in heads )
        {
            Console.Write(header + " ");
        }
        return heads;
    }
}

