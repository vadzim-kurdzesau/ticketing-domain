using System;

namespace IWent.Services.Caching;

public interface ICacheConfiguration
{
    TimeSpan SlidingExpiration { get; set; }
}