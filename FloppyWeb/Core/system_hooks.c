#include <dlfcn.h>
#include "system_hooks.h"

typedef struct SystemHook {
    void* handle;
    hook_func callback;
    int priority;
} SystemHook;

static SystemHook hooks[MAX_HOOKS];
static size_t hook_count = 0;

int install_system_hook(const char* library, const char* function, hook_func callback) {
    if (hook_count >= MAX_HOOKS) return -1;

    void* handle = dlopen(library, RTLD_LAZY);
    if (!handle) return -2;

    hooks[hook_count].handle = handle;
    hooks[hook_count].callback = callback;
    hooks[hook_count].priority = hook_count;

    hook_count++;
    return 0;
}

void* intercept_system_call(const char* function, void* original) {
    for (size_t i = 0; i < hook_count; i++) {
        if (hooks[i].callback) {
            original = hooks[i].callback(original);
        }
    }
    return original;
} 