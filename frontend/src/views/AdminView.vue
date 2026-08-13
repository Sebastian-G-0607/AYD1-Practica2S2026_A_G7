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
  <div class="min-h-screen bg-surface text-on-surface">
    <!-- Header Admin -->
    <header class="bg-surface-container-lowest border-b border-surface-bright sticky top-0 z-30">
      <div class="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between">
        <div class="flex items-center gap-3">
          <div class="w-10 h-10 rounded-xl bg-secondary-container text-on-secondary-container flex items-center justify-center font-bold">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 fill-current" viewBox="0 0 24 24">
              <path d="M12 1L3 5v6c0 5.55 3.84 10.74 9 12 5.16-1.26 9-5.45 9-12V5l-9-4zm0 10.99h7c-.53 4.12-3.28 7.79-7 8.94V12H5V6.3l7-3.11v8.8z"/>
            </svg>
          </div>
          <div>
            <h1 class="text-xl font-bold font-headline-lg">
              Panel de Administración
            </h1>
            <p class="text-xs text-secondary font-medium">
              CineCraft Admin Studio
            </p>
          </div>
        </div>

        <div class="flex items-center gap-4">
          <div class="text-right hidden sm:block">
            <p class="text-sm font-semibold text-on-surface">
              {{ authStore.user?.nombre || 'Administrador' }}
            </p>
            <span class="inline-block px-2 py-0.5 rounded text-xs font-bold bg-secondary-container/20 text-secondary border border-secondary/30">
              ROL: ADMIN
            </span>
          </div>

          <button
            type="button"
            class="px-4 py-2 bg-surface-bright text-on-surface rounded-lg hover:bg-surface-bright/80 transition-opacity text-sm font-semibold"
            @click="handleLogout"
          >
            Cerrar sesión
          </button>
        </div>
      </div>
    </header>

    <!-- Contenido Admin -->
    <main class="max-w-7xl mx-auto px-6 py-10 space-y-8">
      <!-- Tarjeta de Bienvenida Admin -->
      <section class="bg-gradient-to-r from-surface-container-low to-surface-container-high border border-surface-bright rounded-2xl p-8 shadow-xl">
        <div class="max-w-3xl space-y-4">
          <div class="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-secondary-container/20 border border-secondary/30 text-secondary text-xs font-semibold">
            <span>🛡️</span>
            <span>Acceso Privilegiado</span>
          </div>
          <h2 class="text-3xl font-bold font-headline-lg text-on-surface">
            Bienvenido al Portal de Administración, {{ authStore.user?.nombre }}
          </h2>
          <p class="text-on-surface/70 leading-relaxed">
            Esta es la ruta exclusiva configurada para usuarios con rol <strong class="text-secondary">Admin</strong>. Desde aquí podrás gestionar las solicitudes de registro pendientes, visualizar reportes de usuarios y supervisar la actividad global de CineCraft.
          </p>
        </div>
      </section>

      <!-- Módulos de Administración -->
      <section class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <!-- Tarjeta 1: Solicitudes Pendientes -->
        <div class="bg-surface-container-low border border-surface-bright rounded-2xl p-6 hover:border-secondary/50 transition-all space-y-4 group">
          <div class="w-12 h-12 rounded-xl bg-surface-bright flex items-center justify-center text-secondary group-hover:scale-110 transition-transform">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 fill-current" viewBox="0 0 24 24">
              <path d="M19 3h-4.18C14.4 1.84 13.3 1 12 1c-1.3 0-2.4.84-2.82 2H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zm-7 0c.55 0 1 .45 1 1s-.45 1-1 1-1-.45-1-1 .45-1 1-1zm2 14H7v-2h7v2zm3-4H7v-2h10v2zm0-4H7V7h10v2z"/>
            </svg>
          </div>
          <h3 class="text-xl font-bold text-on-surface">
            Solicitudes Pendientes
          </h3>
          <p class="text-sm text-on-surface/60">
            Revisa y aprueba las solicitudes de registro enviadas por nuevos usuarios.
          </p>
        </div>

        <!-- Tarjeta 2: Historial de Solicitudes -->
        <div class="bg-surface-container-low border border-surface-bright rounded-2xl p-6 hover:border-secondary/50 transition-all space-y-4 group">
          <div class="w-12 h-12 rounded-xl bg-surface-bright flex items-center justify-center text-secondary group-hover:scale-110 transition-transform">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 fill-current" viewBox="0 0 24 24">
              <path d="M13 3c-4.97 0-9 4.03-9 9H1l3.89 3.89.07.14L9 12H6c0-3.87 3.13-7 7-7s7 3.13 7 7-3.13 7-7 7c-1.93 0-3.68-.79-4.94-2.06l-1.42 1.42C8.27 19.99 10.51 21 13 21c4.97 0 9-4.03 9-9s-4.03-9-9-9zm-1 5v5l4.28 2.54.72-1.21-3.5-2.08V8H12z"/>
            </svg>
          </div>
          <h3 class="text-xl font-bold text-on-surface">
            Historial de Solicitudes
          </h3>
          <p class="text-sm text-on-surface/60">
            Consulta el historial de solicitudes aprobadas y rechazadas previamente.
          </p>
        </div>

        <!-- Tarjeta 3: Reportes de Admin -->
        <div class="bg-surface-container-low border border-surface-bright rounded-2xl p-6 hover:border-secondary/50 transition-all space-y-4 group">
          <div class="w-12 h-12 rounded-xl bg-surface-bright flex items-center justify-center text-secondary group-hover:scale-110 transition-transform">
            <svg xmlns="http://www.w3.org/2000/svg" class="w-6 h-6 fill-current" viewBox="0 0 24 24">
              <path d="M19 3H5c-1.1 0-2 .9-2 2v14c0 1.1.9 2 2 2h14c1.1 0 2-.9 2-2V5c0-1.1-.9-2-2-2zM9 17H7v-7h2v7zm4 0h-2V7h2v10zm4 0h-2v-4h2v4z"/>
            </svg>
          </div>
          <h3 class="text-xl font-bold text-on-surface">
            Reportes Estadísticos
          </h3>
          <p class="text-sm text-on-surface/60">
            Analiza métricas de reseñas, interacción y actividad general del sistema.
          </p>
        </div>
      </section>
    </main>
  </div>
</template>
