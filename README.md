# MusicBee Browser2 Plugin

一个基于 WebView2 的现代化 MusicBee 网页浏览器插件，用以替代旧版基于 IE WebBrowser 控件的浏览器。

## 功能特性

- 🌐 **现代化 WebView2 内核** - 使用 Microsoft Edge WebView2，支持最新的 Web 标准
- 🎨 **主题适配** - 自动适配 MusicBee 皮肤颜色，图标动态着色
- 🔖 **书签/主页** - 收藏书签、书签列表、设置主页 URL

## 系统要求

- **.NET Framework**: 4.6.2 或更高版本
- **WebView2 Runtime**: 必须安装 Microsoft Edge WebView2 Runtime

### 安装 WebView2 Runtime

如果尚未安装 WebView2 Runtime，可以从以下地址下载：
- [WebView2 Runtime 下载](https://developer.microsoft.com/en-us/microsoft-edge/webview2/)

## 已知问题

### 1. 面板标题栏偏移

使用 `MainPanel` Dock 位置时，插件面板会自动包含 MusicBee 的标题栏区域（约 138 像素偏移）。这是 MusicBee 的标准行为，目前无法以合适的方式移除。

### 2. 主题颜色

当前使用 `SkinSubPanel` 元素获取主题颜色，测试发现似乎不少皮肤并没有很规范地进行设计，以至于穷举所有可用值效果都不理想。

### 3. 自动隐藏地址栏
测试多种方案，都不能很好地实现浮动显示地址栏的效果。目前已经放弃此功能。
