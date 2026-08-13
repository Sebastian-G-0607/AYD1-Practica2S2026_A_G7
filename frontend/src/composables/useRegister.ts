import { ref, reactive } from 'vue'
import axios from 'axios'
import type { RegisterRequest, RegisterResponse } from '@/types/solicitud.types'
import { solicitudService } from '@/services/solicitud.service'
import { validators } from '@/utils/validators'

export function useRegister() {
  const form = reactive<RegisterRequest>({
    nombre: '',
    correo: '',
    contrasenia: '',
    confirmacion_contrasenia: '',
  })

  const formErrors = reactive({
    nombre: '',
    correo: '',
    contrasenia: '',
    confirmacion_contrasenia: '',
  })

  const isLoading = ref(false)
  const apiError = ref<string | null>(null)
  const isSuccess = ref(false)
  const successData = ref<RegisterResponse | null>(null)

  function resetFormErrors(): void {
    formErrors.nombre = ''
    formErrors.correo = ''
    formErrors.contrasenia = ''
    formErrors.confirmacion_contrasenia = ''
  }

  function validateForm(): boolean {
    let valid = true
    resetFormErrors()

    if (!validators.isNotEmpty(form.nombre)) {
      formErrors.nombre = 'El nombre completo es requerido.'
      valid = false
    }

    if (!validators.isNotEmpty(form.correo)) {
      formErrors.correo = 'El correo electrónico es requerido.'
      valid = false
    } else if (!validators.isValidEmail(form.correo)) {
      formErrors.correo = 'Ingresa un correo electrónico válido.'
      valid = false
    }

    if (!validators.isNotEmpty(form.contrasenia)) {
      formErrors.contrasenia = 'La contraseña es requerida.'
      valid = false
    } else if (!validators.minLength(form.contrasenia, 6)) {
      formErrors.contrasenia = 'La contraseña debe tener al menos 6 caracteres.'
      valid = false
    }

    if (!validators.isNotEmpty(form.confirmacion_contrasenia)) {
      formErrors.confirmacion_contrasenia = 'La confirmación de la contraseña es requerida.'
      valid = false
    } else if (!validators.matchesField(form.confirmacion_contrasenia, form.contrasenia)) {
      formErrors.confirmacion_contrasenia = 'Las contraseñas no coinciden.'
      valid = false
    }

    return valid
  }

  function handleApiError(err: unknown): void {
    if (!axios.isAxiosError(err) || !err.response) {
      apiError.value = 'No se pudo conectar con el servidor. Verifica tu conexión.'
      return
    }

    const { status, data } = err.response

    if (status === 400 && data?.errors) {
      const fieldNames = ['nombre', 'correo', 'contrasenia', 'confirmacion_contrasenia'] as const
      let hasMapped = false

      for (const [field, messages] of Object.entries(data.errors)) {
        const lowerField = field.toLowerCase()
        const targetKey = fieldNames.find((k) => k.toLowerCase() === lowerField)
        if (targetKey && Array.isArray(messages) && messages.length > 0) {
          formErrors[targetKey] = messages[0]
          hasMapped = true
        }
      }

      const unmapped = Object.entries(data.errors)
        .filter(([field]) => !fieldNames.some((k) => k.toLowerCase() === field.toLowerCase()))
        .flatMap(([, msgs]) => msgs as string[])

      if (unmapped.length > 0) {
        apiError.value = unmapped.join(' ')
      } else if (!hasMapped) {
        apiError.value = 'Los datos ingresados son inválidos.'
      }
      return
    }

    if (status === 400 && data?.message) {
      apiError.value = data.message
      return
    }

    if (status === 409 && data?.message) {
      apiError.value = data.message
      return
    }

    if (status >= 500) {
      apiError.value = 'Error del servidor. Intenta de nuevo más tarde.'
      return
    }

    apiError.value = 'Ocurrió un error inesperado.'
  }

  async function handleRegister(): Promise<void> {
    apiError.value = null
    if (!validateForm()) return

    isLoading.value = true
    try {
      const response = await solicitudService.crearSolicitud({
        nombre: form.nombre,
        correo: form.correo,
        contrasenia: form.contrasenia,
        confirmacion_contrasenia: form.confirmacion_contrasenia,
      })
      successData.value = response
      isSuccess.value = true
    } catch (err: unknown) {
      handleApiError(err)
    } finally {
      isLoading.value = false
    }
  }

  function resetForm(): void {
    form.nombre = ''
    form.correo = ''
    form.contrasenia = ''
    form.confirmacion_contrasenia = ''
    resetFormErrors()
    isLoading.value = false
    apiError.value = null
    isSuccess.value = false
    successData.value = null
  }

  return {
    form,
    formErrors,
    isLoading,
    apiError,
    isSuccess,
    successData,
    handleRegister,
    resetForm,
  }
}
