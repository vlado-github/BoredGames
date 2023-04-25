# BoredGames
BoredGames platform for creating custom board games.

## BoredGames.Server
Backend server for handling queueing, game state and players actions. 

## BoredGames.Client
### Build BoredGames.Client.CLI
1. Run shell or cmd
2. Position to source folder of solution
3. Execute command:
`dotnet build`

### Run BoredGame.Client.CLI
1. Run shell or cmd
2. Position to directory Debug/net7.0 or Release/net7.0 
3. Execute command:
`export DOTNET_ENV='Local';export BORED_GAMES_API_KEY='BoredGames';dotnet BoredGames.Client.CLI.dll`

