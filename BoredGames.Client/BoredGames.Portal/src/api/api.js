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
        const response = await this.api.get("game/titles").catch(this.handleError);
        return response.data;
    }

    async createGame(request) {
        const response = await this.api.post("game/create", request).catch(this.handleError);

        const playerDetails = await this.getMyDetails();
        localStorage.setItem(LocalStorageKeys.PlayerId, playerDetails.id);
        localStorage.setItem(LocalStorageKeys.PlayerNickName, playerDetails.nickName);
        
        return response.data;
    }

    async joinGame(request) {
        const response = await this.api.put("game/join", request).catch(this.handleError);

        const playerDetails = await this.getMyDetails();
        localStorage.setItem(LocalStorageKeys.PlayerId, playerDetails.id);
        localStorage.setItem(LocalStorageKeys.PlayerNickName, playerDetails.nickName);

        return response.data;
    }

    async getGameDefinition(gameId) {
        const response = await this.api.get(`game/${gameId}/definition`).catch(this.handleError);
        return response.data;
    }

    async getMyDetails() {
        const response = await this.api.get(`player/details`).catch(this.handleError);
        return response.data;
    }

    async getGameState(gameId) {
        const response = await this.api.get(`game/${gameId}/state`).catch(this.handleError);
        return response.data;
    }

    async makeMove(request) {
        const response = await this.api.post("game/makemove", request).catch(this.handleError);
        return response.data;
    }

    async getGameScore(gameId) {
        const response = await this.api.get(`game/${gameId}/score`).catch(this.handleError);
        return response.data;
    }

    async getGameWinner(gameId) {
        const response = await this.api.get(`game/${gameId}/winners`).catch(this.handleError);
        return response.data;
    }

    handleError(error) {
        if (error.status == 400) {
            console.log(error.response.title);
        }
        else {
            console.error('API Request Error:', error);
        }
        throw error;
    }
}

const apiService = new ApiService();

export default apiService;