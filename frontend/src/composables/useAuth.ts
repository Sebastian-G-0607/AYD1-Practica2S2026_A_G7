import { reactive, computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import { validators } from '@/utils/validators'

export function useAuth() {
  const authStore = useAuthStore()
  const router = useRouter()

  const form = reactive({
    email: '',
    password: '',
  })

  const formErrors = reactive({
    email: '',
    password: '',
  })

  function validateForm(): boolean {
    let valid = true
    formErrors.email = ''
    formErrors.password = ''

    if (!validators.isNotEmpty(form.email)) {
      formErrors.email = 'El correo electrónico es requerido.'
      valid = false
    } else if (!validators.isValidEmail(form.email)) {
      formErrors.email = 'Ingresa un correo electrónico válido.'
      valid = false
    }

    if (!validators.isNotEmpty(form.password)) {
      formErrors.password = 'La contraseña es requerida.'
      valid = false
    } else if (!validators.minLength(form.password, 6)) {
      formErrors.password = 'La contraseña debe tener al menos 6 caracteres.'
      valid = false
    }

    return valid
  }

  async function handleLogin(): Promise<void> {
    if (!validateForm()) return
    try {
      await authStore.login({ email: form.email, password: form.password })
      if (!authStore.error) {
        if (authStore.isAdmin) {
          await router.push({ name: 'Admin' })
        } else {
          await router.push({ name: 'Home' })
        }
      }
    } catch {
    }
  }

  return {
    form,
    formErrors,
    isLoading: computed(() => authStore.isLoading),
    apiError: computed(() => authStore.error),
    handleLogin,
    clearApiError: authStore.clearError,
  }
}
