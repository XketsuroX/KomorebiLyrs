#if WINDOWS
using System;
using Windows.Media.Control;

namespace KomorebiLyrs.Services;

public class WindowsMediaService : IMediaService
{
    private GlobalSystemMediaTransportControlsSessionManager? _manager;

    public event EventHandler<MediaInfoEventArgs>? MediaChanged;

    public async void Start()
    {
        try 
        {
            _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            if (_manager != null)
            {
                _manager.CurrentSessionChanged += Manager_CurrentSessionChanged;
                UpdateMediaInfo();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Media API Error: {ex.Message}");
        }
    }

    private void Manager_CurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
    {
        UpdateMediaInfo();
    }

    private async void UpdateMediaInfo()
    {
        var session = _manager?.GetCurrentSession();
        if (session != null)
        {
            var properties = await session.TryGetMediaPropertiesAsync();
            // 觸發事件通知 ViewModel
            MediaChanged?.Invoke(this, new MediaInfoEventArgs 
            { 
                Title = properties.Title, 
                Artist = properties.Artist 
            });
        }
    }
}
#endif