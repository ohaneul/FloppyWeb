package cache

import (
    "sync"
    "time"
    "github.com/coocood/freecache"
)

type CacheService struct {
    cache       *freecache.Cache
    mutex       sync.RWMutex
    statistics  *CacheStats
}

func NewCacheService(sizeMB int) *CacheService {
    return &CacheService{
        cache: freecache.NewCache(sizeMB * 1024 * 1024),
        statistics: NewCacheStats(),
    }
}

// Efficient cache implementation with memory management
func (cs *CacheService) Set(key string, value []byte, expiration time.Duration) error {
    cs.mutex.Lock()
    defer cs.mutex.Unlock()

    if err := cs.cache.Set([]byte(key), value, int(expiration.Seconds())); err != nil {
        // If we're out of memory, clear expired entries and try again
        cs.cache.RemoveExpired()
        return cs.cache.Set([]byte(key), value, int(expiration.Seconds()))
    }
    return nil
}

// Parallel prefetching for better performance
func (cs *CacheService) PrefetchResources(urls []string) {
    var wg sync.WaitGroup
    semaphore := make(chan struct{}, 5) // Limit concurrent fetches

    for _, url := range urls {
        wg.Add(1)
        go func(url string) {
            defer wg.Done()
            semaphore <- struct{}{} // Acquire
            defer func() { <-semaphore }() // Release
            
            cs.fetchAndCache(url)
        }(url)
    }
    wg.Wait()
} 