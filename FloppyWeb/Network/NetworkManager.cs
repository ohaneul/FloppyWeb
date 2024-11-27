using System;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace FloppyWeb.Network
{
    public class NetworkManager
    {
        private static NetworkManager instance;
        public static NetworkManager Instance => instance ??= new NetworkManager();

        public event EventHandler<NetworkStatusChangedEventArgs> NetworkStatusChanged;
        public event EventHandler<NetworkSpeedEventArgs> NetworkSpeedUpdated;

        private bool isNetworkAvailable;
        private NetworkSpeed currentSpeed;

        private readonly RequestCache requestCache;
        private readonly ConnectionMonitor connectionMonitor;
        private readonly BandwidthManager bandwidthManager;
        private readonly ProxyManager proxyManager;

        public NetworkManager()
        {
            requestCache = new RequestCache();
            connectionMonitor = new ConnectionMonitor();
            bandwidthManager = new BandwidthManager();
            proxyManager = new ProxyManager();

            SetupNetworkMonitoring();
            InitializeProxy();
        }

        private void SetupNetworkMonitoring()
        {
            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            StartSpeedMonitoring();
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            isNetworkAvailable = e.IsAvailable;
            NetworkStatusChanged?.Invoke(this, new NetworkStatusChangedEventArgs(e.IsAvailable));
        }

        public bool IsNetworkAvailable() => isNetworkAvailable;

        private async void StartSpeedMonitoring()
        {
            while (true)
            {
                await UpdateNetworkSpeed();
                await Task.Delay(5000); // Update every 5 seconds
            }
        }

        private async Task UpdateNetworkSpeed()
        {
            try
            {
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                long totalBytesReceived = 0;
                long totalBytesSent = 0;

                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        totalBytesReceived += ni.GetIPv4Statistics().BytesReceived;
                        totalBytesSent += ni.GetIPv4Statistics().BytesSent;
                    }
                }

                await Task.Delay(1000);

                long newTotalBytesReceived = 0;
                long newTotalBytesSent = 0;

                foreach (NetworkInterface ni in interfaces)
                {
                    if (ni.OperationalStatus == OperationalStatus.Up)
                    {
                        newTotalBytesReceived += ni.GetIPv4Statistics().BytesReceived;
                        newTotalBytesSent += ni.GetIPv4Statistics().BytesSent;
                    }
                }

                currentSpeed = new NetworkSpeed(
                    downloadSpeed: newTotalBytesReceived - totalBytesReceived,
                    uploadSpeed: newTotalBytesSent - totalBytesSent
                );

                NetworkSpeedUpdated?.Invoke(this, new NetworkSpeedEventArgs(currentSpeed));
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error updating network speed: {ex.Message}");
            }
        }

        public async Task<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
        {
            try
            {
                if (!connectionMonitor.IsNetworkAvailable)
                {
                    return await HandleOfflineRequest(request);
                }

                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                return await httpClient.SendAsync(request, cts.Token);
            }
            catch (Exception ex)
            {
                Logger.LogError($"Network error: {ex.Message}");
                throw new NetworkException("Failed to send request", ex);
            }
        }

        private async Task<HttpResponseMessage> HandleOfflineRequest(HttpRequestMessage request)
        {
            var cachedResponse = requestCache.GetCachedResponse(request.RequestUri.ToString());
            if (cachedResponse != null)
            {
                return cachedResponse;
            }
            throw new OfflineException("No network connection and no cached response available");
        }

        private void InitializeProxy()
        {
            var proxySettings = settingsManager.GetProxySettings();
            if (proxySettings.Enabled)
            {
                proxyManager.ConfigureProxy(proxySettings);
                httpClient = new HttpClient(new HttpClientHandler
                {
                    Proxy = proxyManager.GetConfiguredProxy(),
                    UseProxy = true
                });
            }
        }
    }
} 