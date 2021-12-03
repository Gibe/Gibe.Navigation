#if NETCORE 
using System;
using Microsoft.Extensions.Caching.Memory;

namespace Gibe.Navigation
{
	public interface ICache
	{
		T Get<T>(string key);
		void Add(string key, object value, TimeSpan timeSpan);
		bool Exists(string key);
	}

	public class MemoryCacheWrapper : ICache
	{
		private readonly IMemoryCache _memoryCache;

		public MemoryCacheWrapper(IMemoryCache memoryCache)
		{
			_memoryCache = memoryCache;
		}

		public T Get<T>(string key)
		{
			return _memoryCache.Get<T>(key);
		}

		public void Add(string key, object value, TimeSpan timeSpan)
		{
			_memoryCache.Set(key, value, timeSpan);
		}

		public bool Exists(string key)
		{
			return _memoryCache.TryGetValue(key, out _);
		}
	}
 }
#endif