export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  token: string
  id: number
  email: string
  nombre: string
  role: string
  expiresAt: string
}

export interface AuthUser {
  id: number
  email: string
  nombre: string
  role: string
  expiresAt: string
}

