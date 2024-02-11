using API;

namespace PawControl
{
    internal class Program
    {

        static CancellationToken token;
        // static PawTracker tracker = new(false);
        static readonly List<PawTracker> trackers = new();

        static void Main(string[] args)
        {
            CancellationTokenSource tokenSource = new();
            token = tokenSource.Token;
            // Create trackers
            int trackerCount = 3;
            for (int trackerIndex = 0; trackerIndex < trackerCount; trackerIndex++)
            {
                var tracker = new PawTracker(false);
                var simulationTask = Task.Run(() => tracker.SimulateDeviceAsync(token));
                var printTask = Task.Run(() => PrintState(token));
                trackers.Add(tracker);
            }
            // Wait for exiting
            Console.ReadKey();
            tokenSource.Cancel();
            Console.Clear();
            Console.WriteLine("Terminated running tasks, press any key to exit.");
            Console.ReadKey();
        }

        static async Task<bool> PrintState(CancellationToken token)
        {
            while(!token.IsCancellationRequested)
            {
                ConsoleFastClear();
                // tracker.PrintStatus();
                foreach (var tracker in trackers)
                {
                    tracker.PrintStatus();
                }
                Thread.Sleep(200);
            }
            return true;
        }

        static void ConsoleFastClear()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Gray;
            for (int top = 0; top < Console.WindowHeight; top++)
            {
                Console.SetCursorPosition(0, top);
                Console.Write(new string(' ', Console.WindowWidth));
            }
            Console.SetCursorPosition(0, 0);
        }


    }
}
