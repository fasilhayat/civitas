# Variables
DOCKER_COMPOSE=docker-compose
PROJECT_NAME=civitas

# Build civitas-api image (other images are pulled from Docker Hub)
build:
	$(DOCKER_COMPOSE) build civitas-api

# Start ALL services: nginx, civitas-api, redis
up:
	$(DOCKER_COMPOSE) up -d

# Stop ALL services
down:
	$(DOCKER_COMPOSE) down

# Rebuild civitas-api and restart ALL services
rebuild: down build up

# Tail logs from ALL services
logs:
	$(DOCKER_COMPOSE) logs -f

# Tail logs from a specific service (e.g., make logs-civitas-api)
logs-%:
	$(DOCKER_COMPOSE) logs -f $*

# Restart a specific service (e.g., make restart-nginx)
restart-%:
	$(DOCKER_COMPOSE) restart $*

# Clean volumes (WARNING: removes Redis data too)
clean:
	$(DOCKER_COMPOSE) down -v

# Show container status
ps:
	$(DOCKER_COMPOSE) ps
