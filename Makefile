# Variables
DOCKER_COMPOSE_FILE := $(CURDIR)/docker-compose.yml
PROJECT_NAME := civitas
COMPOSE := docker compose -f $(DOCKER_COMPOSE_FILE) -p $(PROJECT_NAME)

NGINX_CONTAINER := proxy
NGINX_IMAGE := civitas-proxy:v1.0

# Targets

.PHONY: build-nginx-image build build-local run-local run-local-debug rebuild logs logs-% restart-% clean ps down up

# Build the custom nginx image
build-nginx-image:
	docker build -t $(NGINX_IMAGE) ./images/nginx --no-cache

# Build using Docker Compose (Harbor images)
build:
	$(COMPOSE) build

# Build everything locally including the custom NGINX image
build-local: build-nginx-image
	$(COMPOSE) build

# Run with local built images
run-local: build-local
	$(COMPOSE) up -d

run-local-debug: build-local
	$(COMPOSE) up -d redis

# Rebuild civitas-api and restart all services
rebuild: down build up

# Bring down all services
down:
	$(COMPOSE) down

# Bring up all services
up:
	$(COMPOSE) up -d

# Tail logs from all services
logs:
	$(COMPOSE) logs -f

# Tail logs from a specific service
logs-%:
	$(COMPOSE) logs -f $*

# Restart a specific service
restart-%:
	$(COMPOSE) restart $*

# Clean volumes (WARNING: removes Redis data too)
clean:
	$(COMPOSE) down --volumes

# Show container status
ps:
	$(COMPOSE) ps
