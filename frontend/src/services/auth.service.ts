import type { LoginRequest, LoginResponse } from '@/types/auth.types'
import { httpClient } from './http.client'

export const authService = {
  async login(credentials: LoginRequest): Promise<LoginResponse> {
    const { data } = await httpClient.post<LoginResponse>('/api/login', credentials)
    return data
  },
}
