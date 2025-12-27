using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using KomorebiLyrs.Services;

namespace KomorebiLyrs.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string artist = "";
    [ObservableProperty] private string fullInfo = "";
    
    private IMediaService _mediaService;
    private readonly IWindowTraitService _windowTraitService;
    
    [ObservableProperty] private bool _isLocked;
    public MainWindowViewModel(IMediaService mediaService, IWindowTraitService windowTraitService)
    {
        _mediaService = mediaService;
        _windowTraitService = windowTraitService;
        
        _mediaService.MediaChanged += OnMediaChanged;
        _mediaService.Start();
    }

    private void OnMediaChanged(object? sender, MediaInfoEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() => UpdateSongInfo(sender, e));
    }
    private void UpdateSongInfo(object? sender, MediaInfoEventArgs e)
    {
        Title = e.Title;
        Artist = e.Artist;
        FullInfo = Title == String.Empty || Artist == String.Empty ? "" : $"{Title} - {Artist}";
    }
}