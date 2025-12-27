using System;

namespace KomorebiLyrs.Services;

public class DummyMediaService : IMediaService
{
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;
    public void Start()
    {
    }
}