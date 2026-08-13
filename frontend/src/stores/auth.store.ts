import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import type { AuthUser, LoginRequest } from '@/types/auth.types'
import { authService } from '@/services/auth.service'
import axios from 'axios'

const TOKEN_KEY = 'auth_token'
const USER_KEY = 'auth_user'

export const useAuthStore = defineStore('auth', () => {
  const token = ref<string | null>(localStorage.getItem(TOKEN_KEY))
  const user = ref<AuthUser | null>(
    JSON.parse(localStorage.getItem(USER_KEY) ?? 'null')
  )
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const isAuthenticated = computed(() => {
    if (!token.value || !user.value) return false
    // Validar que el token no haya expirado
    return new Date(user.value.expiresAt) > new Date()
  })

  async function login(credentials: LoginRequest): Promise<void> {
    isLoading.value = true
    error.value = null
    try {
      const response = await authService.login(credentials)

      // Persistir en localStorage
      localStorage.setItem(TOKEN_KEY, response.token)
      const userData: AuthUser = {
        email: response.email,
        nombre: response.nombre,
        role: response.role,
        expiresAt: response.expiresAt,
      }
      localStorage.setItem(USER_KEY, JSON.stringify(userData))

      // Actualizar estado reactivo
      token.value = response.token
      user.value = userData
    } catch (err: unknown) {
      if (axios.isAxiosError(err) && err.response) {
        const status = err.response.status
        if (status === 401 || status === 403) {
          error.value = 'Credenciales inválidas. Verifica tu correo y contraseña.'
        } else if (status >= 500) {
          error.value = 'Error del servidor. Intenta de nuevo más tarde.'
        } else {
          error.value = 'Ocurrió un error inesperado.'
        }
      } else {
        error.value = 'No se pudo conectar con el servidor.'
      }
      throw err
    } finally {
      isLoading.value = false
    }
  }

  function logout(): void {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    token.value = null
    user.value = null
    error.value = null
  }

  function clearError(): void {
    error.value = null
  }

  return { token, user, isLoading, error, isAuthenticated, login, logout, clearError }
})
