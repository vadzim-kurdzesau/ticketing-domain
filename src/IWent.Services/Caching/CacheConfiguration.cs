using System;

namespace IWent.Services.Caching;

public class CacheConfiguration : ICacheConfiguration
{
    public TimeSpan AbsoluteExpiration { get; init; }

    public bool IsEnabled { get; init; }
}
