using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace PhotoChannelWebAPI.Cache
{
    public interface ICustomMemoryCache : IMemoryCache
    {
        void Clear();
        void RemoveRange(List<object> keys);
        bool Contains(object key);
       
        public object Get(object key);
        public TItem Get<TItem>(object key);
        public TItem GetOrCreate<TItem>(object key, Func<ICacheEntry, TItem> factory);

        public Task<TItem> GetOrCreateAsync<TItem>(object key, Func<ICacheEntry, Task<TItem>> factory);
        public TItem Set<TItem>(object key, TItem value);
        public TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration);
        public TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
        public TItem Set<TItem>(object key, TItem value, IChangeToken expirationToken);
        public TItem Set<TItem>(object key, TItem value, MemoryCacheEntryOptions options);
        public bool TryGetValue<TItem>(object key, out TItem value);
        IList<object> Keys { get;  }

    }
}
