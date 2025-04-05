# Makefile for Docker Compose setup

# Default action
.DEFAULT_GOAL := help

# Build all services
build: 
	@echo "Building all services..."
	docker-compose build

# Run all services
run:
	@echo "Running all services..."
	docker-compose up -d

# Clean up (stop and remove all containers and volumes)
clean:
	@echo "Stopping and removing all containers and volumes..."
	docker-compose down --volumes --remove-orphans

# Build individual service (NGINX)
build-nginx:
	@echo "Building NGINX container..."
	docker-compose build nginx

# Build individual service (Transporter)
build-transporter:
	@echo "Building Transporter container..."
	docker-compose build transporter

# Build individual service (Orchestrator)
build-orchestrator:
	@echo "Building Orchestrator container..."
	docker-compose build orchestrator

# Build individual service (Transformer)
build-transformer:
	@echo "Building Transformer container..."
	docker-compose build transformer

# Display help information
help:
	@echo "Makefile commands:"
	@echo "  build                - Build all services"
	@echo "  run                  - Run all services"
	@echo "  clean                - Stop and remove all containers and volumes"
	@echo "  build-nginx          - Build only the NGINX service"
	@echo "  build-transporter    - Build only the Transporter service"
	@echo "  build-orchestrator   - Build only the Orchestrator service"
	@echo "  build-transformer    - Build only the Transformer service"
