import axios from 'axios'

let cachedToken = null
let expiresAt = 0

export async function getAuthToken() {
  const now = Date.now()

  if (cachedToken && now < expiresAt) {
    return cachedToken
  }

  const res = await axios.get(
    `${import.meta.env.VITE_BACKEND_API_URL}/api/auth/token`,
    { headers: { 'Content-Type': 'application/json' } }
  )

  cachedToken = res.data.access_token
  expiresAt = now + (res.data.expires_in - 30) * 1000

  return cachedToken
}
