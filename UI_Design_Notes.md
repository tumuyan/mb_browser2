# MusicBee Browser 插件 UI 设计文档

## 1. UI 控件层次结构

### 1.1 完整层次图

```
MusicBee Application Window
└── MusicBee MainPanel (插件容器)
    └── UserControl: panel (自定义控件，根容器)
        ├── WebView2: browser (浏览器控件，DockStyle.Fill)
        └── Control: header (头部控件，手动定位)
            ├── Control: locationBarPrompt (地址栏提示，可选)
            └── TextBox: locationBar (地址栏文本框)
```

### 1.2 控件层级索引

| 层级 | 控件名 | 类型 | 说明 |
|------|--------|------|------|
| 0 | `browser` | `WebView2` | 最底层，填充整个 panel |
| 1 | `header` | `Control` | 在 browser 之上，顶部定位 |
| 2 | `locationBarPrompt` | `Control` | header 的子控件 |
| 2 | `locationBar` | `TextBox` | header 的子控件 |

### 1.3 关键属性

#### panel (根容器)
- **类型**: `UserControl`
- **AutoScroll**: `false`
- **TabIndex**: `0`
- **TabStop**: `false`

#### header (头部控件)
- **类型**: `Control`
- **Dock**: `DockStyle.Top` (自动填充宽度)
- **Height**: `43` (完全显示) / `0` (隐藏状态)
- **Visible**: `true` (始终可见，通过高度控制显示/隐藏)
- **事件**: `Paint`, `Resize`, `MouseMove`, `MouseClick`, `MouseLeave`

#### browser (浏览器)
- **类型**: `WebView2`
- **Dock**: `DockStyle.Fill`
- **Padding**: 动态调整
  - header 隐藏时：`(0, 0, 0, 0)`
  - header 显示时：`(0, header.Height, 0, 0)`
- **事件**: `NavigationStarting`, `NavigationCompleted`, `SourceChanged`

#### locationBar (地址栏)
- **类型**: `TextBox`
- **BorderStyle**: `BorderStyle.FixedSingle`
- **事件**: `KeyDown`, `GotFocus`, `LostFocus`

#### locationBar (地址栏)
- **类型**: `TextBox`
- **BorderStyle**: `BorderStyle.FixedSingle`
- **Location**: 动态计算
  - X: `leftButtonWidth + buttonSpacing` (约 132 像素)
  - Y: `2` (固定在顶部)
  - Width: `header.Width - leftButtonWidth - rightButtonWidth - buttonSpacing * 2`
  - Height: `locationBar.Font.Height + 6` (约 19-21 像素)

---

## 2. 按钮布局设计

### 2.1 布局常量

```csharp
private const int BUTTON_WIDTH = 32;      // 按钮位置计算宽度（包含间隙）
private const int BUTTON_HEIGHT = 24;     // 按钮实际宽度和高度
private const int LEFT_MARGIN_BUTTONS = 1;   // 左侧保留 1 个按钮的间距
private const int RIGHT_MARGIN_BUTTONS = 1;  // 右侧保留 1 个按钮的间距
```

### 2.2 按钮位置计算

#### 左侧按钮（从左到右）

| 按钮 | X 坐标计算 | X 实际值 | Y | 宽度 | 高度 |
|------|-----------|---------|---|------|------|
| 后退 | `(LEFT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH` | 32 | `(header.Height - BUTTON_HEIGHT) / 2` | 24 | 24 |
| 前进 | `(LEFT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH` | 64 | 同上 | 24 | 24 |
| 主页 | `(LEFT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH` | 96 | 同上 | 24 | 24 |

#### 右侧按钮（从右到左）

