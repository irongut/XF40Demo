using System;
using System.Threading;

namespace XF40Demo.Models
{
    public struct DownloadOptions
    {
        public CancellationTokenSource CancelToken { get; }

        public TimeSpan Expiry { get; }

        public bool IgnoreCache { get; }

        public DownloadOptions(CancellationTokenSource cancelToken = null)
        {
            CancelToken = cancelToken;
            IgnoreCache = false;
        }

        public DownloadOptions(TimeSpan expiry)
        {
            CancelToken = null;
            Expiry = expiry;
            IgnoreCache = false;
        }

        public DownloadOptions(CancellationTokenSource cancelToken, TimeSpan expiry)
        {
            CancelToken = cancelToken;
            Expiry = expiry;
            IgnoreCache = false;
        }

        public DownloadOptions(TimeSpan expiry, bool ignoreCache)
        {
            CancelToken = null;
            Expiry = expiry;
            IgnoreCache = ignoreCache;
        }

        public DownloadOptions(CancellationTokenSource cancelToken, TimeSpan expiry, bool ignoreCache)
        {
            CancelToken = cancelToken;
            Expiry = expiry;
            IgnoreCache = ignoreCache;
        }
    }
}
