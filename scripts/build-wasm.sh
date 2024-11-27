#!/bin/bash
set -e

echo "Building WebAssembly components..."

# Ensure emscripten is available
if ! command -v emcc &> /dev/null; then
    echo "Error: emscripten not found"
    exit 1
fi

# Create build directory
mkdir -p build/wasm

# Build C++ files
emcc src/native/*.cpp \
    -s WASM=1 \
    -s EXPORTED_RUNTIME_METHODS='["ccall", "cwrap"]' \
    -s EXPORTED_FUNCTIONS='["_malloc", "_free"]' \
    -s ALLOW_MEMORY_GROWTH=1 \
    -O3 \
    -o build/wasm/floppy.js

echo "WebAssembly build complete!" 