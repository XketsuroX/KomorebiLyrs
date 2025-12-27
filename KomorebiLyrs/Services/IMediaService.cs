using System;

namespace KomorebiLyrs.Services;

public interface IMediaService
{
    event EventHandler<MediaInfoEventArgs>? MediaChanged;
    void Start();
}

public class MediaInfoEventArgs : EventArgs
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = string.Empty;
}