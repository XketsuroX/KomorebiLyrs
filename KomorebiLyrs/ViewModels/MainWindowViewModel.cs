using System;
using System.Threading;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace KomorebiLyrs.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    

    [ObservableProperty] private string mainLyrics = "Waiting for music";
    
    private readonly DispatcherTimer _timer;
    private int _count;
    public MainWindowViewModel()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _timer.Tick += Timer_tick;
        
        _timer.Start();
    }

    private void Timer_tick(object? sender, EventArgs e)
    {
        _count++;

        MainLyrics = _count % 2 == 0 ? "誰かの願いが叶うころ" : "あの子が泣いてるよ";
    }

    
}