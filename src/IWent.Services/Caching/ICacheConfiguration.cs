using System;

namespace IWent.Services.Caching;

public interface ICacheConfiguration
{
    TimeSpan AbsoluteExpiration { get; init; }

    bool IsEnabled { get; init; }
}