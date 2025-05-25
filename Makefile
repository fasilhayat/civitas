# Variables
DOCKER_COMPOSE_FILE=$(CURDIR)/docker-compose.yml
PROJECT_NAME=civitas

NGINX_CONTAINER=proxy
NGINX_IMAGE=civitas-proxy:v1.0

# Build the custom nginx image from the nginx Dockerfile
build-nginx-image:
	docker build -t $(NGINX_IMAGE) ./images/nginx --no-cache

# Build everything using Docker Compose (Harbor images)
build:
	docker-compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME) build

# Build everything locally (Explicit local build)
build-local: build-nginx-image
	docker-compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME) build --no-cache

# Run using local built images
run-local: build-local
	docker-compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME) up -d

run-local-debug: build-local
	docker-compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME) up -d redis

# Rebuild civitas-api and restart ALL services
rebuild: down build up

# Tail logs from ALL services
logs:
	$(DOCKER_COMPOSE_FILE) logs -f

# Tail logs from a specific service (e.g., make logs-civitas-api)
logs-%:
	$(DOCKER_COMPOSE_FILE) logs -f $*

# Restart a specific service (e.g., make restart-nginx)
restart-%:
	$(DOCKER_COMPOSE_FILE) restart $*

# Clean volumes (WARNING: removes Redis data too)
clean:
	docker-compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME) down --volumes

# Show container status
ps:
	$(DOCKER_COMPOSE_FILE) ps
