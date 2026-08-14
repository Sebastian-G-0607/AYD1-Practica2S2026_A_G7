<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { getResenias, toggleDestacar, toggleArchivar } from '@/services/resenia.service'
import type { Resenia } from '@/types/resenia.types'

const reseniasDestacadas = ref<Resenia[]>([])
const loading = ref(true)
const error = ref('')

async function cargarDestacadas() {
  try {
    loading.value = true
    error.value = ''
    const todas = await getResenias()
    reseniasDestacadas.value = todas.filter(r => r.destacada)
  } catch (err) {
    console.error(err)
    error.value = 'Error al cargar las reseñas destacadas.'
  } finally {
    loading.value = false
  }
}

async function handleToggleDestacar(id: number) {
  try {
    await toggleDestacar(id)
    await cargarDestacadas()
  } catch (err) {
    console.error('Error al cambiar estado de destacada:', err)
  }
}

async function handleToggleArchivar(id: number) {
  try {
    await toggleArchivar(id)
    await cargarDestacadas()
  } catch (err) {
    console.error('Error al archivar la reseña:', err)
  }
}

onMounted(() => {
  cargarDestacadas()
})
</script>

<template>
  <section class="space-y-4">
    <div class="flex items-center justify-between">
      <h2 class="text-xl font-bold font-headline-md text-on-surface flex items-center gap-2">
        <span class="material-symbols-outlined text-yellow-400">star</span>
        Reseñas Destacadas
      </h2>
    </div>

    <!-- Cargando -->
    <p v-if="loading" class="text-sm text-on-surface/60">
      Cargando reseñas destacadas...
    </p>

    <!-- Error -->
    <p v-else-if="error" class="text-sm text-red-400">
      {{ error }}
    </p>

    <!-- Sin elementos -->
    <div v-else-if="reseniasDestacadas.length === 0" class="p-6 rounded-xl bg-surface-container-low border border-surface-bright text-center">
      <p class="text-sm text-on-surface/60">
        No tienes reseñas marcadas como destacadas.
      </p>
    </div>

    <!-- Tarjetas Destacadas -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <article
        v-for="resenia in reseniasDestacadas"
        :key="resenia.id"
        class="p-5 rounded-xl bg-surface-container-low border border-yellow-500/20 shadow-md space-y-3"
      >
        <div class="flex items-start justify-between gap-2">
          <div>
            <h3 class="font-bold text-lg text-on-surface">
              {{ resenia.tituloPelicula }}
            </h3>
            <span class="inline-block mt-1 px-2.5 py-0.5 rounded-full bg-surface-bright text-xs text-primary font-semibold">
              {{ resenia.etiqueta }}
            </span>
          </div>

          <div class="text-yellow-400 text-sm whitespace-nowrap">
            {{ '★'.repeat(resenia.calificacion) }}<span class="text-on-surface/20">{{ '★'.repeat(5 - resenia.calificacion) }}</span>
          </div>
        </div>

        <p v-if="resenia.comentario" class="text-sm text-on-surface/80 line-clamp-3 italic">
          "{{ resenia.comentario }}"
        </p>

        <div class="pt-2 border-t border-surface-bright flex items-center justify-between text-xs">
          <button
            type="button"
            class="text-yellow-400 hover:underline flex items-center gap-1 font-semibold"
            @click="handleToggleDestacar(resenia.id)"
          >
            <span class="material-symbols-outlined text-sm">star_half</span>
            Quitar Destacado
          </button>

          <button
            type="button"
            class="text-on-surface/60 hover:text-on-surface flex items-center gap-1 font-semibold"
            @click="handleToggleArchivar(resenia.id)"
          >
            <span class="material-symbols-outlined text-sm">archive</span>
            Archivar
          </button>
        </div>
      </article>
    </div>
  </section>
</template>