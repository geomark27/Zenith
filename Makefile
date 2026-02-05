# Zenith HRMS - Makefile

.PHONY: build run test clean restore help ef-restore

# Variables
APP_NAME=Zenith.API
SOLUTION=Zenith.slnx
API_PROJECT=Zenith.API/Zenith.API.csproj
INFRA_PROJECT=Zenith.Infrastructure/Zenith.Infrastructure.csproj
BRANCH := $(shell git branch --show-current)
DOTNET_EF=dotnet tool run dotnet-ef

# Cargar variables de entorno desde .env
ifneq (,$(wildcard ./.env))
    include .env
    export
endif

# ============================================
# AYUDA
# ============================================

help: ## Muestra esta ayuda
	@echo ""
	@echo "  Zenith HRMS - Comandos disponibles"
	@echo ""
	@echo "  Compilacion y Ejecucion:"
	@echo "    make build        - Compila la solucion"
	@echo "    make run          - Ejecuta la API"
	@echo "    make watch        - Ejecuta con hot reload"
	@echo "    make dev          - Setup completo desarrollo (DB + migrate + run)"
	@echo ""
	@echo "  Docker/SQL Server:"
	@echo "    make db-up        - Inicia SQL Server en Docker"
	@echo "    make db-down      - Detiene SQL Server"
	@echo "    make db-restart   - Reinicia SQL Server"
	@echo "    make db-logs      - Muestra logs de SQL Server"
	@echo "    make db-clean     - Elimina SQL Server y volumenes"
	@echo "    make db-shell     - Accede a sqlcmd en el contenedor"
	@echo ""
	@echo "  Base de Datos (EF Core):"
	@echo "    make db-migrate   - Ejecuta migraciones pendientes"
	@echo "    make db-update    - Alias para db-migrate"
	@echo "    make db-add m=X   - Crea nueva migracion con nombre X"
	@echo "    make db-remove    - Elimina ultima migracion"
	@echo "    make db-script    - Genera script SQL de migraciones"
	@echo "    make fresh        - Reset completo (clean DB + migrate)"
	@echo ""
	@echo "  Testing y Calidad:"
	@echo "    make test         - Ejecuta los tests"
	@echo "    make test-cov     - Tests con cobertura"
	@echo "    make format       - Formatea el codigo"
	@echo ""
	@echo "  Git (rama actual: $(BRANCH)):"
	@echo "    make push m='msg' - Add + Commit + Push"
	@echo "    make pull         - Pull desde origin"
	@echo "    make status       - Ver estado de git"
	@echo "    make sync m='msg' - Pull + Push (sincronizar)"
	@echo ""
	@echo "  Utilidades:"
	@echo "    make clean        - Limpia archivos generados"
	@echo "    make restore      - Restaura paquetes NuGet"
	@echo "    make setup        - Setup inicial del proyecto"
	@echo ""

# ============================================
# COMPILACION Y EJECUCION
# ============================================

build: ## Compila la solucion
	@echo "Compilando $(APP_NAME)..."
	@dotnet build $(SOLUTION)
	@echo "Compilacion exitosa!"

run: ## Ejecuta la API
	@echo "Ejecutando $(APP_NAME)..."
	@dotnet run --project $(API_PROJECT)

watch: ## Ejecuta con hot reload
	@echo "Ejecutando $(APP_NAME) con hot reload..."
	@dotnet watch run --project $(API_PROJECT)

dev: db-up ## Setup completo desarrollo (DB + migrate + run)
	@echo "Starting development environment..."
	@echo "Esperando SQL Server..."
	@sleep 5
	@$(MAKE) db-migrate
	@$(MAKE) run

restore: ## Restaura paquetes NuGet
	@echo "Restaurando paquetes..."
	@dotnet restore $(SOLUTION)
	@echo "Paquetes restaurados!"

clean: ## Limpia archivos generados
	@echo "Limpiando archivos generados..."
	@dotnet clean $(SOLUTION)
	@rm -rf */bin */obj
	@echo "Limpieza completada!"

# ============================================
# DOCKER (SQL Server)
# ============================================

db-up: ## Inicia SQL Server en Docker
	@echo "Starting SQL Server..."
	@docker compose up -d sqlserver
	@echo "SQL Server running on localhost:$(DB_PORT)"

db-down: ## Detiene SQL Server
	@echo "Stopping SQL Server..."
	@docker compose stop sqlserver

db-restart: ## Reinicia SQL Server
	@echo "Restarting SQL Server..."
	@docker compose restart sqlserver

db-logs: ## Muestra logs de SQL Server
	@docker compose logs -f sqlserver

db-clean: ## Elimina SQL Server y volumenes
	@echo "Cleaning database..."
	@docker compose down -v
	@echo "Database cleaned!"

db-shell: ## Accede a sqlcmd en el contenedor
	@docker compose exec sqlserver /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $(DB_PASSWORD) -C

