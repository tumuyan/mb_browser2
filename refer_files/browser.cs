// mb_WebBrowser, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MusicBeePlugin.Plugin
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.Win32;
using MusicBeePlugin;

public class Plugin
{
	public struct MusicBeeApiInterface
	{
		public short InterfaceVersion;

		public short ApiRevision;

		public MB_ReleaseStringDelegate MB_ReleaseString;

		public MB_TraceDelegate MB_Trace;

		public Setting_GetPersistentStoragePathDelegate Setting_GetPersistentStoragePath;

		public Setting_GetSkinDelegate Setting_GetSkin;

		public Setting_GetSkinElementColourDelegate Setting_GetSkinElementColour;

		public Setting_IsWindowBordersSkinnedDelegate Setting_IsWindowBordersSkinned;

		public Library_GetFilePropertyDelegate Library_GetFileProperty;

		public Library_GetFileTagDelegate Library_GetFileTag;

		public Library_SetFileTagDelegate Library_SetFileTag;

		public Library_CommitTagsToFileDelegate Library_CommitTagsToFile;

		public Library_GetLyricsDelegate Library_GetLyrics;

		public Library_GetArtworkDelegate Library_GetArtwork;

		public Library_QueryFilesDelegate Library_QueryFiles;

		public Library_QueryGetNextFileDelegate Library_QueryGetNextFile;

		public Player_GetPositionDelegate Player_GetPosition;

		public Player_SetPositionDelegate Player_SetPosition;

		public Player_GetPlayStateDelegate Player_GetPlayState;

		public Player_ActionDelegate Player_PlayPause;

		public Player_ActionDelegate Player_Stop;

		public Player_ActionDelegate Player_StopAfterCurrent;

		public Player_ActionDelegate Player_PlayPreviousTrack;

		public Player_ActionDelegate Player_PlayNextTrack;

		public Player_ActionDelegate Player_StartAutoDj;

		public Player_ActionDelegate Player_EndAutoDj;

		public Player_GetVolumeDelegate Player_GetVolume;

		public Player_SetVolumeDelegate Player_SetVolume;

		public Player_GetMuteDelegate Player_GetMute;

		public Player_SetMuteDelegate Player_SetMute;

		public Player_GetShuffleDelegate Player_GetShuffle;

		public Player_SetShuffleDelegate Player_SetShuffle;

		public Player_GetRepeatDelegate Player_GetRepeat;

		public Player_SetRepeatDelegate Player_SetRepeat;

		public Player_GetEqualiserEnabledDelegate Player_GetEqualiserEnabled;

		public Player_SetEqualiserEnabledDelegate Player_SetEqualiserEnabled;

		public Player_GetDspEnabledDelegate Player_GetDspEnabled;

		public Player_SetDspEnabledDelegate Player_SetDspEnabled;

		public Player_GetScrobbleEnabledDelegate Player_GetScrobbleEnabled;

		public Player_SetScrobbleEnabledDelegate Player_SetScrobbleEnabled;

		public NowPlaying_GetFileUrlDelegate NowPlaying_GetFileUrl;

		public NowPlaying_GetDurationDelegate NowPlaying_GetDuration;

		public NowPlaying_GetFilePropertyDelegate NowPlaying_GetFileProperty;

		public NowPlaying_GetFileTagDelegate NowPlaying_GetFileTag;

		public NowPlaying_GetLyricsDelegate NowPlaying_GetLyrics;

		public NowPlaying_GetArtworkDelegate NowPlaying_GetArtwork;

		public NowPlayingList_ActionDelegate NowPlayingList_Clear;

		public Library_QueryFilesDelegate NowPlayingList_QueryFiles;

		public Library_QueryGetNextFileDelegate NowPlayingList_QueryGetNextFile;

		public NowPlayingList_FileActionDelegate NowPlayingList_PlayNow;

		public NowPlayingList_FileActionDelegate NowPlayingList_QueueNext;

		public NowPlayingList_FileActionDelegate NowPlayingList_QueueLast;

		public NowPlayingList_ActionDelegate NowPlayingList_PlayLibraryShuffled;

		public Playlist_QueryPlaylistsDelegate Playlist_QueryPlaylists;

		public Playlist_QueryGetNextPlaylistDelegate Playlist_QueryGetNextPlaylist;

		public Playlist_GetTypeDelegate Playlist_GetType;

		public Playlist_QueryFilesDelegate Playlist_QueryFiles;

		public Library_QueryGetNextFileDelegate Playlist_QueryGetNextFile;

		public MB_WindowHandleDelegate MB_GetWindowHandle;

		public MB_RefreshPanelsDelegate MB_RefreshPanels;

		public MB_SendNotificationDelegate MB_SendNotification;

		public MB_AddMenuItemDelegate MB_AddMenuItem;

		public Setting_GetFieldNameDelegate Setting_GetFieldName;

		public Library_QueryGetAllFilesDelegate Library_QueryGetAllFiles;

		public Library_QueryGetAllFilesDelegate NowPlayingList_QueryGetAllFiles;

		public Library_QueryGetAllFilesDelegate Playlist_QueryGetAllFiles;

		public MB_CreateBackgroundTaskDelegate MB_CreateBackgroundTask;

		public MB_SetBackgroundTaskMessageDelegate MB_SetBackgroundTaskMessage;

		public MB_RegisterCommandDelegate MB_RegisterCommand;

		public Setting_GetDefaultFontDelegate Setting_GetDefaultFont;

		public Player_GetShowTimeRemainingDelegate Player_GetShowTimeRemaining;

		public NowPlayingList_GetCurrentIndexDelegate NowPlayingList_GetCurrentIndex;

		public NowPlayingList_GetFileUrlDelegate NowPlayingList_GetListFileUrl;

		public NowPlayingList_GetFilePropertyDelegate NowPlayingList_GetFileProperty;

		public NowPlayingList_GetFileTagDelegate NowPlayingList_GetFileTag;

		public NowPlaying_GetSpectrumDataDelegate NowPlaying_GetSpectrumData;

		public NowPlaying_GetSoundGraphDelegate NowPlaying_GetSoundGraph;

		public MB_GetPanelBoundsDelegate MB_GetPanelBounds;

		public MB_AddPanelDelegate MB_AddPanel;

		public MB_RemovePanelDelegate MB_RemovePanel;

		public MB_GetLocalisationDelegate MB_GetLocalisation;

		public NowPlayingList_IsAnyPriorTracksDelegate NowPlayingList_IsAnyPriorTracks;

		public NowPlayingList_IsAnyFollowingTracksDelegate NowPlayingList_IsAnyFollowingTracks;

		public Player_ShowEqualiserDelegate Player_ShowEqualiser;

		public Player_GetAutoDjEnabledDelegate Player_GetAutoDjEnabled;

		public Player_GetStopAfterCurrentEnabledDelegate Player_GetStopAfterCurrentEnabled;

		public Player_GetCrossfadeDelegate Player_GetCrossfade;

		public Player_SetCrossfadeDelegate Player_SetCrossfade;

		public Player_GetReplayGainModeDelegate Player_GetReplayGainMode;

		public Player_SetReplayGainModeDelegate Player_SetReplayGainMode;

		public Player_QueueRandomTracksDelegate Player_QueueRandomTracks;

		public Setting_GetDataTypeDelegate Setting_GetDataType;

		public NowPlayingList_GetNextIndexDelegate NowPlayingList_GetNextIndex;

		public NowPlaying_GetArtistPictureDelegate NowPlaying_GetArtistPicture;

		public NowPlaying_GetArtworkDelegate NowPlaying_GetDownloadedArtwork;

		public MB_ShowNowPlayingAssistantDelegate MB_ShowNowPlayingAssistant;

		public NowPlaying_GetLyricsDelegate NowPlaying_GetDownloadedLyrics;

		public Player_GetShowRatingTrackDelegate Player_GetShowRatingTrack;

		public Player_GetShowRatingLoveDelegate Player_GetShowRatingLove;

		public MB_CreateParameterisedBackgroundTaskDelegate MB_CreateParameterisedBackgroundTask;

		public Setting_GetLastFmUserIdDelegate Setting_GetLastFmUserId;

		public Playlist_GetNameDelegate Playlist_GetName;

		public Playlist_CreatePlaylistDelegate Playlist_CreatePlaylist;

		public Playlist_SetFilesDelegate Playlist_SetFiles;

		public Library_QuerySimilarArtistsDelegate Library_QuerySimilarArtists;

		public Library_QueryLookupTableDelegate Library_QueryLookupTable;

		public Library_QueryGetLookupTableValueDelegate Library_QueryGetLookupTableValue;

		public NowPlayingList_FilesActionDelegate NowPlayingList_QueueFilesNext;

		public NowPlayingList_FilesActionDelegate NowPlayingList_QueueFilesLast;

		public Setting_GetWebProxyDelegate Setting_GetWebProxy;

		public NowPlayingList_RemoveAtDelegate NowPlayingList_RemoveAt;

		public Playlist_RemoveAtDelegate Playlist_RemoveAt;

		public MB_SetPanelScrollableAreaDelegate MB_SetPanelScrollableArea;

		public MB_InvokeCommandDelegate MB_InvokeCommand;

		public MB_OpenFilterInTabDelegate MB_OpenFilterInTab;

		public MB_SetWindowSizeDelegate MB_SetWindowSize;

		public Library_GetArtistPictureDelegate Library_GetArtistPicture;

		public Pending_GetFileUrlDelegate Pending_GetFileUrl;

		public Pending_GetFilePropertyDelegate Pending_GetFileProperty;

		public Pending_GetFileTagDelegate Pending_GetFileTag;

		public Player_GetButtonEnabledDelegate Player_GetButtonEnabled;

		public NowPlayingList_MoveFilesDelegate NowPlayingList_MoveFiles;

		public Library_GetArtworkDelegate Library_GetArtworkUrl;

		public Library_GetArtistPictureThumbDelegate Library_GetArtistPictureThumb;

		public NowPlaying_GetArtworkDelegate NowPlaying_GetArtworkUrl;

		public NowPlaying_GetArtworkDelegate NowPlaying_GetDownloadedArtworkUrl;

		public NowPlaying_GetArtistPictureThumbDelegate NowPlaying_GetArtistPictureThumb;

		public Playlist_IsInListDelegate Playlist_IsInList;

		public Library_GetArtistPictureUrlsDelegate Library_GetArtistPictureUrls;

		public NowPlaying_GetArtistPictureUrlsDelegate NowPlaying_GetArtistPictureUrls;

		public Playlist_AddFilesDelegate Playlist_AddFiles;

		public Sync_FileStartDelegate Sync_FileStart;

		public Sync_FileEndDelegate Sync_FileEnd;

		public Library_QueryFilesExDelegate Library_QueryFilesEx;

		public Library_QueryFilesExDelegate NowPlayingList_QueryFilesEx;

		public Playlist_QueryFilesExDelegate Playlist_QueryFilesEx;

		public Playlist_MoveFilesDelegate Playlist_MoveFiles;

		public Playlist_PlayNowDelegate Playlist_PlayNow;

		public NowPlaying_IsSoundtrackDelegate NowPlaying_IsSoundtrack;

		public NowPlaying_GetArtistPictureUrlsDelegate NowPlaying_GetSoundtrackPictureUrls;

		public Library_GetDevicePersistentIdDelegate Library_GetDevicePersistentId;

		public Library_SetDevicePersistentIdDelegate Library_SetDevicePersistentId;

		public Library_FindDevicePersistentIdDelegate Library_FindDevicePersistentId;

		public Setting_GetValueDelegate Setting_GetValue;

		public Library_AddFileToLibraryDelegate Library_AddFileToLibrary;

		public Playlist_DeletePlaylistDelegate Playlist_DeletePlaylist;

		public Library_GetSyncDeltaDelegate Library_GetSyncDelta;

		public Library_GetFileTagsDelegate Library_GetFileTags;

		public NowPlaying_GetFileTagsDelegate NowPlaying_GetFileTags;

		public NowPlayingList_GetFileTagsDelegate NowPlayingList_GetFileTags;

		public MB_AddTreeNodeDelegate MB_AddTreeNode;

		public MB_DownloadFileDelegate MB_DownloadFile;

		public MusicBeeVersion MusicBeeVersion
		{
			get
			{
				if (ApiRevision <= 25)
				{
					return MusicBeeVersion.v2_0;
				}
				if (ApiRevision <= 30)
				{
					return MusicBeeVersion.v2_1;
				}
				if (ApiRevision <= 33)
				{
					return MusicBeeVersion.v2_2;
				}
				if (ApiRevision <= 38)
				{
					return MusicBeeVersion.v2_3;
				}
				return MusicBeeVersion.v2_4;
			}
		}
	}

	public enum MusicBeeVersion
	{
		v2_0,
		v2_1,
		v2_2,
		v2_3,
		v2_4
	}

	public enum PluginType
	{
		Unknown = 0,
		General = 1,
		LyricsRetrieval = 2,
		ArtworkRetrieval = 3,
		PanelView = 4,
		DataStream = 5,
		InstantMessenger = 6,
		Storage = 7,
		VideoPlayer = 8,
		DSP = 9,
		WebBrowser = 13
	}

	[StructLayout(LayoutKind.Sequential)]
	public class PluginInfo
	{
		public short PluginInfoVersion;

		public PluginType Type;

		public string Name;

		public string Description;

		public string Author;

		public string TargetApplication;

		public short VersionMajor;

		public short VersionMinor;

		public short Revision;

		public short MinInterfaceVersion;

		public short MinApiRevision;

		public ReceiveNotificationFlags ReceiveNotifications;

		public int ConfigurationPanelHeight;
	}

	[Flags]
	public enum ReceiveNotificationFlags
	{
		StartupOnly = 0,
		PlayerEvents = 1,
		DataStreamEvents = 2,
		TagEvents = 4,
		DownloadEvents = 8
	}

