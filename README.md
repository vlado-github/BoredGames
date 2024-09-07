# BoredGames
BoredGames platform for creating custom board games.

## BoredGames.Server
Backend server for handling queueing, game state and players actions. 
### Build
1. Run shell or cmd
2. Position to source folder of solution _BoredGames.Server_
3. Execute command:
`dotnet build`

### Run
1. Run shell or cmd
2. Position to source folder of solution _BoredGames.Server_
3. Execute command:
`dotnet run --project BoredGames.Server.API --launch-profile local`
4. Open swagger documentation in browser via url _https://localhost:7075/index.html_

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