| 按钮 | X 坐标计算 | X 实际值 | Y | 宽度 | 高度 |
|------|-----------|---------|---|------|------|
| 刷新/停止 | `header.Width - (RIGHT_MARGIN_BUTTONS + 0) * BUTTON_WIDTH - BUTTON_HEIGHT` | header.Width - 56 | `(header.Height - BUTTON_HEIGHT) / 2` | 24 | 24 |
| 书签 | `header.Width - (RIGHT_MARGIN_BUTTONS + 1) * BUTTON_WIDTH - BUTTON_HEIGHT` | header.Width - 88 | 同上 | 24 | 24 |
| 菜单 | `header.Width - (RIGHT_MARGIN_BUTTONS + 2) * BUTTON_WIDTH - BUTTON_HEIGHT` | header.Width - 120 | 同上 | 24 | 24 |

### 2.3 布局特点

- **按钮间隙**: `BUTTON_WIDTH - BUTTON_HEIGHT = 8` 像素
- **左侧边距**: 32 像素（1 个 BUTTON_WIDTH）
- **右侧边距**: 32 像素（1 个 BUTTON_WIDTH）
- **地址栏左侧空间**: 128 像素（4 个 BUTTON_WIDTH）
- **地址栏右侧空间**: 128 像素（4 个 BUTTON_WIDTH）

---

## 3. Header 自动隐藏功能（已废弃）

> **注意**: 该功能已在 v2.1 版本放弃，原因是 JavaScript 注入在页面导航后会失效，导致用户体验不一致。当前版本保持 Header 始终显示（高度 43px）。

### 3.1 设计目标（原始设计）

- **默认状态**: header 完全不占用空间（高度为 0）
- **触发显示**: 鼠标移动到 WebView2 顶部 16px 时显示完整 header
- **自动隐藏**: 鼠标移开且地址栏无焦点时，延迟 2 秒隐藏
- **焦点保护**: 地址栏获得焦点时，header 保持显示不隐藏

### 3.2 尝试的实现方案（JavaScript 注入 + WebMessage，已废弃）

#### 状态管理

```csharp
private bool isHeaderVisible = false;
private System.Windows.Forms.Timer headerHideTimer;
private const int HEADER_HIDE_DELAY = 2000; // 2 秒
private const int HEADER_SHOW_TRIGGER_Y = 16; // 鼠标在顶部 16px 时显示 header
private const int HEADER_FULL_HEIGHT = 43; // header 完整高度
private const int HEADER_HIDDEN_HEIGHT = 0; // header 隐藏时高度（完全不占用空间）
```

#### 隐藏逻辑

```csharp
private void HideHeader()
{
    isHeaderVisible = false;
    // 完全隐藏：高度设为 0，不占用空间
    header.Height = HEADER_HIDDEN_HEIGHT;
    header.Visible = true;  // 保持可见（但高度为 0）
    header.Invalidate();
    
    // 调整 browser 的上边距，使其填充整个区域
    if (browser != null)
    {
        browser.Padding = new Padding(0, 0, 0, 0);
        browser.Invalidate();
    }
    
    Debug.WriteLine($"Browser2: Header hidden (Height={HEADER_HIDDEN_HEIGHT})");
}
```

#### 显示逻辑

```csharp
private void ShowHeader()
{
    isHeaderVisible = true;
    header.Visible = true;
    header.Height = HEADER_FULL_HEIGHT;  // 完整高度
    header.BringToFront();  // 确保 header 在最上层
    header.Invalidate();
    
    // 调整 browser 的上边距，避免被 header 遮挡
    if (browser != null)
    {
        browser.Padding = new Padding(0, header.Height, 0, 0);
        browser.Invalidate();
    }
    
    Debug.WriteLine($"Browser2: Header shown (Height={HEADER_FULL_HEIGHT})");
    headerHideTimer.Stop();
}
```

### 3.3 JavaScript 鼠标追踪

#### 注入脚本

```javascript
let lastY = -1;

document.addEventListener('mousemove', function(e) {
    const currentY = e.clientY;
    
    // 只在 Y 坐标变化时发送消息，减少频率
    if (currentY !== lastY) {
        lastY = currentY;
        
        // 发送鼠标位置到 C#
        window.chrome.webview.postMessage({
            type: 'mousemove',
            y: currentY
        });
    }
});

// 监听鼠标离开窗口
document.addEventListener('mouseleave', function(e) {
    window.chrome.webview.postMessage({
        type: 'mouseleave'
    });
});
```

