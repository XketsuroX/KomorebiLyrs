#if WINDOWS
using System;
using Windows.Media.Control;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class WindowsMediaService : IMediaService
{
    public AppSettings.MediaProviderType ProviderType => AppSettings.MediaProviderType.Windows;
    
    private GlobalSystemMediaTransportControlsSessionManager? _manager;

    private readonly object _locker = new ();
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;

    public async void Start()
    {
        try 
        {
            _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            lock (_locker){
                if (_manager != null) {
                        _manager.CurrentSessionChanged += Manager_CurrentSessionChanged;
                        UpdateMediaInfo();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Media API Error: {ex.Message}");
        }
    }

    public void Stop()
    {
        lock (_locker)
        {
            if (_manager != null)
            {
                _manager.CurrentSessionChanged -= Manager_CurrentSessionChanged;
                _manager = null;
            }
        }
    }

    private void Manager_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
    {
        UpdateMediaInfo();
    }

    private async void UpdateMediaInfo()
    {
        GlobalSystemMediaTransportControlsSessionManager? manager;
        lock (_locker)
        {
            manager = _manager;
        }
        
        if(manager is not null){
            var session = manager?.GetCurrentSession();
            if (session != null)
            {
                var properties = await session.TryGetMediaPropertiesAsync();
                // invoke event to ViewModel
                MediaChanged?.Invoke(this, new MediaInfoEventArgs
                {
                    Title = properties.Title,
                    Artist = properties.Artist
                });
            }
        }
    }
}
#endif