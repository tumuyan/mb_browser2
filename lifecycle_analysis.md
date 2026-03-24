# Browser2 插件生命周期分析

## 核心组件

### UI 组件
- **UserControl panel**: 主面板容器，包含 header 和 browser
- **Control header**: 顶部工具栏，包含地址栏和按钮
- **TextBox locationBar**: 地址栏输入框
- **WebView2 browser**: 网页浏览器控件

### 状态变量
- **string activeUrl**: 当前活动的 URL
- **string defaultUrl**: 默认首页 URL
- **string loadOnceUrl**: 一次性加载的 URL（用于 MusicBee 内部链接）
- **string pendingUrl**: 待加载的 URL（用于异步初始化）
- **CoreWebView2Environment webViewEnvironment**: WebView2 运行环境（全局缓存）

### 事件监听
- **Form.Resize**: 监听 MusicBee 主窗口大小变化（最小化/恢复）
- **panel.VisibleChanged**: 监听面板可见性变化

---

## 生命周期流程

### 1. 插件初始化阶段

**触发时机**: MusicBee 启动时

**执行流程**:
```
Initialise()
  ↓
注册 openHandler 和 closeHandler
  ↓
返回 PluginInfo
```

**状态变化**:
- `mbApiInterface` 初始化
- `openHandler = OpenBrowser`
- `closeHandler = CloseBrowser`

---

### 2. 首次打开浏览器

**触发时机**: 用户点击 MusicBee 树节点中的 "Browser2"

**执行流程**:
```
OpenBrowser()
  ↓
browser == null || browser.CoreWebView2 == null? → YES
  ↓
LoadSettings() → 加载默认首页 URL
  ↓
InitializeBrowser()
  ↓
创建 panel, header, browser
  ↓
InitializeWebView2AndAddPanel()
  ↓
CoreWebView2Environment.CreateAsync()
  ↓
browser.EnsureCoreWebView2Async()
  ↓
AddPanelToMusicBee()
  ↓
TryNavigate() → 导航到默认首页
```

**状态变化**:
- `panel` 创建
- `browser` 创建
- `browser.CoreWebView2` 初始化
- `webViewEnvironment` 创建并缓存
- `activeUrl` 设置为默认首页

---

### 3. 点击 MusicBee 内部链接

**触发时机**: 用户点击 MusicBee 内部的网页链接

**执行流程**:
```
Navigate(url)
  ↓
panel != null? → YES (浏览器已打开)
  ↓
ShowNavigationTarget(url) → 更新地址栏
  ↓
browser.Focus()
  ↓
NavigateTo(url)
  ↓
CoreWebView2.Navigate(url)
  ↓
OpenBrowser()
  ↓
browser.CoreWebView2 != null? → YES
  ↓
AddPanelToMusicBee()
  ↓
loadOnceUrl == null? → YES
  ↓
不加载任何 URL (因为已经在 Navigate 中加载过了)
```

**状态变化**:
- `activeUrl` 更新为新 URL
- `locationBar.Text` 更新为新 URL
- 网页导航到新 URL

---

### 4. 切换标签页（关闭浏览器）

**触发时机**: 用户切换到其他标签页

**执行流程**:
```
CloseBrowser()
  ↓
SaveSettings()
  ↓
暂停所有媒体播放（video, audio）
  ↓
设置 WebView2 IsVisible = false（停止渲染）
  ↓
（不调用 MB_RemovePanel，让 MusicBee 自己管理面板）
```

**状态变化**:
- `panel` 保持存在（MusicBee 自动隐藏）
- **保留** `browser` 实例
- **保留** `browser.CoreWebView2` 实例
- **设置** `CoreWebView2.Controller.IsVisible = false`（停止渲染）
- **保留** `activeUrl`
- **保留** `locationBar.Text`
- **保留** 网页内容
- **暂停** 所有媒体播放（减少资源消耗）

**关键设计**:
- 不调用 `MB_RemovePanel`，参考旧插件的实现
- MusicBee 会在 `closeHandler` 被调用后自动处理面板的显示/隐藏
- 不销毁 WebView2 实例，以便快速切换回来
- 不导航到 `about:blank`，保持网页状态
- 保留 `activeUrl` 和 `locationBar.Text`，以便切换回来时显示
- **暂停媒体播放**，减少 CPU 和内存消耗
- **设置 `IsVisible = false`**，完全停止渲染，最大化资源节省

---

### 5. 切换回来（重新打开浏览器）

**触发时机**: 用户切换回 Browser2 标签页

