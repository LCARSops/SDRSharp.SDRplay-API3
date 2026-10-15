using System;
using System.Runtime.InteropServices;

namespace SDRSharp.SDRplay
{
    /// <summary>
    /// P/Invoke layer for SDRplay API 3.x (3.11+).
    /// Structures match the official headers (RSP1 / RSP1A focused).
    /// </summary>
    public static class NativeMethods
    {
        public const string APIDLL = "sdrplay_api.dll";

        // ------------------------------------------------------------------
        // Constants
        // ------------------------------------------------------------------
        public const float SDRPLAY_API_VERSION = 3.11f;

        public const int SDRPLAY_MAX_DEVICES = 16;
        public const int SDRPLAY_MAX_SER_NO_LEN = 64;
        public const int SDRPLAY_MAX_ROOT_NM_LEN = 32;

        public const byte SDRPLAY_RSP1_ID = 1;
        public const byte SDRPLAY_RSP1A_ID = 255;
        public const byte SDRPLAY_RSP2_ID = 2;
        public const byte SDRPLAY_RSPduo_ID = 3;
        public const byte SDRPLAY_RSPdx_ID = 4;
        public const byte SDRPLAY_RSP1B_ID = 6;
        public const byte SDRPLAY_RSPdxR2_ID = 7;

        // ------------------------------------------------------------------
        // Enums
        // ------------------------------------------------------------------
        public enum sdrplay_api_ErrT : int
        {
            sdrplay_api_Success = 0,
            sdrplay_api_Fail = 1,
            sdrplay_api_InvalidParam = 2,
            sdrplay_api_OutOfRange = 3,
            sdrplay_api_GainUpdateError = 4,
            sdrplay_api_RfUpdateError = 5,
            sdrplay_api_FsUpdateError = 6,
            sdrplay_api_HwError = 7,
            sdrplay_api_AliasingError = 8,
            sdrplay_api_AlreadyInitialised = 9,
            sdrplay_api_NotInitialised = 10,
            sdrplay_api_NotEnabled = 11,
            sdrplay_api_HwVerError = 12,
            sdrplay_api_OutOfMemError = 13,
            sdrplay_api_ServiceNotResponding = 14,
            sdrplay_api_StartPending = 15,
            sdrplay_api_StopPending = 16,
            sdrplay_api_InvalidMode = 17,
            sdrplay_api_FailedVerification1 = 18,
            sdrplay_api_FailedVerification2 = 19,
            sdrplay_api_FailedVerification3 = 20,
            sdrplay_api_FailedVerification4 = 21,
            sdrplay_api_FailedVerification5 = 22,
            sdrplay_api_FailedVerification6 = 23,
            sdrplay_api_InvalidServiceVersion = 24
        }

        public enum sdrplay_api_TunerSelectT : int
        {
            sdrplay_api_Tuner_Neither = 0,
            sdrplay_api_Tuner_A = 1,
            sdrplay_api_Tuner_B = 2,
            sdrplay_api_Tuner_Both = 3
        }

        public enum sdrplay_api_RspDuoModeT : int
        {
            sdrplay_api_RspDuoMode_Unknown = 0,
            sdrplay_api_RspDuoMode_Single_Tuner = 1,
            sdrplay_api_RspDuoMode_Dual_Tuner = 2,
            sdrplay_api_RspDuoMode_Master = 4,
            sdrplay_api_RspDuoMode_Slave = 8
        }

        public enum sdrplay_api_Bw_MHzT : int
        {
            sdrplay_api_BW_Undefined = 0,
            sdrplay_api_BW_0_200 = 200,
            sdrplay_api_BW_0_300 = 300,
            sdrplay_api_BW_0_600 = 600,
            sdrplay_api_BW_1_536 = 1536,
            sdrplay_api_BW_5_000 = 5000,
            sdrplay_api_BW_6_000 = 6000,
            sdrplay_api_BW_7_000 = 7000,
            sdrplay_api_BW_8_000 = 8000
        }

        /// <summary>Canonical bandwidth list for UI (same values as API enum).</summary>
        public static readonly sdrplay_api_Bw_MHzT[] SupportedBandwidths =
        {
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_200,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_300,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_600,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_1_536,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_5_000,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_6_000,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_7_000,
            sdrplay_api_Bw_MHzT.sdrplay_api_BW_8_000
        };

        public static string BandwidthLabel(sdrplay_api_Bw_MHzT bw)
        {
            switch (bw)
            {
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_200: return "0.200 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_300: return "0.300 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_0_600: return "0.600 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_1_536: return "1.536 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_5_000: return "5.000 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_6_000: return "6.000 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_7_000: return "7.000 MHz";
                case sdrplay_api_Bw_MHzT.sdrplay_api_BW_8_000: return "8.000 MHz";
                default: return bw.ToString();
            }
        }

