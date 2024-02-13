using System.Diagnostics;
using Metalama.Framework.Aspects;

namespace GymLog.Application.Aspects;

public class StopwatchAttribute : OverrideMethodAspect
{
    public override dynamic? OverrideMethod()
    {
        Console.WriteLine($"Started stopwatch for method {meta.Target.Method}");
        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            return meta.Proceed();
        }
        finally
        {
            stopwatch.Stop();
            Console.WriteLine($"Stopwatch for method {meta.Target.Method} stopped");

            TimeSpan time = stopwatch.Elapsed;
            Console.WriteLine($@"{meta.Target.Method} - Elapsed time: {time:m\:ss\.fff}");
        }
    }
}