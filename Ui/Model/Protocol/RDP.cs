﻿using System;
using System.Diagnostics;
using Newtonsoft.Json;
using PRM.Model.Protocol.Base;
using PRM.Service;
using PRM.Utils.RdpFile;
using Shawn.Utils;

namespace PRM.Model.Protocol
{
    public enum ERdpWindowResizeMode
    {
        AutoResize = 0,
        Stretch = 1,
        Fixed = 2,
        StretchFullScreen = 3,
        FixedFullScreen = 4,
    }

    public enum ERdpFullScreenFlag
    {
        Disable = 0,
        EnableFullScreen = 1,
        EnableFullAllScreens = 2,
    }

    public enum EDisplayPerformance
    {
        /// <summary>
        /// Auto judge(by connection speed)
        /// </summary>
        Auto = 0,

        /// <summary>
        /// Low(8bit color with no feature support)
        /// </summary>
        Low = 1,

        /// <summary>
        /// Middle(16bit color with only font smoothing and desktop composition)
        /// </summary>
        Middle = 2,

        /// <summary>
        /// High(32bit color with full features support)
        /// </summary>
        High = 3,
    }

    public enum EGatewayMode
    {
        AutomaticallyDetectGatewayServerSettings = 0,
        UseTheseGatewayServerSettings = 1,
        DoNotUseGateway = 2,
    }

    public enum EGatewayLogonMethod
    {
        Password = 0,
        SmartCard = 1,
    }


    public enum EAudioRedirectionMode
    {
        RedirectToLocal = 0,
        LeaveOnRemote = 1,
        Disabled = 2,
    }

    public class RdpLocalSetting
    {
        public DateTime LastUpdateTime { get; set; } = DateTime.MinValue;
        public bool FullScreenLastSessionIsFullScreen { get; set; } = false;
        public int FullScreenLastSessionScreenIndex { get; set; } = -1;
    }

    // ReSharper disable once InconsistentNaming
    public sealed class RDP : ProtocolBaseWithAddressPortUserPwd
    {
        public const string PROTOCOL_NAME = "RDP";
        public RDP() : base(PROTOCOL_NAME, "RDP.V1", "RDP")
        {
            base.Port = "3389";
            base.UserName = "Administrator";
        }

        private bool? _isAdministrativePurposes = false;
        public bool? IsAdministrativePurposes
        {
            get => _isAdministrativePurposes;
            set => SetAndNotifyIfChanged(ref _isAdministrativePurposes, value);
        }

        private string _domain = "";
        public string Domain
        {
            get => _domain;
            set => SetAndNotifyIfChanged(ref _domain, value);
        }

        private string _loadBalanceInfo = "";
        public string LoadBalanceInfo
        {
            get => _loadBalanceInfo;
            set => SetAndNotifyIfChanged(ref _loadBalanceInfo, value);
        }

        #region Display

        private ERdpFullScreenFlag? _rdpFullScreenFlag = ERdpFullScreenFlag.EnableFullScreen;
        public ERdpFullScreenFlag? RdpFullScreenFlag
        {
            get => _rdpFullScreenFlag;
            set
            {
                SetAndNotifyIfChanged(ref _rdpFullScreenFlag, value);
                switch (value)
                {
                    case ERdpFullScreenFlag.EnableFullAllScreens:
                        IsConnWithFullScreen = true;
                        if (RdpWindowResizeMode == ERdpWindowResizeMode.Fixed)
                            RdpWindowResizeMode = ERdpWindowResizeMode.FixedFullScreen;
                        if (RdpWindowResizeMode == ERdpWindowResizeMode.Stretch)
                            RdpWindowResizeMode = ERdpWindowResizeMode.StretchFullScreen;
                        break;

                    case ERdpFullScreenFlag.Disable:
                        IsConnWithFullScreen = false;
                        if (RdpWindowResizeMode == ERdpWindowResizeMode.FixedFullScreen)
                            RdpWindowResizeMode = ERdpWindowResizeMode.Fixed;
                        if (RdpWindowResizeMode == ERdpWindowResizeMode.StretchFullScreen)
                            RdpWindowResizeMode = ERdpWindowResizeMode.Stretch;
                        break;

                    case ERdpFullScreenFlag.EnableFullScreen:
                    default:
                        break;
                }
            }
        }

        private bool? _isConnWithFullScreen = false;
        public bool? IsConnWithFullScreen
        {
            get => _isConnWithFullScreen;
            set => SetAndNotifyIfChanged(ref _isConnWithFullScreen, value);
        }

