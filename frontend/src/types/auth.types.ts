export interface LoginRequest {
  email: string
  password: string
}

export interface LoginResponse {
  token: string
  email: string
  nombre: string
  role: string
  expiresAt: string
}

export interface AuthUser {
  email: string
  nombre: string
  role: string
  expiresAt: string
}
