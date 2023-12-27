import axios from "axios";

class ApiService {
    constructor() {
        this.api = axios.create({
            baseURL: 'https://localhost:7075/api/',
            accept: 'application/json',
            headers: {
                'X-BORED-GAMES-API-KEY': 'BoredGames'
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

    handleError(error) {
        console.error('API Request Error:', error);
        throw error;
    }
}

const apiService = new ApiService();

export default apiService;