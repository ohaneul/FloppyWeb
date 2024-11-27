#!/bin/bash
set -e

echo "Building FloppyWeb..."

# Build WebAssembly components
./scripts/build-wasm.sh

# Build TypeScript/React components
npm run webpack --mode production

echo "Build complete!" 