        private bool? _isFullScreenWithConnectionBar = true;
        public bool? IsFullScreenWithConnectionBar
        {
            get => _isFullScreenWithConnectionBar;
            set => SetAndNotifyIfChanged(ref _isFullScreenWithConnectionBar, value);
        }

        private ERdpWindowResizeMode? _rdpWindowResizeMode = ERdpWindowResizeMode.AutoResize;
        public ERdpWindowResizeMode? RdpWindowResizeMode
        {
            get => _rdpWindowResizeMode;
            set
            {
                var tmp = value;
                if (RdpFullScreenFlag == ERdpFullScreenFlag.Disable)
                {
                    if (tmp == ERdpWindowResizeMode.FixedFullScreen)
                        tmp = ERdpWindowResizeMode.Fixed;
                    if (tmp == ERdpWindowResizeMode.StretchFullScreen)
                        tmp = ERdpWindowResizeMode.Stretch;
                }
                _rdpWindowResizeMode = tmp;
                RaisePropertyChanged(nameof(RdpWindowResizeMode));
            }
        }

        private int? _rdpWidth = 800;
        public int? RdpWidth
        {
            get => _rdpWidth;
            set => SetAndNotifyIfChanged(ref _rdpWidth, value);
        }

        private int? _rdpHeight = 600;
        public int? RdpHeight
        {
            get => _rdpHeight;
            set => SetAndNotifyIfChanged(ref _rdpHeight, value);
        }


        private bool? _isScaleFactorFollowSystem = true;
        public bool? IsScaleFactorFollowSystem
        {
            get => _isScaleFactorFollowSystem;
            set => SetAndNotifyIfChanged(ref _isScaleFactorFollowSystem, value);
        }

        private uint? _scaleFactorCustomValue = 100;
        public uint? ScaleFactorCustomValue
        {
            get => _scaleFactorCustomValue;
            set
            {
                uint? @new = value;
                if (value != null)
                {
                    @new = (uint)value;
                    if (@new > 300)
                        @new = 300;
                    if (@new < 100)
                        @new = 100;
                }
                SetAndNotifyIfChanged(ref _scaleFactorCustomValue, @new);
            }
        }


        private EDisplayPerformance? _displayPerformance = EDisplayPerformance.Auto;
        public EDisplayPerformance? DisplayPerformance
        {
            get => _displayPerformance;
            set => SetAndNotifyIfChanged(ref _displayPerformance, value);
        }

        #endregion Display

        #region resource switch

        private bool? _enableClipboard = true;
        public bool? EnableClipboard
        {
            get => _enableClipboard;
            set => SetAndNotifyIfChanged(ref _enableClipboard, value);
        }

        private bool? _enableDiskDrives = true;
        public bool? EnableDiskDrives
        {
            get => _enableDiskDrives;
            set => SetAndNotifyIfChanged(ref _enableDiskDrives, value);
        }

        private bool? _enableKeyCombinations = true;
        public bool? EnableKeyCombinations
        {
            get => _enableKeyCombinations;
            set => SetAndNotifyIfChanged(ref _enableKeyCombinations, value);
        }


        private EAudioRedirectionMode? _audioRedirectionMode = EAudioRedirectionMode.RedirectToLocal;
        public EAudioRedirectionMode? AudioRedirectionMode
        {
            get => _audioRedirectionMode;
            set => SetAndNotifyIfChanged(ref _audioRedirectionMode, value);
        }


        private bool? _enableAudioCapture = false;
        public bool? EnableAudioCapture
        {
            get => _enableAudioCapture;
            set => SetAndNotifyIfChanged(ref _enableAudioCapture, value);
        }


        private bool? _enablePorts = false;
        public bool? EnablePorts
        {
            get => _enablePorts;
            set => SetAndNotifyIfChanged(ref _enablePorts, value);
        }


        private bool? _enablePrinters = false;
        public bool? EnablePrinters
        {
            get => _enablePrinters;
            set => SetAndNotifyIfChanged(ref _enablePrinters, value);
        }


        private bool? _enableSmartCardsAndWinHello = false;
        public bool? EnableSmartCardsAndWinHello
        {
            get => _enableSmartCardsAndWinHello;
            set => SetAndNotifyIfChanged(ref _enableSmartCardsAndWinHello, value);
        }

        #endregion resource switch

        #region MSTSC model

        private bool _mstscModeEnabled = false;
        public bool MstscModeEnabled
        {
            get => _mstscModeEnabled;
            set => SetAndNotifyIfChanged(ref _mstscModeEnabled, value);
        }


