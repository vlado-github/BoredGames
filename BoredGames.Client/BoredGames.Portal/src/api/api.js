import axios from "axios";
import { uuid } from 'vue-uuid'; 
import LocalStorageKeys from '@/consts/localStorageKeys';

class ApiService {
    constructor() {
        let playerId = localStorage.getItem(LocalStorageKeys.PlayerId);
        if (!playerId){
            playerId = uuid.v4();
            localStorage.setItem(LocalStorageKeys.PlayerId, playerId);
        }

        this.api = axios.create({
            baseURL: `${import.meta.env.VITE_BACKEND_API_URL}/api/`,
            accept: 'application/json',
            headers: {
                'X-BORED-GAMES-API-KEY': `${import.meta.env.VITE_BACKEND_API_KEY}`,
                'X-BORED-GAMES-PLAYER-ID': playerId
            }
        });
    }

    async getTitles() {
        try {
            const response = await this.api.get("game/titles");
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async createGame(request) {
        try {
            const response = await this.api.post("game/create", request);
            let playerId = localStorage.getItem(LocalStorageKeys.PlayerId);
            if (!playerId){
                localStorage.setItem(LocalStorageKeys.PlayerId, response.data.playerId);
            }
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async joinGame(request) {
        try {
            const response = await this.api.put("game/join", request);
            let playerId = localStorage.getItem(LocalStorageKeys.PlayerId);
            if (!playerId){
                localStorage.setItem(LocalStorageKeys.PlayerId, response.data.playerId);
            }
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async getGameState(gameId) {
        try {
            const response = await this.api.get(`game/${gameId}/state`);
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async makeMove(request) {
        try {
            const response = await this.api.post("game/makemove", request);
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async getGameScore(gameId) {
        try {
            const response = await this.api.get(`game/${gameId}/score`);
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async getGameWinner(gameId) {
        try {
            const response = await this.api.get(`game/${gameId}/winners`);
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    handleError(error) {
        console.error('API Request Error:', error);
        throw error;
    }
}

const apiService = new ApiService();

export default apiService;