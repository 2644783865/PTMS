using PTMS.DataServices.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;

namespace PTMS.BusinessLogic.Helpers
{
    public class AppCacheHelper
    {
        private ConcurrentDictionary<string, Type[]> _keysDictionary;
        private readonly IDataChangeEventEmitter _dataEventEmitter;
        private MemoryCache _cacheInstance;

        public AppCacheHelper(IDataChangeEventEmitter dataEventEmitter)
        {
            _cacheInstance = MemoryCache.Default;

            _dataEventEmitter = dataEventEmitter;
            _dataEventEmitter.Subscribe(OnDataChangeEvent);

            _keysDictionary = new ConcurrentDictionary<string, Type[]>();
        }

        public async Task<T> GetCachedAsync<T>(
            string key, 
            Func<Task<T>> getEntityFunc, 
            params Type[] cleanOnEntityChangeArray)
            where T : class
        {
            var result = _cacheInstance[key] as T;

            if (result == null)
            {
                result = await getEntityFunc();

                if (result != null)
                {
                    _cacheInstance.Set(key, result, DateTime.Now.AddDays(999));
                }
            }

            if (!_keysDictionary.ContainsKey(key))
            {
                _keysDictionary.TryAdd(key, cleanOnEntityChangeArray);
            }

            return result;
        }

        public void ClearAll()
        {
            var cacheKeys = _cacheInstance.Select(kvp => kvp.Key).ToList();
            foreach (string cacheKey in cacheKeys)
            {
                _cacheInstance.Remove(cacheKey);
            }
        }

        private void OnDataChangeEvent(DataChangeEvent changeEvent)
        {
            foreach (var pair in _keysDictionary)
            {
                if (pair.Value.Contains(changeEvent.EntityType))
                {
                    _cacheInstance.Remove(pair.Key);
                }
            }
        }
    }
}
