import axios from "axios";
import { uuid } from 'vue-uuid'; 
import LocalStorageKeys from '@/consts/localStorageKeys';

class ApiService {
    constructor() {
        const playerId = localStorage.getItem(LocalStorageKeys.PlayerId);
        if (!playerId){
            localStorage.setItem(LocalStorageKeys.PlayerId, uuid.v4());
        }
        this.api = axios.create({
            baseURL: 'https://localhost:7075/api/',
            accept: 'application/json',
            headers: {
                'X-BORED-GAMES-API-KEY': 'BoredGames',
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
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async joinGame(request) {
        try {
            const response = await this.api.put("game/join", request);
            return response.data;
        } catch (error) {
            throw this.handleError(error);
        }
    }

    async getGameState(gameId) {
        try {
            const response = await this.api.get(`game/${gameId}/state`);
            //console.log(">>state: "+JSON.stringify(response));
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
           // console.log(">>score: "+JSON.stringify(response))
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