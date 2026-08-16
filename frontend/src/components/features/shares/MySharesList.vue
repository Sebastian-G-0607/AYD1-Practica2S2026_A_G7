<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useShares } from '@/composables/useShares'
import ShareReviewModal from '@/components/features/shares/ShareReviewModal.vue'
import type { MySharedItem } from '@/types/shares.types'

const { myShares, isLoading, error, fetchMyShares, shareReviewBatch, unshareReview } = useShares()

const isModalOpen = ref(false)
const modalReseniaId = ref<number | undefined>(undefined)
const modalReseniaTitle = ref<string>('')
const unsharingItem = ref<MySharedItem | null>(null)
const isUnsharing = ref(false)

const getInitials = (name?: string) => {
  if (!name) return 'U'
  const parts = name.trim().split(/\s+/)
  const first = parts[0]
  const second = parts[1]
  if (first && second && first.length > 0 && second.length > 0) {
    return (first.charAt(0) + second.charAt(0)).toUpperCase()
  }
  return name.substring(0, 2).toUpperCase() || 'U'
}

const openModal = () => {
  modalReseniaId.value = undefined
  modalReseniaTitle.value = ''
  isModalOpen.value = true
}

const closeModal = () => {
  isModalOpen.value = false
}

const onShareSubmit = async (payload: { reseniaId: number; userIds: number[] }) => {
  try {
    const res = await shareReviewBatch(payload.reseniaId, payload.userIds)
    alert(res.mensaje || '¡Reseña compartida con éxito!')
    closeModal()
    await fetchMyShares()
  } catch (err: any) {
    const msg =
      err.response?.data?.mensaje ||
      err.response?.data?.message ||
      'Ocurrió un error al compartir la reseña.'
    alert(msg)
  }
}

const confirmUnshare = async (share: MySharedItem) => {
  const isConfirmed = confirm(
    `¿Deseas dejar de compartir la reseña de "${share.tituloPelicula}" con ${share.destinatarioNombre}?`,
  )
  if (!isConfirmed) return

  try {
    isUnsharing.value = true
    unsharingItem.value = share
    const res = await unshareReview(share.reseniaId, share.usuarioDestinatarioId)
    alert(res.mensaje || 'Se ha dejado de compartir la reseña.')
  } catch (err: any) {
    const msg =
      err.response?.data?.mensaje ||
      err.response?.data?.message ||
      'No fue posible cancelar el uso compartido.'
    alert(msg)
  } finally {
    isUnsharing.value = false
    unsharingItem.value = null
  }
}

onMounted(() => {
  fetchMyShares()
})
</script>

