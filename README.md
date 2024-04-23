# SystemMonitor README

## Overview

SystemMonitor is a configurable console application that monitors system resources, specifically CPU and memory usage. It alerts the user if specified thresholds are exceeded, which is useful for maintaining system health and preventing overload. The application can also interact with endpoints when the thresholds are reached or reset, making it adaptable for integrated system monitoring solutions.

## Features

- **CPU Utilization Monitoring**: Tracks CPU usage and alerts when usage exceeds the set threshold.
- **Memory Utilization Monitoring**: Monitors memory usage and triggers alerts when usage surpasses the defined threshold.
- **Configurable Monitoring Intervals**: Allows setting the interval in minutes for how often the system checks the CPU and memory usage.
- **Threshold Exceedance Handling**: Configures how many consecutive times thresholds need to be exceeded before an action is triggered.
- **Endpoint Integration**: Can trigger external actions by calling predefined endpoints when maximum states are reached or reset.

## Requirements

- .NET Core 3.1 or later
- Appropriate permissions to access system performance data

## Installation

1. Clone the repository to your local machine using Git:
   ```bash
   git clone https://github.com/j2ba/SystemMonitor
   ```
2. Navigate to the cloned directory:
   ```bash
   cd SystemMonitor
   ```
3. Build the application using .NET CLI:
   ```bash
   dotnet build
   ```

## Usage

To start monitoring, run the executable with the desired options. If no options are specified, default values are used.

```
SystemMonitor.exe [--cpu-threshold=<value>] [--memory-threshold=<value>] [--check-in-minutes=<value>] [--max-exceeds=<count>] [--cycles-at-max=<count>] [--start-endpoint=<url>] [--end-endpoint=<url>]
```

### Options

- `--cpu-threshold`: The CPU utilization percentage threshold (Default = 75.0%)
- `--memory-threshold`: The memory utilization threshold in MB (Default = 6000 MB)
- `--check-in-minutes`: Time interval between system checks (Default = 15 minutes)
- `--max-exceeds`: Number of times the threshold must be exceeded before an alert is triggered (Default = 2)
- `--cycles-at-max`: Number of cycles the system continues at maximum before resetting (Default = 4)
- `--start-endpoint`: Endpoint to call when the system reaches maximum state (Default = empty)
- `--end-endpoint`: Endpoint to call when the system resets from a maximum state (Default = empty)

### Examples

Run the monitor with a CPU threshold of 80% and memory threshold of 8000 MB:

```bash
SystemMonitor.exe --cpu-threshold=80 --memory-threshold=8000
```

Set the monitoring interval to every 10 minutes with 3 exceedances before an alert:

```bash
SystemMonitor.exe --check-in-minutes=10 --max-exceeds=3
```

## Troubleshooting

- Ensure that you have the necessary administrative privileges to access performance data.
- Verify network connectivity if integrating with external endpoints.
- Check the console output for any error messages that may indicate what went wrong.

## Contributing

Contributions to improve SystemMonitor are welcome. Please fork the repository, make your changes, and submit a pull request for review.
