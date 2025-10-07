FROM node:22 AS builder

# Set working directory
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . .
ENV VITE_ENV=production

# Build the app for production
RUN npm run build
