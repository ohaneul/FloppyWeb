package cache

import (
    "sync"
    "time"
)

type CacheEntry struct {
    Data        []byte
    Expiration  time.Time
    ContentType string
}

type CacheManager struct {
    cache map[string]CacheEntry
    mutex sync.RWMutex
}

func NewCacheManager() *CacheManager {
    cm := &CacheManager{
        cache: make(map[string]CacheEntry),
    }
    go cm.startCleanupRoutine()
    return cm
}

func (cm *CacheManager) Set(key string, data []byte, contentType string, duration time.Duration) {
    cm.mutex.Lock()
    defer cm.mutex.Unlock()

    cm.cache[key] = CacheEntry{
        Data:        data,
        Expiration:  time.Now().Add(duration),
        ContentType: contentType,
    }
}

func (cm *CacheManager) Get(key string) ([]byte, string, bool) {
    cm.mutex.RLock()
    defer cm.mutex.RUnlock()

    if entry, exists := cm.cache[key]; exists {
        if time.Now().Before(entry.Expiration) {
            return entry.Data, entry.ContentType, true
        }
        delete(cm.cache, key)
    }
    return nil, "", false
} 