using System;
using System.Threading.Tasks;

namespace KomorebiLyrs.Services;

public interface IMediaService
{
    event EventHandler<string> SongChanged;
    Task InitializeAsync();
}