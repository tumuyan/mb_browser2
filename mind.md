# MusicBee Browser2 插件功能需求文档

## 项目概述
为 MusicBee 的 WebView2 浏览器插件添加完整的设置管理界面和 Chromium 扩展支持功能。

---

## 一、设置界面功能

### 1.1 插件设置入口
- 在 MusicBee 插件设置的**主页地址栏右侧**添加**"更多设置"按钮**
- 按钮样式：
  - 尺寸：100x28 像素
  - 字体：10pt 粗体
  - 文本："更多设置"
- 点击按钮打开插件设置窗口

### 1.2 设置窗口布局
窗口标题：`{插件名} 设置`（动态显示，如"Browser2 设置"）
窗口尺寸：550x650 像素

#### 设置项列表（从上到下）：

| 序号 | 标签 | 控件类型 | 功能说明 | 绑定变量 |
|------|------|----------|----------|----------|
| 1 | 扩展目录 | TextBox + 浏览按钮 | 设置 Chromium 扩展目录路径 | `extensionsPath` |
| 2 | 安装扩展 | 按钮 | 从 CRX 文件安装扩展 | - |
| 3 | 扩展 | 勾选框 | 启用 Chromium 扩展支持 | `extensionsEnabled` |
| 4 | 管理 | 按钮 | 打开扩展管理页面 (edge://extensions/) | - |
| 5 | 翻译 | 勾选框 | 启用翻译功能 | `useTranslation` |
| 6 | TTS | 勾选框 | 启用文本转语音功能 | `enableTTS` |
| 7 | 深色模式 | 下拉框 | 选择主题模式（浅色/深色/自动） | `darkModeSetting` |
| 8 | 缩放比例 | 数值框 | 设置网页缩放比例 (25%-500%) | `zoomRatio` |
| 9 | 自动保存缩放 | 勾选框 | 自动保存缩放比例 | `autoSaveZoom` |
| 10 | 硬件加速 | 勾选框 | 启用硬件加速 | `hardwareAcceleration` |

### 1.3 底部按钮
- **保存按钮**（100x35 像素，粗体）
  - 点击后立即保存设置到文件
  - 如果扩展启用状态变化，弹出对话框询问是否重启 WebView2
  - 重启后恢复之前的浏览页面
  
- **取消按钮**（100x35 像素）
  - 点击后不保存设置，直接关闭窗口

---

## 二、Chromium 扩展管理功能

### 2.1 扩展支持
- 使用 WebView2 原生 API：`CoreWebView2EnvironmentOptions.AreBrowserExtensionsEnabled = true`
- 通过命令行参数设置扩展目录：`--extensions-dir="{路径}"`
- 扩展目录默认为插件目录下的 `Extensions` 文件夹

### 2.2 扩展安装
**从 CRX 文件安装**按钮功能：
- 文件过滤器：`Chrome 扩展文件 (*.crx)`
- 自动解压 CRX 到扩展目录
- 解压路径：`{扩展目录}/{扩展名}_ext/`
- 提示用户启用扩展并重启 WebView2

### 2.3 扩展管理
**打开扩展管理页面**按钮功能：
- 打开浏览器内置的 `edge://extensions/` 页面
- 可在页面中进行的操作：
  - 启用/禁用扩展
  - 移除扩展
  - 查看扩展详情
  - 开启开发者模式
  - 加载已解压的扩展

### 2.4 扩展目录结构
```
插件目录/
└── Extensions/
    ├── 扩展名 1_ext/
    │   ├── manifest.json
    │   ├── background.js
    │   └── ...
    └── 扩展名 2_ext/
        └── ...
```

---

## 三、设置持久化

### 3.1 存储格式
- 文件路径：`{MusicBee 设置目录}/Browser2Settings.dat`
- 二进制格式（BinaryWriter/Reader）
- 版本号：4

### 3.2 保存内容
```
版本 (int)
defaultUrl (string)
书签数量 (int)
[循环书签数据]
    - Url (string)
    - Name (string)
    - Icon 数据 (byte[])
useTranslation (bool)
extensionsEnabled (bool)
enableTTS (bool)
darkModeSetting (int)
zoomRatio (float)
autoSaveZoom (bool)
hardwareAcceleration (bool)
extensionsPath (string)
```

### 3.3 加载逻辑
- `Configure()` 方法调用时自动加载设置
- `OpenSettingsWindow()` 打开时重新加载确保显示最新值
- 使用 `settingsLoadedForConfig` 标志避免重复加载

### 3.4 保存时机
- 设置窗口点击"保存"按钮时立即保存
- `isSettingsDirty` 标志置为 false
- 插件关闭时检查 `isSettingsDirty` 并保存

---

## 四、WebView2 重启机制

### 4.1 需要重启的情况
- 扩展启用状态变化（`extensionsEnabled`）
- 扩展目录路径变化（`extensionsPath`）
- 硬件加速设置变化（`hardwareAcceleration`）

### 4.2 重启流程
```
1. 保存当前 URL
2. 设置 shouldBrowserBeVisible = false
3. 销毁 WebView2 实例
4. 销毁环境 (webViewEnvironment)
5. 销毁面板 (panel)
6. 重新调用 InitializeBrowser()
7. 导航回保存的 URL
8. 设置 shouldBrowserBeVisible = true
```

### 4.3 用户体验
- 重启前弹出对话框确认
- 重启后自动恢复浏览页面
- 保持浏览历史记录

---

## 五、技术实现要点

### 5.1 核心 API
- `CoreWebView2EnvironmentOptions.AreBrowserExtensionsEnabled`
- `CoreWebView2EnvironmentOptions.AdditionalBrowserArguments`
- `CoreWebView2Environment.CreateAsync()`
- `browser.EnsureCoreWebView2Async()`

### 5.2 文件操作
- `System.IO.Compression.ZipFile.ExtractToDirectory()` - 解压 CRX
- `Directory.CreateDirectory()` - 创建扩展目录
- `BinaryWriter`/`BinaryReader` - 读写设置文件

### 5.3 UI 组件
- `TableLayoutPanel` - 主布局
- `Panel` - 按钮面板
- `TextBox` - 路径输入
- `NumericUpDown` - 缩放比例
- `ComboBox` - 深色模式选择
- `CheckBox` - 各种开关
- `Button` - 操作按钮
- `FolderBrowserDialog` - 选择目录
- `OpenFileDialog` - 选择 CRX 文件

---

## 六、用户使用流程

### 6.1 安装扩展
```
1. 打开 MusicBee → 右键 Browser2 → 设置
2. 点击"更多设置"
3. 设置扩展目录（可选，默认即可）
4. 点击"从 CRX 文件安装"
5. 选择 .crx 文件
6. 勾选"启用 Chromium 扩展"
7. 点击"保存"
8. 确认重启 WebView2
9. 点击"打开扩展管理页面"
10. 在扩展管理页面开启"开发者模式"
11. 扩展已安装并可用
```

### 6.2 管理扩展
```
1. 打开设置窗口
2. 点击"打开扩展管理页面"
3. 在 edge://extensions/ 页面中：
   - 启用/禁用扩展
   - 移除扩展
   - 查看详情
```

### 6.3 修改其他设置
```
1. 打开设置窗口
2. 修改需要的设置项
3. 点击"保存"
4. 如需重启则确认后重启
```

---

## 七、文件清单

### 7.1 修改的文件
- `Browser2.cs` - 主要实现文件

### 7.2 新增的文件
- `需求文档.md` - 本需求文档

### 7.3 依赖项
- `Microsoft.Web.WebView2` (版本 1.0.3908-prerelease)
- `System.IO.Compression` - 解压 CRX

---

## 八、注意事项

### 8.1 兼容性
- WebView2 版本需要支持 `AreBrowserExtensionsEnabled`
- 当前使用版本：1.0.3908-prerelease

### 8.2 安全性
- CRX 文件解压时注意异常处理
- 扩展目录需要写入权限

### 8.3 性能
- 设置加载使用标志避免重复读取
- WebView2 环境全局缓存，避免重复创建

### 8.4 已知限制
- 当前 WebView2 SDK 版本不支持 `GetBrowserExtensionsAsync()`
- 扩展管理通过内置页面实现，而非自定义 UI

---

## 九、编译和部署

### 9.1 编译命令
```bash
dotnet build a:\projectCSharp\MusicBeeBrowser\CSharpDll.csproj
```

### 9.2 部署位置
```
输出目录：bin\Debug\net462\
部署到：A:\Media\MusicBee\Plugins\
```

### 9.3 编译状态
✅ 编译成功，无错误
⚠️ 14 个警告（未使用字段，不影响功能）

---

## 十、版本历史

### v1.2 (当前版本)
- ✅ 添加"更多设置"按钮
- ✅ 实现完整的设置窗口
- ✅ 添加 Chromium 扩展支持
- ✅ 实现设置持久化
- ✅ 添加 WebView2 重启机制
- ✅ 使用 AreBrowserExtensionsEnabled API
- ✅ 扩展管理页面集成

### v1.1
- 基础 WebView2 浏览器功能

---

## 联系信息
- 插件作者：tumuyan
- 插件名称：Browser2
- 插件版本：1.0.2

---

*文档生成时间：2026-03-23*
