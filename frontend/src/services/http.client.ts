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
  console.log('Token en localStorage:', token ? 'Existe' : 'No existe')
  if (token) {
    axiosConfig.headers.Authorization = `Bearer ${token}`
    console.log('Header Authorization agregado')
  }
  return axiosConfig
})

httpClient.interceptors.response.use(
  (response) => response,
  (error) => {
    console.error('Error en la petición:', error.response?.status, error.response?.data)
    return Promise.reject(error)
  }
)