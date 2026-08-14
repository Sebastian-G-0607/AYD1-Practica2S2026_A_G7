<script setup lang="ts">
import { onMounted, ref } from 'vue'
import {
  getResenias,
  updateResenia,
  deleteResenia,
  toggleArchivarResenia,
  toggleDestacar
  
} from '@/services/resenia.service'
import type { Resenia } from '@/types/resenia.types'

const resenias = ref<Resenia[]>([])
const loading = ref(true)
const error = ref('')
const emit = defineEmits(['cambioEstado'])

// =========================
// EDICIÓN
// =========================

const reseniaEditandoId = ref<number | null>(null)

const tituloEditado = ref('')
const calificacionEditada = ref(5)
const comentarioEditado = ref('')
const etiquetaEditada = ref('')
const etiquetaOriginal = ref('')
const etiquetaIdOriginal = ref<number | null>(null)

const guardando = ref(false)
const errorEdicion = ref('')

// =========================
// ELIMINACIÓN
// =========================

const reseniaAEliminar = ref<Resenia | null>(null)
const eliminando = ref(false)
const errorEliminacion = ref('')

// =========================
// CARGAR RESEÑAS
// =========================

async function cargarResenias() {
  try {
    loading.value = true
    error.value = ''

    resenias.value = await getResenias()
  } catch (err) {
    console.error(err)
    error.value = 'No fue posible cargar las reseñas.'
  } finally {
    loading.value = false
  }
}

// =========================
// ARCHIVAR RESEÑA
// =========================

async function archivar(id: number) {
  try {
    await toggleArchivarResenia(id)
    await cargarResenias()
  } catch (err) {
    console.error(err)
    error.value = 'No fue posible archivar la reseña.'
  }
}

// =========================
// DESTACAR RESEÑA
// =========================
async function destacar(id: number) {
  try {
    await toggleDestacar(id)
    await cargarResenias()
    emit('cambioEstado') // Avisa al componente padre que algo cambió
  } catch (err) {
    console.error(err)
    error.value = 'No fue posible destacar la reseña.'
  }
}

// =========================
// EDITAR RESEÑA
// =========================

function iniciarEdicion(resenia: Resenia) {
  reseniaEditandoId.value = resenia.id

  tituloEditado.value = resenia.tituloPelicula
  calificacionEditada.value = resenia.calificacion
  comentarioEditado.value = resenia.comentario ?? ''

  etiquetaEditada.value = resenia.etiqueta
  etiquetaOriginal.value = resenia.etiqueta
  etiquetaIdOriginal.value = resenia.etiquetaId

  errorEdicion.value = ''
}

function cancelarEdicion() {
  reseniaEditandoId.value = null
  errorEdicion.value = ''
}

async function guardarEdicion(id: number) {
  errorEdicion.value = ''

  if (!tituloEditado.value.trim()) {
    errorEdicion.value = 'El título de la película es obligatorio.'
    return
  }

  if (!etiquetaEditada.value.trim()) {
    errorEdicion.value = 'La etiqueta es obligatoria.'
    return
  }

  try {
    guardando.value = true

    const etiquetaCambio =
      etiquetaEditada.value.trim() !== etiquetaOriginal.value.trim()

    await updateResenia(id, {
      tituloPelicula: tituloEditado.value.trim(),
      calificacion: calificacionEditada.value,
      comentario: comentarioEditado.value.trim() || null,

      etiquetaId: etiquetaCambio
        ? null
        : etiquetaIdOriginal.value,

      nuevaEtiqueta: etiquetaCambio
        ? etiquetaEditada.value.trim()
        : null,
    })

    reseniaEditandoId.value = null

    await cargarResenias()
  } catch (err) {
    console.error(err)
    errorEdicion.value = 'No fue posible actualizar la reseña.'
  } finally {
    guardando.value = false
  }
}

// =========================
// ELIMINAR RESEÑA
// =========================

function abrirModalEliminar(resenia: Resenia) {
  reseniaAEliminar.value = resenia
  errorEliminacion.value = ''
}

function cerrarModalEliminar() {
  if (eliminando.value) {
    return
  }

  reseniaAEliminar.value = null
  errorEliminacion.value = ''
}

async function confirmarEliminar() {
  if (!reseniaAEliminar.value) {
    return
  }

  try {
    eliminando.value = true
    errorEliminacion.value = ''

    await deleteResenia(reseniaAEliminar.value.id)

    reseniaAEliminar.value = null

    await cargarResenias()
  } catch (err) {
    console.error(err)
    errorEliminacion.value = 'No fue posible eliminar la reseña.'
  } finally {
    eliminando.value = false
  }
}

