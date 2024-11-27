export const config = {
    production: process.env.NODE_ENV === 'production',
    apiUrl: process.env.API_URL || 'http://localhost:3000',
    wasmPath: process.env.WASM_PATH || '/wasm',
    version: process.env.npm_package_version || '0.0.0',
}; 