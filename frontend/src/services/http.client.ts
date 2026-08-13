import axios from 'axios'
import { config } from '@/constants/config'

export const httpClient = axios.create({
  baseURL: config.apiBaseUrl,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor: inyecta el token si existe
httpClient.interceptors.request.use((axiosConfig) => {
  const token = localStorage.getItem('auth_token')
  if (token) {
    axiosConfig.headers.Authorization = `Bearer ${token}`
  }
  return axiosConfig
})
