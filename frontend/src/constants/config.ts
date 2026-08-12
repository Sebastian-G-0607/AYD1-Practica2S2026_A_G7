// URLs públicas centralizadas del API por ambiente
const API_URLS = {
  local: 'http://localhost:5000',
  develop: 'http://localhost:5000',
  main: 'http://localhost:5000',
} as const

type AppEnvironment = keyof typeof API_URLS

const currentEnv = (import.meta.env.VITE_APP_ENV || 'local') as AppEnvironment

export const config = {
  // Prioridad: 1. Variable de entorno explícita -> 2. URL según ambiente del mapa -> 3. Fallback local
  apiBaseUrl: import.meta.env.VITE_API_BASE_URL || API_URLS[currentEnv] || API_URLS.local,
  appEnv: currentEnv,
} as const