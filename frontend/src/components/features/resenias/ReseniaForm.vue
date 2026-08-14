<script setup lang="ts">
import { ref } from 'vue'
import { createResenia } from '@/services/resenia.service'

const emit = defineEmits<{
  (e: 'created'): void
}>()

const tituloPelicula = ref('')
const calificacion = ref(5)
const comentario = ref('')
const nuevaEtiqueta = ref('')

const loading = ref(false)
const error = ref('')
const success = ref('')

async function handleSubmit() {
  error.value = ''
  success.value = ''

  if (!tituloPelicula.value.trim()) {
    error.value = 'El título de la película es obligatorio.'
    return
  }

  if (!nuevaEtiqueta.value.trim()) {
    error.value = 'La etiqueta es obligatoria.'
    return
  }

  try {
    loading.value = true

    await createResenia({
      tituloPelicula: tituloPelicula.value.trim(),
      calificacion: calificacion.value,
      comentario: comentario.value.trim() || null,
      etiquetaId: null,
      nuevaEtiqueta: nuevaEtiqueta.value.trim(),
    })

    success.value = 'Reseña creada correctamente.'

    tituloPelicula.value = ''
    calificacion.value = 5
    comentario.value = ''
    nuevaEtiqueta.value = ''

    emit('created')
  } catch (err) {
    console.error(err)
    error.value = 'No fue posible crear la reseña.'
  } finally {
    loading.value = false
  }
}
</script>

<template>
  <section
    class="bg-surface-container-low border border-surface-bright rounded-2xl p-6 space-y-5"
  >
    <div>
      <h2 class="text-xl font-bold">
        Nueva Reseña
      </h2>

      <p class="text-sm text-on-surface/60">
        Comparte tu opinión sobre una película.
      </p>
    </div>

    <form
      class="space-y-4"
      @submit.prevent="handleSubmit"
    >
      <div class="space-y-2">
        <label class="text-sm font-semibold">
          Película
        </label>

        <input
          v-model="tituloPelicula"
          type="text"
          placeholder="Ej. The Dark Knight"
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
        />
      </div>

      <div class="space-y-2">
        <label class="text-sm font-semibold">
          Calificación
        </label>

        <select
          v-model="calificacion"
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
        >
          <option :value="5">★★★★★</option>
          <option :value="4">★★★★</option>
          <option :value="3">★★★</option>
          <option :value="2">★★</option>
          <option :value="1">★</option>
        </select>
      </div>

      <div class="space-y-2">
        <label class="text-sm font-semibold">
          Comentario
        </label>

        <textarea
          v-model="comentario"
          rows="4"
          placeholder="¿Qué te pareció la película?"
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary resize-none"
        ></textarea>
      </div>

      <div class="space-y-2">
        <label class="text-sm font-semibold">
          Etiqueta
        </label>

        <input
          v-model="nuevaEtiqueta"
          type="text"
          placeholder="Ej. Ciencia ficción"
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary"
        />
      </div>

      <p
        v-if="error"
        class="text-sm text-red-400"
      >
        {{ error }}
      </p>

      <p
        v-if="success"
        class="text-sm text-green-400"
      >
        {{ success }}
      </p>

      <button
        type="submit"
        :disabled="loading"
        class="w-full px-4 py-3 rounded-lg bg-primary text-on-primary font-bold transition-opacity hover:opacity-90 disabled:opacity-50"
      >
        {{ loading ? 'Publicando...' : 'Publicar reseña' }}
      </button>
    </form>
  </section>
</template>