	public enum NotificationType
	{
		PluginStartup = 0,
		TrackChanging = 16,
		TrackChanged = 1,
		PlayStateChanged = 2,
		AutoDjStarted = 3,
		AutoDjStopped = 4,
		VolumeMuteChanged = 5,
		VolumeLevelChanged = 6,
		NowPlayingListChanged = 7,
		NowPlayingListEnded = 18,
		NowPlayingArtworkReady = 8,
		NowPlayingLyricsReady = 9,
		TagsChanging = 10,
		TagsChanged = 11,
		RatingChanging = 15,
		RatingChanged = 12,
		PlayCountersChanged = 13,
		ScreenSaverActivating = 14,
		ShutdownStarted = 17,
		EmbedInPanel = 19,
		PlayerRepeatChanged = 20,
		PlayerShuffleChanged = 21,
		PlayerEqualiserOnOffChanged = 22,
		PlayerScrobbleChanged = 23,
		ReplayGainChanged = 24,
		FileDeleting = 25,
		FileDeleted = 26,
		ApplicationWindowChanged = 27,
		StopAfterCurrentChanged = 28,
		LibrarySwitched = 29,
		FiledAddedToLibrary = 30,
		FileAddedToInbox = 31,
		SyncCompleted = 32,
		DownloadCompleted = 33
	}

	public enum CallbackType
	{
		SettingsUpdated = 1,
		StorageReady,
		StorageFailed,
		FilesRetrievedChanged,
		FilesRetrievedNoChange,
		FilesRetrievedFail,
		LyricsDownloaded,
		StorageEject
	}

	public enum PluginCloseReason
	{
		MusicBeeClosing = 1,
		UserDisabled,
		StopNoUnload
	}

	public enum FilePropertyType
	{
		Url = 2,
		Kind = 4,
		Format = 5,
		Size = 7,
		Channels = 8,
		SampleRate = 9,
		Bitrate = 10,
		DateModified = 11,
		DateAdded = 12,
		LastPlayed = 13,
		PlayCount = 14,
		SkipCount = 15,
		Duration = 16,
		Status = 21,
		NowPlayingListIndex = 78,
		ReplayGainTrack = 94,
		ReplayGainAlbum = 95
	}

	public enum MetaDataType
	{
		TrackTitle = 65,
		Album = 30,
		AlbumArtist = 31,
		AlbumArtistRaw = 34,
		Artist = 32,
		MultiArtist = 33,
		PrimaryArtist = 19,
		Artists = 144,
		ArtistsWithArtistRole = 145,
		ArtistsWithPerformerRole = 146,
		ArtistsWithGuestRole = 147,
		ArtistsWithRemixerRole = 148,
		Artwork = 40,
		BeatsPerMin = 41,
		Composer = 43,
		MultiComposer = 89,
		Comment = 44,
		Conductor = 45,
		Custom1 = 46,
		Custom2 = 47,
		Custom3 = 48,
		Custom4 = 49,
		Custom5 = 50,
		Custom6 = 96,
		Custom7 = 97,
		Custom8 = 98,
		Custom9 = 99,
		Custom10 = 128,
		Custom11 = 129,
		Custom12 = 130,
		Custom13 = 131,
		Custom14 = 132,
		Custom15 = 133,
		Custom16 = 134,
		DiscNo = 52,
		DiscCount = 54,
		Encoder = 55,
		Genre = 59,
		Genres = 143,
		GenreCategory = 60,
		Grouping = 61,
		Keywords = 84,
		HasLyrics = 63,
		Lyricist = 62,
		Lyrics = 114,
		Mood = 64,
		Occasion = 66,
		Origin = 67,
		Publisher = 73,
		Quality = 74,
		Rating = 75,
		RatingLove = 76,
		RatingAlbum = 104,
		Tempo = 85,
		TrackNo = 86,
		TrackCount = 87,
		Virtual1 = 109,
		Virtual2 = 110,
		Virtual3 = 111,
		Virtual4 = 112,
		Virtual5 = 113,
		Virtual6 = 122,
		Virtual7 = 123,
		Virtual8 = 124,
		Virtual9 = 125,
		Virtual10 = 135,
		Virtual11 = 136,
		Virtual12 = 137,
		Virtual13 = 138,
		Virtual14 = 139,
		Virtual15 = 140,
		Virtual16 = 141,
		Year = 88
	}

	[Flags]
	public enum LibraryCategory
	{
		Music = 0,
		Audiobook = 1,
		Video = 2,
		Inbox = 4
	}

	public enum DeviceIdType
	{
		GooglePlay = 1,
		AppleDevice,
		GooglePlay2,
		AppleDevice2
	}

	public enum DataType
	{
		String,
		Number,
		DateTime,
		Rating
	}

	public enum SettingId
	{
		CompactPlayerFlickrEnabled = 1,
		FileTaggingPreserveModificationTime = 2,
		LastDownloadFolder = 3,
		CustomWebLinkName1 = 11,
		CustomWebLinkName2 = 12,
		CustomWebLinkName3 = 13,
		CustomWebLinkName4 = 14,
		CustomWebLinkName5 = 15,
		CustomWebLinkName6 = 16,
		CustomWebLinkName7 = 29,
		CustomWebLinkName8 = 30,
		CustomWebLinkName9 = 31,
		CustomWebLinkName10 = 32,
		CustomWebLink1 = 17,
		CustomWebLink2 = 18,
		CustomWebLink3 = 19,
		CustomWebLink4 = 20,
		CustomWebLink5 = 21,
		CustomWebLink6 = 22,
		CustomWebLink7 = 33,
		CustomWebLink8 = 34,
		CustomWebLink9 = 35,
		CustomWebLink10 = 36,
		CustomWebLinkNowPlaying1 = 23,
		CustomWebLinkNowPlaying2 = 24,
		CustomWebLinkNowPlaying3 = 25,
		CustomWebLinkNowPlaying4 = 26,
		CustomWebLinkNowPlaying5 = 27,
		CustomWebLinkNowPlaying6 = 28,
		CustomWebLinkNowPlaying7 = 37,
		CustomWebLinkNowPlaying8 = 38,
		CustomWebLinkNowPlaying9 = 39,
		CustomWebLinkNowPlaying10 = 40
	}

	public enum ComparisonType
	{
		Is = 0,
		IsSimilar = 20
	}

	public enum LyricsType
	{
		NotSpecified,
		Synchronised,
		UnSynchronised
	}

	public enum PlayState
	{
		Undefined = 0,
		Loading = 1,
		Playing = 3,
		Paused = 6,
		Stopped = 7
	}

	public enum RepeatMode
	{
		None,
		All,
		One
	}

	public enum PlayButtonType
	{
		PreviousTrack,
		PlayPause,
		NextTrack,
		Stop
	}

	public enum PlaylistFormat
	{
		Unknown = 0,
		M3u = 1,
		Xspf = 2,
		Asx = 3,
		Wpl = 4,
		Pls = 5,
		Auto = 7,
		M3uAscii = 8,
		AsxFile = 9,
		Radio = 10,
		M3uExtended = 11,
		Mbp = 12
	}

	public enum SkinElement
	{
		SkinInputControl = 7,
		SkinInputPanel = 10,
		SkinInputPanelLabel = 14,
		SkinTrackAndArtistPanel = -1
	}

	public enum ElementState
	{
		ElementStateDefault = 0,
		ElementStateModified = 6
	}

	public enum ElementComponent
	{
		ComponentBorder = 0,
		ComponentBackground = 1,
		ComponentForeground = 3
	}

	public enum PluginPanelDock
	{
		ApplicationWindow = 0,
		TrackAndArtistPanel = 1,
		TextBox = 3,
		ComboBox = 4,
		MainPanel = 5
	}

	public enum ReplayGainMode
	{
		Off,
		Track,
		Album,
		Smart
	}

	public enum Command
	{
		NavigateTo = 1
	}

	public enum DownloadTarget
	{
		Inbox = 0,
		MusicLibrary = 1,
		SpecificFolder = 3
	}

	public delegate void MB_ReleaseStringDelegate(string p1);

	public delegate void MB_TraceDelegate(string p1);

	public delegate IntPtr MB_WindowHandleDelegate();

	public delegate void MB_RefreshPanelsDelegate();

	public delegate void MB_SendNotificationDelegate(CallbackType type);

	public delegate ToolStripItem MB_AddMenuItemDelegate(string menuPath, string hotkeyDescription, EventHandler handler);

	public delegate bool MB_AddTreeNodeDelegate(string treePath, string name, Bitmap icon, EventHandler openHandler, EventHandler closeHandler);

	public delegate void MB_RegisterCommandDelegate(string command, EventHandler handler);

	public delegate void MB_CreateBackgroundTaskDelegate(ThreadStart taskCallback, Form owner);

	public delegate void MB_CreateParameterisedBackgroundTaskDelegate(ParameterizedThreadStart taskCallback, object parameters, Form owner);

	public delegate void MB_SetBackgroundTaskMessageDelegate(string message);

	public delegate Rectangle MB_GetPanelBoundsDelegate(PluginPanelDock dock);

	public delegate bool MB_SetPanelScrollableAreaDelegate(Control panel, Size scrollArea, bool alwaysShowScrollBar);

	public delegate Control MB_AddPanelDelegate(Control panel, PluginPanelDock dock);

	public delegate void MB_RemovePanelDelegate(Control panel);

	public delegate string MB_GetLocalisationDelegate(string id, string defaultText);

	public delegate bool MB_ShowNowPlayingAssistantDelegate();

	public delegate bool MB_InvokeCommandDelegate(Command command, object parameter);

	public delegate bool MB_OpenFilterInTabDelegate(MetaDataType field1, ComparisonType comparison1, string value1, MetaDataType field2, ComparisonType comparison1, string value2);

	public delegate bool MB_SetWindowSizeDelegate(int width, int height);

	public delegate bool MB_DownloadFileDelegate(string url, DownloadTarget target, string targetFolder, bool cancelDownload);

	public delegate string Setting_GetFieldNameDelegate(MetaDataType field);

	public delegate string Setting_GetPersistentStoragePathDelegate();

	public delegate string Setting_GetSkinDelegate();

	public delegate int Setting_GetSkinElementColourDelegate(SkinElement element, ElementState state, ElementComponent component);

	public delegate bool Setting_IsWindowBordersSkinnedDelegate();

	public delegate Font Setting_GetDefaultFontDelegate();

	public delegate DataType Setting_GetDataTypeDelegate(MetaDataType field);

	public delegate string Setting_GetLastFmUserIdDelegate();

	public delegate string Setting_GetWebProxyDelegate();

	public delegate bool Setting_GetValueDelegate(SettingId id, ref object value);

	public delegate string Library_GetFilePropertyDelegate(string sourceFileUrl, FilePropertyType type);

	public delegate string Library_GetFileTagDelegate(string sourceFileUrl, MetaDataType field);

	public delegate bool Library_GetFileTagsDelegate(string sourceFileUrl, MetaDataType[] fields, ref string[] results);

	public delegate bool Library_SetFileTagDelegate(string sourceFileUrl, MetaDataType field, string value);

	public delegate string Library_GetDevicePersistentIdDelegate(string sourceFileUrl, DeviceIdType idType);

	public delegate bool Library_SetDevicePersistentIdDelegate(string sourceFileUrl, DeviceIdType idType, string value);

	public delegate bool Library_FindDevicePersistentIdDelegate(int idType, string[] ids, ref string[] values);

	public delegate bool Library_CommitTagsToFileDelegate(string sourceFileUrl);

	public delegate string Library_AddFileToLibraryDelegate(string sourceFileUrl, LibraryCategory category);

	public delegate bool Library_GetSyncDeltaDelegate(string[] cachedFiles, DateTime updatedSince, LibraryCategory categories, ref string[] newFiles, ref string[] updatedFiles, ref string[] deletedFiles);

	public delegate string Library_GetLyricsDelegate(string sourceFileUrl, LyricsType type);

	public delegate string Library_GetArtworkDelegate(string sourceFileUrl, int index);

	public delegate string Library_GetArtistPictureDelegate(string artistName, int fadingPercent, int fadingColor);

	public delegate bool Library_GetArtistPictureUrlsDelegate(string artistName, bool localOnly, ref string[] urls);

	public delegate string Library_GetArtistPictureThumbDelegate(string artistName);

	public delegate bool Library_QueryFilesDelegate(string query);

	public delegate string Library_QueryGetNextFileDelegate();

	public delegate string Library_QueryGetAllFilesDelegate();

	public delegate bool Library_QueryFilesExDelegate(string query, ref string[] files);

	public delegate string Library_QuerySimilarArtistsDelegate(string artistName, double minimumArtistSimilarityRating);

	public delegate bool Library_QueryLookupTableDelegate(string keyTags, string valueTags, string query);

	public delegate string Library_QueryGetLookupTableValueDelegate(string key);

	public delegate int Player_GetPositionDelegate();

	public delegate bool Player_SetPositionDelegate(int position);

	public delegate PlayState Player_GetPlayStateDelegate();

	public delegate bool Player_GetButtonEnabledDelegate(PlayButtonType button);

	public delegate bool Player_ActionDelegate();

	public delegate int Player_QueueRandomTracksDelegate(int count);

	public delegate float Player_GetVolumeDelegate();

	public delegate bool Player_SetVolumeDelegate(float volume);

	public delegate bool Player_GetMuteDelegate();

	public delegate bool Player_SetMuteDelegate(bool mute);

	public delegate bool Player_GetShuffleDelegate();

	public delegate bool Player_SetShuffleDelegate(bool shuffle);

	public delegate RepeatMode Player_GetRepeatDelegate();

	public delegate bool Player_SetRepeatDelegate(RepeatMode repeat);

	public delegate bool Player_GetEqualiserEnabledDelegate();

	public delegate bool Player_SetEqualiserEnabledDelegate(bool enabled);

	public delegate bool Player_GetDspEnabledDelegate();

	public delegate bool Player_SetDspEnabledDelegate(bool enabled);

	public delegate bool Player_GetScrobbleEnabledDelegate();

	public delegate bool Player_SetScrobbleEnabledDelegate(bool enabled);

	public delegate bool Player_GetShowTimeRemainingDelegate();

	public delegate bool Player_GetShowRatingTrackDelegate();

	public delegate bool Player_GetShowRatingLoveDelegate();

	public delegate bool Player_ShowEqualiserDelegate();

	public delegate bool Player_GetAutoDjEnabledDelegate();

	public delegate bool Player_GetStopAfterCurrentEnabledDelegate();

	public delegate bool Player_GetCrossfadeDelegate();

	public delegate bool Player_SetCrossfadeDelegate(bool crossfade);

	public delegate ReplayGainMode Player_GetReplayGainModeDelegate();

	public delegate bool Player_SetReplayGainModeDelegate(ReplayGainMode mode);

	public delegate string NowPlaying_GetFileUrlDelegate();

	public delegate int NowPlaying_GetDurationDelegate();

	public delegate string NowPlaying_GetFilePropertyDelegate(FilePropertyType type);

