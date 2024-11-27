#!/bin/bash
set -e

# Build Docker image
docker build -t floppyweb:latest .

# Tag image for registry
docker tag floppyweb:latest ghcr.io/yourusername/floppyweb:latest

# Push to registry
docker push ghcr.io/yourusername/floppyweb:latest

echo "Deployment complete!" 