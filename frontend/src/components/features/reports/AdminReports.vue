<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import {
  reportesService,
  type UsuarioTopResenias,
  type UsuarioTopCompartidos,
} from '@/services/reportes.service'

const router = useRouter()

const topResenias = ref<UsuarioTopResenias[]>([])
const topCompartidos = ref<UsuarioTopCompartidos[]>([])

const cargando = ref(false)
const error = ref('')

const volverAlPanel = () => {
  router.push('/home')
}

const cargarReportes = async () => {
  cargando.value = true
  error.value = ''

  try {
  const [resenias, compartidos] = await Promise.all([
    reportesService.obtenerTopResenias(),
    reportesService.obtenerTopCompartidos(),
  ])

  topResenias.value = resenias
  topCompartidos.value = compartidos
} catch (err) {
  console.error('Error al cargar reportes:', err)
  error.value = 'No se pudieron cargar los reportes.'
} finally {
  cargando.value = false
}

const maxResenias = computed(() => {
  if (topResenias.value.length === 0) return 1

  return Math.max(
    ...topResenias.value.map((usuario) => usuario.cantidadResenias)
  )
})

const maxCompartidos = computed(() => {
  if (topCompartidos.value.length === 0) return 1

  return Math.max(
    ...topCompartidos.value.map((usuario) => usuario.cantidadCompartidos)
  )
})

const porcentajeResenias = (cantidad: number) => {
  return `${(cantidad / maxResenias.value) * 100}%`
}

const porcentajeCompartidos = (cantidad: number) => {
  return `${(cantidad / maxCompartidos.value) * 100}%`
}

const obtenerIniciales = (nombre: string) => {
  return nombre
    .split(' ')
    .filter(Boolean)
    .map((palabra) => palabra.charAt(0))
    .join('')
    .substring(0, 2)
    .toUpperCase()
}

onMounted(() => {
  cargarReportes()
})
</script>

<template>
  <section class="min-h-screen bg-surface p-8 text-on-surface">
    <div class="mx-auto max-w-7xl">

      <!-- Regresar al dashboard -->
      <button
        @click="volverAlPanel"
        class="mb-5 flex items-center gap-2 text-sm font-semibold text-on-surface-variant transition hover:text-primary"
      >
        <span class="text-xl">←</span>
        <span>Volver al panel</span>
      </button>

      <!-- Encabezado -->
      <div
        class="relative overflow-hidden rounded-xl bg-surface-container-high p-8 shadow-lg"
      >
        <div
          class="absolute -right-20 -top-20 h-64 w-64 rounded-full bg-primary-container opacity-20 blur-3xl"
        ></div>

        <div class="relative z-10">
          <p
            class="mb-2 text-sm font-semibold uppercase tracking-widest text-primary"
          >
            Administración
          </p>

          <h1 class="text-4xl font-bold text-on-surface">
            Análisis de la Plataforma
          </h1>

          <p class="mt-3 max-w-2xl text-on-surface-variant">
            Consulta los usuarios con mayor cantidad de reseñas y los usuarios
            cuyas reseñas han sido compartidas más veces dentro de CineCraft.
          </p>
        </div>
      </div>

      <!-- Cargando -->
      <div
        v-if="cargando"
        class="mt-8 rounded-xl border border-white/5 bg-surface-container p-8 text-center"
      >
        <div
          class="mx-auto mb-4 h-8 w-8 animate-spin rounded-full border-4 border-surface-bright border-t-primary"
        ></div>

        <p class="text-on-surface-variant">
          Cargando reportes...
        </p>
      </div>

      <!-- Error -->
      <div
        v-else-if="error"
        class="mt-8 rounded-xl border border-red-500/20 bg-red-500/10 p-6 text-center"
      >
        <p class="font-semibold text-red-300">
          {{ error }}
        </p>

        <button
          @click="cargarReportes"
          class="mt-4 rounded-full bg-primary-container px-5 py-2 text-sm font-semibold text-white transition hover:opacity-90"
        >
          Intentar nuevamente
        </button>
      </div>

      <!-- Reportes -->
      <div
        v-else
        class="mt-8 grid grid-cols-1 gap-6 xl:grid-cols-2"
      >

        <!-- Top reseñas -->
        <article
          class="rounded-xl border border-white/5 bg-surface-container p-6 shadow-xl md:p-8"
        >
          <div class="mb-7">
            <p
              class="text-xs font-semibold uppercase tracking-[0.2em] text-primary"
            >
              Ranking
            </p>

            <h2 class="mt-2 text-2xl font-bold text-on-surface">
              Top 5 usuarios con más reseñas
            </h2>

            <p class="mt-1 text-sm text-on-surface-variant">
              Usuarios con mayor cantidad de reseñas publicadas.
            </p>
          </div>

          <div
            v-if="topResenias.length === 0"
            class="rounded-lg bg-surface-container-high p-5 text-center text-sm text-on-surface-variant"
          >
            No hay datos disponibles.
          </div>

          <div
            v-else
            class="space-y-6"
          >
            <div
              v-for="(usuario, index) in topResenias"
              :key="usuario.idUsuario"
              class="group"
            >
              <div class="mb-2 flex items-center justify-between gap-4">
                <div class="flex min-w-0 items-center gap-3">
                  <span
                    class="w-6 flex-shrink-0 text-xs font-semibold text-on-surface-variant"
                  >
                    {{ String(index + 1).padStart(2, '0') }}
                  </span>

                  <div
                    class="flex h-9 w-9 flex-shrink-0 items-center justify-center rounded-full bg-primary/10 text-xs font-bold text-primary"
                  >
                    {{ obtenerIniciales(usuario.nombre) }}
                  </div>

                  <span
                    class="truncate font-semibold text-on-surface"
                  >
                    {{ usuario.nombre }}
                  </span>
                </div>

                <span
                  class="flex-shrink-0 text-sm font-bold text-primary"
                >
                  {{ usuario.cantidadResenias }} reseñas
                </span>
              </div>

              <div
                class="h-3 overflow-hidden rounded-full bg-surface-container-lowest shadow-inner"
              >
                <div
                  class="h-full rounded-full bg-primary-container transition-all duration-700 group-hover:brightness-110"
                  :style="{
                    width: porcentajeResenias(usuario.cantidadResenias),
                  }"
                ></div>
              </div>
            </div>
          </div>
        </article>

        <!-- Top compartidos -->
        <article
          class="rounded-xl border border-white/5 bg-surface-container p-6 shadow-xl md:p-8"
        >
          <div class="mb-7">
            <p
              class="text-xs font-semibold uppercase tracking-[0.2em] text-yellow-400"
            >
              Ranking
            </p>

            <h2 class="mt-2 text-2xl font-bold text-on-surface">
              Top 5 usuarios con más reseñas compartidas
            </h2>

            <p class="mt-1 text-sm text-on-surface-variant">
              Usuarios cuyas reseñas han sido compartidas más veces.
            </p>
          </div>

          <div
            v-if="topCompartidos.length === 0"
            class="rounded-lg bg-surface-container-high p-5 text-center text-sm text-on-surface-variant"
          >
            No hay datos disponibles.
          </div>

          <div
            v-else
            class="space-y-6"
          >
            <div
              v-for="(usuario, index) in topCompartidos"
              :key="usuario.idUsuario"
              class="group"
            >
              <div class="mb-2 flex items-center justify-between gap-4">
                <div class="flex min-w-0 items-center gap-3">
                  <span
                    class="w-6 flex-shrink-0 text-xs font-semibold text-on-surface-variant"
                  >
                    {{ String(index + 1).padStart(2, '0') }}
                  </span>

                  <div
                    class="flex h-9 w-9 flex-shrink-0 items-center justify-center rounded-full bg-yellow-400/10 text-xs font-bold text-yellow-400"
                  >
                    {{ obtenerIniciales(usuario.nombre) }}
                  </div>

                  <span
                    class="truncate font-semibold text-on-surface"
                  >
                    {{ usuario.nombre }}
                  </span>
                </div>

                <span
                  class="flex-shrink-0 text-sm font-bold text-yellow-400"
                >
                  {{ usuario.cantidadCompartidos }} compartidos
                </span>
              </div>

              <div
                class="h-3 overflow-hidden rounded-full bg-surface-container-lowest shadow-inner"
              >
                <div
                  class="h-full rounded-full bg-yellow-400 transition-all duration-700 group-hover:brightness-110"
                  :style="{
                    width: porcentajeCompartidos(usuario.cantidadCompartidos),
                  }"
                ></div>
              </div>
            </div>
          </div>
        </article>

      </div>
    </div>
  </section>
</template>