#### WebMessage 处理逻辑

```csharp
private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    string message = e.WebMessageAsJson;
    
    if (message.Contains("\"type\":\"mousemove\""))
    {
        // 提取 Y 坐标
        int y = ParseYFromJson(message);
        
        if (y <= HEADER_SHOW_TRIGGER_Y)
        {
            // 鼠标在顶部 16px 区域，显示 header
            if (!isHeaderVisible)
            {
                Debug.WriteLine($"Mouse at top area (Y={y} <= {HEADER_SHOW_TRIGGER_Y}), showing header");
            }
            panel.Invoke(new Action(() => ShowHeader()));
        }
        else if (isHeaderVisible)
        {
            // 鼠标在 header 下方，检查地址栏焦点后启动隐藏定时器
            panel.Invoke(new Action(() =>
            {
                if (locationBar != null && !locationBar.ContainsFocus)
                {
                    Debug.WriteLine($"Mouse below trigger area (Y={y}), locationBar not focused, starting hide timer");
                    headerHideTimer.Stop();
                    headerHideTimer.Start();
                }
                else
                {
                    Debug.WriteLine($"Mouse below trigger area (Y={y}), but locationBar has focus, not hiding");
                }
            }));
        }
    }
}
```

### 3.4 地址栏焦点处理

#### GotFocus 事件

```csharp
private void LocationBar_GotFocus(object sender, EventArgs e)
{
    // 地址栏获得焦点时，停止隐藏定时器，保持 header 显示
    Debug.WriteLine("Browser2: LocationBar got focus, stopping hide timer");
    headerHideTimer.Stop();
    // 确保 header 显示
    if (!isHeaderVisible)
    {
        ShowHeader();
    }
}
```

#### LostFocus 事件

```csharp
private void LocationBar_LostFocus(object sender, EventArgs e)
{
    // 地址栏失去焦点时，不立即隐藏
    // 等待鼠标移动事件或 mouseleave 事件处理
    Debug.WriteLine("Browser2: LocationBar lost focus, will hide header if mouse not in trigger area");
}
```

### 3.5 事件处理流程

```
JavaScript 监听鼠标移动
    ↓
document.addEventListener('mousemove')
    ↓
window.chrome.webview.postMessage({type: 'mousemove', y: Y})
    ↓
CoreWebView2_WebMessageReceived
    ↓
解析 JSON 获取 Y 坐标
    ↓
判断：Y <= 16?
    ├─ 是 → ShowHeader()
    │        ├─ header.Height = 43
    │        ├─ header.BringToFront()
    │        └─ browser.Padding = (0, 43, 0, 0)
    │
    └─ 否 → 检查地址栏焦点
             ├─ 地址栏有焦点 → 不隐藏
             └─ 地址栏无焦点 → headerHideTimer.Start()
                      ↓
                  2 秒后 Tick
                      ↓
                  HideHeader()
                      ├─ header.Height = 0
                      └─ browser.Padding = (0, 0, 0, 0)

地址栏 GotFocus
    ↓
LocationBar_GotFocus
    ↓
headerHideTimer.Stop()
    ↓
ShowHeader() (如果需要)

地址栏 LostFocus
    ↓
LocationBar_LostFocus
    ↓
等待鼠标事件触发隐藏逻辑
```

### 3.6 为什么放弃这个方案？

#### 问题：JavaScript 注入在页面导航后失效

**核心问题**: WebView2 加载新页面时，会完全销毁旧页面的 DOM 和 JavaScript 环境。

**具体表现**:
1. 用户点击链接或输入新 URL
2. 旧页面卸载，所有注入的 JavaScript 被清除
3. 新页面加载完成
4. 需要重新注入 JavaScript 脚本
5. 但 `AddScriptToExecuteOnDocumentCreatedAsync()` 只在文档**首次创建**时执行

**技术限制**:
```csharp
// ❌ 这个 API 只在文档首次创建时执行一次
await browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script);

// ❌ 页面导航后不会自动重新执行
browser.CoreWebView2.NavigationCompleted += (s, e) => {
    // 即使在这里重新注入，也已经错过了最佳时机
    // 用户在导航后的鼠标移动可能无法被捕获
};
```

