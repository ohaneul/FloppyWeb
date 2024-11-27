#!/bin/bash
set -e

# Ensure development dependencies are installed
npm install

# Build WebAssembly components in development mode
DEVELOPMENT=1 ./scripts/build-wasm.sh

# Start development server
npm run webpack serve --mode development 