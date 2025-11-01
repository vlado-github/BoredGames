
# BoredGames

![Client Deploy](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.portal.yml/badge.svg)

![Server Deploy](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.server.yml/badge.svg)

BoredGames platform for hosting custom games and games' backends.

## Docker

Run docker compose command from root directory `BoredGames`:

```bash
docker compose --env-file .env.local up -d --force-recreate
```

## BoredGames.Server

Contains two parts:

- _BoredGames.Server.GameServer_: a backend server for handling queueing, players actions, game logic and game state.
- _BoredGames.API_: a REST API for clients to interact with the game server.

### Build

1. Run shell or cmd
2. Position to source folder of solution _BoredGames.Server_
3. Execute command:
   `dotnet build`

### Run

1. Run shell or cmd
2. Position to source folder of solution _BoredGames.Server_
3. Execute command:
   `dotnet run --project BoredGames.Server.GameServer --launch-profile local`
4. Execute command:
   `dotnet run --project BoredGames.API --launch-profile local`
5. Open swagger documentation in browser via url _https://localhost:7075/index.html_

## BoredGames.Client

### Build BoredGames.Portal

1. Run shell or cmd
2. Position to source folder of project
3. Execute command:
   `yarn build`

### Run BoredGame.Portal

1. Run shell or cmd
2. Position to source folder of project
3. Execute command:
   `yarn dev`