	public delegate string NowPlaying_GetFileTagDelegate(MetaDataType field);

	public delegate bool NowPlaying_GetFileTagsDelegate(MetaDataType[] fields, ref string[] results);

	public delegate string NowPlaying_GetLyricsDelegate();

	public delegate string NowPlaying_GetArtworkDelegate();

	public delegate string NowPlaying_GetArtistPictureDelegate(int fadingPercent);

	public delegate bool NowPlaying_GetArtistPictureUrlsDelegate(bool localOnly, ref string[] urls);

	public delegate string NowPlaying_GetArtistPictureThumbDelegate();

	public delegate bool NowPlaying_IsSoundtrackDelegate();

	public delegate int NowPlaying_GetSpectrumDataDelegate(float[] fftData);

	public delegate bool NowPlaying_GetSoundGraphDelegate(float[] graphData);

	public delegate int NowPlayingList_GetCurrentIndexDelegate();

	public delegate int NowPlayingList_GetNextIndexDelegate(int offset);

	public delegate string NowPlayingList_GetFileUrlDelegate(int index);

	public delegate string NowPlayingList_GetFilePropertyDelegate(int index, FilePropertyType type);

	public delegate string NowPlayingList_GetFileTagDelegate(int index, MetaDataType field);

	public delegate bool NowPlayingList_GetFileTagsDelegate(int index, MetaDataType[] fields, ref string[] results);

	public delegate bool NowPlayingList_ActionDelegate();

	public delegate bool NowPlayingList_FileActionDelegate(string sourceFileUrl);

	public delegate bool NowPlayingList_FilesActionDelegate(string[] sourceFileUrls);

	public delegate bool NowPlayingList_IsAnyPriorTracksDelegate();

	public delegate bool NowPlayingList_IsAnyFollowingTracksDelegate();

	public delegate bool NowPlayingList_RemoveAtDelegate(int index);

	public delegate bool NowPlayingList_MoveFilesDelegate(int[] fromIndices, int toIndex);

	public delegate string Playlist_GetNameDelegate(string playlistUrl);

	public delegate PlaylistFormat Playlist_GetTypeDelegate(string playlistUrl);

	public delegate bool Playlist_IsInListDelegate(string playlistUrl, string filename);

	public delegate bool Playlist_QueryPlaylistsDelegate();

	public delegate string Playlist_QueryGetNextPlaylistDelegate();

	public delegate bool Playlist_QueryFilesDelegate(string playlistUrl);

	public delegate bool Playlist_QueryFilesExDelegate(string playlistUrl, ref string[] filenames);

	public delegate string Playlist_CreatePlaylistDelegate(string folderName, string playlistName, string[] filenames);

	public delegate bool Playlist_DeletePlaylistDelegate(string playlistUrl);

	public delegate bool Playlist_SetFilesDelegate(string playlistUrl, string[] filenames);

	public delegate bool Playlist_AddFilesDelegate(string playlistUrl, string[] filenames);

	public delegate bool Playlist_RemoveAtDelegate(string playlistUrl, int index);

	public delegate bool Playlist_MoveFilesDelegate(string playlistUrl, int[] fromIndices, int toIndex);

	public delegate bool Playlist_PlayNowDelegate(string playlistUrl);

	public delegate string Pending_GetFileUrlDelegate();

	public delegate string Pending_GetFilePropertyDelegate(FilePropertyType field);

	public delegate string Pending_GetFileTagDelegate(MetaDataType field);

	public delegate string Sync_FileStartDelegate(string filename);

	public delegate void Sync_FileEndDelegate(string filename, bool success, string errorMessage);

	private delegate void ExecuteItemScannedDelegate(MediaFile file);

	private sealed class PopupContainer : Form, IMessageFilter
	{
		public delegate void FavoriteClickedEventHandler(Bookmark bookmark);

		public delegate void FavoriteRemovedEventHandler(int index);

		private ListBox list;

		private List<Bookmark> favourites;

		private int hotIndex;