#### 为什么 WinForms 事件不工作？

**WebView2 的事件拦截**:
- WebView2 是一个独立的控件，基于 Chromium 内核
- 它会在控件级别拦截所有鼠标消息
- WinForms 的 `MouseMove`、`MouseLeave` 事件无法穿透 WebView2

**尝试过的方案**:
- ❌ `panel.MouseMove` - 事件被 WebView2 拦截
- ❌ `header.MouseMove` - 同上
- ❌ `browser.MouseMove` - 这是 WebView2 的事件，但只在鼠标在 browser 上时触发，无法用于 header 区域

#### 可能的替代方案（未实现）

1. **在 NavigationCompleted 后重新注入**
   ```csharp
   browser.CoreWebView2.NavigationCompleted += async (s, e) => {
       await InjectMouseMoveScript();
   };
   ```
   - 问题：注入延迟，用户体验不一致

2. **使用 Windows Hooks**
   - 全局钩子监听鼠标消息
   - 问题：复杂度高，性能开销大，可能影响其他应用

3. **使用透明覆盖层**
   - 在 WebView2 上方放置透明 Panel
   - 问题：可能影响 WebView2 的交互，显示异常

**最终决定**: 放弃自动隐藏，保持 Header 始终显示。

### 3.7 为什么曾经考虑使用 JavaScript 注入？

**原始问题**: WebView2 控件会拦截所有鼠标事件，导致 WinForms 的 `MouseMove`、`MouseLeave` 事件无法触发。

**当时的解决方案**: 
1. 通过 JavaScript 注入到网页中，在 DOM 层面监听鼠标事件
2. 使用 `window.chrome.webview.postMessage()` 将事件数据发送到 C#
3. C# 通过 `CoreWebView2.WebMessageReceived` 事件接收并处理

**关键 API**:
- `CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync()`: 页面加载前注入脚本
- `window.chrome.webview.postMessage()`: JS → C# 通信
- `CoreWebView2.WebMessageReceived`: C# 接收 JS 消息
- `e.WebMessageAsJson`: 获取 JSON 格式的消息内容

### 3.8 UI 层次与坐标系统（原始设计）

```
┌─────────────────────────────────────────┐
│  MusicBee MainPanel                     │
│  ┌───────────────────────────────────┐  │
│  │ UserControl (panel)               │  │
│  │  ┌─────────────────────────────┐  │  │
│  │  │ Header (0px / 43px)         │  │  │ ← DockStyle.Top
│  │  │ - Buttons (Back, Forward,   │  │  │
│  │  │   Home, Refresh, Bookmark)  │  │  │
│  │  │ - TextBox (locationBar)     │  │  │
│  │  ├─────────────────────────────┤  │  │
│  │  │                             │  │  │
│  │  │  WebView2 (browser)         │  │  │ ← DockStyle.Fill
│  │  │  - 显示网页内容             │  │  │
│  │  │  - JavaScript 监听鼠标      │  │  │
│  │  │                             │  │  │
│  │  │  鼠标 Y 坐标 (clientY)       │  │  │
│  │  │  ├─ 0-16px: 触发显示 header │  │  │
│  │  │  └─ 17px+: 可能触发隐藏     │  │  │
│  │  │                             │  │  │
│  │  └─────────────────────────────┘  │  │
│  └───────────────────────────────────┘  │
└─────────────────────────────────────────┘
```

**坐标说明**:
- JavaScript 中的 `e.clientY` 是相对于浏览器视口顶部的坐标
- Y=0 是网页内容的顶部（header 下方）
- Y≤16 时触发 header 显示

**当前实现**:
- Header 始终显示，高度固定为 43px
- 不再使用 JavaScript 注入监听鼠标
- 不再使用自动隐藏功能

---

## 4. 主题颜色系统

### 4.1 全局主题变量

```csharp
private Color themeForegroundColor;
private Color themeBackgroundColor;
```

