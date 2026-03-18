# MusicBee Browser2 插件开发经验总结

## 项目概述
将原有的基于 IE WebBrowser 控件的 MusicBee 浏览器插件现代化，使用 WebView2 替代传统 IE 内核。
refer_files\browser.cs 为旧版本的参考文件，MusicBeeInterface.cs 是官方提供的SDK接口。

## 关键技术点

### 1. WebView2 初始化
WebView2 必须异步初始化，且需要缓存 Environment 对象避免重复创建错误。

```csharp
private CoreWebView2Environment webViewEnvironment;

private async void InitializeWebView2AndAddPanel()
{
    if (browser?.CoreWebView2 != null)
    {
        AddPanelToMusicBee();
        return;
    }
    if (webViewEnvironment == null)
    {
        webViewEnvironment = await CoreWebView2Environment.CreateAsync();
    }
    await browser.EnsureCoreWebView2Async(webViewEnvironment);
    browser.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
    AddPanelToMusicBee();
}
```

### 2. 面板添加时机
面板必须在 WebView2 完全初始化后才能添加到 MusicBee，否则可能导致 URL 路由到其他标签页。

### 3. 平台目标
MusicBee 是 32 位进程，项目必须编译为 x86 架构：

```xml
<Platform Condition=" '$(Platform)' == '' ">x86</Platform>
```

### 4. 菜单注册
`MB_AddMenuItem` 需要预存的委托引用：

```csharp
// 在 Initialise 中预存委托
openHandler = OpenBrowser;
closeHandler = CloseBrowser;

// 在 ReceiveNotification(PluginStartup) 中使用
mbApiInterface.MB_AddMenuItem("mnuTools/Browser2", "MusicBee Webserver", openHandler);
```

### 5. MB_AddTreeNode 限制
该 API 在部分 MusicBee 版本中存在内部 bug（NullReferenceException），建议仅使用 `MB_AddMenuItem` 注册菜单。
service 必须使用16px尺寸白色线条的图标，否则会导致菜单显示异常。

### 6. 关闭面板处理
使用 `MB_RemovePanel` 而非 `Dispose`，避免访问已销毁控件导致崩溃：

```csharp
public void CloseBrowser(object sender, EventArgs e)
{
    SaveSettings();
    if (panel != null)
    {
        mbApiInterface.MB_RemovePanel(panel);
    }
}
```

### 7. 调试技巧
需要复制runtime目录到plugin目录。

## 常见错误

### HRESULT: 0x8007000B
**原因**: DLL 架构不匹配（x86 vs x64）
**解决**: 编译为 x86 平台

### WebView2 已初始化错误
**原因**: 重复调用 `EnsureCoreWebView2Async` 使用不同的 Environment
**解决**: 缓存并复用同一个 `CoreWebView2Environment` 对象

### 关闭崩溃
**原因**: 尝试访问已销毁的控件
**解决**: 使用 `MB_RemovePanel` 代替 `panel.Dispose()`

## 文件结构
```
MusicBeeBrowser/
├── MusicBeeInterface.cs    # MusicBee SDK 接口定义
├── Browser2.cs              # 插件主代码
├── CSharpDll.csproj         # 项目文件
├── Properties/              # 资源文件（如有）
└── refer_files/
    └── browser.cs           # 旧版插件反编译参考
```

## API 版本
- MusicBee API Revision: 58
- WebView2: 1.0.3908-prerelease
- 目标框架: .NET Framework 4.8