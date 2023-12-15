var processorThreshold = 75.0;
var memoryThreshold = 6000.00;
var checkInMinutes = 15;
var maxThreshholdCount = 2;
var cyclesAtMaxState = 4;
var startEndPoint = string.Empty;
var endEndPoint = string.Empty;
//var startEndPoint = "https://prod-60.eastus.logic.azure.com:443/workflows/5f4a2351132641039efa93977ed7f7f4/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=wG3JBBDgv9eMi_ueIVHFdgeERgzDwndKGWq7pkEn-3o";
//var endEndPoint = "https://prod-82.eastus.logic.azure.com:443/workflows/e36f539610cf4586986a0ea9f468fb27/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=EfQq1WiHJTKGDj5r4HtSK1anthIioBwPjGZuISR7kug";

foreach (var arg in args)
{
    if (arg.StartsWith("--cpu-threshold="))
    {
        processorThreshold = double.Parse(arg.Substring("--cpu-threshold=".Length));
    }
    else if (arg.StartsWith("--memory-threshold="))
    {
        memoryThreshold = double.Parse(arg.Substring("--memory-threshold=".Length));
    } else if (arg.StartsWith("--check-in-minutes="))
    {
        checkInMinutes = int.Parse(arg.Substring("--check-in-minutes=".Length));
    }
    else if (arg.StartsWith("--cycles-at-max="))
    {
        cyclesAtMaxState = int.Parse(arg.Substring("--cycles-at-max=".Length));
    }
    else if (arg.StartsWith("--max-exceeds="))
    {
        maxThreshholdCount = int.Parse(arg.Substring("--max-exceeds=".Length));
    }
    else if (arg.StartsWith("--start-endpoint="))
    {
        startEndPoint = arg.Substring("--start-endpoint=".Length);
    }
    else if (arg.StartsWith("--end-endpoint="))
    {
        endEndPoint = arg.Substring("--end-endpoint=".Length);
    }
    else if (arg.StartsWith("--help"))
    {
        Console.WriteLine();
        Console.WriteLine("Description: ");
        Console.WriteLine("   SystemMonitor is a tool that monitors the CPU and memory usage of a system and sends an alert if the usage exceeds a threshold.");
        Console.WriteLine();
        Console.WriteLine("Usage: ");
        Console.WriteLine("   SystemMonitor.exe [--cpu-threshold=70.0] [--memory-threshold=28000.0] [--check-in-minutes=15] [--max-threshold-count=2]");
        Console.WriteLine();
        Console.WriteLine("Options:");
        Console.WriteLine($"   --cpu-threshold\t\tThe CPU utilization percent threshold (Default = {processorThreshold})");
        Console.WriteLine($"   --memory-threshold\t\tThe memory utilization MB threshold (Default = {memoryThreshold})");
        Console.WriteLine($"   --check-in-minutes\t\tThe number of minutes to wait between checks (Default = {checkInMinutes})");
        Console.WriteLine($"   --max-exceeds\t\tThe number of times the threshold must be exceeded before an alert is sent (Default = {maxThreshholdCount})");
        Console.WriteLine($"   --cycles-at-max\t\tThe number of times the check should run before reducing (Default = {cyclesAtMaxState})");
        Console.WriteLine($"   --start-endpoint\t\tThe endpoint to call when at max state (Default = {startEndPoint})");
        Console.WriteLine($"   --end-endpoint\t\tThe endpoint to call when max state resets (Default = {endEndPoint})");
        Console.WriteLine();
        Console.WriteLine();
        return;
    }
}

Console.Clear();
Console.WriteLine("Monitor Started ...");
var systemMonitor = new InstanceMonitor(processorThreshold, memoryThreshold, maxThreshholdCount, checkInMinutes, cyclesAtMaxState, startEndPoint, endEndPoint);
var cancellationTokenSource = new CancellationTokenSource();

await systemMonitor.StartMonitoringAsync(cancellationTokenSource.Token);