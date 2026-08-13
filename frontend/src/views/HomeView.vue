<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'

const router = useRouter()
const authStore = useAuthStore()

const irAReportes = () => {
  router.push('/admin/reportes')
}

const cerrarSesion = () => {
  authStore.logout()
  router.push('/login')
}
</script>

<template>
  <main class="min-h-screen bg-surface text-on-surface">
    <div class="flex min-h-screen">

      <!-- Sidebar -->
      <aside
        class="flex w-72 flex-col border-r border-white/5 bg-surface-container-lowest"
      >
        <!-- Logo -->
        <div class="flex items-center gap-3 px-8 py-8">
          <div
            class="flex h-10 w-10 items-center justify-center rounded-full border border-primary bg-primary/10"
          >
            <span class="text-xl text-primary">🎬</span>
          </div>

          <span class="text-2xl font-bold tracking-tight">
            CineCraft
          </span>
        </div>

        <!-- Navegación -->
        <nav class="flex-1 px-4">

          <p
            class="mb-3 px-4 text-xs font-semibold uppercase tracking-widest text-on-surface-variant opacity-60"
          >
            Menú Principal
          </p>

          <button
            class="mb-2 flex w-full items-center gap-3 rounded-lg bg-surface-container-high px-4 py-3 text-left text-on-surface transition hover:bg-surface-bright"
          >
            <span>🏠</span>
            <span>Panel</span>
          </button>

          <div class="mt-8">
            <p
              class="mb-3 px-4 text-xs font-semibold uppercase tracking-widest text-on-surface-variant opacity-60"
            >
              Administración
            </p>

            <button
              class="mb-2 flex w-full items-center gap-3 rounded-lg px-4 py-3 text-left text-on-surface-variant transition hover:bg-surface-container-high hover:text-on-surface"
            >
              <span>⏳</span>
              <span>Solicitudes Pendientes</span>
            </button>

            <button
              class="mb-2 flex w-full items-center gap-3 rounded-lg px-4 py-3 text-left text-on-surface-variant transition hover:bg-surface-container-high hover:text-on-surface"
            >
              <span>🕒</span>
              <span>Historial de Solicitudes</span>
            </button>

            <button
              @click="irAReportes"
              class="flex w-full items-center gap-3 rounded-lg px-4 py-3 text-left text-on-surface-variant transition hover:bg-primary-container hover:text-white"
            >
              <span>📊</span>
              <span>Reportes de Administrador</span>
            </button>
          </div>
        </nav>

        <!-- Cerrar sesión -->
        <div class="border-t border-white/5 p-4">
          <button
            @click="cerrarSesion"
            class="flex w-full items-center justify-center gap-2 rounded-lg bg-surface-container-high px-4 py-3 text-sm font-semibold text-on-surface transition hover:bg-surface-bright"
          >
            Cerrar sesión
          </button>
        </div>
      </aside>

      <!-- Contenido -->
      <section class="flex-1 p-8">

        <!-- Header -->
        <header
          class="mb-8 flex items-center justify-between rounded-xl border border-white/5 bg-surface-container-low p-6"
        >
          <div>
            <p class="text-sm text-on-surface-variant">
              Bienvenido
            </p>

            <h1 class="mt-1 text-3xl font-bold">
              Panel de Administración
            </h1>
          </div>

          <div class="text-right">
            <p class="font-semibold text-primary">
              {{ authStore.user?.nombre ?? 'Administrador' }}
            </p>

            <p class="text-sm text-on-surface-variant">
              {{ authStore.user?.email }}
            </p>
          </div>
        </header>

        <!-- Bienvenida -->
        <section
          class="relative overflow-hidden rounded-2xl bg-surface-container-high p-8"
        >
          <div
            class="absolute -right-20 -top-20 h-64 w-64 rounded-full bg-primary-container opacity-20 blur-3xl"
          ></div>

          <div class="relative z-10">
            <p
              class="mb-2 text-xs font-semibold uppercase tracking-widest text-primary"
            >
              CineCraft
            </p>

            <h2 class="text-3xl font-bold">
              Administración de la plataforma
            </h2>

            <p class="mt-3 max-w-2xl text-on-surface-variant">
              Desde este panel puedes acceder a las herramientas administrativas
              disponibles dentro de CineCraft.
            </p>
          </div>
        </section>

        <!-- Cards -->
        <section class="mt-8 grid grid-cols-1 gap-6 md:grid-cols-3">

          <article
            class="rounded-xl border border-white/5 bg-surface-container p-6"
          >
            <div class="mb-4 text-3xl">
              ⏳
            </div>

            <h3 class="text-lg font-semibold">
              Solicitudes Pendientes
            </h3>

            <p class="mt-2 text-sm text-on-surface-variant">
              Revisa las solicitudes de nuevos usuarios.
            </p>
          </article>

          <article
            class="rounded-xl border border-white/5 bg-surface-container p-6"
          >
            <div class="mb-4 text-3xl">
              🕒
            </div>

            <h3 class="text-lg font-semibold">
              Historial
            </h3>

            <p class="mt-2 text-sm text-on-surface-variant">
              Consulta el historial de solicitudes procesadas.
            </p>
          </article>

          <article
            @click="irAReportes"
            class="cursor-pointer rounded-xl border border-primary/20 bg-surface-container p-6 transition duration-300 hover:-translate-y-1 hover:border-primary/50 hover:bg-surface-container-high"
          >
            <div class="mb-4 text-3xl">
              📊
            </div>

            <h3 class="text-lg font-semibold">
              Reportes
            </h3>

            <p class="mt-2 text-sm text-on-surface-variant">
              Consulta los usuarios con más reseñas y reseñas compartidas.
            </p>

            <button
              class="mt-5 rounded-full bg-primary-container px-5 py-2 text-sm font-semibold text-white transition hover:opacity-90"
            >
              Ver reportes
            </button>
          </article>

        </section>
      </section>
    </div>
  </main>
</template>