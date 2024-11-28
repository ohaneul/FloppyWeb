package cache

import (
    "sync"
    "time"
    "container/heap"
    "encoding/binary"
    "github.com/cespare/xxhash/v2"
)

type CacheItem struct {
    Key       string
    Value     []byte
    Size      int64
    Priority  float64
    Accessed  time.Time
    ExpiresAt time.Time
}

type AdvancedCache struct {
    items     map[string]*CacheItem
    maxSize   int64
    currSize  int64
    mutex     sync.RWMutex
    pq        PriorityQueue
    metrics   *CacheMetrics
}

func NewAdvancedCache(maxSize int64) *AdvancedCache {
    cache := &AdvancedCache{
        items:    make(map[string]*CacheItem),
        maxSize:  maxSize,
        pq:       make(PriorityQueue, 0),
        metrics:  NewCacheMetrics(),
    }
    
    go cache.startMaintenanceLoop()
    return cache
}

func (c *AdvancedCache) Set(key string, value []byte, ttl time.Duration) error {
    c.mutex.Lock()
    defer c.mutex.Unlock()

    size := int64(len(value))
    if size > c.maxSize {
        return ErrItemTooLarge
    }

    // Make room if needed
    for c.currSize+size > c.maxSize {
        if !c.evictOne() {
            return ErrCacheFull
        }
    }

    item := &CacheItem{
        Key:       key,
        Value:     value,
        Size:      size,
        Priority:  calculatePriority(value),
        Accessed:  time.Now(),
        ExpiresAt: time.Now().Add(ttl),
    }

    if existing, exists := c.items[key]; exists {
        c.currSize -= existing.Size
        c.pq.Remove(existing)
    }

    c.items[key] = item
    c.currSize += size
    heap.Push(&c.pq, item)
    c.metrics.RecordSet(size)

    return nil
}

func (c *AdvancedCache) Get(key string) ([]byte, bool) {
    c.mutex.RLock()
    defer c.mutex.RUnlock()

    if item, exists := c.items[key]; exists && time.Now().Before(item.ExpiresAt) {
        item.Accessed = time.Now()
        item.Priority *= 1.1 // Increase priority on access
        c.pq.Update(item)
        c.metrics.RecordHit()
        return item.Value, true
    }

    c.metrics.RecordMiss()
    return nil, false
}

func calculatePriority(value []byte) float64 {
    // Use xxHash for fast hashing
    hash := xxhash.Sum64(value)
    return float64(binary.BigEndian.Uint64(hash[:]))
}

func (c *AdvancedCache) startMaintenanceLoop() {
    ticker := time.NewTicker(time.Minute)
    for range ticker.C {
        c.performMaintenance()
    }
}

func (c *AdvancedCache) performMaintenance() {
    c.mutex.Lock()
    defer c.mutex.Unlock()

    now := time.Now()
    for c.pq.Len() > 0 {
        item := c.pq.Peek().(*CacheItem)
        if now.After(item.ExpiresAt) {
            c.removeItem(item)
        } else {
            break
        }
    }
} 