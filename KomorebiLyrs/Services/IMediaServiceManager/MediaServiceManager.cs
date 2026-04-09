using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using KomorebiLyrs.Models;

namespace KomorebiLyrs.Services;

public class MediaServiceManager: IMediaServiceManager
{
    private readonly SettingService _settingService;
    private readonly IServiceProvider _serviceProvider;
    private IMediaService? _currentStrategy;

    public AppSettings.MediaProviderType CurrentProvider => _currentStrategy?.ProviderType ?? AppSettings.MediaProviderType.Dummy;
    public event EventHandler<MediaInfoEventArgs>? MediaChanged;

    // Inject IServiceProvider to resolve keyed services
    public MediaServiceManager(SettingService settingService, IServiceProvider serviceProvider)
    {
        _settingService = settingService;
        _serviceProvider = serviceProvider;
        _settingService.SettingsChanged += OnSettingsChanged;
        
        // Initialize the first strategy based on current settings
        UpdateStrategy(_settingService.GetSettings());
    }

    private void OnSettingsChanged(object? sender, AppSettings newSettings)
    {
        UpdateStrategy(newSettings);
    }

    private void UpdateStrategy(AppSettings settings)
    {
        // Resolve the strategy that matches the current settings
        var newStrategy = _serviceProvider.GetKeyedService<IMediaService>(settings.MediaProvider) 
                          ?? _serviceProvider.GetKeyedService<IMediaService>(AppSettings.MediaProviderType.Dummy);

        // If the strategy instance hasn't changed, avoid re-subscribing and re-starting
        if (ReferenceEquals(newStrategy, _currentStrategy))
        {
            return;
        }

        // Unsubscribe from the old strategy to prevent memory leaks
        if (_currentStrategy != null)
        {
            _currentStrategy.MediaChanged -= OnStrategyMediaChanged;
            _currentStrategy.Stop();
        }

        _currentStrategy = newStrategy;

        // Subscribe to and start the new strategy
        if (_currentStrategy != null)
        {
            _currentStrategy.MediaChanged += OnStrategyMediaChanged;
            _currentStrategy.Start();
        }
    }

    // Bubble up the event from the underlying strategy to whoever is listening to the Manager
    private void OnStrategyMediaChanged(object? sender, MediaInfoEventArgs e)
    {
        MediaChanged?.Invoke(this, e);
    }

    public void Start()
    {
        _currentStrategy?.Start();
    }
}