        public enum sdrplay_api_If_kHzT : int
        {
            sdrplay_api_IF_Undefined = -1,
            sdrplay_api_IF_Zero = 0,
            sdrplay_api_IF_0_450 = 450,
            sdrplay_api_IF_1_620 = 1620,
            sdrplay_api_IF_2_048 = 2048
        }

        public enum sdrplay_api_LoModeT : int
        {
            sdrplay_api_LO_Undefined = 0,
            sdrplay_api_LO_Auto = 1,
            sdrplay_api_LO_120MHz = 2,
            sdrplay_api_LO_144MHz = 3,
            sdrplay_api_LO_168MHz = 4
        }

        public enum sdrplay_api_MinGainReductionT : int
        {
            sdrplay_api_EXTENDED_MIN_GR = 0,
            sdrplay_api_NORMAL_MIN_GR = 20
        }

        public enum sdrplay_api_AgcControlT : int
        {
            sdrplay_api_AGC_DISABLE = 0,
            sdrplay_api_AGC_100HZ = 1,
            sdrplay_api_AGC_50HZ = 2,
            sdrplay_api_AGC_5HZ = 3,
            sdrplay_api_AGC_CTRL_EN = 4
        }

        public enum sdrplay_api_AdsbModeT : int
        {
            sdrplay_api_ADSB_DECIMATION = 0,
            sdrplay_api_ADSB_NO_DECIMATION_LOWPASS = 1,
            sdrplay_api_ADSB_NO_DECIMATION_BANDPASS_2MHZ = 2,
            sdrplay_api_ADSB_NO_DECIMATION_BANDPASS_3MHZ = 3
        }

        public enum sdrplay_api_TransferModeT : int
        {
            sdrplay_api_ISOCH = 0,
            sdrplay_api_BULK = 1
        }

        public enum sdrplay_api_RspDx_AntennaSelectT : int
        {
            sdrplay_api_RspDx_ANTENNA_A = 0,
            sdrplay_api_RspDx_ANTENNA_B = 1,
            sdrplay_api_RspDx_ANTENNA_C = 2
        }

        public enum sdrplay_api_Rsp2_AntennaSelectT : int
        {
            sdrplay_api_Rsp2_ANTENNA_A = 5,
            sdrplay_api_Rsp2_ANTENNA_B = 6
        }

        public enum sdrplay_api_Rsp2_AmPortSelectT : int
        {
            sdrplay_api_Rsp2_AMPORT_2 = 0,
            sdrplay_api_Rsp2_AMPORT_1 = 1
        }

        public enum sdrplay_api_RspDuo_AmPortSelectT : int
        {
            sdrplay_api_RspDuo_AMPORT_2 = 0,
            sdrplay_api_RspDuo_AMPORT_1 = 1
        }

        [Flags]
        public enum sdrplay_api_ReasonForUpdateT : uint
        {
            sdrplay_api_Update_None = 0x00000000,
            sdrplay_api_Update_Dev_Fs = 0x00000001,
            sdrplay_api_Update_Dev_Ppm = 0x00000002,
            sdrplay_api_Update_Dev_SyncUpdate = 0x00000004,
            sdrplay_api_Update_Dev_ResetFlags = 0x00000008,
            sdrplay_api_Update_Rsp1a_BiasTControl = 0x00000010,
            sdrplay_api_Update_Rsp1a_RfNotchControl = 0x00000020,
            sdrplay_api_Update_Rsp1a_RfDabNotchControl = 0x00000040,
            sdrplay_api_Update_Tuner_Gr = 0x00008000,
            sdrplay_api_Update_Tuner_GrLimits = 0x00010000,
            sdrplay_api_Update_Tuner_Frf = 0x00020000,
            sdrplay_api_Update_Tuner_BwType = 0x00040000,
            sdrplay_api_Update_Tuner_IfType = 0x00080000,
            sdrplay_api_Update_Tuner_DcOffset = 0x00100000,
            sdrplay_api_Update_Tuner_LoMode = 0x00200000,
            sdrplay_api_Update_Ctrl_DCoffsetIQimbalance = 0x00400000,
            sdrplay_api_Update_Ctrl_Decimation = 0x00800000,
            sdrplay_api_Update_Ctrl_Agc = 0x01000000,
            sdrplay_api_Update_Ctrl_AdsbMode = 0x02000000,
            sdrplay_api_Update_Ctrl_OverloadMsgAck = 0x04000000
        }

