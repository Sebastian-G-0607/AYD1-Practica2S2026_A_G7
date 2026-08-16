

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getReseniasArchivadas, toggleArchivar, deleteResenia } from '@/services/resenia.service'
import type { Resenia } from '@/types/resenia.types'

const reseniasArchivadas = ref<Resenia[]>([])
const loading = ref(true)
const error = ref('')

const reseniaAEliminar = ref<Resenia | null>(null)
const eliminando = ref(false)
const errorEliminacion = ref('')

async function cargarArchivadas() {
  try {
    loading.value = true
    error.value = ''
    reseniasArchivadas.value = await getReseniasArchivadas()
  } catch (err) {
    console.error(err)
    error.value = 'No fue posible cargar las reseñas archivadas.'
  } finally {
    loading.value = false
  }
}

async function handleDesarchivar(id: number) {
  try {
    await toggleArchivar(id)
    await cargarArchivadas()
  } catch (err) {
    console.error('Error al desarchivar la reseña:', err)
  }
}

function abrirModalEliminar(resenia: Resenia) {
  reseniaAEliminar.value = resenia
  errorEliminacion.value = ''
}

function cerrarModalEliminar() {
  if (eliminando.value) return
  reseniaAEliminar.value = null
  errorEliminacion.value = ''
}

async function confirmarEliminar() {
  if (!reseniaAEliminar.value) return

  try {
    eliminando.value = true
    errorEliminacion.value = ''
    await deleteResenia(reseniaAEliminar.value.id)
    reseniaAEliminar.value = null
    await cargarArchivadas()
  } catch (err) {
    console.error(err)
    errorEliminacion.value = 'No fue posible eliminar la reseña archivada.'
  } finally {
    eliminando.value = false
  }
}

onMounted(() => {
  cargarArchivadas()
})
</script>

<template>
  <section class="space-y-6">
    <div>
      <h2 class="text-2xl font-bold font-headline-md text-on-surface">
        Reseñas Archivadas
      </h2>
      <p class="text-sm text-on-surface/60 mt-1">
        Consulta y gestiona tus opiniones archivadas.
      </p>
    </div>

    <!-- Cargando -->
    <p v-if="loading" class="text-sm text-on-surface/60">
      Cargando reseñas archivadas...
    </p>

    <!-- Error -->
    <p v-else-if="error" class="text-sm text-red-400">
      {{ error }}
    </p>

    <!-- Estado vacío -->
    <div
      v-else-if="reseniasArchivadas.length === 0"
      class="p-8 rounded-2xl bg-surface-container-low border border-surface-bright text-center space-y-2"
    >
      <span class="material-symbols-outlined text-4xl text-on-surface/40">inventory_2</span>
      <p class="text-on-surface/60 font-medium">
        No tienes reseñas archivadas.
      </p>
    </div>

    <!-- Lista de Archivadas -->
    <div v-else class="divide-y divide-surface-bright">
      <article
        v-for="resenia in reseniasArchivadas"
        :key="resenia.id"
        class="py-6 first:pt-0 last:pb-0 space-y-4"
      >
        <div class="flex items-start justify-between gap-4">
          <div>
            <h3 class="text-xl font-bold text-on-surface">
              {{ resenia.tituloPelicula }}
            </h3>

            <span class="inline-block mt-2 px-3 py-1 rounded-full bg-surface-bright text-xs font-semibold text-primary">
              {{ resenia.etiqueta }}
            </span>

            <div class="mt-4 flex items-center gap-5">
              <button
                type="button"
                class="text-sm font-semibold text-primary hover:underline flex items-center gap-1"
                @click="handleDesarchivar(resenia.id)"
              >
                <span class="material-symbols-outlined text-sm">unarchive</span>
                Desarchivar
              </button>

              <button
                type="button"
                class="text-sm font-semibold text-red-400 hover:text-red-300 hover:underline transition-colors flex items-center gap-1"
                @click="abrirModalEliminar(resenia)"
              >
                <span class="material-symbols-outlined text-sm">delete</span>
                Eliminar definitivamente
              </button>
            </div>
          </div>

          <div class="text-lg text-yellow-400 whitespace-nowrap">
            {{ '★'.repeat(resenia.calificacion) }}
            <span class="text-on-surface/20">{{ '★'.repeat(5 - resenia.calificacion) }}</span>
          </div>
        </div>

        <p v-if="resenia.comentario" class="text-on-surface/75 leading-relaxed">
          {{ resenia.comentario }}
        </p>

        <p v-else class="text-sm italic text-on-surface/40">
          Sin comentario.
        </p>
      </article>
    </div>

    <!-- Modal Eliminar -->
    <div
      v-if="reseniaAEliminar"
      class="fixed inset-0 z-50 flex items-center justify-center p-4"
    >
      <div
        class="absolute inset-0 bg-black/70 backdrop-blur-sm"
        @click="cerrarModalEliminar"
      />

      <div class="relative z-10 w-full max-w-md bg-surface-container-low border border-surface-bright rounded-2xl shadow-2xl overflow-hidden">
        <div class="flex items-center justify-between px-6 py-5 border-b border-surface-bright">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 flex items-center justify-center rounded-full bg-red-500/10 text-red-400">
              <span class="material-symbols-outlined">warning</span>
            </div>

            <div>
              <h3 class="text-lg font-bold">Eliminar reseña archivada</h3>
              <p class="text-xs text-on-surface/50">Confirmación requerida</p>
            </div>
          </div>

          <button
            type="button"
            :disabled="eliminando"
            class="text-on-surface/50 hover:text-on-surface text-2xl leading-none transition-colors"
            @click="cerrarModalEliminar"
          >
            ×
          </button>
        </div>

        <div class="px-6 py-6 space-y-4">
          <p class="text-on-surface/80 leading-relaxed">
            ¿Estás seguro de que deseas eliminar permanentemente la reseña archivada de
            <span class="font-bold text-on-surface">"{{ reseniaAEliminar.tituloPelicula }}"</span>?
          </p>

          <p v-if="errorEliminacion" class="text-sm text-red-400">
            {{ errorEliminacion }}
          </p>
        </div>

        <div class="flex justify-end gap-3 px-6 py-5 border-t border-surface-bright">
          <button
            type="button"
            :disabled="eliminando"
            class="px-5 py-2.5 rounded-lg bg-surface-bright text-on-surface font-semibold hover:opacity-80 transition-opacity"
            @click="cerrarModalEliminar"
          >
            Cancelar
          </button>

          <button
            type="button"
            :disabled="eliminando"
            class="px-5 py-2.5 rounded-lg bg-red-600 text-white font-semibold hover:bg-red-500 transition-colors"
            @click="confirmarEliminar"
          >
            {{ eliminando ? 'Eliminando...' : 'Eliminar' }}
          </button>
        </div>
      </div>
    </div>
  </section>
</template>