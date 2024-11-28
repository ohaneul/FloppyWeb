# FloppyWeb Browser

![Build Status](https://github.com/ohaneul/FloppyWeb/workflows/CI/badge.svg)
![License](https://img.shields.io/badge/license-MIT-blue.svg)
![Version](https://img.shields.io/badge/version-0.1.0-green.svg)

A modern, lightweight web browser built with React, TypeScript, and WebAssembly.

## Quick Start

# Clone repository
git clone https://github.com/ohaneul/FloppyWeb.git

# Install dependencies
npm install

# Start development server
npm start

## Features

- 🚀 Fast and lightweight web browser
- 🔍 Smart address bar with search suggestions
- 📊 Built-in developer tools
- 🎨 Customizable themes
- 🔒 Privacy-focused browsing
- 💻 Cross-platform support
- 🧩 Modular architecture

## Prerequisites

### All Platforms
- Node.js (v18+)
- npm (v8+)
- Git
- Emscripten (for WebAssembly)

### Windows Additional Requirements
- Visual Studio 2019+ with C++ workload
- Windows SDK
- Git Bash or WSL2

### macOS Additional Requirements
- Xcode Command Line Tools
- Homebrew

### Linux Additional Requirements
- GCC/G++ 7.0+
- Build Essential packages

## Installation

### Windows
# Using PowerShell as Administrator
choco install visualstudio2019-workload-vctools
choco install python emscripten
refreshenv
npm install

### macOS
# Install Homebrew if needed
/bin/bash -c "$(curl -fsSL https://raw.githubusercontent.com/Homebrew/install/HEAD/install.sh)"

brew install node emscripten python
npm install

### Linux (Ubuntu/Debian)
sudo apt update
sudo apt install -y build-essential python3 nodejs npm cmake git
git clone https://github.com/emscripten-core/emsdk.git
cd emsdk
./emsdk install latest
./emsdk activate latest
source ./emsdk_env.sh
cd ../FloppyWeb
npm install

## Development

# Start development server
npm start

# Run tests
npm test

# Build for production
npm run build

# Build WebAssembly components
npm run build:wasm

## Project Structure

FloppyWeb/
├── src/
│   ├── components/        # React components
│   │   ├── SmartBar.tsx
│   │   └── ...
│   ├── utils/            # Utility functions
│   │   ├── Debouncer.ts
│   │   └── ...
│   ├── services/         # Business logic
│   ├── tests/           # Test files
│   └── native/          # WebAssembly/C++ code
├── public/              # Static files
├── scripts/            # Build scripts
└── dist/              # Build output

## Architecture

### Frontend
- React 18 with TypeScript
- Custom state management
- WebAssembly integration
- Component-based architecture

### Performance
- Native code compiled to WebAssembly
- Smart caching strategies
- Lazy loading
- Component memoization

### Core Components
- SmartBar: Unified search and navigation
- TabManager: Multi-tab support
- WebView: Content rendering
- DevTools: Developer tools integration

## Testing

# Run all tests
npm test

# Run specific tests
npm test SmartBar

# Run with coverage
npm test -- --coverage

## Configuration

### Environment Variables
NODE_ENV=development
DEBUG=true
API_URL=http://localhost:3000
WASM_DEBUG=true

### Build Configuration
{
  "name": "floppyweb",
  "version": "0.1.0",
  "scripts": {
    "start": "webpack serve --mode development",
    "build": "webpack --mode production",
    "test": "jest",
    "build:wasm": "./scripts/build-wasm.sh"
  }
}

## Troubleshooting

### Common Issues

1. Build Failures
# Clear build cache and node_modules
rm -rf node_modules
npm cache clean --force
npm install

2. WebAssembly Issues
# Rebuild WebAssembly components
npm run build:wasm

3. Test Failures
# Clear Jest cache
npm test -- --clearCache

## Contributing
1. Fork the repository
2. Create your feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact
- Author: Haneul
- Project Link: [https://github.com/yourusername/FloppyWeb](https://github.com/ohaneul/FloppyWeb)

## Acknowledgments
- React team for the amazing framework
- WebAssembly for native performance
- Open source community for their contributions