<template>
  <div class="space-y-8 max-w-7xl mx-auto">
    <!-- Encabezado -->
    <div class="flex flex-col md:flex-row md:items-center justify-between gap-4">
      <div>
        <h1 class="font-display-lg text-3xl md:text-4xl font-bold text-on-surface mb-2">
          Mis Compartidas
        </h1>
        <p class="font-body-lg text-on-surface-variant max-w-2xl text-sm md:text-base">
          Gestiona las reseñas de películas que has recomendado a otros miembros de la comunidad.
        </p>
      </div>

      <button
        type="button"
        class="flex items-center gap-2 bg-primary hover:bg-primary-container text-on-primary px-6 py-3 rounded-xl font-semibold shadow-lg shadow-primary/20 hover:-translate-y-0.5 transform duration-300 self-start md:self-auto transition-all text-sm"
        @click="openModal"
      >
        <span class="material-symbols-outlined text-lg">share</span>
        <span>Compartir Reseña</span>
      </button>
    </div>

    <!-- Estado de Carga -->
    <div v-if="isLoading" class="p-16 text-center text-on-surface-variant flex flex-col items-center gap-4">
      <span class="material-symbols-outlined text-4xl animate-spin text-primary">sync</span>
      <p>Cargando tus reseñas compartidas...</p>
    </div>

    <!-- Estado de Error -->
    <div
      v-else-if="error"
      class="p-6 rounded-xl bg-red-500/10 border border-red-500/20 text-red-300 flex items-center gap-3"
    >
      <span class="material-symbols-outlined text-2xl">error</span>
      <p class="text-sm font-medium">{{ error }}</p>
    </div>

    <!-- Estado Vacío -->
    <div
      v-else-if="myShares.length === 0"
      class="bg-surface-container rounded-2xl p-16 text-center text-on-surface-variant border border-outline-variant/10 shadow-xl flex flex-col items-center gap-4 my-6"
    >
      <span class="material-symbols-outlined text-6xl text-on-surface-variant opacity-40">share</span>
      <h3 class="text-xl font-bold text-on-surface">No has compartido reseñas todavía</h3>
      <p class="max-w-md text-sm opacity-80">
        Comparte tus mejores opiniones con la comunidad seleccionando una de tus reseñas.
      </p>
      <button
        type="button"
        @click="openModal"
        class="mt-3 px-6 py-2.5 bg-primary text-on-primary rounded-xl font-semibold hover:bg-primary-container transition-all text-sm shadow-md"
      >
        Compartir una reseña ahora
      </button>
    </div>

    <!-- Tabla / Lista de Reseñas Compartidas -->
    <div
      v-else
      class="bg-surface-container rounded-2xl overflow-hidden shadow-2xl border border-outline-variant/10"
    >
      <!-- Cabecera de Tabla en Desktop -->
      <div
        class="hidden md:grid grid-cols-12 gap-4 px-8 py-4 bg-surface-container-high/60 border-b border-outline-variant/10 text-xs font-semibold text-on-surface-variant uppercase tracking-wider"
      >
        <div class="col-span-5">Película y Comentario</div>
        <div class="col-span-4">Destinatario</div>
        <div class="col-span-2 text-center">Calificación</div>
        <div class="col-span-1 text-right">Acción</div>
      </div>

      <div class="divide-y divide-outline-variant/10">
        <div
          v-for="share in myShares"
          :key="`${share.reseniaId}-${share.usuarioDestinatarioId}`"
          class="grid grid-cols-1 md:grid-cols-12 gap-4 px-6 md:px-8 py-5 items-center hover:bg-surface-container-high/30 transition-colors group"
        >
          <!-- Columna: Título, Etiqueta y Comentario -->
          <div class="col-span-1 md:col-span-5 flex gap-4 items-start md:items-center">
            <div
              class="w-12 h-16 rounded-lg overflow-hidden shrink-0 shadow-md bg-surface-dim flex items-center justify-center text-primary group-hover:scale-105 transition-transform duration-300 border border-outline-variant/10"
            >
              <span class="material-symbols-outlined text-2xl">movie</span>
            </div>
            <div class="min-w-0 flex-1">
              <h3 class="text-base font-bold text-on-surface group-hover:text-primary transition-colors truncate">
                {{ share.tituloPelicula }}
              </h3>
              <span
                v-if="share.etiqueta"
                class="inline-block mt-1 text-[10px] px-2 py-0.5 rounded-full bg-primary/10 text-primary font-medium border border-primary/20"
              >
                {{ share.etiqueta }}
              </span>
              <p class="text-xs text-on-surface-variant opacity-75 line-clamp-2 mt-1">
                {{ share.comentario || 'Sin comentario adicional.' }}
              </p>
            </div>
          </div>

          <!-- Columna: Destinatario -->
          <div class="col-span-1 md:col-span-4 flex items-center gap-3">
            <div
              class="w-9 h-9 rounded-full bg-primary/20 text-primary flex items-center justify-center text-xs font-bold shrink-0"
            >
              {{ getInitials(share.destinatarioNombre) }}
            </div>
            <div class="min-w-0 truncate">
              <p class="text-sm font-semibold text-on-surface truncate">
                {{ share.destinatarioNombre }}
              </p>
              <p class="text-xs text-on-surface-variant opacity-60 truncate">
                {{ share.destinatarioCorreo }}
              </p>
            </div>
          </div>

          <!-- Columna: Calificación -->
          <div class="col-span-1 md:col-span-2 flex items-center md:justify-center">
            <div class="flex items-center gap-0.5 text-yellow-400 text-sm">
              <span>{{ '★'.repeat(share.calificacion || 5) }}</span>
              <span class="text-on-surface/20">{{ '★'.repeat(5 - (share.calificacion || 5)) }}</span>
            </div>
          </div>

          <!-- Columna: Acción (Dejar de Compartir) -->
          <div class="col-span-1 md:col-span-1 flex justify-end">
            <button
              type="button"
              :disabled="isUnsharing && unsharingItem?.reseniaId === share.reseniaId && unsharingItem?.usuarioDestinatarioId === share.usuarioDestinatarioId"
              @click="confirmUnshare(share)"
              class="p-2 text-on-surface-variant hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-colors text-xs flex items-center gap-1"
              title="Dejar de compartir"
            >
              <span class="material-symbols-outlined text-lg">link_off</span>
              <span class="md:hidden">Dejar de compartir</span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Compartir Reseña -->
    <ShareReviewModal
      :is-open="isModalOpen"
      :resenia-id="modalReseniaId"
      :resenia-title="modalReseniaTitle"
      @close="closeModal"
      @share="onShareSubmit"
    />
  </div>
</template>