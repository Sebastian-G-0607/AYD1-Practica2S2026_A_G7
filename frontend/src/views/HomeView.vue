<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth.store'
import { useRouter } from 'vue-router'
import ReseniasList from '@/components/features/resenias/ReseniasList.vue'
import ReseniaForm from '@/components/features/resenias/ReseniaForm.vue'

const authStore = useAuthStore()
const router = useRouter()

const reseniasKey = ref(0)
const mostrarFormulario = ref(false)

function abrirFormulario() {
  mostrarFormulario.value = true
}

function cerrarFormulario() {
  mostrarFormulario.value = false
}

function handleReseniaCreated() {
  reseniasKey.value++
  mostrarFormulario.value = false
}

function handleLogout() {
  authStore.logout()
  router.push({ name: 'Login' })
}
</script>

<template>
  <div class="min-h-screen bg-surface text-on-surface">
    <!-- Encabezado -->
    <header
      class="sticky top-0 z-30 bg-surface/90 backdrop-blur-xl border-b border-surface-bright"
    >
      <div
        class="max-w-7xl mx-auto px-6 py-4 flex items-center justify-between"
      >
        <div>
          <h1 class="text-2xl font-bold font-headline-lg">
            CineCraft
          </h1>

          <p
            v-if="authStore.user"
            class="text-sm text-on-surface/60 mt-1"
          >
            Bienvenido,
            <span class="font-semibold text-primary">
              {{ authStore.user.nombre }}
            </span>
          </p>
        </div>

        <button
          type="button"
          class="px-4 py-2 bg-surface-bright text-on-surface rounded-lg hover:opacity-80 transition-opacity text-sm font-semibold"
          @click="handleLogout"
        >
          Cerrar sesión
        </button>
      </div>
    </header>

    <!-- Contenido -->
    <main class="max-w-7xl mx-auto px-6 py-10">
      <div
        class="bg-surface-container-low border border-surface-bright rounded-2xl p-8"
      >
        <ReseniasList :key="reseniasKey" />
      </div>
    </main>

    <!-- Botón flotante Nueva Reseña -->
    <button
      type="button"
      class="fixed bottom-8 right-8 z-40 bg-primary-container text-on-primary-container
             px-5 py-4 rounded-full shadow-2xl font-bold
             flex items-center gap-2
             hover:scale-105 transition-transform"
      @click="abrirFormulario"
    >
      <span class="text-2xl leading-none">
        +
      </span>

      <span>
        Nueva Reseña
      </span>
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
      ></div>

      <!-- Contenido -->
      <div
        class="relative z-10 w-full max-w-2xl max-h-[90vh] overflow-y-auto"
      >
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
  </div>
</template>