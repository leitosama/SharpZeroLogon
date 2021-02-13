using System;
using System.Runtime.InteropServices;

namespace SharpZeroLogon
{
    internal class Netapi32
    {
        public enum NETLOGON_SECURE_CHANNEL_TYPE : int
        {
            NullSecureChannel = 0,
            MsvApSecureChannel = 1,
            WorkstationSecureChannel = 2,
            TrustedDnsDomainSecureChannel = 3,
            TrustedDomainSecureChannel = 4,
            UasServerSecureChannel = 5,
            ServerSecureChannel = 6
        }

        [StructLayout(LayoutKind.Explicit, Size = 516)]
        public struct NL_TRUST_PASSWORD
        {
            [FieldOffset(0)]
            public ushort Buffer;

            [FieldOffset(512)]
            public uint Length;
        }

        [StructLayout(LayoutKind.Explicit, Size = 12)]
        public struct NETLOGON_AUTHENTICATOR
        {
            [FieldOffset(0)]
            public NETLOGON_CREDENTIAL Credential;

            [FieldOffset(8)]
            public uint Timestamp;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct NETLOGON_CREDENTIAL
        {
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public byte[] data;
        }


        [DllImport("netapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern int I_NetServerReqChallenge(string domain, string computer, ref NETLOGON_CREDENTIAL ClientChallenge, out NETLOGON_CREDENTIAL ServerChallenge);
        [DllImport("netapi32.dll", CharSet = CharSet.Unicode)]
        internal static extern int I_NetServerAuthenticate2(string domain, string account, int SecureChannelType, string computername, ref NETLOGON_CREDENTIAL ClientCredential, out NETLOGON_CREDENTIAL ServerCredential, ref int NegotiateFlags);


        [DllImport("netapi32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        public static extern int I_NetServerPasswordSet2(
            string PrimaryName,
            string AccountName,
            NETLOGON_SECURE_CHANNEL_TYPE AccountType,
            string ComputerName,
            ref NETLOGON_AUTHENTICATOR Authenticator,
            out NETLOGON_AUTHENTICATOR ReturnAuthenticator,
            ref NL_TRUST_PASSWORD ClearNewPassword
            );
    }
}
