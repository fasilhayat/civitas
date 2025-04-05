# Define variables
DOCKER_IMAGE_NAME = civitas-api
DOCKER_CONTAINER_NAME = civitas-container
DOCKER_COMPOSE_FILE = docker-compose.yml
DOCKERFILE_PATH = ./Civitas.Api/Dockerfile

# Build Docker image
build:
	@echo "Building Docker image for Civitas API..."
	docker build -t $(DOCKER_IMAGE_NAME) -f $(DOCKERFILE_PATH) .

# Run Docker container
run:
	@echo "Running Docker container for Civitas API..."
	docker run -d --name $(DOCKER_CONTAINER_NAME) -p 8080:80 $(DOCKER_IMAGE_NAME)

# Stop Docker container
stop:
	@echo "Stopping Docker container..."
	docker stop $(DOCKER_CONTAINER_NAME)

# Remove Docker container
rm:
	@echo "Removing Docker container..."
	docker rm $(DOCKER_CONTAINER_NAME)

# Build and run container (shortcut)
up: build run

# Build and stop container (for restart)
restart: stop build run

# Clean up (remove stopped containers and unused images)
clean:
	@echo "Cleaning up Docker environment..."
	docker system prune -f

# Build and run Docker Compose (if you have a compose file)
docker-compose-up:
	@echo "Running Docker Compose for Civitas..."
	docker-compose -f $(DOCKER_COMPOSE_FILE) up -d

# Docker Compose down (stop and remove containers)
docker-compose-down:
	@echo "Stopping and removing Docker Compose containers..."
	docker-compose -f $(DOCKER_COMPOSE_FILE) down

# Check Docker container status
status:
	@echo "Checking Docker container status..."
	docker ps -a
