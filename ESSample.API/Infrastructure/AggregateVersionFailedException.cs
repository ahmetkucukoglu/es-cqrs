namespace ESSample.API.Infrastructure
{
    using System;

    public class AggregateVersionFailedException : Exception
    {
        public string Stream { get; set; }
        public long ExpectedVersion { get; set; }
        public long LastVersion { get; set; }

        public AggregateVersionFailedException(string stream, long expectedVersion, long lastVersion)
            : base($"Version failed. Stream: {stream}. Expected Version: {expectedVersion}. Last Version: {lastVersion}")
        {
            Stream = stream;
            ExpectedVersion = expectedVersion;
            LastVersion = lastVersion;
        }
    }
}
