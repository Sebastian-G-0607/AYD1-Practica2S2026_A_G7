<script setup lang="ts">
import { useAuthStore } from '@/stores/auth.store'
import { useRouter } from 'vue-router'

const emit = defineEmits<{
  (e: 'toggle-sidebar'): void
}>()

const authStore = useAuthStore()
const router = useRouter()

function handleLogout() {
  authStore.logout()
  router.push({ name: 'Login' })
}
</script>

<template>
  <header class="sticky top-0 z-30 h-20 bg-surface/80 backdrop-blur-2xl px-4 md:px-8 flex items-center justify-between border-b border-outline-variant/10 shadow-sm gap-4">
    <div class="flex items-center gap-3 md:gap-4 flex-1 max-w-3xl">
      <button
        type="button"
        class="lg:hidden p-2 text-on-surface-variant hover:text-on-surface rounded-xl hover:bg-surface-container-high transition-colors shrink-0"
        aria-label="Abrir menú"
        @click="emit('toggle-sidebar')"
      >
        <span class="material-symbols-outlined text-[24px]">menu</span>
      </button>

      <div class="relative flex-1 max-w-2xl group">
        <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant group-focus-within:text-primary transition-colors text-[20px]">
          search
        </span>
        <input
          type="text"
          placeholder="Buscar películas, reseñas o creadores..."
          class="w-full bg-surface-container-high/50 border border-outline-variant/10 rounded-full py-2.5 pl-11 pr-4 text-xs md:text-sm text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-2 focus:ring-primary/20 focus:border-primary transition-all font-body-md"
        />
      </div>
    </div>

    <div class="flex items-center gap-3 md:gap-5 shrink-0">
      <button
        type="button"
        class="relative p-2.5 rounded-full hover:bg-surface-container-high transition-colors text-on-surface-variant hover:text-on-surface"
        title="Notificaciones"
      >
        <span class="material-symbols-outlined text-[22px]">notifications</span>
        <span class="absolute top-2 right-2 w-2 h-2 bg-primary rounded-full" />
      </button>

      <div class="h-8 w-px bg-outline-variant/20 hidden sm:block" />

      <div class="flex items-center gap-3">
        <router-link
          to="/profile"
          class="flex items-center gap-3 hover:opacity-80 transition-opacity"
          title="Ir a Configuración de Perfil"
        >
          <div class="text-right hidden sm:block">
            <p class="text-xs md:text-sm font-semibold text-on-surface leading-tight">
              {{ authStore.user?.nombre || 'Usuario' }}
            </p>
            <p class="text-[11px] text-primary font-medium capitalize">
              ROL: {{ authStore.user?.role || 'estandar' }}
            </p>
          </div>

          <div class="w-9 h-9 md:w-10 md:h-10 rounded-full bg-surface-container-high flex items-center justify-center text-on-surface border border-outline-variant/20 shrink-0">
            <span class="material-symbols-outlined text-primary text-[20px]">person</span>
          </div>
        </router-link>

        <button
          type="button"
          class="hidden md:inline-flex px-3 py-1.5 bg-surface-container-high hover:bg-surface-bright text-on-surface rounded-lg text-xs font-semibold transition-colors border border-outline-variant/20"
          @click="handleLogout"
        >
          Cerrar sesión
        </button>
      </div>
    </div>
  </header>
</template>