onMounted(() => {
  cargarResenias()
})
</script>

<template>
  <section class="space-y-5">
    <!-- Encabezado -->
    <div>
      <h2 class="text-2xl font-bold">
        Mis Reseñas
      </h2>

      <p class="text-sm text-on-surface/60 mt-1">
        Tus opiniones y calificaciones de películas.
      </p>
    </div>

    <!-- Cargando -->
    <p v-if="loading">
      Cargando reseñas...
    </p>

    <!-- Error -->
    <p
      v-else-if="error"
      class="text-red-400"
    >
      {{ error }}
    </p>

    <!-- Sin reseñas -->
    <p
      v-else-if="resenias.length === 0"
      class="text-on-surface/60"
    >
      Aún no tienes reseñas.
    </p>

    <!-- Lista -->
    <div
      v-else
      class="divide-y divide-surface-bright"
    >
      <article
        v-for="resenia in resenias"
        :key="resenia.id"
        class="py-6 first:pt-0 last:pb-0 space-y-4"
      >
        <!-- ===================== -->
        <!-- VISTA NORMAL -->
        <!-- ===================== -->

        <template v-if="reseniaEditandoId !== resenia.id">
          <div class="flex items-start justify-between gap-4">
            <div>
              <h3 class="text-xl font-bold">
                {{ resenia.tituloPelicula }}
              </h3>

              <!-- Etiqueta -->
              <span
                class="inline-block mt-2 px-3 py-1 rounded-full bg-surface-bright text-xs font-semibold"
              >
                {{ resenia.etiqueta }}
              </span>

              <!-- Acciones -->
              <div class="mt-4 flex items-center gap-5">
                <button
                  type="button"
                  class="text-sm font-semibold text-primary hover:underline"
                  @click="iniciarEdicion(resenia)"
                >
                  Editar reseña
                </button>

                <button
                  type="button"
                  class="text-sm font-semibold transition-colors flex items-center gap-1"
                  :class="resenia.destacada ? 'text-yellow-400 hover:text-yellow-300' : 'text-on-surface/50 hover:text-on-surface'"
                  @click="destacar(resenia.id)"
                >
                  {{ resenia.destacada ? '★ Quitar destacado' : '☆ Destacar' }}
                </button>

                <button
                  type="button"
                  class="text-sm font-semibold text-primary hover:underline"
                  @click="iniciarEdicion(resenia)"
                >
                  Editar reseña
                </button>

                <button
                  type="button"
                  class="text-sm font-semibold text-amber-400 hover:text-amber-300 hover:underline transition-colors"
                  @click="archivar(resenia.id)"
                >
                  Archivar reseña
                  </button>


                <button
                  type="button"
                  class="text-sm font-semibold text-red-400 hover:text-red-300 hover:underline transition-colors"
                  @click="abrirModalEliminar(resenia)"
                >
                  Eliminar reseña
                </button>
              </div>
            </div>

            <!-- Estrellas -->
            <div class="text-lg text-yellow-400 whitespace-nowrap">
              {{ '★'.repeat(resenia.calificacion) }}

              <span class="text-on-surface/20">
                {{ '★'.repeat(5 - resenia.calificacion) }}
              </span>
            </div>
          </div>

          <!-- Comentario -->
          <p
            v-if="resenia.comentario"
            class="text-on-surface/75 leading-relaxed"
          >
            {{ resenia.comentario }}
          </p>

          <p
            v-else
            class="text-sm italic text-on-surface/40"
          >
            Sin comentario.
          </p>
        </template>

        <!-- ===================== -->
        <!-- MODO EDICIÓN -->
        <!-- ===================== -->

        <template v-else>
          <div
            class="bg-surface-container-low border border-surface-bright rounded-xl p-5 space-y-4"
          >
            <div>
              <h3 class="text-lg font-bold">
                Editar reseña
              </h3>

              <p class="text-sm text-on-surface/60">
                Modifica los datos de tu reseña.
              </p>
            </div>

            <!-- Película -->
            <div class="space-y-2">
              <label class="text-sm font-semibold">
                Película
              </label>

              <input
                v-model="tituloEditado"
                type="text"
                class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
              />
            </div>

            <!-- Calificación -->
            <div class="space-y-2">
              <label class="text-sm font-semibold">
                Calificación
              </label>

              <select
                v-model="calificacionEditada"
                class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
              >
                <option :value="5">★★★★★</option>
                <option :value="4">★★★★</option>
                <option :value="3">★★★</option>
                <option :value="2">★★</option>
                <option :value="1">★</option>
              </select>
            </div>

            <!-- Comentario -->
            <div class="space-y-2">
              <label class="text-sm font-semibold">
                Comentario
              </label>

              <textarea
                v-model="comentarioEditado"
                rows="4"
                class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary resize-none"
              ></textarea>
            </div>

            <!-- Etiqueta -->
            <div class="space-y-2">
              <label class="text-sm font-semibold">
                Etiqueta
              </label>

              <input
                v-model="etiquetaEditada"
                type="text"
                class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
              />
            </div>

            <!-- Error edición -->
            <p
              v-if="errorEdicion"
              class="text-sm text-red-400"
            >
              {{ errorEdicion }}
            </p>

            <!-- Botones edición -->
            <div class="flex gap-3">
              <button
                type="button"
                :disabled="guardando"
                class="px-5 py-2 rounded-lg bg-primary text-on-primary font-semibold hover:opacity-90 disabled:opacity-50"
                @click="guardarEdicion(resenia.id)"
              >
                {{ guardando ? 'Guardando...' : 'Guardar cambios' }}
              </button>

              <button
                type="button"
                :disabled="guardando"
                class="px-5 py-2 rounded-lg bg-surface-bright text-on-surface font-semibold hover:opacity-80 disabled:opacity-50"
                @click="cancelarEdicion"
              >
                Cancelar
              </button>
            </div>
          </div>
        </template>
      </article>
    </div>

    <!-- ================================= -->
    <!-- MODAL DE CONFIRMACIÓN DE DELETE -->
    <!-- ================================= -->

    <div
      v-if="reseniaAEliminar"
      class="fixed inset-0 z-50 flex items-center justify-center p-4"
    >
      <!-- Fondo oscuro -->
      <div
        class="absolute inset-0 bg-black/70 backdrop-blur-sm"
        @click="cerrarModalEliminar"
      ></div>

      <!-- Modal -->
      <div
        class="relative z-10 w-full max-w-md bg-surface-container-low border border-surface-bright rounded-2xl shadow-2xl overflow-hidden"
      >
        <!-- Encabezado -->
        <div
          class="flex items-center justify-between px-6 py-5 border-b border-surface-bright"
        >
          <div class="flex items-center gap-3">
            <!-- Icono -->
            <div
              class="w-10 h-10 flex items-center justify-center rounded-full bg-red-500/10 text-red-400"
            >
              <span class="text-xl">
                !
              </span>
            </div>

            <div>
              <h3 class="text-lg font-bold">
                Eliminar reseña
              </h3>

              <p class="text-xs text-on-surface/50">
                Confirmación requerida
              </p>
            </div>
          </div>

          <!-- Cerrar -->
          <button
            type="button"
            :disabled="eliminando"
            class="text-on-surface/50 hover:text-on-surface text-2xl leading-none transition-colors disabled:opacity-50"
            @click="cerrarModalEliminar"
          >
            ×
          </button>
        </div>

        <!-- Contenido -->
        <div class="px-6 py-6 space-y-4">
          <p class="text-on-surface/80 leading-relaxed">
            ¿Estás seguro de que deseas eliminar la reseña de
            <span class="font-bold text-on-surface">
              "{{ reseniaAEliminar.tituloPelicula }}"
            </span>?
          </p>

          <div
            class="rounded-xl border border-red-500/20 bg-red-500/5 px-4 py-3"
          >
            <p class="text-sm text-red-300">
              Esta acción eliminará la reseña de tu lista.
            </p>
          </div>

          <p
            v-if="errorEliminacion"
            class="text-sm text-red-400"
          >
            {{ errorEliminacion }}
          </p>
        </div>

        <!-- Botones -->
        <div
          class="flex justify-end gap-3 px-6 py-5 border-t border-surface-bright"
        >
          <button
            type="button"
            :disabled="eliminando"
            class="px-5 py-2.5 rounded-lg bg-surface-bright text-on-surface font-semibold hover:opacity-80 transition-opacity disabled:opacity-50"
            @click="cerrarModalEliminar"
          >
            Cancelar
          </button>

          <button
            type="button"
            :disabled="eliminando"
            class="px-5 py-2.5 rounded-lg bg-red-600 text-white font-semibold hover:bg-red-500 transition-colors disabled:opacity-50"
            @click="confirmarEliminar"
          >
            {{ eliminando ? 'Eliminando...' : 'Eliminar reseña' }}
          </button>
        </div>
      </div>
    </div>
  </section>
</template>