db-status: ## Verifica el estado de SQL Server
	@docker compose ps sqlserver

# ============================================
# EF CORE MIGRATIONS
# ============================================

ef-restore: ## Restaura herramientas .NET locales
	@dotnet tool restore

db-migrate: ef-restore ## Ejecuta migraciones pendientes
	@echo "Running migrations..."
	@$(DOTNET_EF) database update --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)
	@echo "Migrations applied!"

db-update: db-migrate ## Alias para db-migrate

db-add: ef-restore ## Crea nueva migracion (uso: make db-add m=NombreMigracion)
	@if [ -z "$(m)" ]; then \
		echo "Error: Debes proporcionar un nombre"; \
		echo "   Uso: make db-add m='NombreMigracion'"; \
		exit 1; \
	fi
	@echo "Creating migration: $(m)..."
	@$(DOTNET_EF) migrations add $(m) --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)
	@echo "Migration created!"

db-remove: ef-restore ## Elimina la ultima migracion
	@echo "Removing last migration..."
	@$(DOTNET_EF) migrations remove --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)
	@echo "Migration removed!"

db-script: ef-restore ## Genera script SQL de migraciones
	@echo "Generating SQL script..."
	@$(DOTNET_EF) migrations script --project $(INFRA_PROJECT) --startup-project $(API_PROJECT) -o migrations.sql
	@echo "Script generated: migrations.sql"

db-list: ef-restore ## Lista todas las migraciones
	@echo "Listing migrations..."
	@$(DOTNET_EF) migrations list --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)

db-auto: ef-restore ## Crea migracion con timestamp y aplica (si hay cambios)
	@echo "Creating migration with timestamp (if needed)..."
	@ts=$$(date +%Y%m%d%H%M%S); \
	$(DOTNET_EF) migrations add Auto_$$ts --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)
	@echo "Applying migrations..."
	@$(DOTNET_EF) database update --project $(INFRA_PROJECT) --startup-project $(API_PROJECT)
	@echo "Done."

fresh: db-clean db-up ## Reset completo (clean DB + migrate)
	@echo "Fresh install..."
	@echo "Esperando SQL Server..."
	@sleep 5
	@$(MAKE) db-migrate
	@echo "Database fresh!"

# ============================================
# TESTING
# ============================================

test: ## Ejecuta los tests
	@echo "Running tests..."
	@dotnet test $(SOLUTION)

test-cov: ## Ejecuta tests con cobertura
	@echo "Running tests with coverage..."
	@dotnet test $(SOLUTION) --collect:"XPlat Code Coverage"
	@echo "Coverage report generated!"

# ============================================
# CALIDAD DE CODIGO
# ============================================

format: ## Formatea el codigo
	@echo "Formatting code..."
	@dotnet format $(SOLUTION)
	@echo "Code formatted!"

# ============================================
# GIT
# ============================================

push: ## Push rapido (uso: make push m="mensaje")
	@if [ -z "$(m)" ]; then \
		echo "Error: Debes proporcionar un mensaje"; \
		echo "   Uso: make push m='tu mensaje de commit'"; \
		exit 1; \
	fi
	@echo "Agregando archivos..."
	@git add .
	@echo "Commiteando: $(m)"
	@git commit -m "$(m)"
	@echo "Pusheando a origin/$(BRANCH)..."
	@git push origin $(BRANCH)
	@echo "Push completado!"

pull: ## Pull desde origin
	@echo "Pulling desde origin/$(BRANCH)..."
	@git fetch origin
	@git pull origin $(BRANCH)
	@echo "Pull completado!"

status: ## Ver estado de git
	@echo "Estado de Git (rama: $(BRANCH)):"
	@echo ""
	@git status

sync: ## Sincronizar (pull + push)
	@if [ -z "$(m)" ]; then \
		echo "Error: Debes proporcionar un mensaje"; \
		echo "   Uso: make sync m='tu mensaje de commit'"; \
		exit 1; \
	fi
	@echo "Pulling cambios..."
	@git pull origin $(BRANCH)
	@echo "Agregando archivos..."
	@git add .
	@echo "Commiteando: $(m)"
	@git commit -m "$(m)"
	@echo "Pusheando a origin/$(BRANCH)..."
	@git push origin $(BRANCH)
	@echo "Sincronizacion completada!"

# ============================================
# SETUP INICIAL
# ============================================

setup: ## Setup inicial del proyecto
	@echo "Setting up Zenith HRMS..."
	@if [ ! -f .env ]; then \
		echo "Creando archivo .env..."; \
		cp .env.example .env; \
		echo "Archivo .env creado. Edita las credenciales si es necesario."; \
	fi
	@$(MAKE) restore
	@$(MAKE) db-up
	@echo "Esperando SQL Server..."
	@sleep 5
	@$(MAKE) db-migrate
	@echo ""
	@echo "Setup completado!"
	@echo "Ejecuta 'make run' para iniciar la aplicacion"
