up:
	docker compose -f Infrastructure/compose.yaml up --build

down:
	docker compose -f Infrastructure/compose.yaml down

unit:
	docker compose -f Infrastructure/compose.UnitTests.yaml up --build --abort-on-container-exit

integration:
	docker compose -f Infrastructure/compose.IntegrationTests.yaml up --build --abort-on-container-exit