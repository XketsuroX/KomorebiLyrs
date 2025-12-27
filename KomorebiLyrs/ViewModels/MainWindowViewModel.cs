using System;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;

using KomorebiLyrs.Services;

namespace KomorebiLyrs.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private string title = "";
    [ObservableProperty] private string artist = "";
    [ObservableProperty] private string fullInfo = "";
    
    private IMediaService _mediaService;
    
    public MainWindowViewModel(IMediaService mediaService)
    {
        _mediaService = mediaService;
        _mediaService.MediaChanged += OnMediaChanged;
        _mediaService.Start();
    
    }

    private void OnMediaChanged(object? sender, MediaInfoEventArgs e)
    {
        Title = e.Title;
        Artist = e.Artist;
        FullInfo = Title == String.Empty || Artist == String.Empty ? "" : $"{Title} - {Artist}";
    }

 
}