#include <stdlib.h>
#include <string.h>
#include "memory_manager.h"

typedef struct MemoryPool {
    void* base;
    size_t size;
    size_t used;
    uint8_t* bitmap;
} MemoryPool;

MemoryPool* create_memory_pool(size_t size) {
    MemoryPool* pool = (MemoryPool*)malloc(sizeof(MemoryPool));
    if (!pool) return NULL;

    pool->base = aligned_alloc(16, size);  // 16-byte alignment for SIMD
    pool->size = size;
    pool->used = 0;
    pool->bitmap = (uint8_t*)calloc(size / (8 * PAGE_SIZE), 1);

    return pool;
}

void* fast_alloc(MemoryPool* pool, size_t size) {
    if (pool->used + size > pool->size) {
        return NULL;  // Out of memory
    }

    // Find first fit using optimized bitmap scanning
    size_t blocks_needed = (size + PAGE_SIZE - 1) / PAGE_SIZE;
    size_t index = find_free_blocks(pool->bitmap, blocks_needed);
    
    if (index == (size_t)-1) return NULL;

    // Mark blocks as used
    mark_blocks(pool->bitmap, index, blocks_needed);
    pool->used += blocks_needed * PAGE_SIZE;

    return (void*)((char*)pool->base + (index * PAGE_SIZE));
} 