**执行流程**:
```
OpenBrowser()
  ↓
browser.CoreWebView2 != null? → YES
  ↓
设置 WebView2 IsVisible = true（恢复渲染）
  ↓
AddPanelToMusicBee()
  ↓
loadOnceUrl == null? → YES
  ↓
恢复地址栏显示: locationBar.Text = activeUrl
  ↓
不加载任何 URL (保持网页状态)
```

**状态变化**:
- `panel` 重新显示（MusicBee 自动显示）
- **设置** `CoreWebView2.Controller.IsVisible = true`（恢复渲染）
- 地址栏恢复显示 `activeUrl`
- 网页内容保持不变

---

### 6. 关闭浏览器（用户主动关闭）

**触发时机**: 用户点击关闭按钮

**执行流程**:
```
CloseBrowser()
  ↓
SaveSettings()
  ↓
MB_RemovePanel(panel)
```

**状态变化**:
- `panel` 从 MusicBee 移除
- **保留** `browser` 实例
- **保留** 网页内容

---

## 关键设计决策

### 1. WebView2 实例复用

**决策**: 不销毁 WebView2 实例，而是保留它以便快速切换

**优点**:
- 切换标签页时无需重新初始化 WebView2
- 保持网页状态，用户体验更好
- 减少资源消耗

**缺点**:
- 内存占用较高（WebView2 实例一直存在）

---

### 2. 不导航到 about:blank

**决策**: 切换标签页时不导航到 `about:blank`

**优点**:
- 保持网页状态
- 切换回来时无需重新加载

**缺点**:
- 网页内容一直在后台运行（可能消耗资源）

---

### 3. 保留 activeUrl 和 locationBar.Text

**决策**: 切换标签页时保留 URL 状态

**优点**:
- 切换回来时地址栏能正确显示当前 URL
- 用户体验更好

---

## 状态管理总结

| 状态变量 | 首次打开 | 点击内部链接 | 切换标签页 | 切换回来 |
|---------|---------|------------|----------|---------|
| `panel` | 创建 | 保持 | **保持（隐藏）** | 保持（显示） |
| `browser` | 创建 | 保持 | **保留** | 保持 |
| `browser.CoreWebView2` | 初始化 | 保持 | **保留** | 保持 |
| `CoreWebView2.Controller.IsVisible` | true | 保持 | **false** | **true** |
| `activeUrl` | 设置默认首页 | 更新为新 URL | **保留** | 保持 |
| `locationBar.Text` | 设置默认首页 | 更新为新 URL | **保留** | 保持 |
| 网页内容 | 加载默认首页 | 加载新 URL | **保留** | 保持 |
| 媒体播放 | - | - | **暂停** | 保持暂停 |

**注意**: 
- 切换标签页时，MusicBee 自动隐藏/显示面板，插件不需要调用 `MB_RemovePanel`
- 设置 `IsVisible = false` 会完全停止渲染，最大化资源节省

---

## 潜在问题和改进方向

### 1. 内存管理

**问题**: WebView2 实例一直存在，可能占用较多内存

**改进方向**:
- 可以在插件关闭时（`Close(PluginCloseReason)`）销毁 WebView2
- 或者实现一个超时机制，长时间不使用时销毁 WebView2

---

### 2. 状态同步

**问题**: 如果网页内部发生导航（比如用户点击网页内的链接），`activeUrl` 可能不同步

**改进方向**:
- 监听 `SourceChanged` 事件，更新 `activeUrl`
- 当前代码已经实现了这一点（第 1025-1033 行）

---

### 3. 多标签页支持

**问题**: 当前设计不支持多个浏览器标签页

**改进方向**:
- 如果需要支持多标签页，需要重构为每个标签页一个 WebView2 实例的设计

---

## 代码位置参考

- **初始化**: [Browser2.cs:77-98](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L77-L98)
- **打开浏览器**: [Browser2.cs:351-398](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L351-L398)
- **关闭浏览器**: [Browser2.cs:1081-1109](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L1081-L1109)
- **导航**: [Browser2.cs:951-962](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L951-L962)
- **初始化 WebView2**: [Browser2.cs:465-496](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L465-L496)
- **添加面板**: [Browser2.cs:498-503](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L498-L503)
- **导航到 URL**: [Browser2.cs:964-1017](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L964-L1017)
- **SourceChanged 事件**: [Browser2.cs:1025-1033](file:///a:/projectCSharp/MusicBeeBrowser/Browser2.cs#L1025-L1033)

---

## 总结

当前的生命周期设计采用了**实例复用**的策略，通过保留 WebView2 实例和网页状态，实现了快速的标签页切换体验。这种设计在单标签页场景下表现良好，但如果需要支持多标签页或需要更严格的内存管理，可能需要重构。
