using API.Endpoints.DeviceInterface;
using API.Models.DeviceInterface;
using Newtonsoft.Json;

namespace API
{
    public class PawTracker : PawClient
    {
        [JsonIgnore]
        private readonly string _masterToken = "sGFXpr0wNnYjL3efenAgXrEX4KZS4noOw7orHnXA4x";
        [JsonIgnore]
        private readonly Random _random = new();

        public bool Initialized
        {
            get
            {
                return AuthToken != _masterToken;
            }
        }
        public DateTime LastSeen { get; set; } = DateTime.UnixEpoch;
        public bool IsCharging { get; set; } = false;
        public bool IsSubmittingData { get; set; } = false;

        public string DeviceId { get; set; } = Guid.NewGuid().ToString();
        public double Latitude { get; set; } = 0.0;
        public double Longitude { get; set; } = 0.0;
        public string DeviceVersion { get; set; } = "0.0";
        public double BatteryLevel { get; set; } = 100;
        public string ICCID { get; set; } = "890000000000000000F";
        public bool IsTracking { get; set; } = false;
        public int TrackingFrequency { get; set; } = 1;

        public PawTracker(bool isLogging = true) : base(isLogging)
        {
            // Initial auth token is the master token
            AuthToken = _masterToken;
            // Version
            int majorVersion = _random.Next(1, 5);
            int minorVersion = _random.Next(1, 20);
            DeviceVersion = $"{majorVersion}.{minorVersion}";
            // Believable initial battery level
            BatteryLevel = _random.Next(65, 100);
            // ICCID
            int iccidFirstPart = _random.Next(10000000, 99999999);
            int iccidSecondPart = _random.Next(10000000, 99999999);
            ICCID = $"89{iccidFirstPart}{iccidSecondPart}F";
            // Position
            Latitude = _random.NextDouble() * 90;
            Longitude = _random.NextDouble() * 180;
            bool isLatitudeNegative = _random.Next(0, 10) >= 5;
            bool isLongitudeNegative = _random.Next(0, 10) >= 5;
            Latitude *= isLatitudeNegative ? -1 : 1;
            Longitude *= isLongitudeNegative ? -1 : 1;
            // Debug
            if (IsLogging) Console.WriteLine("[Construct]\n{0}", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public override string ToString() {
            char batterySuffix = IsCharging ? '+' : '-';
            string trackingInfo = IsSubmittingData ? (IsTracking ? "Tracking and submitting data." : "Tracking disabled, could submit data.") : "Can't submit data.";
            return $"[{DeviceId}] {AuthToken} v{DeviceVersion} {BatteryLevel:F1}% ({batterySuffix}) [{Latitude:F3} : {Longitude:F3}] - {trackingInfo}";
        }

        public void PrintStatus()
        {
            bool trackingState = IsTracking && IsSubmittingData;
            Console.ForegroundColor = trackingState ? ConsoleColor.DarkGreen : ConsoleColor.DarkRed;
            Console.Write("[#] ");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(ToString());
            Console.WriteLine(JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        // Internal functionality

        public void DepleteBattery(double amount = 0.1)
        {
            double newLevel = BatteryLevel - amount;
            BatteryLevel = newLevel <= 0.0 ? 0.0 : newLevel;
        }

        public void Charge(double amount = 1.0)
        {
            double newLevel = BatteryLevel + amount;
            BatteryLevel = newLevel >= 100.0 ? 100.0 : newLevel;
        }

        public int GetBatteryLevel()
        {
            return Convert.ToInt32(Math.Round(BatteryLevel));
        }

        public void Move(double latitude, double longitude)
        {
            double newLatitude = Latitude + latitude;
            double newLongitude = Longitude + longitude;
            Latitude = newLatitude % 90;
            Longitude = newLongitude % 180;
            if (IsLogging) Console.WriteLine($"[Move] Latitude = {Latitude}, Longitude = {Longitude}");
        }

        // Server communication

        public DeviceInitResponseBody? Init()
        {
            var response = SendRequest<DeviceInitResponseBody>(new DeviceInitEndpoint(), new DeviceInitRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
            });
            if (response == null) return response;
            else if (response.IsUpdate)
            {
                DeviceVersion = response.UpdateNewVersion;
            }
            AuthToken = response.AuthToken == string.Empty ? AuthToken : response.AuthToken;
            if (IsLogging) Console.WriteLine($"[Init] AuthToken = {AuthToken}");
            return response;
        }

        public async Task<DeviceInitResponseBody?> InitAsync()
        {
            var response = await SendRequestAsync<DeviceInitResponseBody>(new DeviceInitEndpoint(), new DeviceInitRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
            });
            if (response == null) return response;
            else if (response.IsUpdate)
            {
                DeviceVersion = response.UpdateNewVersion;
            }
            AuthToken = response.AuthToken == string.Empty ? AuthToken : response.AuthToken;
            return response;
        }

        public DevicePingResponseBody? Ping()
        {
            var response = SendRequest<DevicePingResponseBody>(new DevicePingEndpoint(), new DevicePingRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
                BatteryLevel = GetBatteryLevel(),
            });
            if (response == null) return response;
            IsTracking = response != null ? response.IsTracking : IsTracking;
            TrackingFrequency = response != null ? response.TrackingFrequency : TrackingFrequency;
            if (IsLogging) Console.WriteLine($"[Ping] IsTracking = {IsTracking} TrackingFrequency = {TrackingFrequency}");
            return response;
        }

        public async Task<DevicePingResponseBody?> PingAsync()
        {
            var response = await SendRequestAsync<DevicePingResponseBody>(new DevicePingEndpoint(), new DevicePingRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
                BatteryLevel = GetBatteryLevel(),
            });
            if (response == null) return response;
            IsTracking = response != null ? response.IsTracking : IsTracking;
            TrackingFrequency = response != null ? response.TrackingFrequency : TrackingFrequency;
            return response;
        }

