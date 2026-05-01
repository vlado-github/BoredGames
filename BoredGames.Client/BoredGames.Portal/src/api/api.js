import axios from "axios";
import { getAuthToken } from "./auth";

class ApiService {
  constructor() {
    this.api = axios.create({
      baseURL: `${import.meta.env.VITE_BACKEND_API_URL}/api/`,
      accept: 'application/json'
    });
  }

  async getTitles() {
    const response = await this.api
      .get("game/titles")
      .catch(this.handleError);
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