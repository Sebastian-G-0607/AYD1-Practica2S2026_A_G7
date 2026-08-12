<script setup lang="ts">
import { useAuthStore } from '@/stores/auth.store'
import { useRouter } from 'vue-router'

const authStore = useAuthStore()
const router = useRouter()

function handleLogout() {
  authStore.logout()
  router.push({ name: 'Login' })
}
</script>

<template>
  <div class="min-h-screen bg-surface text-on-surface p-8 flex flex-col items-center justify-center">
    <div class="max-w-md w-full bg-surface-container-low border border-surface-bright p-8 rounded-2xl text-center space-y-4">
      <h1 class="text-2xl font-bold font-headline-lg">Inicio / Home</h1>
      <p v-if="authStore.user" class="text-sm text-on-surface/70">
        Bienvenido, <span class="font-semibold text-primary">{{ authStore.user.nombre }}</span> ({{ authStore.user.email }})
      </p>
      <div class="pt-4">
        <button
          @click="handleLogout"
          class="px-4 py-2 bg-surface-bright text-on-surface rounded-lg hover:bg-surface-variant transition-colors text-sm font-semibold"
        >
          Cerrar sesión
        </button>
      </div>
    </div>
  </div>
</template>