        public DeviceDataResponseBody? Data()
        {
            var response = SendRequest<DeviceDataResponseBody>(new DeviceDataEndpoint(), new DeviceDataRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
                BatteryLevel = GetBatteryLevel(),
                Latitude = Latitude,
                Longitude = Longitude,
                ICCID = ICCID,
            });
            LastSeen = DateTime.Now;
            if (response == null) return response;
            IsTracking = response != null ? response.IsTracking : IsTracking;
            TrackingFrequency = response != null ? response.TrackingFrequency : TrackingFrequency;
            if (IsLogging) Console.WriteLine($"[Data] IsTracking = {IsTracking} TrackingFrequency = {TrackingFrequency}");
            return response;
        }

        public async Task<DeviceDataResponseBody?> DataAsync()
        {
            var response = await SendRequestAsync<DeviceDataResponseBody>(new DeviceDataEndpoint(), new DeviceDataRequestBody
            {
                DeviceId = DeviceId,
                DeviceVersion = DeviceVersion,
                BatteryLevel = GetBatteryLevel(),
                Latitude = Latitude,
                Longitude = Longitude,
                ICCID = ICCID,
            });
            LastSeen = DateTime.Now;
            if (response == null) return response;
            IsTracking = response != null ? response.IsTracking : IsTracking;
            TrackingFrequency = response != null ? response.TrackingFrequency : TrackingFrequency;
            return response;
        }

        // Simulation

        public void SimulateDevice()
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SimulateLocationProviderAsync(CancellationToken cancellationToken)
        {
            IsSubmittingData = true;
            while (!cancellationToken.IsCancellationRequested)
            {
                // If the battery level drops below 5%, enter an emergency power saving mode, and wait for the device to charge
                if(BatteryLevel <= 5.0)
                {
                    IsSubmittingData = false;
                    while (BatteryLevel <= 50.0) Thread.Sleep(1000);
                    IsSubmittingData = true;
                }
                // Track
                if (IsTracking)
                {
                    await DataAsync();
                    DepleteBattery(2.0);
                }
                Thread.Sleep(TimeSpan.FromSeconds(TrackingFrequency));
                DepleteBattery();
            }
            IsSubmittingData = false;
            return true;
        }

        public async Task<bool> SimulateDeviceAsync(CancellationToken cancellationToken)
        {
            var locationProviderTask = Task.Run(() => SimulateLocationProviderAsync(cancellationToken), cancellationToken);
            while (!cancellationToken.IsCancellationRequested)
            {
                // Enter the charging state
                if(BatteryLevel <= 1.0)
                {
                    IsCharging = true;
                    double targetBatteryLevel = _random.Next(75, 100);
                    while(BatteryLevel <= targetBatteryLevel)
                    {
                        Charge(_random.NextDouble());
                        Thread.Sleep(50);
                    }
                    IsCharging = false;
                }
                // Initialize or ping
                if(Initialized) await PingAsync();
                else await InitAsync();
                DepleteBattery(0.5);
                // Move on each axis somewhere between -1 and 1
                Move(_random.NextDouble() * _random.Next(-1, 1), _random.NextDouble() * _random.Next(-1, 1));
                Thread.Sleep(1000);
            }
            // Wait for the location provider to exit
            bool locationProviderExited = locationProviderTask.IsCanceled || locationProviderTask.IsCompleted || locationProviderTask.IsCompletedSuccessfully || locationProviderTask.IsFaulted;
            if (!locationProviderExited)
            {
                await locationProviderTask;
            }
            return true;
        }

    }
}
