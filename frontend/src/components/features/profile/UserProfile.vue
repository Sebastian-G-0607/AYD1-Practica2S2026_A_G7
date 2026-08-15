<script setup lang="ts">
import { reactive, ref, computed, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth.store'
import { getProfile, updateProfile } from '@/services/profile.service'

const authStore = useAuthStore()

const userProfile = reactive({
  nombre: '',
  usuario: '',
  correo: '',
  contraseniaActual: '',
  nuevaContrasenia: '',
  confirmarContrasenia: '',
  totalResenias: 0,
  totalCompartidas: 0,
  rol: 'estandar',
})

const isLoading = ref(false)
const isSaving = ref(false)
const statusMessage = ref<{ type: 'success' | 'error'; text: string } | null>(null)

const userInitials = computed(() => {
  const nameStr = userProfile.nombre || authStore.user?.nombre || ''
  const names = nameStr.trim().split(/\s+/)

  if (names.length >= 2 && names[0] && names[1]) {
    return (names[0].charAt(0) + names[1].charAt(0)).toUpperCase()
  } else if (names.length >= 1 && names[0]) {
    return names[0].substring(0, 2).toUpperCase()
  }
  return 'U'
})

const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/

const loadUserData = async () => {
  if (!authStore.user?.id) return

  try {
    isLoading.value = true
    const profile = await getProfile(authStore.user.id)
    userProfile.nombre = profile.nombre
    userProfile.correo = profile.correo
    userProfile.rol = profile.rol
    userProfile.totalResenias = profile.totalResenias
    userProfile.totalCompartidas = profile.totalCompartidas
    
    const firstName = (profile.nombre.trim().split(' ')[0] || 'usuario').toLowerCase()
    userProfile.usuario = `@${firstName}_cine`
  } catch (err: any) {
    console.error('Error al cargar perfil:', err)
    if (authStore.user) {
      userProfile.nombre = authStore.user.nombre || ''
      userProfile.correo = authStore.user.email || ''
      userProfile.rol = authStore.user.role || 'estandar'
      const firstName = (userProfile.nombre.trim().split(' ')[0] || 'usuario').toLowerCase()
      userProfile.usuario = `@${firstName}_cine`
    }
  } finally {
    isLoading.value = false
  }
}

onMounted(() => {
  loadUserData()
})

const saveProfile = async () => {
  statusMessage.value = null

  if (!userProfile.nombre.trim()) {
    statusMessage.value = { type: 'error', text: 'El nombre completo es obligatorio.' }
    return
  }

  if (!userProfile.correo.trim()) {
    statusMessage.value = { type: 'error', text: 'El correo electrónico es obligatorio.' }
    return
  }

  if (!emailRegex.test(userProfile.correo.trim())) {
    statusMessage.value = { type: 'error', text: 'Por favor ingresa un correo electrónico válido.' }
    return
  }

  if (userProfile.nuevaContrasenia) {
    if (userProfile.nuevaContrasenia.length < 6) {
      statusMessage.value = {
        type: 'error',
        text: 'La nueva contraseña debe contener al menos 6 caracteres.',
      }
      return
    }

    if (!userProfile.contraseniaActual) {
      statusMessage.value = {
        type: 'error',
        text: 'Debes ingresar tu contraseña actual para confirmar el cambio de contraseña.',
      }
      return
    }

    if (userProfile.nuevaContrasenia !== userProfile.confirmarContrasenia) {
      statusMessage.value = {
        type: 'error',
        text: 'Las nuevas contraseñas no coinciden. Por favor verifícalas.',
      }
      return
    }
  }

  const correoOriginal = authStore.user?.email || ''
  const isChangingSensitiveData =
    Boolean(userProfile.nuevaContrasenia) ||
    userProfile.correo.trim().toLowerCase() !== correoOriginal.toLowerCase()

  if (isChangingSensitiveData) {
    const isConfirmed = confirm(
      'Estás a punto de actualizar información crítica (correo electrónico o contraseña). ¿Deseas guardar los cambios?',
    )
    if (!isConfirmed) return
  }

  try {
    isSaving.value = true
    const payload = {
      nombre: userProfile.nombre.trim(),
      correo: userProfile.correo.trim(),
      contraseniaActual: userProfile.contraseniaActual || undefined,
      nuevaContrasenia: userProfile.nuevaContrasenia || undefined,
    }

    if (!authStore.user?.id) throw new Error('No hay sesión de usuario activa.')
    const response = await updateProfile(authStore.user.id, payload)

    authStore.updateUserProfile({
      nombre: response.nombre || userProfile.nombre,
      email: response.correo || userProfile.correo,
    })

    statusMessage.value = {
      type: 'success',
      text: response.mensaje || '¡Perfil de usuario actualizado exitosamente!',
    }

    userProfile.contraseniaActual = ''
    userProfile.nuevaContrasenia = ''
    userProfile.confirmarContrasenia = ''
  } catch (error: any) {
    console.error(error)
    const mensaje =
      error.response?.data?.mensaje ||
      error.response?.data?.message ||
      error.message ||
      'Ocurrió un error al actualizar el perfil.'
    statusMessage.value = { type: 'error', text: mensaje }
  } finally {
    isSaving.value = false
  }
}
</script>

<template>
  <div class="space-y-8 max-w-5xl mx-auto">
    <!-- Header -->
    <div class="flex flex-col gap-2">
      <h1 class="font-display-lg text-3xl md:text-4xl font-bold text-on-surface">
        Configuración de Perfil
      </h1>
      <p class="font-body-lg text-on-surface-variant opacity-80 text-sm md:text-base">
        Administra tus datos de cuenta, seguridad y preferencias de la plataforma.
      </p>
    </div>

    <!-- Alert / Toast Banner -->
    <div
      v-if="statusMessage"
      class="p-4 rounded-xl flex items-center gap-3 transition-all duration-300 shadow-md"
      :class="
        statusMessage.type === 'success'
          ? 'bg-emerald-500/10 border border-emerald-500/30 text-emerald-300'
          : 'bg-red-500/10 border border-red-500/30 text-red-300'
      "
    >
      <span class="material-symbols-outlined text-2xl shrink-0">
        {{ statusMessage.type === 'success' ? 'check_circle' : 'error' }}
      </span>
      <p class="text-sm font-medium flex-1">{{ statusMessage.text }}</p>
      <button
        type="button"
        @click="statusMessage = null"
        class="text-xs opacity-70 hover:opacity-100 p-1"
      >
        ✕
      </button>
    </div>

    <!-- Loading State -->
    <div v-if="isLoading" class="p-16 text-center text-on-surface-variant flex flex-col items-center gap-4">
      <span class="material-symbols-outlined text-4xl animate-spin text-primary">sync</span>
      <p>Cargando información del perfil...</p>
    </div>

    <!-- Content Grid -->
    <div v-else class="grid grid-cols-1 md:grid-cols-3 gap-8">
      <!-- Columna Izquierda: Tarjeta de Avatar y Estadísticas -->
      <div class="md:col-span-1 flex flex-col gap-6">
        <div
          class="bg-surface-container rounded-2xl p-6 flex flex-col items-center gap-4 shadow-xl border border-outline-variant/10"
        >
          <div class="relative group">
            <div
              class="w-32 h-32 md:w-36 md:h-36 rounded-full bg-primary/20 text-primary flex items-center justify-center font-display-lg text-5xl font-bold shadow-xl transition-transform duration-300 group-hover:scale-105"
            >
              {{ userInitials }}
            </div>
          </div>
          <div class="text-center">
            <h2 class="font-headline-md text-xl font-bold text-on-surface">
              {{ userProfile.nombre || 'Usuario CineCraft' }}
            </h2>
            <p class="text-xs text-primary font-mono mt-1">
              {{ userProfile.usuario }}
            </p>
            <span
              class="inline-block mt-2 px-3 py-1 rounded-full text-xs font-semibold uppercase tracking-wider bg-surface-container-high text-primary border border-primary/20"
            >
              ROL: {{ userProfile.rol || 'ESTÁNDAR' }}
            </span>
          </div>
        </div>

        <!-- Estadísticas en vivo -->
        <div
          class="bg-surface-container rounded-2xl p-6 flex flex-col gap-4 shadow-xl border border-outline-variant/10"
        >
          <h3 class="font-headline-md text-base font-bold text-on-surface flex items-center gap-2">
            <span class="material-symbols-outlined text-primary text-xl">analytics</span>
            Actividad en CineCraft
          </h3>
          <div class="grid grid-cols-2 gap-4">
            <div class="flex flex-col bg-surface-dim/40 p-4 rounded-xl border border-outline-variant/10">
              <span class="text-3xl font-extrabold text-primary">{{ userProfile.totalResenias }}</span>
              <span class="text-xs text-on-surface-variant uppercase tracking-wider opacity-70 mt-1">Reseñas</span>
            </div>
            <div class="flex flex-col bg-surface-dim/40 p-4 rounded-xl border border-outline-variant/10">
              <span class="text-3xl font-extrabold text-secondary-container">{{ userProfile.totalCompartidas }}</span>
              <span class="text-xs text-on-surface-variant uppercase tracking-wider opacity-70 mt-1">Compartidas</span>
            </div>
          </div>
        </div>
      </div>

      <!-- Columna Derecha: Formularios de Edición -->
      <div class="md:col-span-2 flex flex-col gap-6">
        <!-- Datos Personales -->
        <div class="bg-surface-container rounded-2xl p-6 md:p-8 shadow-xl border border-outline-variant/10 space-y-6">
          <div class="border-b border-outline-variant/10 pb-4">
            <h3 class="font-headline-md text-lg font-bold text-on-surface">
              Información de la Cuenta
            </h3>
            <p class="text-xs text-on-surface-variant opacity-70 mt-1">
              Actualiza tu nombre visible y tu dirección de correo electrónico vinculada.
            </p>
          </div>

          <div class="space-y-4">
            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="flex flex-col gap-2">
                <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                  Nombre Completo <span class="text-primary">*</span>
                </label>
                <input
                  v-model="userProfile.nombre"
                  type="text"
                  placeholder="Tu nombre completo"
                  class="bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-all text-sm"
                />
              </div>

              <div class="flex flex-col gap-2">
                <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                  Alias en Plataforma
                </label>
                <input
                  v-model="userProfile.usuario"
                  type="text"
                  disabled
                  class="bg-surface-dim/50 border border-outline-variant/10 rounded-xl px-4 py-3 text-on-surface-variant/70 font-mono text-sm cursor-not-allowed"
                />
              </div>
            </div>

            <div class="flex flex-col gap-2">
              <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                Correo Electrónico <span class="text-primary">*</span>
              </label>
              <input
                v-model="userProfile.correo"
                type="email"
                placeholder="tu.correo@ejemplo.com"
                class="bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-all text-sm"
              />
            </div>
          </div>
        </div>

        <!-- Seguridad y Contraseña -->
        <div class="bg-surface-container rounded-2xl p-6 md:p-8 shadow-xl border border-outline-variant/10 space-y-6">
          <div class="border-b border-outline-variant/10 pb-4">
            <h3 class="font-headline-md text-lg font-bold text-on-surface flex items-center gap-2">
              <span class="material-symbols-outlined text-primary text-xl">lock</span>
              Seguridad y Cambio de Contraseña
            </h3>
            <p class="text-xs text-on-surface-variant opacity-70 mt-1">
              Deja estos campos en blanco si no deseas cambiar tu contraseña actual.
            </p>
          </div>

          <div class="space-y-4">
            <div class="flex flex-col gap-2">
              <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                Contraseña Actual
              </label>
              <input
                v-model="userProfile.contraseniaActual"
                type="password"
                placeholder="••••••••"
                class="bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-all text-sm"
              />
            </div>

            <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div class="flex flex-col gap-2">
                <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                  Nueva Contraseña
                </label>
                <input
                  v-model="userProfile.nuevaContrasenia"
                  type="password"
                  placeholder="Mínimo 6 caracteres"
                  class="bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-all text-sm"
                />
              </div>

              <div class="flex flex-col gap-2">
                <label class="font-label-md text-xs font-semibold text-on-surface-variant uppercase tracking-wider">
                  Confirmar Nueva Contraseña
                </label>
                <input
                  v-model="userProfile.confirmarContrasenia"
                  type="password"
                  placeholder="Repite la nueva contraseña"
                  class="bg-surface-dim border border-outline-variant/20 rounded-xl px-4 py-3 text-on-surface font-body-md focus:outline-none focus:border-primary focus:ring-1 focus:ring-primary transition-all text-sm"
                />
              </div>
            </div>
          </div>
        </div>

        <!-- Botón de Guardar -->
        <div class="flex justify-end pt-2 pb-6">
          <button
            @click="saveProfile"
            :disabled="isSaving"
            type="button"
            class="bg-primary hover:bg-primary-container text-on-primary font-label-md font-semibold px-8 py-3.5 rounded-xl shadow-lg shadow-primary/20 transition-all duration-300 transform hover:-translate-y-0.5 disabled:opacity-50 disabled:cursor-not-allowed flex items-center gap-2"
          >
            <span v-if="isSaving" class="material-symbols-outlined text-sm animate-spin">sync</span>
            <span v-else class="material-symbols-outlined text-sm">save</span>
            <span>{{ isSaving ? 'Guardando cambios...' : 'Guardar Cambios' }}</span>
          </button>
        </div>
      </div>
    </div>
  </div>
</template>