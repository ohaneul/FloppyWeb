FROM rust:1.70 as rust-builder
WORKDIR /app
COPY . .
RUN cargo build --release

FROM node:20 as node-builder
WORKDIR /app
COPY . .
RUN npm install && npm run build

FROM ubuntu:22.04
WORKDIR /app
COPY --from=rust-builder /app/target/release/floppy-web /app/
COPY --from=node-builder /app/dist /app/dist/

RUN apt-get update && apt-get install -y \
    libssl-dev \
    ca-certificates \
    && rm -rf /var/lib/apt/lists/*

EXPOSE 3000
CMD ["./floppy-web"] 