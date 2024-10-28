up-all:
	docker compose -f Infrastructure/compose.yaml up --build

up-db:
	docker compose -f Infrastructure/compose.db.yaml up --build

debug:
	docker compose -f Infrastructure/compose.debug.yaml up --build

down:
	docker compose -f Infrastructure/compose.yaml down
	docker compose -f Infrastructure/compose.debug.yaml down
	docker compose -f Infrastructure/compose.IntegrationTests.yaml down
	docker compose -f Infrastructure/compose.UnitTests.yaml down

unit:
	docker compose -f Infrastructure/compose.UnitTests.yaml up --build --abort-on-container-exit

integration:
	docker compose -f Infrastructure/compose.IntegrationTests.yaml up --build --abort-on-container-exit