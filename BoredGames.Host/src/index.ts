import fastify from "fastify";
import fastifyStatic from "@fastify/static";
import path, { dirname } from "path";
import { fileURLToPath } from "url";
import fs from "fs";

const server = fastify({ logger: true });
const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const gamesRoot = path.join(__dirname, "../static/games");

// Read all game directories inside /static/games/
const gameDirs = fs
  .readdirSync(gamesRoot, { withFileTypes: true })
  .filter((dirent) => dirent.isDirectory())
  .map((dirent) => dirent.name);

console.log("🕹️ Available games:", gameDirs);

// Dynamically register a static route for each game
for (const game of gameDirs) {
  const gamePath = path.join(gamesRoot, game);

  server.register(fastifyStatic, {
    root: gamePath,
    prefix: `/${game}/`,
    decorateReply: false, // prevents conflicts if reusing reply.sendFile
    wildcard: false,
  });

  // Optional: fallback to index.html for Unity internal routing
  server.get(`/${game}/*`, async (req, reply) => {
    return reply.sendFile("index.html", gamePath);
  });
}

// Start server
const start = async () => {
  try {
    await server.listen({ port: 3000 });
  } catch (err) {
    server.log.error(err);
    process.exit(1);
  }
};

start();
