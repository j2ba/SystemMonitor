using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class InstanceMonitor
{
    private readonly double cpuThreshold;
    private readonly double memoryThreshold;
    private readonly HttpClient httpClient;
    private readonly string alertUrl = "https://placeholder/start";

    public InstanceMonitor(double cpuThresholdPercentage, double memoryThresholdMB)
    {
        this.cpuThreshold = cpuThresholdPercentage;
        this.memoryThreshold = memoryThresholdMB;
        this.httpClient = new HttpClient();
    }

    public async Task StartMonitoringAsync(CancellationToken cancellationToken)
    {
        var exceedThresholdCount = 2;
        var currentExceedThresholdCount = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            double cpuUsage, memoryUsage;
            GetCurrentCpuAndMemoryUsage(out cpuUsage, out memoryUsage);

            Console.Clear();
            Console.WriteLine($"CPU Usage: {cpuUsage}%");
            Console.WriteLine($"Memory Usage: {memoryUsage} MB");

            if (cpuUsage > cpuThreshold || memoryUsage > memoryThreshold)
            {
                //await httpClient.GetAsync(alertUrl);
                currentExceedThresholdCount++;
            }
            else
            {
                currentExceedThresholdCount--;
                Console.WriteLine("Reducing");
            }

            if (currentExceedThresholdCount >= exceedThresholdCount)
            {
                Console.WriteLine("Alert sent due to high resource usage.");
            }

            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken); // Check every 30 seconds
        }
    }

    private void GetCurrentCpuAndMemoryUsage(out double cpuUsage, out double memoryUsage)
    {
        using (PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total"))
        using (PerformanceCounter ramCounter = new PerformanceCounter("Memory", "Available MBytes"))
        {
            cpuCounter.NextValue();
            Thread.Sleep(1000);

            cpuUsage = cpuCounter.NextValue();

            memoryUsage = ramCounter.NextValue();
            var totalMemory = 32768;// / 1024 / 1024;
            memoryUsage = totalMemory - memoryUsage;
        }
    }
}
