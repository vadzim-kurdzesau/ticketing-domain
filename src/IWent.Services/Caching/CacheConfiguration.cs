using System;

namespace IWent.Services.Caching;

public class CacheConfiguration : ICacheConfiguration
{
    public TimeSpan SlidingExpiration { get; set; }
}