        public enum sdrplay_api_ReasonForUpdateExtension1T : uint
        {
            sdrplay_api_Update_Ext1_None = 0x00000000
        }

        public enum sdrplay_api_EventT : int
        {
            sdrplay_api_GainChange = 0,
            sdrplay_api_PowerOverloadChange = 1,
            sdrplay_api_DeviceRemoved = 2,
            sdrplay_api_RspDuoModeChange = 3,
            sdrplay_api_DeviceFailure = 4
        }

        public enum sdrplay_api_PowerOverloadCbEventIdT : int
        {
            sdrplay_api_Overload_Detected = 0,
            sdrplay_api_Overload_Corrected = 1
        }

        // ------------------------------------------------------------------
        // Device enumeration structure
        // Pack=8 matches MSVC default; valid field present since API 3.08
        // ------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 8)]
        public struct sdrplay_api_DeviceT
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = SDRPLAY_MAX_SER_NO_LEN)]
            public string SerNo;                    // 64

            public byte hwVer;
            // padding inserted by Pack=8 before next 4-byte enum
            public sdrplay_api_TunerSelectT tuner;
            public sdrplay_api_RspDuoModeT rspDuoMode;
            public byte valid;
            // padding before double
            public double rspDuoSampleFreq;
            public IntPtr dev;
        }

        // ------------------------------------------------------------------
        // DevParams (pointed to by DeviceParamsT.devParams)
        // ------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_FsFreqT
        {
            public double fsHz;
            public byte syncUpdate;
            public byte reCal;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_SyncUpdateT
        {
            public uint sampleNum;
            public uint period;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_ResetFlagsT
        {
            public byte resetGainUpdate;
            public byte resetRfUpdate;
            public byte resetFsUpdate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_Rsp1aParamsT
        {
            public byte rfNotchEnable;
            public byte rfDabNotchEnable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_Rsp2ParamsT
        {
            public byte extRefOutputEn;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RspDuoParamsT
        {
            public int extRefOutputEn;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RspDxParamsT
        {
            public byte hdrEnable;
            public byte biasTEnable;
            public sdrplay_api_RspDx_AntennaSelectT antennaSel;
            public byte rfNotchEnable;
            public byte rfDabNotchEnable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_DevParamsT
        {
            public double ppm;
            public sdrplay_api_FsFreqT fsFreq;
            public sdrplay_api_SyncUpdateT syncUpdate;
            public sdrplay_api_ResetFlagsT resetFlags;
            public sdrplay_api_TransferModeT mode;
            public uint samplesPerPkt;
            public sdrplay_api_Rsp1aParamsT rsp1aParams;
            public sdrplay_api_Rsp2ParamsT rsp2Params;
            public sdrplay_api_RspDuoParamsT rspDuoParams;
            public sdrplay_api_RspDxParamsT rspDxParams;
        }

        // ------------------------------------------------------------------
        // Tuner / control / channel params
        // ------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_GainValuesT
        {
            public float curr;
            public float max;
            public float min;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_GainT
        {
            public int gRdB;
            public byte LNAstate;
            public byte syncUpdate;
            public sdrplay_api_MinGainReductionT minGr;
            public sdrplay_api_GainValuesT gainVals;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RfFreqT
        {
            public double rfHz;
            public byte syncUpdate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_DcOffsetTunerT
        {
            public byte dcCal;
            public byte speedUp;
            public int trackTime;
            public int refreshRateTime;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_TunerParamsT
        {
            public sdrplay_api_Bw_MHzT bwType;
            public sdrplay_api_If_kHzT ifType;
            public sdrplay_api_LoModeT loMode;
            public sdrplay_api_GainT gain;
            public sdrplay_api_RfFreqT rfFreq;
            public sdrplay_api_DcOffsetTunerT dcOffsetTuner;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_DcOffsetT
        {
            public byte DCenable;
            public byte IQenable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_DecimationT
        {
            public byte enable;
            public byte decimationFactor;
            public byte wideBandSignal;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_AgcT
        {
            public sdrplay_api_AgcControlT enable;
            public int setPoint_dBfs;
            public ushort attack_ms;
            public ushort decay_ms;
            public ushort decay_delay_ms;
            public ushort decay_threshold_dB;
            public int syncUpdate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_ControlParamsT
        {
            public sdrplay_api_DcOffsetT dcOffset;
            public sdrplay_api_DecimationT decimation;
            public sdrplay_api_AgcT agc;
            public sdrplay_api_AdsbModeT adsbMode;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_Rsp1aTunerParamsT
        {
            public byte biasTEnable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_Rsp2TunerParamsT
        {
            public byte biasTEnable;
            public sdrplay_api_Rsp2_AmPortSelectT amPortSel;
            public sdrplay_api_Rsp2_AntennaSelectT antennaSel;
            public byte rfNotchEnable;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RspDuo_ResetSlaveFlagsT
        {
            public byte resetGainUpdate;
            public byte resetRfUpdate;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RspDuoTunerParamsT
        {
            public byte biasTEnable;
            public sdrplay_api_RspDuo_AmPortSelectT tuner1AmPortSel;
            public byte tuner1AmNotchEnable;
            public byte rfNotchEnable;
            public byte rfDabNotchEnable;
            public sdrplay_api_RspDuo_ResetSlaveFlagsT resetSlaveFlags;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RspDxTunerParamsT
        {
            public int hdrBw;   // sdrplay_api_RspDx_HdrModeBwT
        }

        /// <summary>
        /// Full receive-channel block (pointed to by DeviceParamsT.rxChannelA/B).
        /// Includes RSP2/Duo/Dx tuner tails so the overall size matches the API.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_RxChannelParamsT
        {
            public sdrplay_api_TunerParamsT tunerParams;
            public sdrplay_api_ControlParamsT ctrlParams;
            public sdrplay_api_Rsp1aTunerParamsT rsp1aTunerParams;
            public sdrplay_api_Rsp2TunerParamsT rsp2TunerParams;
            public sdrplay_api_RspDuoTunerParamsT rspDuoTunerParams;
            public sdrplay_api_RspDxTunerParamsT rspDxTunerParams;
        }

        /// <summary>
        /// Top-level pointer block returned by GetDeviceParams.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_DeviceParamsT
        {
            public IntPtr devParams;      // -> sdrplay_api_DevParamsT
            public IntPtr rxChannelA;     // -> sdrplay_api_RxChannelParamsT
            public IntPtr rxChannelB;     // -> sdrplay_api_RxChannelParamsT (RSPduo)
        }

        // ------------------------------------------------------------------
        // Callbacks
        // ------------------------------------------------------------------
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_StreamCbParamsT
        {
            public uint firstSampleNum;
            public int grChanged;
            public int rfChanged;
            public int fsChanged;
            public uint numSamples;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_GainCbParamT
        {
            public uint gRdB;
            public uint lnaGRdB;
            public double currGain;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_PowerOverloadCbParamT
        {
            public sdrplay_api_PowerOverloadCbEventIdT powerOverloadChangeType;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public unsafe delegate void sdrplay_api_StreamCallback_t(
            short* xi,
            short* xq,
            IntPtr paramsPtr,
            uint numSamples,
            uint reset,
            IntPtr cbContext);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void sdrplay_api_EventCallback_t(
            sdrplay_api_EventT eventId,
            sdrplay_api_TunerSelectT tuner,
            IntPtr paramsPtr,
            IntPtr cbContext);

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct sdrplay_api_CallbackFnsT
        {
            public sdrplay_api_StreamCallback_t StreamACbFn;
            public sdrplay_api_StreamCallback_t StreamBCbFn;
            public sdrplay_api_EventCallback_t EventCbFn;
        }

        // ------------------------------------------------------------------
        // API functions
        // ------------------------------------------------------------------
        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Open();

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Close();

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_ApiVersion(out float apiVer);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_LockDeviceApi();

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_UnlockDeviceApi();

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_GetDevices(
            [In, Out] sdrplay_api_DeviceT[] devices,
            out uint numDevs,
            uint maxDevs);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_SelectDevice(ref sdrplay_api_DeviceT device);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_ReleaseDevice(ref sdrplay_api_DeviceT device);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr sdrplay_api_GetErrorString(sdrplay_api_ErrT err);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_DebugEnable(IntPtr dev, uint enable);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_GetDeviceParams(
            IntPtr dev,
            out IntPtr deviceParams);   // -> sdrplay_api_DeviceParamsT*

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Init(
            IntPtr dev,
            ref sdrplay_api_CallbackFnsT callbackFns,
            IntPtr cbContext);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Uninit(IntPtr dev);

        [DllImport(APIDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern sdrplay_api_ErrT sdrplay_api_Update(
            IntPtr dev,
            sdrplay_api_TunerSelectT tuner,
            sdrplay_api_ReasonForUpdateT reasonForUpdate,
            sdrplay_api_ReasonForUpdateExtension1T reasonForUpdateExt1);

        public static string GetErrorString(sdrplay_api_ErrT err)
        {
            IntPtr p = sdrplay_api_GetErrorString(err);
            return p == IntPtr.Zero ? err.ToString() : Marshal.PtrToStringAnsi(p);
        }
    }
}
