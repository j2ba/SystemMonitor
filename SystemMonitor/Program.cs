var processorThreshold = 70.0;
var memoryThreshold = 28000.0;
var checkInMinutes = 15;
var maxThreshholdCount = 2;

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
    } else if (arg.StartsWith("--max-exceeds="))
    {
        maxThreshholdCount = int.Parse(arg.Substring("--max-exceeds=".Length));
    } else if (arg.StartsWith("--help"))
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
        Console.WriteLine();
        Console.WriteLine();

        return;
    }
}
//var systemMonitor = new InstanceMonitor(70.0, 28000);
//var cancellationTokenSource = new CancellationTokenSource();

//await systemMonitor.StartMonitoringAsync(cancellationTokenSource.Token);