        private string _rdpFileAdditionalSettings = "";
        public string RdpFileAdditionalSettings
        {
            get => _rdpFileAdditionalSettings;
            set => SetAndNotifyIfChanged(ref _rdpFileAdditionalSettings, value);
        }

        #endregion

        #region Gateway

        private EGatewayMode? _gatewayMode = EGatewayMode.DoNotUseGateway;

        public EGatewayMode? GatewayMode
        {
            get => _gatewayMode;
            set => SetAndNotifyIfChanged(ref _gatewayMode, value);
        }

        private bool? _gatewayBypassForLocalAddress = true;

        public bool? GatewayBypassForLocalAddress
        {
            get => _gatewayBypassForLocalAddress;
            set => SetAndNotifyIfChanged(ref _gatewayBypassForLocalAddress, value);
        }

        private string _gatewayHostName = "";

        public string GatewayHostName
        {
            get => _gatewayHostName;
            set => SetAndNotifyIfChanged(ref _gatewayHostName, value);
        }

        private EGatewayLogonMethod? _gatewayLogonMethod = EGatewayLogonMethod.Password;

        public EGatewayLogonMethod? GatewayLogonMethod
        {
            get => _gatewayLogonMethod;
            set => SetAndNotifyIfChanged(ref _gatewayLogonMethod, value);
        }

        private string _gatewayUserName = "";

        public string GatewayUserName
        {
            get => _gatewayUserName;
            set => SetAndNotifyIfChanged(ref _gatewayUserName, value);
        }

        private string _gatewayPassword = "";

        public string GatewayPassword
        {
            get => _gatewayPassword;
            set => SetAndNotifyIfChanged(ref _gatewayPassword, value);
        }

        #endregion Gateway

        //private RdpLocalSetting _autoSetting = new RdpLocalSetting();
        //public RdpLocalSetting AutoSetting
        //{
        //    get => _autoSetting;
        //    private set => SetAndNotifyIfChanged(ref _autoSetting, value);
        //}

        public override bool IsOnlyOneInstance()
        {
            return true;
        }

        public override ProtocolBase? CreateFromJsonString(string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<RDP>(jsonString);
            }
            catch (Exception e)
            {
                SimpleLogHelper.Debug(e);
                return null;
            }
        }

        public override double GetListOrder()
        {
            return 0;
        }

