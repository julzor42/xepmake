using System;

namespace BuildCommon.Logging
{
    public static class Log
    {
        public static void Write(string Message = default(string), string Class = "Generation") => Console.WriteLine($"[{Class}] {Message}");

    }
}
