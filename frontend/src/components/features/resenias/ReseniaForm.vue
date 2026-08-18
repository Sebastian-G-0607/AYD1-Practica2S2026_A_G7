<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { createResenia, getEtiquetas } from '@/services/resenia.service'
import type { Etiqueta } from '@/types/resenia.types'

const emit = defineEmits<{
  (e: 'created'): void
}>()

const tituloPelicula = ref('')
const calificacion = ref(5)
const comentario = ref('')

const etiquetas = ref<Etiqueta[]>([])
const modoEtiqueta = ref<'existente' | 'nueva'>('nueva')
const etiquetaId = ref<number | null>(null)
const nuevaEtiqueta = ref('')

const loading = ref(false)
const loadingEtiquetas = ref(false)
const error = ref('')
const success = ref('')

async function cargarEtiquetas() {
  try {
    loadingEtiquetas.value = true
    const list = await getEtiquetas()
    etiquetas.value = list

    if (list.length > 0) {
      modoEtiqueta.value = 'existente'
      if (!etiquetaId.value || !list.some((e) => e.id === etiquetaId.value)) {
        etiquetaId.value = list[0]?.id ?? null
      }
    } else {
      modoEtiqueta.value = 'nueva'
      etiquetaId.value = null
    }
  } catch (err) {
    console.error('Error al cargar etiquetas:', err)
  } finally {
    loadingEtiquetas.value = false
  }
}

async function handleSubmit() {
  error.value = ''
  success.value = ''

  if (!tituloPelicula.value.trim()) {
    error.value = 'El título de la película es obligatorio.'
    return
  }

  if (modoEtiqueta.value === 'existente') {
    if (!etiquetaId.value) {
      error.value = 'Debes seleccionar una etiqueta.'
      return
    }
  } else {
    const desc = nuevaEtiqueta.value.trim()
    if (!desc) {
      error.value = 'La etiqueta es obligatoria.'
      return
    }

    // Validación case-insensitive para evitar duplicados
    const yaExiste = etiquetas.value.some(
      (e) => e.descripcion.trim().toLowerCase() === desc.toLowerCase(),
    )

    if (yaExiste) {
      error.value = 'Ya existe una etiqueta con ese nombre.'
      return
    }
  }

  try {
    loading.value = true

    await createResenia({
      tituloPelicula: tituloPelicula.value.trim(),
      calificacion: calificacion.value,
      comentario: comentario.value.trim() || null,
      etiquetaId: modoEtiqueta.value === 'existente' ? etiquetaId.value : null,
      nuevaEtiqueta: modoEtiqueta.value === 'nueva' ? nuevaEtiqueta.value.trim() : null,
    })

    success.value = 'Reseña creada correctamente.'

    tituloPelicula.value = ''
    calificacion.value = 5
    comentario.value = ''
    nuevaEtiqueta.value = ''

    await cargarEtiquetas()

    emit('created')
  } catch (err: any) {
    console.error(err)
    error.value =
      err?.response?.data?.message ||
      err?.response?.data?.title ||
      'No fue posible crear la reseña.'
  } finally {
    loading.value = false
  }
}

onMounted(() => {
  cargarEtiquetas()
})
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
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary text-on-surface"
        />
      </div>

      <div class="space-y-2">
        <label class="text-sm font-semibold">
          Calificación
        </label>

        <select
          v-model="calificacion"
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary text-on-surface"
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
          class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary resize-none text-on-surface"
        ></textarea>
      </div>

      <div class="space-y-2">
        <div class="flex items-center justify-between">
          <label class="text-sm font-semibold">
            Etiqueta
          </label>

          <div
            v-if="etiquetas.length > 0"
            class="flex items-center gap-1 p-1 rounded-lg bg-surface border border-surface-bright text-xs"
          >
            <button
              type="button"
              class="px-2.5 py-1 rounded-md font-semibold transition-colors"
              :class="
                modoEtiqueta === 'existente'
                  ? 'bg-primary text-on-primary'
                  : 'text-on-surface/60 hover:text-on-surface'
              "
              @click="modoEtiqueta = 'existente'"
            >
              Existente
            </button>
            <button
              type="button"
              class="px-2.5 py-1 rounded-md font-semibold transition-colors"
              :class="
                modoEtiqueta === 'nueva'
                  ? 'bg-primary text-on-primary'
                  : 'text-on-surface/60 hover:text-on-surface'
              "
              @click="modoEtiqueta = 'nueva'"
            >
              Nueva
            </button>
          </div>
        </div>

        <!-- Selector de etiqueta existente -->
        <div v-if="modoEtiqueta === 'existente'">
          <select
            v-model="etiquetaId"
            class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary text-on-surface"
          >
            <option :value="null" disabled>
              Selecciona una etiqueta...
            </option>
            <option
              v-for="etiqueta in etiquetas"
              :key="etiqueta.id"
              :value="etiqueta.id"
            >
              {{ etiqueta.descripcion }}
            </option>
          </select>
        </div>

        <!-- Input para nueva etiqueta -->
        <div v-else>
          <input
            v-model="nuevaEtiqueta"
            type="text"
            placeholder="Ej. Ciencia ficción"
            class="w-full px-4 py-3 rounded-lg bg-surface border border-surface-bright outline-none focus:border-primary text-on-surface"
          />
        </div>
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