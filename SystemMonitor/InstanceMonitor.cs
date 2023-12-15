using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

public class InstanceMonitor
{
    private readonly double _cpuThreshold;
    private readonly double _memoryThreshold;
    private readonly HttpClient _httpClient;
    private readonly string _raiseEndpoint = string.Empty;
    private readonly string _reduceEndpoint = string.Empty;
    private readonly int _exceedThresholdCount = 2;
    private readonly int _cyclesAtMaxStateBeforeReducing = 4;
    private readonly int _currentExceedThresholdCount = 0;
    private readonly int _checkInMinutes = 15;

    public InstanceMonitor(double cpuThresholdPercentage, double memoryThresholdMB, int exceedThresholdCount, int checkInMinutes, int cyclesAtMaxStateBeforeReducing, string startEndPoint, string endEndPoint)
    {
        this._cpuThreshold = cpuThresholdPercentage;
        this._exceedThresholdCount = exceedThresholdCount;
        this._cyclesAtMaxStateBeforeReducing = cyclesAtMaxStateBeforeReducing;
        this._checkInMinutes = checkInMinutes;
        this._memoryThreshold = memoryThresholdMB;
        this._raiseEndpoint = startEndPoint;
        this._reduceEndpoint = endEndPoint;
        this._httpClient = new HttpClient();
    }

    public async Task StartMonitoringAsync(CancellationToken cancellationToken)
    {
        var currentExceedThresholdCount = 0;
        var currentCyclesBeforeReduce = 0;
        while (!cancellationToken.IsCancellationRequested)
        {
            double cpuUsage, memoryUsage;
            GetCurrentCpuAndMemoryUsage(out cpuUsage, out memoryUsage);

            Console.Clear();
            Console.WriteLine($"CPU Usage: {cpuUsage}%");
            Console.WriteLine($"Memory Usage: {memoryUsage} MB");

            if (cpuUsage > _cpuThreshold || memoryUsage > _memoryThreshold)
            {
                if (currentExceedThresholdCount < _exceedThresholdCount)
                {
                    currentExceedThresholdCount++;
                    Console.WriteLine($"({currentExceedThresholdCount}) warnings");
                    if (currentExceedThresholdCount == _exceedThresholdCount)
                    {
                        await _httpClient.GetAsync(_raiseEndpoint);
                        Console.WriteLine($"Alert sent due to high resource usage. ({currentExceedThresholdCount}) times");
                        currentCyclesBeforeReduce = _cyclesAtMaxStateBeforeReducing;
                    }
                } else if (currentExceedThresholdCount == _exceedThresholdCount)
                {
                    Console.WriteLine($"In exceeded resource state");
                }
            }
            else if(currentExceedThresholdCount > 0)
            {
                if (currentExceedThresholdCount == _exceedThresholdCount)
                {
                    if (currentCyclesBeforeReduce > 0)
                    {
                        currentCyclesBeforeReduce--;
                        Console.WriteLine($"In exceeded resource state: {_cyclesAtMaxStateBeforeReducing - currentCyclesBeforeReduce} of {_cyclesAtMaxStateBeforeReducing}");
                    }
                    else
                    {
                        currentExceedThresholdCount = 0;
                        await _httpClient.GetAsync(_reduceEndpoint);
                        Console.WriteLine($"In exceeded resource state: Normalized");
                    }
                }
                else
                {
                    currentExceedThresholdCount--;
                    Console.WriteLine("Reducing");
                    if (currentExceedThresholdCount == 0)
                    {
                        Console.WriteLine("Back to normal");
                    }
                }
            }

            await Task.Delay(TimeSpan.FromMinutes(_checkInMinutes), cancellationToken); // Check every 30 seconds
            //await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken); // Check every 30 seconds
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
