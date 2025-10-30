import axios from "axios";
import LocalStorageKeys from '@/consts/localStorageKeys';

class ApiService {
    constructor() {
        this.api = axios.create({
            baseURL: `${import.meta.env.VITE_BACKEND_API_URL}/api/`,
            accept: 'application/json',
            headers: {
                'X-BORED-GAMES-API-KEY': `${import.meta.env.VITE_BACKEND_API_KEY}`,
            }
        });
    }

    async getTitles() {
        const response = await this.api.get("game/titles").catch(this.handleError);
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