# KomorebiLyrs

A lightweight and elegant lyric display tool built with **Avalonia UI**, designed for a minimalist and distraction-free music experience.

## Vision & Goal
The primary mission of **KomorebiLyrs** is to provide **high-performance dynamic lyrics** (synchronized scrolling) with a beautiful UI. 

Current focus: Implementing a robust lyrics synchronization engine inspired by modern lyrics display tools like *lyrs*.

## Features
- **Modern UI**: Built with Avalonia UI for a smooth and responsive experience.
- **Multiple Media Providers**:
  - **Windows Media**: Native integration with Windows System Media Transport Controls (SMTC).
  - **Tuna OBS (Recommended)**: Acts as a local server (port `1608`) to receive real-time track metadata and progress from players like *YouTube Music Desktop App*.
- **Click-Through Support**: Enable overlay mode without interfering with your workflow.
- **Customizable**: Adjustable window opacity and minimalist design.

## 🛠️ Roadmap (Upcoming)
- [ ] **Dynamic Lyrics**: Synchronized scrolling based on track progress (LRC format support).
- [ ] **Lyrics Provider**: Auto-fetching lyrics from online APIs (NetEase, QQ Music, etc.).
- [ ] **Rich Styling**: Support for lyric animations, glow effects, and custom fonts.
- [ ] **Cover Art Rendering**: Enhanced visualization of album art.

### Long Term Goal
- [ ] **Multi Platform Support**: Support multiple platform. (Currently only compatibility on Window is being considered)

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Windows 10/11 (Current target OS)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/KomorebiLyrs.git
   ```
2. Open `KomorebiLyrs.sln` in your IDE (Visual Studio / JetBrains Rider).
3. Build and Run.

### Setting up Tuna OBS
1. In KomorebiLyrs settings, select **Tuna** as the Media Provider.
2. Ensure your music player has the **Tuna OBS** plugin enabled.
3. Set the plugin's server address to `http://127.0.0.1:1608`.

## Architecture
- **Avalonia UI**: Cross-platform XAML-based UI framework.
- **Dependency Injection**: Clean service management using `Microsoft.Extensions.DependencyInjection`.
- **Strategy Pattern**: Flexible `IMediaService` architecture to easily switch between different music sources.

## Credits
Inspired by the [lyrs](https://github.com/organization/lyrs) project.

---
*Komorebi (木漏れ日) - Sunlight filtering through the leaves of trees.*
