<div align="center">
<div align="center">
💖 <b>Found BoredGames interesting?</b>

⭐ Please consider giving us a star to support the project! ⭐
</div>

   <p>
      <img src="https://boredgames.lol/assets/clashofhands-logo.png" alt="ClashOfHands game logo" width="200px">
   </p>

   <h1>BoredGames</h1>
   <h3>Web platform for hosting custom web-based games and games' backends.</h3>
   
   [![GitHub Release](https://github.com/vlado-github/BoredGames/releases/badge.svg)](https://github.com/vlado-github/BoredGames/releases)

   [![Client Deploy](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.portal.yml/badge.svg)](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.portal.yml)

   [![Server Deploy](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.server.yml/badge.svg)](https://github.com/vlado-github/BoredGames/actions/workflows/boredgames.server.yml)

   <p>
      <a href="https://boredgames.lol">Website 🌎</a> • 
      <a href="https://github.com/vlado-github/BoredGames?tab=readme-ov-file#-getting-started">Installation ⚙️</a>
   </p>
</div>

## 📚 Introduction

<a href="https://boredgames.lol">BoredGames</a> is a web platform for creators to host their games and for players to enjoy playing web-based games. Games that run in browser can be played on any device without any installation. Platform API provides backend logic for scoring and multiplayer gaming.

<div align="center">
   <img alt="BoredGames website with gallery of games" src="https://boredgames.lol/assets/screenshot.png"/>
   <div align="center">
      <caption>BoredGames website</caption>
   </div>
</div>

## 🌟 Key Features

<a href="https://boredgames.lol">BoredGames</a> offers tools for your game:

### 🔧 Hosting
- **WebGL/Wasm/JS:** upload a game draft which is then reviewed and hosted for **free**

### 🏆 Scoring and muliplayer API
- **Scoring:** tracks gameplay scores 
- **Multiplayer:** play with other people on the network

### 📧 Player Profiles
- **Anonymous:** play without registration by using ephemeral session
- **Registered:** crete account to keep your scores and library of your favourite games

### 📱 Mobile-Friendly
- **Cross Platform:** runs in any browser natively
- **Screen Size:** supports different screen resolutions


## 🚀 Getting Started

### 🐳 Quick Start with Docker

> [!IMPORTANT]  
> Please ensure you have Docker and Docker Compose installed on your system. If not, you can download them from the
> official Docker website: [Docker](https://www.docker.com/get-started).

1. **Clone the Repository:**
   ```bash
   git clone git@github.com:vlado-github/BoredGames.git
   ```

2. **Navigate to the Root Directory (_BoredGames_):**
   ```bash
   cd BoredGames
   ```

3. **Start the Docker Containers:**
   ```bash
   docker compose --env-file .env.local up -d --force-recreate
   ```

4. **Open API documentation:**
   ```
   Open your browser and navigate to http://localhost:5008/index.html
   ```

### 💻 Local Setup

### BoredGames.Server

> [!IMPORTANT]  
> Please ensure you have .NET SDK 8 installed on your system. If not, you can download them from the
> official Microsoft website: [.NET](https://dotnet.microsoft.com/en-us/download/dotnet/8.0).

Server side contains two parts:

- _BoredGames.Server.GameServer_: a backend server for handling queueing, players actions, game logic and game state.
- _BoredGames.API_: a REST API for clients to interact with the game server.


#### Build

1. Run shell or cmd
2. Position to source folder of solution (_BoredGames/BoredGames.Server_)
3. Execute command:
   `dotnet build`

#### Run

1. Run shell or cmd
2. Position to source folder of solution (_BoredGames/BoredGames.Server_)
3. Execute command:
   `dotnet run --project BoredGames.Server.GameServer --launch-profile local`
4. Execute command:
   `dotnet run --project BoredGames.API --launch-profile local`
5. Open swagger documentation in browser via url _https://localhost:7075/index.html_

### BoredGames.Client

> [!IMPORTANT]  
> Please ensure you have NodeJS and NPM installed on your system. If not, you can download them from the
> official NodeJS website: [NodeJS](https://nodejs.org/en).

#### Build

1. Run shell or cmd
2. Position to source folder of project (_BoredGames/BoredGames.Client/BoredGames.Portal_)
3. Execute command:
   `npm install`

#### Run

1. Run shell or cmd
2. Position to source folder of project (_BoredGames/BoredGames.Client\BoredGames.Portal_)
3. Execute command:
   `npm run dev`