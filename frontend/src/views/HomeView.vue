<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/auth.store'
import MainLayout from '@/layouts/MainLayout.vue'
import ReseniasList from '@/components/features/resenias/ReseniasList.vue'
import ReseniaForm from '@/components/features/resenias/ReseniaForm.vue'
import ReseniasDestacadas from '@/components/features/resenias/ReseniasDestacadas.vue'

const router = useRouter()
const authStore = useAuthStore()

const destacadasRef = ref<InstanceType<typeof ReseniasDestacadas> | null>(null)
const reseniasListRef = ref<InstanceType<typeof ReseniasList> | null>(null)
const mostrarFormulario = ref(false)

function abrirFormulario() {
  mostrarFormulario.value = true
}

function cerrarFormulario() {
  mostrarFormulario.value = false
}

function handleReseniasListChange() {
  destacadasRef.value?.cargarDestacadas()
}

function handleDestacadasChange() {
  reseniasListRef.value?.cargarResenias()
}

async function handleReseniaCreated() {
  mostrarFormulario.value = false
  await Promise.all([
    reseniasListRef.value?.cargarResenias(),
    destacadasRef.value?.cargarDestacadas()
  ])
}
</script>

<template>
  <MainLayout>
    <!-- Encabezado / Banner Tablero Stitch "Reseñas Destacadas" -->
    <section class="relative overflow-hidden rounded-2xl bg-surface-container-low border border-surface-bright/50 p-6 md:p-8 shadow-xl">
      <div class="absolute -top-32 -left-32 w-96 h-96 bg-primary/10 rounded-full blur-3xl pointer-events-none" />

      <div class="relative z-10 space-y-6">
        <div class="flex flex-col md:flex-row md:items-end justify-between gap-4">
          <div>
            <h1 class="text-3xl md:text-4xl font-bold font-headline-lg text-on-surface">
              Bienvenido de nuevo, {{ authStore.user?.nombre || 'Cinéfilo' }}
            </h1>
            <p class="text-on-surface/70 mt-1 max-w-2xl text-sm md:text-base">
              Las experiencias cinematográficas más impactantes de la comunidad y la gestión de tus opiniones.
            </p>
          </div>

          <button
            type="button"
            class="px-5 py-3 bg-primary-container text-on-primary-container rounded-xl font-bold hover:scale-105 transition-all shadow-lg flex items-center gap-2 shrink-0 self-start md:self-auto"
            @click="abrirFormulario"
          >
            <span class="text-xl leading-none">+</span>
            <span>Nueva Reseña</span>
          </button>
        </div>

        <!-- Bento Grid Destacados (Stitch Tablero) -->
        <div class="grid grid-cols-1 md:grid-cols-12 gap-4 pt-2">
          <!-- Highlight Principal (8 Cols) -->
          <div class="md:col-span-8 group relative rounded-xl overflow-hidden p-6 bg-gradient-to-br from-surface-dim to-surface-container-high border border-surface-bright/60 shadow-lg min-h-[220px] flex flex-col justify-between">
            <div class="flex items-center justify-between">
              <span class="px-3 py-1 bg-surface-container/80 rounded-full text-xs font-semibold text-primary border border-primary/20">
                Ciencia Ficción
              </span>
              <div class="text-secondary flex gap-0.5 text-sm">
                ★★★★★
              </div>
            </div>

            <div class="mt-4 space-y-2">
              <h3 class="text-xl md:text-2xl font-bold font-headline-md text-on-surface group-hover:text-primary transition-colors">
                Neon Genesis: Echoes
              </h3>
              <p class="text-sm text-on-surface/80 line-clamp-2 italic">
                "Un logro visualmente asombroso que redefine la estética ciberpunk moderna al tiempo que ofrece una narrativa central profundamente humana."
              </p>
            </div>
          </div>

          <!-- Highlight Secundario (4 Cols) -->
          <div class="md:col-span-4 group relative rounded-xl overflow-hidden p-6 bg-gradient-to-tr from-surface-container-high to-surface-dim border border-surface-bright/60 shadow-lg min-h-[220px] flex flex-col justify-between">
            <div class="flex items-center justify-between">
              <span class="px-3 py-1 bg-surface-container/80 rounded-full text-xs font-semibold text-secondary border border-secondary/20">
                Cine de Autor
              </span>
              <div class="text-secondary flex gap-0.5 text-sm">
                ★★★★☆
              </div>
            </div>

            <div class="mt-4 space-y-2">
              <h3 class="text-lg font-bold font-headline-md text-on-surface">
                Flora Anomalous
              </h3>
              <p class="text-xs text-on-surface/75 line-clamp-2 italic">
                "Construcción de mundos en su máxima expresión, inmersiva y aterradora."
              </p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <ReseniasDestacadas
      ref="destacadasRef"
      @cambioEstado="handleDestacadasChange"
    />

    <!-- Sección Mis Reseñas (Contenido actual de HomeView) -->
    <section class="bg-surface-container-low border border-surface-bright/50 rounded-2xl p-6 md:p-8 shadow-xl">
      <ReseniasList
        ref="reseniasListRef"
        @cambioEstado="handleReseniasListChange"
      />
    </section>

    <!-- Botón Flotante Nueva Reseña (Stitch Tablero FAB) -->
    <button
      type="button"
      class="fixed bottom-8 right-8 z-40 bg-primary-container text-on-primary-container
             px-5 py-4 rounded-full shadow-2xl font-bold
             flex items-center gap-2
             hover:scale-105 transition-transform"
      @click="abrirFormulario"
    >
      <span class="text-2xl leading-none">+</span>
      <span class="hidden sm:inline">Nueva Reseña</span>
    </button>

    <!-- Modal Nueva Reseña -->
    <div
      v-if="mostrarFormulario"
      class="fixed inset-0 z-50 flex items-center justify-center p-4"
    >
      <!-- Fondo -->
      <div
        class="absolute inset-0 bg-black/70 backdrop-blur-sm"
        @click="cerrarFormulario"
      />

      <!-- Contenido -->
      <div class="relative z-10 w-full max-w-2xl max-h-[90vh] overflow-y-auto">
        <!-- Botón cerrar -->
        <button
          type="button"
          class="absolute top-4 right-5 z-20 text-on-surface/60 hover:text-on-surface text-3xl"
          @click="cerrarFormulario"
        >
          ×
        </button>

        <ReseniaForm @created="handleReseniaCreated" />
      </div>
    </div>
  </MainLayout>
</template>