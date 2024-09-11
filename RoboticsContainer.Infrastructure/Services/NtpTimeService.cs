using System.Net.Sockets;

namespace RoboticsContainer.Infrastructure.Services
{
    public class NtpTimeService
    {
        public async Task<DateTime> GetNtpTimeAsync(string ntpServer = "ntp-server")
        {
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B; // Setting protocol version

            try
            {
                using (var client = new UdpClient(ntpServer, 123))
                {
                    await client.SendAsync(ntpData, ntpData.Length);
                    var result = await client.ReceiveAsync();

                    ntpData = result.Buffer;

                    ulong intPart = BitConverter.ToUInt32(ntpData, 40);
                    ulong fractPart = BitConverter.ToUInt32(ntpData, 44);

                    intPart = SwapEndianness(intPart);
                    fractPart = SwapEndianness(fractPart);

                    var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);

                    var ntpTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                    return ntpTime.ToUniversalTime();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error getting NTP time: " + ex.Message);
                return DateTime.UtcNow; // Fallback to local system time
            }
        }

        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }
    }

}
