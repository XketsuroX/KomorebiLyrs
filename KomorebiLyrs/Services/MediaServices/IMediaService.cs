using System;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public interface IMediaService
{
    AppSettings.MediaProviderType ProviderType { get; }
    event EventHandler<MediaInfoEventArgs>? MediaChanged;
    void Start();
    
    void Stop();
}

public class MediaInfoEventArgs : EventArgs
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
    
    public string? Album { get; set; }
    
    public string? CoverUrl { get; set; }
    
    public double ProgressMs { get; set; }
    
    public double DurationMs { get; set; }
    
    public bool IsPlaying { get; set; }
}