### 4.2 取色逻辑

```csharp
themeForegroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentForeground);
themeBackgroundColor = GetThemeColor(SkinElement.SkinSubPanel, ElementComponent.ComponentBackground);
```

### 4.3 图标着色

#### CreateThemedIcon 方法

```csharp
private Bitmap CreateThemedIcon(string iconFileName, Color bgColor, Color fgColor)
{
    // 1. 自动拼接路径：Resources + iconFileName
    // 2. 加载原始图标
    // 3. 使用 bgColor 填充背景
    // 4. 使用 fgColor 重新映射图标中的黑色
    // 5. 返回主题化后的图标
}
```

#### 已加载的图标

| 图标字段 | 文件名 | 用途 |
|---------|--------|------|
| `backIcon` | `iconmonstr-arrow-left-alt-filled-16.png` | 后退按钮 |
| `forwardIcon` | `iconmonstr-arrow-right-alt-filled-16.png` | 前进按钮 |
| `homeIcon` | `iconmonstr-home-7-16.png` | 主页按钮 |
| `refreshIcon` | `iconmonstr-refresh-lined-16.png` | 刷新按钮 |
| `stopIcon` | `iconmonstr-x-mark-9-16.png` | 停止按钮 |
| `starFilledIcon` | `iconmonstr-star-filled-16.png` | 已收藏书签 |
| `starLinedIcon` | `iconmonstr-star-lined-16.png` | 未收藏书签 |
| `menuIcon` | `iconmonstr-menu-square-lined-16.png` | 书签菜单 |

### 4.4 颜色调试

#### DebugSkinColors 方法

遍历所有 `SkinElement` 的取值（0-30），打印有效的配色信息：

```
ID	BG	FG	BDR
-------------------------------------------
2	#B05352	#FFFFFF	#B05352
```

#### 已知 SkinElement

| ID | 名称 | Background | Foreground | Border |
|----|------|------------|------------|--------|
| -1 | SkinTrackAndArtistPanel | #27BD8F | #000000 | #27BD8F |
| 0 | SkinSubPanel | #27BD8F | #000000 | #27BD8F |
| 7 | SkinInputControl | #FFEBCD | #000000 | #E1A01B |
| 10 | SkinInputPanel | #F3DEB1 | #000000 | #EBC069 |
| 14 | SkinInputPanelLabel | #F3DEB1 | #000000 | #EBC069 |

---

## 5. MusicBee 面板系统

### 5.1 面板添加方式

```csharp
mbApiInterface.MB_AddPanel(panel, PluginPanelDock.MainPanel);
```

### 5.2 面板 Dock 位置

| Dock 位置 | 说明 | 标题栏 |
|----------|------|--------|
| `MainPanel` | 主面板区域 | 有标题栏（约 138 像素偏移） |
| `ApplicationWindow` | 应用程序窗口 | 待测试 |
| `TrackAndArtistPanel` | 曲目和艺术家面板 | 未知 |

### 5.3 标题栏问题

#### 问题描述
- 使用 `MainPanel` 时，panel 会被自动添加一个标题栏区域
- panel 的 `Location.Y = 138`，表示上方有 138 像素的偏移
- 这个标题栏不是独立控件，而是 MusicBee 主窗口布局的一部分

#### 已尝试的解决方案

1. **反射查找标题栏控件**: 遍历控件树，未找到独立标题栏
2. **尝试非法 SkinElement ID**: 遍历 0-30，未找到对应 HeaderBar 的配色
3. **修改 Dock 位置**: 改为 `ApplicationWindow`，显示异常
4. **直接调整 panel 位置**: 设置 `DockStyle.Fill`，导致填充整个窗口

#### 当前状态
- **接受标题栏存在**: 这是 MusicBee MainPanel 的标准行为
- **暂不实现隐藏**: 保留 `HidePanelTitleBar()` 方法供未来使用

---

## 6. 待解决问题清单

### 6.1 已完成功能

