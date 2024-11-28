package performance

import (
    "sync"
    "time"
)

type ResourceOptimizer struct {
    memoryLimit   int64
    activeThreads int32
    resourcePool  *sync.Pool
    metrics       *OptimizationMetrics
}

func NewResourceOptimizer(memoryLimit int64) *ResourceOptimizer {
    return &ResourceOptimizer{
        memoryLimit:  memoryLimit,
        resourcePool: &sync.Pool{
            New: func() interface{} {
                return make([]byte, 0, 4096)
            },
        },
        metrics: NewOptimizationMetrics(),
    }
}

func (ro *ResourceOptimizer) OptimizeResource(resource []byte) ([]byte, error) {
    if atomic.LoadInt32(&ro.activeThreads) >= MaxThreads {
        return nil, ErrTooManyThreads
    }

    atomic.AddInt32(&ro.activeThreads, 1)
    defer atomic.AddInt32(&ro.activeThreads, -1)

    buffer := ro.resourcePool.Get().([]byte)
    defer ro.resourcePool.Put(buffer)

    optimized, err := ro.compressAndOptimize(resource, buffer)
    if err != nil {
        return nil, err
    }

    ro.metrics.RecordOptimization(
        len(resource),
        len(optimized),
        time.Now(),
    )

    return optimized, nil
} 