		private bool isContextMenuOpen;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				if (Environment.OSVersion.Version.Major >= 6)
				{
					createParams.ExStyle |= 33554432;
				}
				createParams.ClassStyle |= 131072;
				return createParams;
			}
		}

		public event FavoriteClickedEventHandler FavoriteClicked;

		public event FavoriteRemovedEventHandler FavoriteRemoved;

		public PopupContainer(Rectangle bounds, List<Bookmark> favourites)
		{
			list = new ListBox();
			hotIndex = -1;
			isContextMenuOpen = false;
			this.favourites = favourites;
			BackColor = Color.White;
			Text = null;
			base.ControlBox = false;
			base.FormBorderStyle = FormBorderStyle.None;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.Manual;
			list.Font = mbApiInterface.Setting_GetDefaultFont();
			list.ItemHeight = ((list.Font.Height < 17) ? 20 : (list.Font.Height + 4));
			bounds.Height = ((favourites.Count == 0) ? list.ItemHeight : (favourites.Count * list.ItemHeight)) + 20;
			list.BorderStyle = BorderStyle.None;
			list.BackColor = Color.White;
			list.ForeColor = Color.Black;
			list.ScrollAlwaysVisible = false;
			list.Bounds = new Rectangle(10, 10, bounds.Width - 11, bounds.Height - 20);
			list.SelectionMode = SelectionMode.One;
			list.DrawMode = DrawMode.OwnerDrawFixed;
			list.BeginUpdate();
			if (favourites.Count == 0)
			{
				list.Items.Add("No bookmarks have been saved");
			}
			else
			{
				int num = favourites.Count - 1;
				for (int i = 0; i <= num; i++)
				{
					list.Items.Add(favourites[i].Name);
				}
			}
			list.EndUpdate();
			list.ContextMenuStrip = new ContextMenuStrip();
			list.ContextMenuStrip.Items.Add(new ToolStripMenuItem("Remove Bookmark", null, removeBookmarkMenuItem_Click));
			base.Controls.Add(list);
			base.Bounds = bounds;
			base.ActiveControl = list;
			list.DrawItem += list_DrawItem;
			list.KeyDown += list_KeyDown;
			list.MouseMove += list_MouseMove;
			list.MouseLeave += list_MouseLeave;
			list.ContextMenuStrip.Opening += listContextMenu_Opening;
			list.ContextMenuStrip.Closing += listContextMenu_Closing;
		}

		protected override void Dispose(bool disposing)
		{
			Application.RemoveMessageFilter(this);
			list.ContextMenuStrip.Dispose();
			base.Dispose(disposing);
		}

		protected override void OnShown(EventArgs e)
		{
			Application.AddMessageFilter(this);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			using Pen pen = new Pen(Color.FromArgb(210, 210, 210));
			e.Graphics.DrawRectangle(pen, 0, 0, base.Width - 1, base.Height - 1);
		}

		private void list_DrawItem(object sender, DrawItemEventArgs e)
		{
			Color color = ((e.Index != hotIndex && e.Index != list.SelectedIndex) ? Color.White : Color.FromArgb(235, 235, 235));
			Rectangle bounds = e.Bounds;
			bounds.Width -= 10;
			using (SolidBrush brush = new SolidBrush(color))
			{
				e.Graphics.FillRectangle(brush, bounds);
			}
			if (favourites.Count > 0)
			{
				bounds.X += 22;
				bounds.Width -= 32;
				int num = bounds.Top + (bounds.Height - 16) / 2;
				if (favourites[e.Index].Icon == null)
				{
					e.Graphics.DrawRectangle(Pens.Silver, new Rectangle(4, num, 16, 16));
				}
				else
				{
					e.Graphics.DrawImage(favourites[e.Index].Icon, new Point(4, num));
				}
			}
			TextRenderer.DrawText(e.Graphics, list.Items[e.Index].ToString(), list.Font, bounds, Color.Black, color, TextFormatFlags.EndEllipsis | TextFormatFlags.HidePrefix | TextFormatFlags.VerticalCenter | TextFormatFlags.LeftAndRightPadding);
		}

		private void list_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Dispose();
			}
		}

		private void list_MouseMove(object sender, MouseEventArgs e)
		{
			int num = list.IndexFromPoint(list.PointToClient(Cursor.Position));
			if (num != hotIndex)
			{
				hotIndex = num;
				list.Invalidate();
			}
		}

		private void list_MouseLeave(object sender, EventArgs e)
		{
			if (hotIndex != -1)
			{
				hotIndex = -1;
				list.Invalidate();
			}
		}

		private void listContextMenu_Opening(object sender, CancelEventArgs e)
		{
			isContextMenuOpen = true;
			list.ContextMenuStrip.Items[0].Enabled = list.SelectedIndex >= 0 && list.SelectedIndex < favourites.Count && !favourites[list.SelectedIndex].Custom;
		}

		private void listContextMenu_Closing(object sender, CancelEventArgs e)
		{
			isContextMenuOpen = false;
		}

		private void removeBookmarkMenuItem_Click(object sender, EventArgs e)
		{
			if (list.SelectedIndex >= 0 && list.SelectedIndex < favourites.Count)
			{
				FavoriteRemoved?.Invoke(list.SelectedIndex);
				Dispose();
			}
		}

		private bool MouseEvenHandler(ref Message m)
		{
			if (m.Msg == 513 || m.Msg == 516 || m.Msg == 519)
			{
				try
				{
					if (m.HWnd != list.Handle)
					{
						if (!isContextMenuOpen)
						{
							Dispose();
						}
					}
					else
					{
						int num = list.IndexFromPoint(list.PointToClient(Cursor.Position));
						if (m.Msg == 513 && num >= 0 && num < favourites.Count)
						{
							FavoriteClicked?.Invoke(favourites[num]);
							Dispose();
						}
						else
						{
							list.SelectedIndex = num;
						}
					}
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					ProjectData.ClearProjectError();
				}
			}
			return false;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			//ILSpy generated this explicit interface implementation from .override directive in MouseEvenHandler
			return this.MouseEvenHandler(ref m);
		}
	}

	private struct Bookmark
	{
		public string Url;

		public string Name;

		public Bitmap Icon;

		public bool Custom;

		public Bookmark(string url, string name, Bitmap icon, bool custom = false)
		{
			this = default(Bookmark);
			Url = url;
			Name = name;
			Icon = icon;
			Custom = custom;
		}
	}

	private sealed class BookmarkComparer : Comparer<Bookmark>
	{
		public override int Compare(Bookmark bookmark1, Bookmark bookmark2)
		{
			return string.Compare(bookmark1.Name, bookmark2.Name, StringComparison.OrdinalIgnoreCase);
		}
	}

	private sealed class MediaWebBrowser : WebBrowser, IMessageFilter
	{
		public delegate void NavigationLinkClickedEventHandler(object sender, string url);

		public delegate void LinkPlayUrlEventHandler(object sender, string url, string mimeType);

		public delegate void ItemScannedEventHandler(object sender, MediaFile file);

		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr parameter);

		public int ItemCount;

		private int itemsPendingCount;

		private string lastMouseOverUrl;

		private string lastStatusText;

		private bool browserLinkClicked;

		private Thread scannerThread;

		private bool navigationDocumentComplete;

		private bool navigationScanActive;

		private bool navigationScanComplete;

		private ManualResetEvent initialDownloadComplete;

		private IntPtr DocumentHandle
		{
			get
			{
				List<IntPtr> list = GetChildWindows(base.Handle);
				while (list.Count > 0)
				{
					List<IntPtr> childWindows = GetChildWindows(list[0]);
					if (childWindows.Count == 0)
					{
						return list[0];
					}
					list = childWindows;
				}
				return IntPtr.Zero;
			}
		}

		public event NavigationLinkClickedEventHandler NavigationLinkClicked;

		public event EventHandler NavigationLoading;

		public event EventHandler NavigationCompleted;

		public event LinkPlayUrlEventHandler LinkPlayUrl;

		public event EventHandler ScanStarting;

		public event ItemScannedEventHandler ItemScanned;

		public event EventHandler ScanCompleted;

		public MediaWebBrowser()
		{
			ItemCount = 0;
			itemsPendingCount = 0;
			lastMouseOverUrl = "";
			lastStatusText = "";
			scannerThread = null;
			navigationDocumentComplete = true;
			navigationScanActive = false;
			navigationScanComplete = true;
			initialDownloadComplete = new ManualResetEvent(initialState: false);
			base.ScriptErrorsSuppressed = true;
			Application.AddMessageFilter(this);
		}

		protected override void Dispose(bool disposing)
		{
			Application.RemoveMessageFilter(this);
			TerminateFileScanner();
			initialDownloadComplete.Dispose();
			base.Dispose(disposing);
		}

		public void PostMouseWheelMessage(MouseEventArgs e)
		{
			PostMessage(DocumentHandle, 522, new IntPtr(e.Delta << 16), new IntPtr((e.Y << 16) | e.X));
		}

		public new void Navigate(string url)
		{
			lastMouseOverUrl = "";
			browserLinkClicked = true;
			base.Navigate(url);
		}

		public new void GoBack()
		{
			lastMouseOverUrl = "";
			browserLinkClicked = false;
			base.GoBack();
		}

		public new void GoForward()
		{
			lastMouseOverUrl = "";
			browserLinkClicked = false;
			base.GoForward();
		}

		public new void Refresh()
		{
			lastMouseOverUrl = "";
			browserLinkClicked = false;
			base.Refresh();
		}

		public new void Stop()
		{
			if (!navigationDocumentComplete || !navigationScanComplete)
			{
				navigationDocumentComplete = true;
				OnScanCompleted();
			}
			base.Stop();
			TerminateFileScanner();
		}

		private bool MouseHandler(ref Message m)
		{
			if (m.Msg == 513 && !string.IsNullOrEmpty(lastMouseOverUrl))
			{
				try
				{
					Point point = new Point(m.LParam.ToInt32() & 0xFFFF, m.LParam.ToInt32() >> 16);
					if (point.X < 0 || point.X > 32767)
					{
						point.X = Math.Abs(65536 - point.X);
					}
					if (point.X >= 0 && point.X < base.Width - SystemInformation.VerticalScrollBarWidth)
					{
						List<IntPtr> childWindows = GetChildWindows(base.Handle);
						while (childWindows.Count > 0)
						{
							if (childWindows[0] == m.HWnd)
							{
								string mimeType = ((!lastMouseOverUrl.StartsWith("http", StringComparison.Ordinal)) ? null : GetMimeTypeFromUrl(lastMouseOverUrl));
								if (IsPlayableLink(mimeType))
								{
									browserLinkClicked = false;
									try
									{
										LinkPlayUrl?.Invoke(this, lastMouseOverUrl, mimeType);
										return true;
									}
									catch (Exception projectError)
									{
										ProjectData.SetProjectError(projectError);
										ProjectData.ClearProjectError();
									}
								}
								browserLinkClicked = true;
								break;
							}
							childWindows = GetChildWindows(childWindows[0]);
						}
					}
				}
				catch (Exception ex)
				{
					ProjectData.SetProjectError(ex);
					Exception ex2 = ex;
					ProjectData.ClearProjectError();
				}
			}
			else if (m.Msg == 522)
			{
				try
				{
					Control topmostControl = GetTopmostControl(base.Parent, new Point(m.LParam.ToInt32()));
					if (m.HWnd != topmostControl.Handle && topmostControl is MediaWebBrowser)
					{
						IntPtr documentHandle = ((MediaWebBrowser)topmostControl).DocumentHandle;
						if (documentHandle == IntPtr.Zero || documentHandle == m.HWnd)
						{
							return false;
						}
						PostMessage(documentHandle, 522, m.WParam, m.LParam);
						return true;
					}
				}
				catch (Exception ex3)
				{
					ProjectData.SetProjectError(ex3);
					Exception ex4 = ex3;
					ProjectData.ClearProjectError();
				}
			}
			return false;
		}

		bool IMessageFilter.PreFilterMessage(ref Message m)
		{
			//ILSpy generated this explicit interface implementation from .override directive in MouseHandler
			return this.MouseHandler(ref m);
		}

		private bool IsPlayableLink(string mimeType)
		{
			switch (mimeType)
			{
			case "audio/mpeg":
			case "audio/x-mpeg":
			case "audio/aac":
			case "audio/mp4":
			case "audio/x-aac":
			case "audio/x-m4a":
			case "audio/x-m4b":
			case "audio/x-m4p":
			case "audio/aacp":
			case "audio/ms-wma":
			case "audio/x-ms-wma":
			case "audio/x-ms-wax":
			case "application/ogg":
			case "audio/ogg":
			case "audio/x-ogg":
			case "audio/flac":
			case "audio/x-flac":
			case "audio/wv":
			case "audio/x-wv":
			case "audio/x-aiff":
			case "application/xspf+xml":
			case "audio/x-mpegurl":
			case "audio/x-scpls":
			case "video/x-ms-asf":
			case "application/octet-stream":
				return true;
			default:
				return false;
			}
		}

		protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
		{
			try
			{
				if (browserLinkClicked)
				{
					browserLinkClicked = false;
					string text = e.Url.ToString();
					string text2 = GetMimeTypeFromUrl(text);
					if (string.IsNullOrEmpty(text2))
					{
						text2 = HttpSession.RequestMimeType(text, 2500);
					}
					if (Operators.CompareString(text2, "application/octet-stream", TextCompare: false) == 0 && Operators.CompareString(Path.GetExtension(text), ".exe", TextCompare: false) == 0)
					{
						text2 = "";
					}
					if (IsPlayableLink(text2))
					{
						e.Cancel = true;
						LinkPlayUrl?.Invoke(this, text, text2);
					}
					else
					{
						NavigationLinkClicked?.Invoke(this, text);
					}
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		protected override void OnNewWindow(CancelEventArgs e)
		{
		}

		protected override void OnProgressChanged(WebBrowserProgressChangedEventArgs e)
		{
			try
			{
				if (e.CurrentProgress > 0 && e.CurrentProgress == e.MaximumProgress)
				{
					initialDownloadComplete.Set();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
		{
			try
			{
				mbApiInterface.MB_SetBackgroundTaskMessage("");
				if (e.Url == base.Url)
				{
					NavigationLoading?.Invoke(this, EventArgs.Empty);
					navigationScanComplete = false;
					navigationDocumentComplete = false;
					StartWebScan(base.Url);
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
		}

		protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
		{
			if (e.Url == base.Url)
			{
				initialDownloadComplete.Set();
			}
			NavigationCompleted?.Invoke(this, EventArgs.Empty);
		}

		protected override void OnStatusTextChanged(EventArgs e)
		{
			string text = base.StatusText;
			lastMouseOverUrl = ((!text.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !text.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ? "" : text);
			if (Operators.CompareString(text, lastStatusText, TextCompare: false) != 0)
			{
				lastStatusText = text;
				mbApiInterface.MB_SetBackgroundTaskMessage(text);
			}
		}

		private static string GetMimeTypeFromUrl(string url)
		{
			int num = url.IndexOf('?');
			if (num != -1)
			{
				url = url.Substring(0, num);
			}
			return Path.GetExtension(url).ToLower() switch
			{
				".mp3" => "audio/mpeg", 
				".m4a" => "audio/x-m4a", 
				".m4b" => "audio/x-m4b", 
				".aac" => "audio/x-aac", 
				".wma" => "audio/wma", 
				".ogg" => "audio/ogg", 
				".opus" => "audio/opus", 
				".flac" => "audio/flac", 
				".wv" => "audio/wv", 
				".m3u" => "audio/x-mpegurl", 
				".pls" => "audio/x-scpls", 
				_ => null, 
			};
		}

		private void StartWebScan(Uri url)
		{
			TerminateFileScanner();
			navigationScanActive = true;
			itemsPendingCount = 0;
			ItemCount = 0;
			ScanStarting?.Invoke(this, EventArgs.Empty);
			if (Operators.CompareString(url.ToString(), "about:blank", TextCompare: false) != 0)
			{
				initialDownloadComplete.Reset();
				scannerThread = new Thread(ExecuteWebScan);
				scannerThread.IsBackground = true;
				scannerThread.Start(base.Url);
			}
		}

		private void ExecuteWebScan(object parameters)
		{
			try
			{
				mbApiInterface.MB_SetBackgroundTaskMessage("Scanning for music files...");
				initialDownloadComplete.WaitOne();
				Thread.Sleep(1000);
				Uri uri = (Uri)parameters;
				HttpSession httpSession = new HttpSession(20000);
				string text;
				try
				{
					text = (string)httpSession.Request(uri.ToString(), RequestType.Content);
				}
				catch (Exception projectError)
				{
					ProjectData.SetProjectError(projectError);
					text = "";
					ProjectData.ClearProjectError();
				}
				int length = text.Length;
				HashSet<string> hashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				SortedList<int, string> sortedList = new SortedList<int, string>();
				string[] array = new string[3] { ".mp3", ".m4a", ".wma" };
				string result = default(string);
				foreach (string value in array)
				{
					int num = 0;
					int num2 = 0;
					do
					{
						int num3 = text.IndexOf(value, num, StringComparison.OrdinalIgnoreCase);
						int num4 = num3 + 4;
						if (num3 == -1 || num4 == length)
						{
							break;
						}
						for (num3 = num4; num3 < length && text[num3] == ' '; num3++)
						{
						}
						int num5;
						switch (text[num3])
						{
						case '"':
							num5 = text.LastIndexOf('"', num4 - 1, num4 - num2);
							break;
						case '\'':
							num5 = text.LastIndexOf('\'', num4 - 1, num4 - num2);
							break;
						case ' ':
						case '>':
							num5 = text.LastIndexOfAny(new char[2] { '=', '>' }, num4 - 1, num4 - num2);
							break;
						default:
							num5 = -1;
							break;
						}
						if (num5 != -1)
						{
							int num6;
							if (text[num5] == '=')
							{
								num6 = num5;
							}
							else
							{
								num6 = num5 - 1;
								while (num6 > 0 && char.IsWhiteSpace(text[num6]))
								{
									num6--;
								}
							}
							if (text[num6] != '=')
							{
								num5 = -1;
							}
							else
							{
								num6--;
								while (num6 > 0 && char.IsWhiteSpace(text[num6]))
								{
									num6--;
								}
								if (char.ToLower(text[num6]) != 'f' && char.ToLower(text[num6]) != 'l')
								{
									num5 = -1;
								}
							}
						}
						if (num5 != -1)
						{
							string text2 = text.Substring(num5 + 1, num4 - num5 - 1);
							if (text2.IndexOf('?') == -1 && TryParse(text2, uri, ref result) && hashSet.Add(result))
							{
								sortedList.Add(num5, result);
							}
						}
						num = num4 + 4;
						if (num5 != -1)
						{
							num2 = num;
						}
					}
					while (num < text.Length);
				}
				itemsPendingCount = sortedList.Count;
				ItemCount = 0;
				foreach (string value2 in sortedList.Values)
				{
					try
					{
						if (sortedList.Values.Count > 1)
						{
							mbApiInterface.MB_SetBackgroundTaskMessage($"Scanning {ItemCount + 1} of {sortedList.Values.Count}...");
						}
						MediaFile mediaFile = new MediaFile();
						mediaFile.Url = value2;
						mediaFile.Status = mbApiInterface.Library_GetFileProperty(value2, FilePropertyType.Status);
						mediaFile.Artist = mbApiInterface.Library_GetFileTag(value2, MetaDataType.Artist);
						mediaFile.Title = mbApiInterface.Library_GetFileTag(value2, MetaDataType.TrackTitle);
						mediaFile.Album = mbApiInterface.Library_GetFileTag(value2, MetaDataType.Album);
						mediaFile.Size = mbApiInterface.Library_GetFileProperty(value2, FilePropertyType.Size);
						mediaFile.Duration = mbApiInterface.Library_GetFileProperty(value2, FilePropertyType.Duration);
						mediaFile.Origin = uri.AbsoluteUri;
						if (mediaFile.Title.Length == 0)
						{
							mediaFile.Title = Uri.UnescapeDataString(Path.GetFileNameWithoutExtension(value2));
						}
						itemsPendingCount--;
						ItemCount++;
						if (navigationScanActive)
						{
							ItemScanned?.Invoke(this, mediaFile);
						}
					}
					catch (Exception projectError2)
					{
						ProjectData.SetProjectError(projectError2);
						ProjectData.ClearProjectError();
					}
				}
			}
			catch (Exception projectError3)
			{
				ProjectData.SetProjectError(projectError3);
				ProjectData.ClearProjectError();
			}
			try
			{
				ScanCompleted?.Invoke(this, EventArgs.Empty);
			}
			catch (Exception projectError4)
			{
				ProjectData.SetProjectError(projectError4);
				ProjectData.ClearProjectError();
			}
		}

		private bool TryParse(string location, Uri baseUri, ref string result)
		{
			if (!Uri.TryCreate(location, UriKind.RelativeOrAbsolute, out var result2))
			{
				result = "";
				return false;
			}
			if (!result2.IsAbsoluteUri)
			{
				if (!Uri.TryCreate(baseUri, result2, out result2))
				{
					result = "";
					return false;
				}
				if (result2.IsFile)
				{
					result = Uri.UnescapeDataString(result2.LocalPath);
					return true;
				}
			}
			if (result2.IsFile)
			{
				result = result2.LocalPath;
			}
			else
			{
				result = Uri.UnescapeDataString(result2.AbsoluteUri);
			}
			return true;
		}

		private void TerminateFileScanner()
		{
			navigationScanActive = false;
			if (scannerThread != null && scannerThread.IsAlive)
			{
				scannerThread.Abort();
			}
			itemsPendingCount = 0;
		}

		private void OnScanCompleted()
		{
			navigationScanComplete = true;
			if (navigationDocumentComplete)
			{
				NavigationCompleted?.Invoke(this, EventArgs.Empty);
			}
		}

		private static Control GetTopmostControl(Control control, Point mousePosition)
		{
			if (control.Controls.Count == 0)
			{
				return control;
			}
			Control childAtPoint = control.GetChildAtPoint(control.PointToClient(mousePosition), GetChildAtPointSkip.Invisible);
			if (childAtPoint == null)
			{
				return control;
			}
			if (childAtPoint is MediaWebBrowser)
			{
				return childAtPoint;
			}
			return GetTopmostControl(childAtPoint, mousePosition);
		}

		private static List<IntPtr> GetChildWindows(IntPtr handle)
		{
			List<IntPtr> list = new List<IntPtr>();
			GCHandle value = GCHandle.Alloc(list);
			try
			{
				EnumChildWindows(handle, EnumChildrenProc, GCHandle.ToIntPtr(value));
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
			}
			finally
			{
				if (value.IsAllocated)
				{
					value.Free();
				}
			}
			List<IntPtr> list2 = new List<IntPtr>();
			foreach (IntPtr item in list)
			{
				if (GetParent(item) == handle)
				{
					list2.Add(item);
				}
			}
			return list2;
		}

		private static bool EnumChildrenProc(IntPtr handle, IntPtr parameter)
		{
			List<IntPtr> list = (List<IntPtr>)GCHandle.FromIntPtr(parameter).Target;
			if (list == null)
			{
				return false;
			}
			list.Add(handle);
			return true;
		}

		[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		private static extern IntPtr GetParent(IntPtr hWnd);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpfn, IntPtr lParam);

		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
	}

	private class HttpSession
	{
		public string UserAgent;

		public int Timeout;

		public string ResponseContentType;

		public long ResponseContentLength;

		public static WebProxy Proxy = null;

		private static Encoding iso8859Encoding = Encoding.GetEncoding("ISO-8859-1");

		public HttpSession(int timeout)
		{
			UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/6.0)";
			Timeout = 20000;
			Timeout = timeout;
		}

		public static string RequestMimeType(string url, int timeout)
		{
			string result;
			try
			{
				if (Operators.CompareString(url, "about:blank", TextCompare: false) == 0)
				{
					result = "";
				}
				else
				{
					HttpSession httpSession = new HttpSession(timeout);
					httpSession.Request(url, RequestType.Header);
					result = httpSession.ResponseContentType.ToLower().Split(';')[0].TrimEnd();
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				result = "";
				ProjectData.ClearProjectError();
			}
			return result;
		}

		public object Request(string url, RequestType type)
		{
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.UserAgent = UserAgent;
			httpWebRequest.AllowAutoRedirect = true;
			httpWebRequest.ContentType = "application/x-www-form-urlencoded";
			httpWebRequest.Accept = "*/*";
			httpWebRequest.Headers.Add("Accept-Encoding", "gzip");
			httpWebRequest.Method = ((type == RequestType.Header) ? "HEAD" : "GET");
			httpWebRequest.ContentLength = 0L;
			httpWebRequest.Timeout = Timeout;
			httpWebRequest.Proxy = Proxy;
			IAsyncResult asyncResult;
			try
			{
				asyncResult = httpWebRequest.BeginGetResponse(null, null);
				if (!((ManualResetEvent)asyncResult.AsyncWaitHandle).WaitOne(Timeout, exitContext: false))
				{
					throw new WebException("The operation has timed out.", WebExceptionStatus.Timeout);
				}
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				try
				{
					httpWebRequest.Abort();
				}
				catch (Exception projectError2)
				{
					ProjectData.SetProjectError(projectError2);
					ProjectData.ClearProjectError();
				}
				throw;
			}
			using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.EndGetResponse(asyncResult))
			{
				ResponseContentType = httpWebResponse.ContentType;
				ResponseContentLength = httpWebResponse.ContentLength;
				if (type != RequestType.Header)
				{
					using Stream stream = httpWebResponse.GetResponseStream();
					Encoding encoding = ((!string.IsNullOrEmpty(httpWebResponse.ContentEncoding) && Operators.CompareString(httpWebResponse.ContentEncoding, "gzip", TextCompare: false) != 0) ? Encoding.GetEncoding(httpWebResponse.ContentEncoding) : (string.IsNullOrEmpty(httpWebResponse.CharacterSet) ? iso8859Encoding : Encoding.GetEncoding(httpWebResponse.CharacterSet)));
					int num = (int)ResponseContentLength;
					if (num <= 0)
					{
						num = 4096;
					}
					byte[] array2;
					using (MemoryStream memoryStream = new MemoryStream(num))
					{
						byte[] array = new byte[4096];
						while (true)
						{
							int num2 = stream.Read(array, 0, 4096);
							if (num2 == 0)
							{
								break;
							}
							memoryStream.Write(array, 0, num2);
						}
						stream.Close();
						if (string.Compare(httpWebResponse.ContentEncoding, "gzip", StringComparison.OrdinalIgnoreCase) == 0)
						{
							memoryStream.Position = 0L;
							using GZipStream gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress);
							using MemoryStream memoryStream2 = new MemoryStream();
							while (true)
							{
								int num3 = gZipStream.Read(array, 0, 4096);
								if (num3 == 0)
								{
									break;
								}
								memoryStream2.Write(array, 0, num3);
							}
							array2 = memoryStream2.ToArray();
						}
						else
						{
							array2 = memoryStream.ToArray();
						}
					}
					switch (type)
					{
					case RequestType.Data:
						return array2;
					case RequestType.Content:
						if (array2.Length > 1 && array2[0] == byte.MaxValue && array2[1] == 254)
						{
							return Encoding.Unicode.GetString(array2, 2, array2.Length - 2);
						}
						if (array2.Length > 1 && array2[0] == 254 && array2[1] == byte.MaxValue)
						{
							return Encoding.BigEndianUnicode.GetString(array2, 2, array2.Length - 2);
						}
						if (array2.Length > 2 && array2[0] == 239 && array2[1] == 187 && array2[2] == 191)
						{
							return Encoding.UTF8.GetString(array2, 3, array2.Length - 3);
						}
						return encoding.GetString(array2, 0, array2.Length);
					}
				}
			}
			return null;
		}
	}

	private enum RequestType
	{
		Header,
		Content,
		Data
	}

	private class MediaFile
	{
		public string Url;

		public string Status;

		public string Artist;

		public string Title;

		public string Album;

		public string Size;

		public string Duration;

		public string Origin;
	}

	[CompilerGenerated]
	internal sealed class _Closure$__234-0
	{
		public PlayState $VB$Local_playState;

		public string $VB$Local_url;

		public _Closure$__234-1 $VB$NonLocal_$VB$Closure_2;

		public _Closure$__234-0(_Closure$__234-0 arg0)
		{
			if (arg0 != null)
			{
				$VB$Local_playState = arg0.$VB$Local_playState;
				$VB$Local_url = arg0.$VB$Local_url;
			}
		}

		[SpecialName]
		internal void _Lambda$__0()
		{
			foreach (ListViewItem item in $VB$NonLocal_$VB$Closure_2.$VB$Me.fileList.Items)
			{
				PlayState playState = $VB$Local_playState;
				if (playState == PlayState.Playing || playState == PlayState.Paused)
				{
					MediaFile mediaFile = (MediaFile)item.Tag;
					if (Operators.CompareString($VB$Local_url, mediaFile.Url, TextCompare: false) == 0 && Operators.CompareString(item.SubItems[0].Text, "Downloading", TextCompare: false) != 0)
					{
						if ($VB$Local_playState == PlayState.Paused)
						{
							item.SubItems[0].Text = "Paused";
						}
						else
						{
							item.SubItems[0].Text = "Playing";
						}
					}
				}
				else if (Operators.CompareString(item.SubItems[0].Text, "Playing", TextCompare: false) == 0 || Operators.CompareString(item.SubItems[0].Text, "Paused", TextCompare: false) == 0)
				{
					item.SubItems[0].Text = "";
				}
			}
		}
	}

	[CompilerGenerated]
	internal sealed class _Closure$__234-1
	{
		public string $VB$Local_sourceFileUrl;

		public Plugin $VB$Me;

		public _Closure$__234-1(_Closure$__234-1 arg0)
		{
			if (arg0 != null)
			{
				$VB$Local_sourceFileUrl = arg0.$VB$Local_sourceFileUrl;
			}
		}

		[SpecialName]
		internal void _Lambda$__1()
		{
			foreach (ListViewItem item in $VB$Me.fileList.Items)
			{
				if (string.Compare(((MediaFile)item.Tag).Url, $VB$Local_sourceFileUrl, StringComparison.OrdinalIgnoreCase) == 0)
				{
					item.SubItems[0].Text = "Download Completed";
					break;
				}
			}
		}
	}

	public const short PluginInfoVersion = 1;

	public const short MinInterfaceVersion = 31;

	public const short MinApiRevision = 43;

	private static MusicBeeApiInterface mbApiInterface = default(MusicBeeApiInterface);

	private PluginInfo about;

	private Bitmap pluginIcon;

	private EventHandler openHandler;

	private EventHandler closeHandler;

	private const int locationBarLeft = 107;

	private bool isSettingsDirty;

	private int settingsVersion;

	private UserControl panel;

	private Control header;

	private MediaWebBrowser browser;

	private TextBox locationBar;

	private Control locationBarPrompt;

	private ListView fileList;

	private ToolStripMenuItem playNowMenuItem;

	private ToolStripMenuItem queueNextMenuItem;

	private ToolStripMenuItem queueLastMenuItem;

	private ToolStripMenuItem downloadMenuItem;

	private ToolStripMenuItem downloadToMenu;

	private ToolStripMenuItem downloadToLibraryMenuItem;

	private ToolStripMenuItem downloadToInboxMenuItem;

	private ToolStripMenuItem downloadToFolderMenuItem;

	private ToolStripMenuItem downloadToLastFolderMenuItem;

	private Bitmap favouritesButton;

	private Bitmap browseButtons;

	private Bitmap backHighlightButton;

	private Bitmap backEnabledButton;

	private Bitmap forwardHighlightButton;

	private Bitmap forwardEnabledButton;

	private Bitmap refreshButton;

	private Bitmap stopButton;

	private Bitmap bookmarkButton;

	private Bitmap bookmarkSavedButton;

	private Bitmap fileAlertIcon;

	private Bitmap playingIcon;

	private Bitmap playingPausedIcon;

	private Bitmap downloadingIcon;

	private Bitmap downloadCompleteIcon;

	private bool isLoading;

	private Thread iconLoader;

	private bool currentIsFavourite;

	private string currentIconHost;

	private Bitmap currentIcon;

	private TypeConverter bitmapConverter;

	private TypeConverter iconConverter;

	private PopupContainer favouritesPopup;

	private long favouritesPopupLastOpened;

	private List<Bookmark> favourites;

	private string defaultUrl;

	private string activeUrl;

	private string loadOnceUrl;

	private string lastArtist;

	private bool reloadOnArtistChanged;

	private string lastAlbumArtist;

	private bool reloadOnAlbumArtistChanged;

	private string lastTitle;

	private bool reloadOnTrackChanged;

	private string lastAlbum;

	private bool reloadOnAlbumChanged;

	private DownloadTarget downloadCategory;

	private string lastDownloadFolder;

	private bool isMouseOverFavourites;

	private bool isMouseOverBrowseBack;

	private bool isMouseOverBrowseForward;

	private bool isMouseOverBookmark;

	private Rectangle FavouritesHighlightBounds => new Rectangle(12, 9, 24, 24);

	private Rectangle FavouritesButtonBounds => new Rectangle(16, 13, 16, 16);

	private Rectangle BrowseBackButtonBounds => new Rectangle(54, 8, 28, 29);

	private Rectangle BrowseForwardButtonBounds => new Rectangle(81, 10, 25, 20);

	private Rectangle CurrentIconBounds => new Rectangle(110, 13, 16, 16);

	private Rectangle RefreshStopButtonBounds => new Rectangle(header.Width - 50 - 20, 13, 16, 16);

	private Rectangle BookmarkHighlightBounds => new Rectangle(header.Width - 36, 9, 24, 24);

	private Rectangle BookmarkButtonBounds => new Rectangle(header.Width - 32, 13, 16, 16);

	public Plugin()
	{
		about = new PluginInfo();
		openHandler = OpenWebBrowser;
		closeHandler = CloseWebBrowser;
		isSettingsDirty = false;
		isLoading = false;
		currentIsFavourite = false;
		currentIcon = null;
		bitmapConverter = TypeDescriptor.GetConverter(typeof(Bitmap));
		iconConverter = TypeDescriptor.GetConverter(typeof(Icon));
		favourites = new List<Bookmark>();
		defaultUrl = null;
		activeUrl = null;
		loadOnceUrl = null;
		downloadCategory = DownloadTarget.Inbox;
		lastDownloadFolder = "";
		isMouseOverFavourites = false;
		isMouseOverBrowseBack = false;
		isMouseOverBrowseForward = false;
		isMouseOverBookmark = false;
	}

	[DllImport("kernel32.dll")]
	[SuppressUnmanagedCodeSecurity]
	private static extern void CopyMemory(ref MusicBeeApiInterface mbApiInterface, IntPtr src, int length);

	public PluginInfo Initialise(IntPtr apiInterfacePtr)
	{
		CopyMemory(ref mbApiInterface, apiInterfacePtr, 4);
		if (mbApiInterface.MusicBeeVersion == MusicBeeVersion.v2_0)
		{
			CopyMemory(ref mbApiInterface, apiInterfacePtr, 456);
		}
		else if (mbApiInterface.MusicBeeVersion == MusicBeeVersion.v2_1)
		{
			CopyMemory(ref mbApiInterface, apiInterfacePtr, 516);
		}
		else if (mbApiInterface.MusicBeeVersion == MusicBeeVersion.v2_2)
		{
			CopyMemory(ref mbApiInterface, apiInterfacePtr, 584);
		}
		else if (mbApiInterface.MusicBeeVersion == MusicBeeVersion.v2_3)
		{
			CopyMemory(ref mbApiInterface, apiInterfacePtr, 596);
		}
		else
		{
			CopyMemory(ref mbApiInterface, apiInterfacePtr, Marshal.SizeOf((object)mbApiInterface));
		}
		about.PluginInfoVersion = 1;
		about.Name = "Web Browser";
		about.Description = "Play and download tracks from mp3 blogs";
		about.Author = "Steven Mayall";
		about.TargetApplication = "";
		about.Type = PluginType.WebBrowser;
		about.VersionMajor = 2;
		about.VersionMinor = 0;
		about.Revision = 1;
		about.MinInterfaceVersion = 31;
		about.MinApiRevision = 43;
		about.ReceiveNotifications = ReceiveNotificationFlags.PlayerEvents | ReceiveNotificationFlags.DownloadEvents;
		about.ConfigurationPanelHeight = 0;
		return about;
	}

	public bool Configure(IntPtr panelHandle)
	{
		return false;
	}

	private string LoadSettings()
	{
		string result = null;
		string path = mbApiInterface.Setting_GetPersistentStoragePath() + "WebBrowserSettings.dat";
		if (File.Exists(path))
		{
			try
			{
				using FileStream input = new FileStream(path, FileMode.Open, FileAccess.Read);
				using BinaryReader binaryReader = new BinaryReader(input);
				settingsVersion = binaryReader.ReadInt32();
				result = (defaultUrl = binaryReader.ReadString());
				int num = binaryReader.ReadInt32();
				favourites.Clear();
				int num2 = num - 1;
				for (int i = 0; i <= num2; i++)
				{
					Bookmark item = new Bookmark
					{
						Url = binaryReader.ReadString(),
						Name = binaryReader.ReadString()
					};
					int num3 = binaryReader.ReadInt32();
					if (num3 > 0)
					{
						byte[] array = new byte[num3 - 1 + 1];
						binaryReader.Read(array, 0, num3);
						try
						{
							item.Icon = (Bitmap)bitmapConverter.ConvertFrom(array);
						}
						catch (Exception projectError)
						{
							ProjectData.SetProjectError(projectError);
							ProjectData.ClearProjectError();
						}
					}
					favourites.Add(item);
				}
				if (settingsVersion == 1)
				{
					object value = default(object);
					mbApiInterface.Setting_GetValue(SettingId.LastDownloadFolder, ref value);
					if (value != null)
					{
						lastDownloadFolder = (string)value;
					}
				}
				else
				{
					downloadCategory = (DownloadTarget)binaryReader.ReadInt32();
					lastDownloadFolder = binaryReader.ReadString();
				}
			}
			catch (Exception projectError2)
			{
				ProjectData.SetProjectError(projectError2);
				ProjectData.ClearProjectError();
			}
		}
		if (string.IsNullOrEmpty(lastDownloadFolder))
		{
			lastDownloadFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
		}
		if (settingsVersion < 3)
		{
			InitialiseV3Favorites();
			result = "http://en.wikipedia.org/wiki/Special:Search?search=<artist>";
		}
		return result;
	}

	private void InitialiseV3Favorites()
	{
		if (settingsVersion < 3)
		{
			settingsVersion = 3;
			ResourceManager resourceManager = new ResourceManager("MusicBeePlugin.Images", Assembly.GetAssembly(GetType()));
			favourites.Add(new Bookmark("http://en.wikipedia.org/wiki/Special:Search?search=<artist>", "Wikipedia", (Bitmap)resourceManager.GetObject("Wikipedia")));
			favourites.Add(new Bookmark("http://www.last.fm/music/<artist>", "last.fm", (Bitmap)resourceManager.GetObject("LastFm")));
			favourites.Add(new Bookmark("http://www.youtube.com/results?search_query=<artist>", "youtube", (Bitmap)resourceManager.GetObject("YouTube")));
			resourceManager.ReleaseAllResources();
		}
	}

	private void UpdateMusicBeeFavorites()
	{
		InitialiseV3Favorites();
		for (int i = favourites.Count - 1; i >= 0; i += -1)
		{
			if (favourites[i].Custom)
			{
				favourites.RemoveAt(i);
			}
		}
		int num = 0;
		object value = default(object);
		object value2 = default(object);
		do
		{
			if (mbApiInterface.Setting_GetValue((SettingId)(11 + num), ref value) && mbApiInterface.Setting_GetValue((SettingId)(17 + num), ref value2))
			{
				favourites.Add(new Bookmark((string)value2, (string)value, null, custom: true));
			}
			num++;
		}
		while (num <= 5);
		int num2 = 0;
		do
		{
			if (mbApiInterface.Setting_GetValue((SettingId)(29 + num2), ref value) && mbApiInterface.Setting_GetValue((SettingId)(33 + num2), ref value2))
			{
				favourites.Add(new Bookmark((string)value2, (string)value, null, custom: true));
			}
			num2++;
		}
		while (num2 <= 3);
		favourites.Sort(new BookmarkComparer());
	}

	public void SaveSettings()
	{
		if (settingsVersion < 3)
		{
			InitialiseV3Favorites();
		}
		string path = mbApiInterface.Setting_GetPersistentStoragePath() + "WebBrowserSettings.dat";
		List<Bookmark> list = new List<Bookmark>(favourites);
		for (int i = favourites.Count - 1; i >= 0; i += -1)
		{
			if (favourites[i].Custom)
			{
				favourites.RemoveAt(i);
			}
		}
		try
		{
		}
		finally
		{
			using (FileStream output = new FileStream(path, FileMode.Create, FileAccess.Write))
			{
				using BinaryWriter binaryWriter = new BinaryWriter(output);
				binaryWriter.Write(3);
				binaryWriter.Write((defaultUrl == null) ? "" : defaultUrl);
				binaryWriter.Write(favourites.Count);
				int num = favourites.Count - 1;
				for (int j = 0; j <= num; j++)
				{
					binaryWriter.Write(favourites[j].Url);
					binaryWriter.Write(string.IsNullOrEmpty(favourites[j].Name) ? "No Title" : favourites[j].Name);
					if (favourites[j].Icon == null)
					{
						binaryWriter.Write(0);
						continue;
					}
					try
					{
						byte[] array = (byte[])bitmapConverter.ConvertTo(favourites[j].Icon, typeof(byte[]));
						binaryWriter.Write(array.Length);
						binaryWriter.Write(array);
					}
					catch (Exception projectError)
					{
						ProjectData.SetProjectError(projectError);
						binaryWriter.Write(0);
						ProjectData.ClearProjectError();
					}
				}
				binaryWriter.Write((int)downloadCategory);
				binaryWriter.Write(lastDownloadFolder);
			}
			favourites = list;
		}
		isSettingsDirty = false;
	}

	public void Close(PluginCloseReason reason)
	{
		if (isSettingsDirty)
		{
			SaveSettings();
		}
	}

	public void Uninstall()
	{
	}

	public void ReceiveNotification(string sourceFileUrl, NotificationType type)
	{
		_Closure$__234-1 arg = default(_Closure$__234-1);
		_Closure$__234-1 CS$<>8__locals9 = new _Closure$__234-1(arg);
		CS$<>8__locals9.$VB$Me = this;
		CS$<>8__locals9.$VB$Local_sourceFileUrl = sourceFileUrl;
		switch (type)
		{
		case NotificationType.PluginStartup:
		{
			ResourceManager resourceManager = new ResourceManager("MusicBeePlugin.Images", Assembly.GetAssembly(GetType()));
			Bitmap icon = (Bitmap)resourceManager.GetObject("WebBrowser");
			resourceManager.ReleaseAllResources();
			mbApiInterface.MB_AddTreeNode("Services", "Web Browser", icon, openHandler, closeHandler);
			try
			{
				RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION", writable: true);
				if (registryKey == null)
				{
					registryKey = Registry.CurrentUser.CreateSubKey("Software\\Microsoft\\Internet Explorer\\Main\\FeatureControl\\FEATURE_BROWSER_EMULATION");
				}
				if (registryKey.GetValue("MusicBee.exe") == null)
				{
					registryKey.SetValue("MusicBee.exe", 11000, RegistryValueKind.DWord);
				}
				registryKey.Dispose();
				break;
			}
			catch (Exception projectError)
			{
				ProjectData.SetProjectError(projectError);
				ProjectData.ClearProjectError();
				break;
			}
		}
		case NotificationType.TrackChanged:
			if (browser != null)
			{
				if (reloadOnTrackChanged && string.Compare(mbApiInterface.NowPlaying_GetFileTag(MetaDataType.TrackTitle), lastTitle, StringComparison.OrdinalIgnoreCase) != 0)
				{
					NavigateTo(activeUrl);
				}
				else if (reloadOnAlbumChanged && string.Compare(mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Album), lastAlbum, StringComparison.OrdinalIgnoreCase) != 0)
				{
					NavigateTo(activeUrl);
				}
				else if (reloadOnArtistChanged && string.Compare(mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist), lastArtist, StringComparison.OrdinalIgnoreCase) != 0)
				{
					NavigateTo(activeUrl);
				}
				else if (reloadOnAlbumArtistChanged && string.Compare(mbApiInterface.NowPlaying_GetFileTag(MetaDataType.AlbumArtist), lastAlbumArtist, StringComparison.OrdinalIgnoreCase) != 0)
				{
					NavigateTo(activeUrl);
				}
			}
			break;
		case NotificationType.PlayStateChanged:
		{
			if (fileList == null)
			{
				break;
			}
			_Closure$__234-0 arg2 = default(_Closure$__234-0);
			_Closure$__234-0 CS$<>8__locals13 = new _Closure$__234-0(arg2);
			CS$<>8__locals13.$VB$NonLocal_$VB$Closure_2 = CS$<>8__locals9;
			CS$<>8__locals13.$VB$Local_url = mbApiInterface.NowPlaying_GetFileUrl();
			CS$<>8__locals13.$VB$Local_playState = mbApiInterface.Player_GetPlayState();
			MethodInvoker method2 = [SpecialName] () =>
			{
				foreach (ListViewItem item in CS$<>8__locals13.$VB$NonLocal_$VB$Closure_2.$VB$Me.fileList.Items)
				{
					PlayState playState = CS$<>8__locals13.$VB$Local_playState;
					if (playState == PlayState.Playing || playState == PlayState.Paused)
					{
						MediaFile mediaFile = (MediaFile)item.Tag;
						if (Operators.CompareString(CS$<>8__locals13.$VB$Local_url, mediaFile.Url, TextCompare: false) == 0 && Operators.CompareString(item.SubItems[0].Text, "Downloading", TextCompare: false) != 0)
						{
							if (CS$<>8__locals13.$VB$Local_playState == PlayState.Paused)
							{
								item.SubItems[0].Text = "Paused";
							}
							else
							{
								item.SubItems[0].Text = "Playing";
							}
						}
					}
					else if (Operators.CompareString(item.SubItems[0].Text, "Playing", TextCompare: false) == 0 || Operators.CompareString(item.SubItems[0].Text, "Paused", TextCompare: false) == 0)
					{
						item.SubItems[0].Text = "";
					}
				}
			};
			fileList.Invoke(method2);
			break;
		}
		case NotificationType.DownloadCompleted:
		{
			if (fileList == null)
			{
				break;
			}
			MethodInvoker method = [SpecialName] () =>
			{
				foreach (ListViewItem item2 in CS$<>8__locals9.$VB$Me.fileList.Items)
				{
					if (string.Compare(((MediaFile)item2.Tag).Url, CS$<>8__locals9.$VB$Local_sourceFileUrl, StringComparison.OrdinalIgnoreCase) == 0)
					{
						item2.SubItems[0].Text = "Download Completed";
						break;
					}
				}
			};
			fileList.Invoke(method);
			break;
		}
		}
	}

	private void OpenWebBrowser(object sender, EventArgs e)
	{
		string text = loadOnceUrl;
		loadOnceUrl = null;
		if (browseButtons == null)
		{
			activeUrl = LoadSettings();
			if (text == null)
			{
				text = activeUrl;
			}
			ResourceManager resourceManager = new ResourceManager("MusicBeePlugin.Images", Assembly.GetAssembly(GetType()));
			favouritesButton = (Bitmap)resourceManager.GetObject("Favourites");
			browseButtons = (Bitmap)resourceManager.GetObject("ArrowButtons");
			backHighlightButton = (Bitmap)resourceManager.GetObject("BackButtonHighlight");
			backEnabledButton = (Bitmap)resourceManager.GetObject("BackButtonEnabled");
			forwardHighlightButton = (Bitmap)resourceManager.GetObject("ForwardButtonHighlight");
			forwardEnabledButton = (Bitmap)resourceManager.GetObject("ForwardButtonEnabled");
			refreshButton = (Bitmap)resourceManager.GetObject("Refresh");
			stopButton = (Bitmap)resourceManager.GetObject("Stop");
			bookmarkButton = (Bitmap)resourceManager.GetObject("Bookmark");
			bookmarkSavedButton = (Bitmap)resourceManager.GetObject("BookmarkSaved");
			fileAlertIcon = (Bitmap)resourceManager.GetObject("FileInfoAlert");
			playingIcon = (Bitmap)resourceManager.GetObject("Playing");
			playingPausedIcon = (Bitmap)resourceManager.GetObject("PlayingPaused");
			downloadingIcon = (Bitmap)resourceManager.GetObject("Download");
			downloadCompleteIcon = (Bitmap)resourceManager.GetObject("DownloadComplete");
			resourceManager.ReleaseAllResources();
		}
		else if (string.IsNullOrEmpty(text))
		{
			text = activeUrl;
		}
		Font font = mbApiInterface.Setting_GetDefaultFont();
		panel = new UserControl();
		panel.AutoScroll = false;
		if (string.IsNullOrEmpty(text))
		{
			locationBarPrompt = new Control();
			locationBarPrompt.BackColor = Color.White;
			locationBarPrompt.ForeColor = Color.FromArgb(115, 115, 115);
			locationBarPrompt.Font = new Font(font, FontStyle.Italic);
			locationBarPrompt.TabStop = false;
			locationBarPrompt.Cursor = Cursors.IBeam;
			locationBarPrompt.Text = "Enter address or select a bookmark";
			locationBarPrompt.Paint += locationBarPrompt_Paint;
			locationBarPrompt.MouseDown += locationBarPrompt_MouseDown;
		}
		locationBar = (TextBox)mbApiInterface.MB_AddPanel(null, PluginPanelDock.TextBox);
		locationBar.BorderStyle = BorderStyle.None;
		locationBar.BackColor = Color.White;
		locationBar.ForeColor = Color.Black;
		locationBar.Font = font;
		locationBar.TabStop = true;
		locationBar.KeyDown += locationBar_KeyDown;
		header = new Control();
		header.Height = 43;
		header.Controls.AddRange(new Control[2] { locationBarPrompt, locationBar });
		header.Dock = DockStyle.Top;
		header.TabStop = false;
		header.Paint += header_Paint;
		header.Resize += header_Resize;
		header.MouseMove += header_MouseMove;
		header.MouseClick += header_MouseClick;
		browser = new MediaWebBrowser();
		browser.Dock = DockStyle.Fill;
		browser.TabStop = false;
		browser.NavigationLinkClicked += browser_NavigationLinkClicked;
		browser.LinkPlayUrl += browser_LinkPlayUrl;
		browser.NavigationLoading += browser_NavigationLoading;
		browser.NavigationCompleted += browser_NavigationCompleted;
		browser.ScanStarting += browser_ScanStarting;
		browser.ItemScanned += browser_ItemScanned;
		browser.ScanCompleted += browser_ScanCompleted;
		panel.Controls.AddRange(new Control[2] { browser, header });
		panel.TabStop = false;
		mbApiInterface.MB_AddPanel(panel, PluginPanelDock.MainPanel);
		mbApiInterface.MB_SetBackgroundTaskMessage("\0");
		if (!string.IsNullOrEmpty(text))
		{
			ShowNavigationTarget(text);
		}
		MethodInvoker method = [SpecialName] () =>
		{
			locationBar.Focus();
		};
		ThreadStart start = [SpecialName] () =>
		{
			Thread.Sleep(250);
			try
			{
				if (string.IsNullOrEmpty(text))
				{
					panel.Invoke(method);
				}
				else
				{
					NavigateTo(text);
				}
			}
			catch (Exception ex)
			{
				ProjectData.SetProjectError(ex);
				Exception ex2 = ex;
				ProjectData.ClearProjectError();
			}
		};
		new Thread(start).Start();
	}

	public void Navigate(string url)
	{
		if (panel == null)
		{
			loadOnceUrl = url;
			return;
		}
		ShowNavigationTarget(url);
		browser.Focus();
		NavigateTo(locationBar.Text);
	}

	private void CloseWebBrowser(object sender, EventArgs e)
	{
		SaveSettings();
		if (iconLoader != null && iconLoader.IsAlive)
		{
			iconLoader.Abort();
		}
		panel.Dispose();
		header = null;
		locationBarPrompt = null;
		locationBar = null;
		browser = null;
		fileList = null;
		panel = null;
		if (currentIcon != null)
		{
			currentIcon.Dispose();
			currentIcon = null;
		}
		currentIconHost = null;
		reloadOnArtistChanged = false;
		reloadOnAlbumArtistChanged = false;
		reloadOnAlbumChanged = false;
		reloadOnTrackChanged = false;
	}

	private void header_Paint(object sender, PaintEventArgs e)
	{
		using (LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, 1, header.Height - 1), Color.FromArgb(246, 248, 250), Color.FromArgb(235, 237, 239), LinearGradientMode.Vertical))
		{
			e.Graphics.FillRectangle(brush, new Rectangle(0, 0, header.Width, header.Height - 1));
		}
		using (Pen pen = new Pen(Color.FromArgb(187, 190, 193)))
		{
			e.Graphics.DrawLine(pen, 0, header.Height - 1, header.Width, header.Height - 1);
		}
		if (isMouseOverFavourites)
		{
			Rectangle favouritesHighlightBounds = FavouritesHighlightBounds;
			using (LinearGradientBrush brush2 = new LinearGradientBrush(new Rectangle(0, 0, 1, header.Height - 1), Color.FromArgb(251, 253, 255), Color.FromArgb(245, 247, 249), LinearGradientMode.Vertical))
			{
				e.Graphics.FillRectangle(brush2, favouritesHighlightBounds);
			}
			favouritesHighlightBounds.Width--;
			favouritesHighlightBounds.Height--;
			using Pen pen2 = new Pen(Color.FromArgb(210, 210, 210));
			e.Graphics.DrawRectangle(pen2, favouritesHighlightBounds);
		}
		e.Graphics.DrawImage(favouritesButton, FavouritesButtonBounds.Location);
		e.Graphics.DrawImage(browseButtons, new Point(50, 4));
		if (browser.CanGoBack)
		{
			e.Graphics.DrawImage(isMouseOverBrowseBack ? backHighlightButton : backEnabledButton, new Point(59, 13));
		}
		if (browser.CanGoForward)
		{
			e.Graphics.DrawImage(isMouseOverBrowseForward ? forwardHighlightButton : forwardEnabledButton, new Point(88, 14));
		}
		if (isMouseOverBookmark)
		{
			Rectangle bookmarkHighlightBounds = BookmarkHighlightBounds;
			using (LinearGradientBrush brush3 = new LinearGradientBrush(new Rectangle(0, 0, 1, header.Height - 1), Color.FromArgb(251, 253, 255), Color.FromArgb(245, 247, 249), LinearGradientMode.Vertical))
			{
				e.Graphics.FillRectangle(brush3, bookmarkHighlightBounds);
			}
			bookmarkHighlightBounds.Width--;
			bookmarkHighlightBounds.Height--;
			using Pen pen3 = new Pen(Color.FromArgb(210, 210, 210));
			e.Graphics.DrawRectangle(pen3, bookmarkHighlightBounds);
		}
		e.Graphics.DrawImage((!currentIsFavourite) ? bookmarkButton : bookmarkSavedButton, BookmarkButtonBounds.Location);
		e.Graphics.FillRectangle(Brushes.White, new Rectangle(107, 10, header.Width - 107 - 50, 22));
		if (currentIcon != null)
		{
			e.Graphics.DrawImage(currentIcon, CurrentIconBounds);
		}
		e.Graphics.DrawImage(isLoading ? stopButton : refreshButton, RefreshStopButtonBounds.Location);
		using Pen pen4 = new Pen(Color.FromArgb(188, 190, 205));
		e.Graphics.DrawRectangle(pen4, new Rectangle(106, 9, header.Width - 106 - 50, 23));
	}

	private void header_Resize(object sender, EventArgs e)
	{
		locationBar.Bounds = new Rectangle(129, (header.Height - locationBar.Font.Height) / 2, header.Width - 129 - 50 - 20 - 5, locationBar.Font.Height);
		if (locationBarPrompt != null)
		{
			locationBarPrompt.Bounds = new Rectangle(locationBar.Left + 1, locationBar.Top, locationBar.Width - 1, locationBar.Height);
		}
		header.Invalidate();
	}

	private void header_MouseMove(object sender, MouseEventArgs e)
	{
		if (FavouritesHighlightBounds.Contains(e.Location))
		{
			if (!isMouseOverFavourites)
			{
				isMouseOverFavourites = true;
				header.Invalidate(FavouritesHighlightBounds);
			}
		}
		else if (isMouseOverFavourites)
		{
			isMouseOverFavourites = false;
			header.Invalidate(FavouritesHighlightBounds);
		}
		if (browser.CanGoBack)
		{
			if (BrowseBackButtonBounds.Contains(e.Location))
			{
				if (!isMouseOverBrowseBack)
				{
					isMouseOverBrowseBack = true;
					header.Invalidate(BrowseBackButtonBounds);
				}
			}
			else if (isMouseOverBrowseBack)
			{
				isMouseOverBrowseBack = false;
				header.Invalidate(BrowseBackButtonBounds);
			}
		}
		if (browser.CanGoForward)
		{
			if (BrowseForwardButtonBounds.Contains(e.Location))
			{
				if (!isMouseOverBrowseForward)
				{
					isMouseOverBrowseForward = true;
					header.Invalidate(BrowseForwardButtonBounds);
				}
			}
			else if (isMouseOverBrowseForward)
			{
				isMouseOverBrowseForward = false;
				header.Invalidate(BrowseForwardButtonBounds);
			}
		}
		if (BookmarkHighlightBounds.Contains(e.Location))
		{
			if (!isMouseOverBookmark)
			{
				isMouseOverBookmark = true;
				header.Invalidate(BookmarkHighlightBounds);
			}
		}
		else if (isMouseOverBookmark)
		{
			isMouseOverBookmark = false;
			header.Invalidate(BookmarkHighlightBounds);
		}
	}

	private void header_MouseClick(object sender, MouseEventArgs e)
	{
		if (FavouritesHighlightBounds.Contains(e.Location))
		{
			UpdateMusicBeeFavorites();
			if (favouritesPopup == null && (DateTime.UtcNow.Ticks - favouritesPopupLastOpened) / 10000 > 500)
			{
				Point point = header.PointToScreen(FavouritesButtonBounds.Location);
				favouritesPopup = new PopupContainer(new Rectangle(point.X - 10, point.Y + 25, 300, 0), favourites);
				favouritesPopup.Disposed += favouritesPopup_Disposed;
				favouritesPopup.Show(header);
				favouritesPopup.FavoriteClicked += favouritesPopup_FavoriteClicked;
				favouritesPopup.FavoriteRemoved += favouritesPopup_FavoriteRemoved;
			}
		}
		else if (BrowseBackButtonBounds.Contains(e.Location))
		{
			if (browser.CanGoBack)
			{
				browser.GoBack();
			}
		}
		else if (BrowseForwardButtonBounds.Contains(e.Location))
		{
			if (browser.CanGoForward)
			{
				browser.GoForward();
			}
		}
		else if (RefreshStopButtonBounds.Contains(e.Location))
		{
			if (string.IsNullOrEmpty(locationBar.Text))
			{
				return;
			}
			if (isLoading)
			{
				browser.Stop();
				if ((object)browser.Url != null)
				{
					ShowNavigationTarget(browser.Url.ToString());
				}
				isLoading = false;
			}
			else
			{
				browser.Refresh();
				isLoading = true;
			}
			header.Invalidate(RefreshStopButtonBounds);
		}
		else
		{
			if (!BookmarkHighlightBounds.Contains(e.Location) || string.IsNullOrEmpty(locationBar.Text))
			{
				return;
			}
			bool flag = false;
			int num = favourites.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				if (string.Compare(favourites[i].Url, locationBar.Text, StringComparison.OrdinalIgnoreCase) == 0)
				{
					flag = true;
					RemoveFavourite(i);
					break;
				}
			}
			if (!flag)
			{
				favourites.Add(new Bookmark(locationBar.Text, string.IsNullOrEmpty(browser.DocumentTitle) ? "No title" : browser.DocumentTitle, (currentIcon == null) ? null : currentIcon.Clone(new Rectangle(0, 0, 16, 16), PixelFormat.Format32bppPArgb)));
				favourites.Sort(new BookmarkComparer());
				SaveSettings();
				mbApiInterface.MB_SetBackgroundTaskMessage("Bookmark has been saved");
				currentIsFavourite = true;
				header.Invalidate(BookmarkButtonBounds);
			}
		}
	}

	private void RemoveFavourite(int index)
	{
		Bookmark bookmark = favourites[index];
		favourites.RemoveAt(index);
		SaveSettings();
		SetLocationBarText(locationBar.Text);
		mbApiInterface.MB_SetBackgroundTaskMessage("Bookmark '" + bookmark.Name + "' removed");
	}

	private void locationBarPrompt_Paint(object sender, PaintEventArgs e)
	{
		TextRenderer.DrawText(e.Graphics, locationBarPrompt.Text, locationBarPrompt.Font, new Point(0, 0), locationBarPrompt.ForeColor, TextFormatFlags.NoPrefix | TextFormatFlags.NoPadding);
	}

	private void locationBarPrompt_MouseDown(object sender, MouseEventArgs e)
	{
		locationBarPrompt.Dispose();
		locationBarPrompt = null;
		locationBar.Focus();
	}

	private void locationBar_KeyDown(object sender, KeyEventArgs e)
	{
		if (locationBarPrompt != null)
		{
			locationBarPrompt.Dispose();
			locationBarPrompt = null;
		}
		if (e.KeyCode == Keys.Return && locationBar.Text.Length > 0)
		{
			e.Handled = true;
			e.SuppressKeyPress = true;
			if (locationBar.Text.IndexOf("://", StringComparison.OrdinalIgnoreCase) == -1)
			{
				locationBar.Text = "http://" + locationBar.Text;
			}
			reloadOnArtistChanged = false;
			reloadOnAlbumArtistChanged = false;
			reloadOnAlbumChanged = false;
			reloadOnTrackChanged = false;
			Navigate(locationBar.Text);
			isSettingsDirty = true;
		}
	}

	private void browser_LinkPlayUrl(object sender, string url, string mimeType)
	{
		mbApiInterface.NowPlayingList_PlayNow(url);
	}

	private void browser_NavigationLinkClicked(object sender, string url)
	{
		isSettingsDirty = true;
		ShowNavigationTarget(url);
	}

	private void NavigateTo(string url)
	{
		try
		{
			int num = url.IndexOf("<Artist>", StringComparison.OrdinalIgnoreCase);
			reloadOnArtistChanged = num != -1;
			if (num != -1)
			{
				lastArtist = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Artist);
				url = url.Remove(num, 8).Insert(num, Uri.EscapeDataString(lastArtist));
			}
			num = url.IndexOf("<Album Artist>", StringComparison.OrdinalIgnoreCase);
			reloadOnAlbumArtistChanged = num != -1;
			if (num != -1)
			{
				lastAlbumArtist = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.AlbumArtist);
				url = url.Remove(num, 14).Insert(num, Uri.EscapeDataString(lastAlbumArtist));
			}
			num = url.IndexOf("<Title>", StringComparison.OrdinalIgnoreCase);
			reloadOnTrackChanged = num != -1;
			if (num != -1)
			{
				lastTitle = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.TrackTitle);
				url = url.Remove(num, 7).Insert(num, Uri.EscapeDataString(lastTitle));
			}
			num = url.IndexOf("<Album>", StringComparison.OrdinalIgnoreCase);
			reloadOnAlbumChanged = num != -1;
			if (num != -1)
			{
				lastAlbum = mbApiInterface.NowPlaying_GetFileTag(MetaDataType.Album);
				url = url.Remove(num, 7).Insert(num, Uri.EscapeDataString(lastAlbum));
			}
			browser.Navigate(url);
		}
		catch (Exception ex)
		{
			ProjectData.SetProjectError(ex);
			Exception ex2 = ex;
			ProjectData.ClearProjectError();
		}
	}

	private void browser_NavigationLoading(object sender, EventArgs e)
	{
		try
		{
			string url = browser.Url.ToString();
			ShowNavigationTarget(url);
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
		header.Invalidate(BrowseBackButtonBounds);
		header.Invalidate(BrowseForwardButtonBounds);
	}

	private void browser_NavigationCompleted(object sender, EventArgs e)
	{
		isLoading = false;
		using (Graphics graphics = header.CreateGraphics())
		{
			graphics.DrawImage(refreshButton, RefreshStopButtonBounds.Location);
		}
		header.Update();
	}

	private void browser_ScanStarting(object sender, EventArgs e)
	{
		if (fileList != null)
		{
			fileList.Height = 0;
			fileList.Items.Clear();
		}
	}

	private void browser_ScanCompleted(object sender, EventArgs e)
	{
		mbApiInterface.MB_SetBackgroundTaskMessage("");
		if (browser.ItemCount <= 1)
		{
			mbApiInterface.MB_SetBackgroundTaskMessage("\0");
		}
		else
		{
			mbApiInterface.MB_SetBackgroundTaskMessage("\0" + $"{browser.ItemCount} files");
		}
	}

	private void browser_ItemScanned(object sender, MediaFile file)
	{
		panel.Invoke(new ExecuteItemScannedDelegate(ExecuteItemScanned), file);
	}

	private void ExecuteItemScanned(MediaFile file)
	{
		if (fileList == null)
		{
			ColumnHeader columnHeader = new ColumnHeader();
			columnHeader.Text = " ";
			columnHeader.Width = 21;
			ColumnHeader columnHeader2 = new ColumnHeader();
			columnHeader2.Text = "URL";
			ColumnHeader columnHeader3 = new ColumnHeader();
			columnHeader3.Text = "Artist";
			ColumnHeader columnHeader4 = new ColumnHeader();
			columnHeader4.Text = "Title";
			ColumnHeader columnHeader5 = new ColumnHeader();
			columnHeader5.Text = "Album";
			ColumnHeader columnHeader6 = new ColumnHeader();
			columnHeader6.Text = "Size";
			columnHeader6.Width = 55;
			ColumnHeader columnHeader7 = new ColumnHeader();
			columnHeader7.Text = "Time";
			columnHeader7.Width = 40;
			fileList = new ListView();
			fileList.Dock = DockStyle.Bottom;
			fileList.Columns.AddRange(new ColumnHeader[7] { columnHeader, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6, columnHeader7 });
			fileList.View = View.Details;
			fileList.BorderStyle = BorderStyle.None;
			fileList.FullRowSelect = true;
			fileList.MultiSelect = true;
			fileList.Font = locationBar.Font;
			fileList.Size = new Size(panel.Width, (fileList.Font.Height + 3) * 7 + 8);
			fileList.TabStop = false;
			fileList.ShowItemToolTips = true;
			fileList.OwnerDraw = true;
			fileList.ContextMenuStrip = new ContextMenuStrip();
			playNowMenuItem = new ToolStripMenuItem("Play Now", null, playNowMenuItem_Click);
			queueNextMenuItem = new ToolStripMenuItem("Queue Next", null, queueNextMenuItem_Click);
			queueLastMenuItem = new ToolStripMenuItem("Queue Last", null, queueLastMenuItem_Click);
			downloadMenuItem = new ToolStripMenuItem("", null, downloadMenuItem_Click);
			downloadToMenu = new ToolStripMenuItem("Download To");
			downloadToLibraryMenuItem = new ToolStripMenuItem("Music Library", null, downloadToLibraryMenuItem_Click);
			downloadToInboxMenuItem = new ToolStripMenuItem("Inbox", null, downloadToInboxMenuItem_Click);
			downloadToFolderMenuItem = new ToolStripMenuItem("Folder...", null, downloadToFolderMenuItem_Click);
			downloadToLastFolderMenuItem = new ToolStripMenuItem("", null, downloadToLastFolderMenuItem_Click);
			downloadToMenu.DropDownItems.AddRange(new ToolStripItem[6]
			{
				downloadToLibraryMenuItem,
				downloadToInboxMenuItem,
				new ToolStripSeparator(),
				downloadToFolderMenuItem,
				new ToolStripSeparator(),
				downloadToLastFolderMenuItem
			});
			fileList.ContextMenuStrip.Items.AddRange(new ToolStripItem[6]
			{
				playNowMenuItem,
				queueNextMenuItem,
				queueLastMenuItem,
				new ToolStripSeparator(),
				downloadMenuItem,
				downloadToMenu
			});
			FitColumns();
			panel.Controls.Add(fileList);
			fileList.Resize += fileList_Resize;
			fileList.DrawColumnHeader += fileList_DrawColumnHeader;
			fileList.DrawSubItem += fileList_DrawSubItem;
			fileList.ContextMenuStrip.Opening += fileListContextMenu_Opening;
			fileList.MouseDoubleClick += fileList_MouseDoubleClick;
		}
		else if (fileList.Height == 0)
		{
			fileList.Height = (fileList.Font.Height + 3) * 6 + 8;
		}
		ListViewItem listViewItem = ((Operators.CompareString(file.Status, "Opened", TextCompare: false) == 0) ? new ListViewItem(new string[7] { "", file.Url, file.Artist, file.Title, file.Album, file.Size, file.Duration }) : new ListViewItem(new string[7]
		{
			"Unable to open: " + file.Url,
			file.Url,
			"",
			"",
			"",
			"",
			""
		}));
		listViewItem.Tag = file;
		fileList.Items.Add(listViewItem);
	}

	private void fileList_DrawColumnHeader(object sender, DrawListViewColumnHeaderEventArgs e)
	{
		e.DrawDefault = true;
	}

	private void fileList_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
	{
		Color foreColor;
		Color color;
		if (!fileList.SelectedIndices.Contains(e.Item.Index))
		{
			foreColor = Color.Black;
			color = Color.White;
		}
		else
		{
			foreColor = Color.White;
			color = Color.FromArgb(51, 153, 255);
		}
		using (SolidBrush brush = new SolidBrush(color))
		{
			e.Graphics.FillRectangle(brush, e.Bounds);
		}
		if (e.ColumnIndex == 0)
		{
			if (e.SubItem.Text.Length > 0)
			{
				Bitmap image = e.SubItem.Text switch
				{
					"Playing" => playingIcon, 
					"Paused" => playingPausedIcon, 
					"Downloading" => downloadingIcon, 
					"Download Completed" => downloadCompleteIcon, 
					_ => fileAlertIcon, 
				};
				e.Graphics.DrawImage(image, new Point(3, e.Bounds.Top + (e.Bounds.Height - 16) / 2));
			}
		}
		else
		{
			TextRenderer.DrawText(e.Graphics, e.SubItem.Text, e.SubItem.Font, e.Bounds, foreColor, color, TextFormatFlags.EndEllipsis | TextFormatFlags.HidePrefix | TextFormatFlags.VerticalCenter);
		}
	}

	private void fileList_Resize(object sender, EventArgs e)
	{
		FitColumns();
	}

	private void FitColumns()
	{
		int num = fileList.Width - SystemInformation.VerticalScrollBarWidth - 21 - 55 - 40;
		ColumnHeader columnHeader = fileList.Columns[1];
		ColumnHeader columnHeader2 = fileList.Columns[2];
		ColumnHeader columnHeader3 = fileList.Columns[3];
		ColumnHeader columnHeader4 = fileList.Columns[4];
		columnHeader2.Width = (int)Math.Round((double)num * 0.21);
		columnHeader3.Width = (int)Math.Round((double)num * 0.25);
		columnHeader4.Width = (int)Math.Round((double)num * 0.17);
		num -= columnHeader2.Width + columnHeader3.Width + columnHeader4.Width + 1;
		columnHeader.Width = num;
	}

	private void fileList_MouseDoubleClick(object sender, EventArgs e)
	{
		ListViewHitTestInfo listViewHitTestInfo = fileList.HitTest(fileList.PointToClient(Control.MousePosition));
		if (listViewHitTestInfo.Item != null)
		{
			MediaFile mediaFile = (MediaFile)listViewHitTestInfo.Item.Tag;
			mbApiInterface.NowPlayingList_PlayNow(mediaFile.Url);
		}
	}

	private void fileListContextMenu_Opening(object sender, CancelEventArgs e)
	{
		if (fileList.HitTest(fileList.PointToClient(Control.MousePosition)).Item == null)
		{
			e.Cancel = true;
		}
		else
		{
			downloadMenuItem.Text = "Download Now";
			foreach (ListViewItem selectedItem in fileList.SelectedItems)
			{
				if (Operators.CompareString(selectedItem.SubItems[0].Text, "Downloading", TextCompare: false) == 0)
				{
					downloadMenuItem.Text = "Cancel Download";
					break;
				}
			}
		}
		downloadToLibraryMenuItem.Checked = downloadCategory == DownloadTarget.MusicLibrary;
		downloadToInboxMenuItem.Checked = downloadCategory == DownloadTarget.Inbox;
		downloadToLastFolderMenuItem.Text = GetShortenedPath(lastDownloadFolder.Replace("&", "&&"));
		downloadToLastFolderMenuItem.Checked = downloadCategory == DownloadTarget.SpecificFolder;
	}

	private string GetShortenedPath(string folder)
	{
		int num = (folder.EndsWith("\\") ? folder.LastIndexOf('\\', folder.Length - 2) : folder.LastIndexOf('\\'));
		if (num == -1)
		{
			return folder;
		}
		int num2 = folder.IndexOf('\\');
		if (num2 == num)
		{
			return folder;
		}
		num2 = folder.IndexOf('\\', num2 + 1);
		if (num2 == num)
		{
			return folder;
		}
		string text = folder.Substring(0, num2 + 1) + "..." + folder.Substring(num);
		if (text.Length < folder.Length)
		{
			return text;
		}
		return folder;
	}

	private void playNowMenuItem_Click(object sender, EventArgs e)
	{
		int num = fileList.SelectedItems.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			MediaFile mediaFile = (MediaFile)fileList.SelectedItems[i].Tag;
			if (i == 0)
			{
				mbApiInterface.NowPlayingList_PlayNow(mediaFile.Url);
			}
			else
			{
				mbApiInterface.NowPlayingList_QueueNext(mediaFile.Url);
			}
		}
	}

	private void queueNextMenuItem_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem selectedItem in fileList.SelectedItems)
		{
			MediaFile mediaFile = (MediaFile)selectedItem.Tag;
			mbApiInterface.NowPlayingList_QueueNext(mediaFile.Url);
		}
	}

	private void queueLastMenuItem_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem selectedItem in fileList.SelectedItems)
		{
			MediaFile mediaFile = (MediaFile)selectedItem.Tag;
			mbApiInterface.NowPlayingList_QueueLast(mediaFile.Url);
		}
	}

	private void downloadMenuItem_Click(object sender, EventArgs e)
	{
		foreach (ListViewItem selectedItem in fileList.SelectedItems)
		{
			MediaFile mediaFile = (MediaFile)selectedItem.Tag;
			if (Operators.CompareString(downloadMenuItem.Text, "Cancel Download", TextCompare: false) == 0)
			{
				if (Operators.CompareString(selectedItem.SubItems[0].Text, "Downloading", TextCompare: false) == 0)
				{
					selectedItem.SubItems[0].Text = "";
					mbApiInterface.MB_DownloadFile(mediaFile.Url, downloadCategory, lastDownloadFolder, cancelDownload: true);
				}
			}
			else if (Operators.CompareString(selectedItem.SubItems[0].Text, "Downloading", TextCompare: false) != 0 && mbApiInterface.MB_DownloadFile(mediaFile.Url, downloadCategory, lastDownloadFolder, cancelDownload: false))
			{
				selectedItem.SubItems[0].Text = "Downloading";
			}
		}
	}

	private void downloadToLibraryMenuItem_Click(object sender, EventArgs e)
	{
		downloadCategory = DownloadTarget.MusicLibrary;
	}

	private void downloadToInboxMenuItem_Click(object sender, EventArgs e)
	{
		downloadCategory = DownloadTarget.Inbox;
	}

	private void downloadToFolderMenuItem_Click(object sender, EventArgs e)
	{
		using FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		folderBrowserDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
		if (folderBrowserDialog.ShowDialog(panel) == DialogResult.OK)
		{
			lastDownloadFolder = folderBrowserDialog.SelectedPath;
			downloadCategory = DownloadTarget.SpecificFolder;
		}
	}

	private void downloadToLastFolderMenuItem_Click(object sender, EventArgs e)
	{
		downloadCategory = DownloadTarget.SpecificFolder;
	}

	private void ShowNavigationTarget(string url)
	{
		SetLocationBarText(url);
		string host = GetHost(url);
		if (string.Compare(host, currentIconHost, StringComparison.OrdinalIgnoreCase) != 0)
		{
			if (iconLoader != null && iconLoader.IsAlive)
			{
				iconLoader.Abort();
			}
			if (currentIcon != null)
			{
				currentIcon.Dispose();
				currentIcon = null;
			}
			currentIconHost = host;
			int num = favourites.Count - 1;
			for (int i = 0; i <= num; i++)
			{
				if (string.Compare(GetHost(favourites[i].Url), host, StringComparison.OrdinalIgnoreCase) == 0)
				{
					if (favourites[i].Icon != null)
					{
						currentIcon = favourites[i].Icon.Clone(new Rectangle(0, 0, 16, 16), PixelFormat.Format32bppPArgb);
					}
					break;
				}
			}
			if (currentIcon != null)
			{
				using Graphics graphics = header.CreateGraphics();
				graphics.FillRectangle(Brushes.White, CurrentIconBounds);
				graphics.DrawImage(currentIcon, CurrentIconBounds);
			}
			else
			{
				using (Graphics graphics2 = header.CreateGraphics())
				{
					graphics2.FillRectangle(Brushes.White, CurrentIconBounds);
				}
				if (currentIconHost.Length > 0)
				{
					iconLoader = new Thread(LoadCurrentIcon);
					iconLoader.IsBackground = true;
					iconLoader.Start();
				}
			}
		}
		if (!isLoading)
		{
			isLoading = true;
			using Graphics graphics3 = header.CreateGraphics();
			graphics3.DrawImage(stopButton, RefreshStopButtonBounds.Location);
		}
		header.Update();
	}

	private void SetLocationBarText(string value)
	{
		locationBar.Text = value;
		bool flag = currentIsFavourite;
		currentIsFavourite = false;
		int num = favourites.Count - 1;
		for (int i = 0; i <= num; i++)
		{
			if (string.Compare(favourites[i].Url, value, StringComparison.OrdinalIgnoreCase) == 0)
			{
				currentIsFavourite = true;
				break;
			}
		}
		if (currentIsFavourite != flag)
		{
			header.Invalidate(BookmarkButtonBounds);
		}
	}

	private string GetHost(string url)
	{
		int startIndex = url.IndexOf("://", StringComparison.OrdinalIgnoreCase) + 3;
		int num = url.IndexOf('/', startIndex);
		if (num == -1)
		{
			num = url.Length;
		}
		return url.Substring(0, num);
	}

	private void LoadCurrentIcon()
	{
		try
		{
			byte[] array = (byte[])new HttpSession(20000).Request(currentIconHost + "/favicon.ico", RequestType.Data);
			if (array == null || array.Length <= 4)
			{
				return;
			}
			switch (BitConverter.ToUInt32(array, 0))
			{
			case 3774863615u:
			case 3791640831u:
			case 169478669u:
			case 944130375u:
			case 1196314761u:
				currentIcon = (Bitmap)bitmapConverter.ConvertFrom(array);
				break;
			default:
			{
				if (BitConverter.ToUInt16(array, 0) == 19778)
				{
					currentIcon = (Bitmap)bitmapConverter.ConvertFrom(array);
					break;
				}
				Icon icon = (Icon)iconConverter.ConvertFrom(array);
				if (icon.Width == 16 && icon.Height == 16)
				{
					currentIcon = (Bitmap)bitmapConverter.ConvertFrom(icon);
					break;
				}
				Bitmap image = new Bitmap(16, 16, PixelFormat.Format32bppPArgb);
				using (Graphics graphics = Graphics.FromImage(image))
				{
					graphics.PixelOffsetMode = PixelOffsetMode.Half;
					graphics.InterpolationMode = InterpolationMode.Bicubic;
					graphics.DrawIcon(icon, new Rectangle(0, 0, 16, 16));
				}
				currentIcon = image;
				break;
			}
			}
			MethodInvoker method = [SpecialName] () =>
			{
				using (Graphics graphics2 = header.CreateGraphics())
				{
					graphics2.DrawImage(currentIcon, CurrentIconBounds);
				}
				header.Update();
			};
			panel.Invoke(method);
		}
		catch (Exception projectError)
		{
			ProjectData.SetProjectError(projectError);
			ProjectData.ClearProjectError();
		}
	}

	private void favouritesPopup_FavoriteClicked(Bookmark favourite)
	{
		if (locationBarPrompt != null)
		{
			locationBarPrompt.Dispose();
			locationBarPrompt = null;
		}
		panel.Focus();
		defaultUrl = favourite.Url;
		activeUrl = defaultUrl;
		ShowNavigationTarget(activeUrl);
		ThreadStart start = [SpecialName] () =>
		{
			NavigateTo(activeUrl);
		};
		new Thread(start).Start();
		isSettingsDirty = true;
	}

	private void favouritesPopup_FavoriteRemoved(int index)
	{
		RemoveFavourite(index);
	}

	private void favouritesPopup_Disposed(object sender, EventArgs e)
	{
		favouritesPopup = null;
		favouritesPopupLastOpened = DateTime.UtcNow.Ticks;
		panel.Update();
	}
}