1. **Header 自动隐藏功能 ❌（已放弃）**
   - 原因：JavaScript 注入在页面导航后失效
   - 当前状态：保持 Header 始终显示（高度 43px）
   - 详见：第 3 章 - Header 自动隐藏功能（已废弃）

2. **按钮布局优化 ✅**
   - [x] 等间距按钮布局
   - [x] 按钮间隙 8 像素
   - [x] 左右侧各保留 1 个按钮宽度的边距

3. **图标主题适配 ✅**
   - [x] CreateThemedIcon 方法
   - [x] 自动加载并着色图标
   - [x] 支持前景色和背景色

4. **地址栏功能 ✅**
   - [x] URL 输入和导航
   - [x] 焦点事件处理（为自动隐藏功能准备，现已废弃）

### 6.2 中优先级

5. **主题颜色优化**
   - [ ] 当前使用 `SkinSubPanel`，颜色为青绿色 (#27BD8F)
   - [ ] 用户期望使用蓝色 (#1BA0E1 或 #21A6FF)
   - [ ] 需要找到正确的 SkinElement

6. **标题栏区域**
   - [ ] MainPanel 的 138 像素标题栏区域
   - [ ] 考虑是否在 header 中添加自定义标题

### 6.3 低优先级

5. **代码清理**
   - [ ] 删除未使用的字段（`isMouseOverBack`, `isMouseOverForward`, `isMouseOverHome`）
   - [ ] 删除调试代码（`DebugSkinColors` 调用）
   - [ ] 移除 header 的临时绿色背景

---

## 7. 调试技巧总结

### 7.1 JavaScript 注入调试

```csharp
private async System.Threading.Tasks.Task InjectMouseMoveScript()
{
    string script = @"...";
    await browser.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script);
    Debug.WriteLine("Browser2: JavaScript mouse tracking injected");
}
```

### 7.2 WebMessage 通信调试

```csharp
private void CoreWebView2_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
{
    string message = e.WebMessageAsJson;
    Debug.WriteLine($"Browser2: WebMessage received: {message}");
    Debug.WriteLine($"Browser2: Mouse at Y={y}, isHeaderVisible={isHeaderVisible}");
}
```

### 7.3 Header 状态调试

```csharp
Debug.WriteLine($"Browser2: Header hidden (Height={HEADER_HIDDEN_HEIGHT})");
Debug.WriteLine($"Browser2: Header shown (Height={HEADER_FULL_HEIGHT})");
Debug.WriteLine($"Browser2: Mouse at top area (Y={y} <= {HEADER_SHOW_TRIGGER_Y}), showing header");
Debug.WriteLine($"Browser2: Mouse below trigger area (Y={y}), locationBar not focused, starting hide timer");
```

### 7.4 地址栏焦点调试

```csharp
private void LocationBar_GotFocus(object sender, EventArgs e)
{
    Debug.WriteLine("Browser2: LocationBar got focus, stopping hide timer");
}

private void LocationBar_LostFocus(object sender, EventArgs e)
{
    Debug.WriteLine("Browser2: LocationBar lost focus, will hide header if mouse not in trigger area");
}
```

### 7.5 控件层级调试（历史方法）

```csharp
Debug.WriteLine($"Panel parent is {panel.Parent.GetType().FullName}");
foreach (Control control in panel.Parent.Controls)
{
    Debug.WriteLine($"Found control: {control.GetType.FullName} - Height: {control.Height}");
}
```

### 7.6 颜色调试

```csharp
Debug.WriteLine($"Header_Paint - Bg={themeBackgroundColor.ToArgb():X8}, Fg={themeForegroundColor.ToArgb():X8}");
```

---

## 8. 参考资料

### 8.1 皮肤配置文件
- 路径：`a:\Media\MusicBee\Skins\Multi-Color\Mellon Remix.xml`
- 关键颜色：
  - `Blue = "27,160,225"` (RGB 27, 160, 225)
  - `Light Stripe = "177,222,243"`
  - `Dark Stripe = "27,160,225"`

### 8.2 MusicBee API
- `MB_AddPanel()`: 添加插件面板
- `Setting_GetSkinElementColour()`: 获取皮肤元素颜色
- `PluginPanelDock`: 面板停靠位置枚举

### 8.3 Windows Forms
- `Control.BringToFront()`: 将控件置于最上层
- `Control.SendToBack()`: 将控件置于最底层
- `Control.SetChildIndex()`: 调整子控件索引
- `Padding`: 控件内边距

---

**文档生成时间**: 2026-03-20
**最后更新**: 2026-03-20 (放弃自动隐藏，保持 Header 始终显示)
**版本**: 2.1

---

## 9. 版本历史

### v2.1 (2026-03-20) - 放弃自动隐藏方案

#### 问题发现
- ❌ **JavaScript 注入在页面导航后失效**
  - 当用户点击链接、输入新 URL 或刷新页面时，WebView2 会加载新页面
  - 新页面会完全替换旧页面的 DOM，包括所有注入的 JavaScript
  - `AddScriptToExecuteOnDocumentCreatedAsync()` 只会在文档**首次创建**时执行
  - 页面导航后，需要重新注入脚本，但此时已经错过了最佳的注入时机

#### 技术限制
1. **AddScriptToExecuteOnDocumentCreatedAsync 的行为**
   - 该 API 设计用于在文档创建时自动执行脚本
   - 但对于页面导航（Navigation），脚本不会自动重新注入
   - 需要在每次 `NavigationCompleted` 后重新注入，但这会导致：
     - 脚本注入延迟，出现短暂的"无鼠标追踪"状态
     - 代码复杂度增加
     - 性能开销

2. **页面导航时机**
   ```
   用户点击链接
       ↓
   NavigationStarting 事件
       ↓
   旧页面卸载（JavaScript 环境销毁）
       ↓
   新页面加载
       ↓
   NavigationCompleted 事件
       ↓
   此时才能重新注入脚本 ← 问题：已经错过了鼠标移动事件
   ```

3. **用户体验问题**
   - 每次页面切换后，用户需要重新移动鼠标才能触发注入
   - 可能出现 header 显示不一致的情况
   - 增加了不可预测的行为

#### 决定
- **放弃 Header 自动隐藏功能**
- **保持 Header 始终显示**（高度 43px）
- 优点：
  - 稳定可靠，不依赖 JavaScript 注入
  - 用户体验一致
  - 代码简洁易维护

#### 保留功能
- ✅ 主题图标适配
- ✅ 按钮布局优化
- ✅ 地址栏功能
- ✅ 书签功能

### v2.0 (2026-03-20) - JavaScript 注入方案（已废弃）

#### 新增功能（已废弃）
- ❌ JavaScript 注入监听鼠标移动
- ❌ WebMessage 实现 JS ↔ C# 通信
- ❌ Header 默认高度为 0（完全不占用空间）
- ❌ 鼠标在顶部 16px 触发显示
- ❌ 地址栏焦点保护（获得焦点时不隐藏）
- ❌ 鼠标移开 + 地址栏无焦点时 2 秒隐藏

#### 技术实现（已废弃）
- `CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync()`: 注入脚本
- `window.chrome.webview.postMessage()`: JS → C# 通信
- `CoreWebView2.WebMessageReceived`: C# 接收消息
- `locationBar.GotFocus/LostFocus`: 焦点事件处理

#### 常量配置（已废弃）
```csharp
private const int HEADER_SHOW_TRIGGER_Y = 16;  // 触发区域高度
private const int HEADER_FULL_HEIGHT = 43;     // 完整高度
private const int HEADER_HIDDEN_HEIGHT = 0;    // 隐藏高度
private const int HEADER_HIDE_DELAY = 2000;    // 隐藏延迟（毫秒）
```

### v1.0 (2026-03-20) - 初始版本

#### 功能
- 基本浏览器功能（后退、前进、主页、刷新、停止）
- 书签功能
- 主题图标适配
- 按钮布局优化

#### 尝试方案（已废弃）
- ❌ Panel.MouseMove 监听
- ❌ Header.MouseMove 监听
- ❌ 透明覆盖层
- ❌ 保留 10 像素高度方案