        /// <summary>
        /// To rdp file object
        /// </summary>
        /// <returns></returns>
        public RdpConfig ToRdpConfig(IDataService dataService)
        {
            var rdpConfig = new RdpConfig($"{this.Address}:{this.GetPort()}", this.UserName, dataService.DecryptOrReturnOriginalString(Password), RdpFileAdditionalSettings)
            {
                Domain = this.Domain,
                LoadBalanceInfo = this.LoadBalanceInfo,
                AuthenticationLevel = 0,
                DisplayConnectionBar = this.IsFullScreenWithConnectionBar == true ? 1 : 0
            };

            switch (this.RdpFullScreenFlag)
            {
                case ERdpFullScreenFlag.Disable:
                    rdpConfig.ScreenModeId = 1;
                    rdpConfig.DesktopWidth = this.RdpWidth > 0 ? this.RdpWidth ?? 800 : 800;
                    rdpConfig.DesktopHeight = this.RdpHeight > 0 ? this.RdpHeight ?? 600 : 600;
                    break;

                case ERdpFullScreenFlag.EnableFullAllScreens:
                    rdpConfig.ScreenModeId = 2;
                    rdpConfig.UseMultimon = 1;
                    break;

                case ERdpFullScreenFlag.EnableFullScreen:
                    rdpConfig.ScreenModeId = 2;
                    break;

                default:
                    break;
            }

            switch (this.RdpWindowResizeMode)
            {
                case ERdpWindowResizeMode.Stretch:
                    rdpConfig.SmartSizing = 1;
                    rdpConfig.DynamicResolution = 0;
                    break;

                case ERdpWindowResizeMode.Fixed:
                    rdpConfig.SmartSizing = 0;
                    rdpConfig.DynamicResolution = 0;
                    rdpConfig.DesktopWidth = this.RdpWidth > 0 ? this.RdpWidth ?? 800 : 800;
                    rdpConfig.DesktopHeight = this.RdpHeight > 0 ? this.RdpHeight ?? 600 : 600;
                    break;

                case ERdpWindowResizeMode.AutoResize:
                default:
                    rdpConfig.SmartSizing = 0;
                    rdpConfig.DynamicResolution = 1;
                    break;
            }

            rdpConfig.NetworkAutodetect = 0;
            switch (this.DisplayPerformance)
            {
                case EDisplayPerformance.Low:
                    rdpConfig.ConnectionType = 1;
                    rdpConfig.SessionBpp = 8;
                    rdpConfig.AllowDesktopComposition = 0;
                    rdpConfig.AllowFontSmoothing = 0;
                    rdpConfig.DisableFullWindowDrag = 1;
                    rdpConfig.DisableThemes = 1;
                    rdpConfig.DisableWallpaper = 1;
                    rdpConfig.DisableMenuAnims = 1;
                    rdpConfig.DisableCursorSetting = 1;
                    break;

                case EDisplayPerformance.Middle:
                    rdpConfig.SessionBpp = 16;
                    rdpConfig.ConnectionType = 3;
                    rdpConfig.AllowDesktopComposition = 1;
                    rdpConfig.AllowFontSmoothing = 1;
                    rdpConfig.DisableFullWindowDrag = 1;
                    rdpConfig.DisableThemes = 1;
                    rdpConfig.DisableWallpaper = 1;
                    rdpConfig.DisableMenuAnims = 1;
                    rdpConfig.DisableCursorSetting = 1;
                    break;

                case EDisplayPerformance.High:
                    rdpConfig.SessionBpp = 32;
                    rdpConfig.ConnectionType = 7;
                    rdpConfig.AllowDesktopComposition = 1;
                    rdpConfig.AllowFontSmoothing = 1;
                    rdpConfig.DisableFullWindowDrag = 0;
                    rdpConfig.DisableThemes = 0;
                    rdpConfig.DisableWallpaper = 0;
                    rdpConfig.DisableMenuAnims = 0;
                    rdpConfig.DisableCursorSetting = 0;
                    break;

                case EDisplayPerformance.Auto:
                default:
                    rdpConfig.NetworkAutodetect = 1;
                    break;
            }


            if (this.EnableDiskDrives == true)
            {
                rdpConfig.DriveStoreDirect = "*";
                rdpConfig.RedirectDrives = 1;
            }
            else
            {
                rdpConfig.DriveStoreDirect = "";
                rdpConfig.RedirectDrives = 0;
            }

            if (this.EnableClipboard == true)
                rdpConfig.RedirectClipboard = 1;
            if (this.EnablePrinters == true)
                rdpConfig.RedirectPrinters = 1;
            if (this.EnablePorts == true)
                rdpConfig.RedirectComPorts = 1;
            else
                rdpConfig.RedirectComPorts = 0;

            if (this.EnableSmartCardsAndWinHello == true)
                rdpConfig.RedirectSmartCards = 1;
            if (this.EnableKeyCombinations == true)
                rdpConfig.KeyboardHook = 2;
            else
                rdpConfig.KeyboardHook = 0;

            if (this.AudioRedirectionMode == EAudioRedirectionMode.RedirectToLocal)
                rdpConfig.AudioMode = 0;
            else if (this.AudioRedirectionMode == EAudioRedirectionMode.LeaveOnRemote)
                rdpConfig.AudioMode = 1;
            else if (this.AudioRedirectionMode == EAudioRedirectionMode.Disabled)
                rdpConfig.AudioMode = 2;
            if (this.EnableAudioCapture == true)
                rdpConfig.AudioCaptureMode = 1;

            rdpConfig.AutoReconnectionEnabled = 1;

            switch (GatewayMode)
            {
                case EGatewayMode.AutomaticallyDetectGatewayServerSettings:
                    rdpConfig.GatewayUsageMethod = 2;
                    break;

                case EGatewayMode.UseTheseGatewayServerSettings:
                    rdpConfig.GatewayUsageMethod = 1;
                    break;

                case EGatewayMode.DoNotUseGateway:
                default:
                    rdpConfig.GatewayUsageMethod = 0;
                    break;
            }
            rdpConfig.GatewayHostname = this.GatewayHostName;
            rdpConfig.GatewayCredentialsSource = 4;
            return rdpConfig;
        }



        public override bool ThisTimeConnWithFullScreen()
        {
            if (this.RdpFullScreenFlag == ERdpFullScreenFlag.EnableFullAllScreens
                || this.IsConnWithFullScreen == true
                || IoC.Get<LocalityService>().RdpLocalityGet(this.Id.ToString())?.FullScreenLastSessionIsFullScreen == true)
                return true;
            return false;
        }
    }
}