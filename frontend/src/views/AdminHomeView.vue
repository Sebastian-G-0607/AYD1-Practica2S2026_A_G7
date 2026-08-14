<script setup lang="ts">
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import MainLayout from '@/layouts/MainLayout.vue'

const router = useRouter()
const authStore = useAuthStore()

const irAReportes = () => {
  router.push('/admin/reportes')
}
</script>

<template>
  <MainLayout>
    <!-- Encabezado / Banner de Administración Stitch -->
    <section class="relative overflow-hidden rounded-2xl bg-surface-container-low border border-outline-variant/10 p-6 md:p-8 shadow-xl">
      <div class="absolute -top-32 -left-32 w-96 h-96 bg-primary/10 rounded-full blur-3xl pointer-events-none" />

      <div class="relative z-10 space-y-4">
        <div class="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-secondary-container/20 border border-secondary/30 text-secondary text-xs font-semibold">
          <span class="material-symbols-outlined text-[16px]">admin_panel_settings</span>
          <span>Acceso de Administrador</span>
        </div>

        <h1 class="text-3xl md:text-4xl font-bold font-headline-lg text-on-surface">
          Administración de la plataforma
        </h1>

        <p class="text-on-surface-variant max-w-2xl text-sm md:text-base leading-relaxed">
          Bienvenido, <strong class="text-primary">{{ authStore.user?.nombre || 'Administrador' }}</strong>. Desde este panel puedes supervisar las solicitudes de registro, revisar métricas clave y analizar la interacción de la comunidad en CineCraft.
        </p>
      </div>
    </section>

    <!-- Módulos de Administración Stitch -->
    <section class="grid grid-cols-1 md:grid-cols-3 gap-6">

      <!-- Módulo 1: Solicitudes Pendientes (No implementado) -->
      <article
        class="bg-surface-container rounded-xl p-6 border border-outline-variant/10 shadow-md hover:border-outline-variant/30 transition-all flex flex-col justify-between group"
      >
        <div>
          <div class="w-12 h-12 rounded-xl bg-surface-container-high flex items-center justify-center text-on-surface-variant mb-4 group-hover:scale-110 transition-transform">
            <span class="material-symbols-outlined text-[28px] text-primary">pending_actions</span>
          </div>

          <h3 class="text-xl font-bold font-headline-md text-on-surface mb-2">
            Solicitudes Pendientes
          </h3>

          <p class="text-sm text-on-surface-variant leading-relaxed">
            Revisa y procesa las solicitudes de nuevos usuarios para ingresar a la plataforma.
          </p>
        </div>

        <div class="mt-6 pt-4 border-t border-outline-variant/10 flex items-center justify-between">
          <span class="text-xs text-on-surface-variant opacity-60">Próximamente</span>
          <span class="material-symbols-outlined text-on-surface-variant opacity-40">chevron_right</span>
        </div>
      </article>

      <!-- Módulo 2: Historial de Solicitudes (No implementado) -->
      <article
        class="bg-surface-container rounded-xl p-6 border border-outline-variant/10 shadow-md hover:border-outline-variant/30 transition-all flex flex-col justify-between group"
      >
        <div>
          <div class="w-12 h-12 rounded-xl bg-surface-container-high flex items-center justify-center text-on-surface-variant mb-4 group-hover:scale-110 transition-transform">
            <span class="material-symbols-outlined text-[28px] text-secondary">history</span>
          </div>

          <h3 class="text-xl font-bold font-headline-md text-on-surface mb-2">
            Historial de Solicitudes
          </h3>

          <p class="text-sm text-on-surface-variant leading-relaxed">
            Consulta el historial completo de solicitudes aprobadas o rechazadas previamente.
          </p>
        </div>

        <div class="mt-6 pt-4 border-t border-outline-variant/10 flex items-center justify-between">
          <span class="text-xs text-on-surface-variant opacity-60">Próximamente</span>
          <span class="material-symbols-outlined text-on-surface-variant opacity-40">chevron_right</span>
        </div>
      </article>

      <!-- Módulo 3: Reportes Estadísticos (Implementado Stitch) -->
      <article
        class="bg-surface-container rounded-xl p-6 border border-primary/30 shadow-xl hover:border-primary/60 transition-all flex flex-col justify-between cursor-pointer group hover:-translate-y-1"
        @click="irAReportes"
      >
        <div>
          <div class="w-12 h-12 rounded-xl bg-primary-container/20 flex items-center justify-center text-primary mb-4 group-hover:scale-110 transition-transform">
            <span class="material-symbols-outlined text-[28px]">analytics</span>
          </div>

          <h3 class="text-xl font-bold font-headline-md text-on-surface mb-2 group-hover:text-primary transition-colors">
            Reportes Estadísticos
          </h3>

          <p class="text-sm text-on-surface-variant leading-relaxed">
            Consulta los rankings de usuarios con más reseñas publicadas y reseñas compartidas.
          </p>
        </div>

        <div class="mt-6 pt-4 border-t border-outline-variant/10 flex items-center justify-between">
          <button
            type="button"
            class="px-5 py-2.5 rounded-full bg-primary-container text-on-primary-container font-semibold text-sm shadow-md hover:bg-primary transition-colors flex items-center gap-2"
          >
            <span>Ver reportes</span>
            <span class="material-symbols-outlined text-[18px]">arrow_forward</span>
          </button>
        </div>
      </article>

    </section>
  </MainLayout>
</template>