using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class TunaMediaService: IMediaService, IDisposable
{
    public AppSettings.MediaProviderType ProviderType => AppSettings.MediaProviderType.Tuna;
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;

    private HttpListener? _listener;
    private bool _isRunning;
    private Task? _listenTask;

    public void Start()
    {
        if(!_isRunning){
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://127.0.0.1:1608/");
            try{            
                _listener.Start();
                _isRunning = true;

                _listenTask = Task.Run(HandleRequests);
                Console.WriteLine("Listening... on port 1608");
            }
            catch(Exception ex){
                Console.WriteLine($"Failed to start TunaMediaService: {ex.Message}");
            }

        }
    }

    public void Stop()
    {
        if(_isRunning && _listener != null){
            _isRunning = false;
            _listener.Stop();
            try
            {
                _listenTask?.Wait(TimeSpan.FromSeconds(1));
            }
            catch
            {}
            _listener.Close();
            _listener = null;
            Console.WriteLine("TunaMediaService stopped.");
        }
    }

    private async Task HandleRequests()
    {
        while (_isRunning && _listenTask != null)
        {
            try
            {
                var context = await _listener.GetContextAsync();
                
                _ = Task.Run(async () => await ProcessRequest(context));
            }
            catch (HttpListenerException) when(!_isRunning)
            { }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling request: {ex.Message}");
            }
        }
    }

    private async Task ProcessRequest(HttpListenerContext context)
    {
        Console.WriteLine($"Received request: {context.Request.HttpMethod} {context.Request.Url}");
        try
        {
            if(context.Request.HttpMethod == "POST"){
                using var reader = new System.IO.StreamReader(context.Request.InputStream);
                var body = await reader.ReadToEndAsync();
                
                var tunaData = System.Text.Json.JsonSerializer.Deserialize<TunaData>(body);
                
                if(tunaData?.Data != null){
                    var args = new MediaInfoEventArgs
                    {
                        Title = tunaData.Data.Title,
                        Artist = tunaData.Data.Artists != null ? string.Join(", ", tunaData.Data.Artists) : "Unknown Artist",
                        Album = "",
                        CoverUrl = tunaData.Data.CoverUrl,
                        ProgressMs = tunaData.Data.Progress,
                        DurationMs = tunaData.Data.Duration,
                        IsPlaying = tunaData.Data.Status == "playing"
                    };
                    MediaChanged?.Invoke(this, args);
                    Console.WriteLine($"Received media info: {tunaData.Data.Title} by {tunaData.Data.Artists} progress:  {tunaData.Data.Progress} duration: {tunaData.Data.Duration}");
                }
            }
            context.Response.StatusCode = (int)HttpStatusCode.OK;
            context.Response.Close();
        }
        catch(Exception ex){
            Console.WriteLine($"Error handling request: {ex.Message}");
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.Close();
        }
    }

    public void Dispose()
    {
        Stop();
    }
}