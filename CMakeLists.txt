cmake_minimum_required(VERSION 3.15)
project(FloppyWeb VERSION 0.1.0)

# Enable testing
include(CTest)
enable_testing()

# Find required packages
find_package(Rust REQUIRED)
find_package(Python3 REQUIRED COMPONENTS Interpreter Development)
find_package(Go REQUIRED)

# Set C++ standard
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

# Add subdirectories
add_subdirectory(src/Core)
add_subdirectory(src/Security)
add_subdirectory(src/Media)

# Configure WASM build
set(WASM_TARGET_TRIPLE "wasm32-unknown-unknown")
set(WASM_BUILD_DIR ${CMAKE_BINARY_DIR}/wasm)

# Configure Rust build
set(RUST_TARGET_DIR ${CMAKE_BINARY_DIR}/rust)
set(CARGO_BUILD_TARGET_DIR ${RUST_TARGET_DIR})

# Configure output directories
set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/lib)
 