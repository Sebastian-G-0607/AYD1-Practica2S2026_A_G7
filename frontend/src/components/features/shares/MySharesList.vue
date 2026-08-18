<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useShares } from '@/composables/useShares'
import ShareReviewModal from '@/components/features/shares/ShareReviewModal.vue'
import type { MySharedItem } from '@/types/shares.types'

const { myShares, isLoading, error, fetchMyShares, shareReviewBatch, unshareReview } = useShares()

const isModalOpen = ref(false)
const modalReseniaId = ref<number | undefined>(undefined)
const modalReseniaTitle = ref<string>('')
const shareToUnshare = ref<MySharedItem | null>(null)
const isUnsharing = ref(false)
const statusNotification = ref<{ type: 'success' | 'error'; message: string } | null>(null)

function showNotification(type: 'success' | 'error', message: string) {
  statusNotification.value = { type, message }
  setTimeout(() => {
    if (statusNotification.value?.message === message) {
      statusNotification.value = null
    }
  }, 4000)
}

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
    closeModal()
    showNotification('success', res.mensaje || '¡Reseña compartida con éxito!')
    await fetchMyShares()
  } catch (err: any) {
    const msg =
      err.response?.data?.mensaje ||
      err.response?.data?.message ||
      'Ocurrió un error al compartir la reseña.'
    showNotification('error', msg)
  }
}

const promptUnshare = (share: MySharedItem) => {
  shareToUnshare.value = share
}

const cancelUnshare = () => {
  if (isUnsharing.value) return
  shareToUnshare.value = null
}

const executeUnshare = async () => {
  if (!shareToUnshare.value) return

  const share = shareToUnshare.value
  try {
    isUnsharing.value = true
    const res = await unshareReview(share.reseniaId, share.usuarioDestinatarioId)
    shareToUnshare.value = null
    showNotification('success', res.mensaje || 'Se ha dejado de compartir la reseña.')
  } catch (err: any) {
    const msg =
      err.response?.data?.mensaje ||
      err.response?.data?.message ||
      'No fue posible cancelar el uso compartido.'
    showNotification('error', msg)
  } finally {
    isUnsharing.value = false
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

    <!-- Mensaje de Éxito / Estado -->
    <div
      v-if="statusNotification"
      class="p-4 rounded-xl flex items-center justify-between shadow-lg relative z-10 animate-fade-in-up border"
      :class="
        statusNotification.type === 'success'
          ? 'bg-primary-container/20 border-primary/40 text-on-surface'
          : 'bg-red-500/20 border-red-500/40 text-red-300'
      "
    >
      <div class="flex items-center gap-3">
        <span
          class="material-symbols-outlined"
          :class="statusNotification.type === 'success' ? 'text-primary' : 'text-red-400'"
        >
          {{ statusNotification.type === 'success' ? 'check_circle' : 'error' }}
        </span>
        <span class="text-sm font-medium">{{ statusNotification.message }}</span>
      </div>
      <button
        type="button"
        @click="statusNotification = null"
        class="text-on-surface-variant hover:text-on-surface p-1"
      >
        <span class="material-symbols-outlined text-sm">close</span>
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
              :disabled="isUnsharing && shareToUnshare?.reseniaId === share.reseniaId && shareToUnshare?.usuarioDestinatarioId === share.usuarioDestinatarioId"
              @click="promptUnshare(share)"
              class="p-2 text-on-surface-variant hover:text-red-400 hover:bg-red-500/10 rounded-lg transition-colors text-xs flex items-center gap-1 disabled:opacity-50"
              title="Dejar de compartir"
            >
              <span class="material-symbols-outlined text-lg">link_off</span>
              <span class="md:hidden">Dejar de compartir</span>
            </button>
          </div>
        </div>
      </div>
    </div>

    <!-- Modal Confirmación Dejar de Compartir (Inspirado en AdminRequestsView) -->
    <div
      v-if="shareToUnshare"
      class="fixed inset-0 z-50 bg-black/70 backdrop-blur-sm flex items-center justify-center p-4 animate-fade-in-up"
    >
      <div class="bg-surface-container border border-outline-variant/20 rounded-2xl p-6 w-full max-w-md shadow-2xl space-y-4">
        <div class="flex items-center justify-between border-b border-outline-variant/10 pb-4">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-xl bg-red-500/10 text-red-400 flex items-center justify-center">
              <span class="material-symbols-outlined">link_off</span>
            </div>
            <div>
              <h3 class="text-lg font-bold font-headline-md text-on-surface">Dejar de Compartir</h3>
              <p class="text-xs text-on-surface-variant">Confirmación de acción</p>
            </div>
          </div>
          <button
            type="button"
            :disabled="isUnsharing"
            @click="cancelUnshare"
            class="text-on-surface-variant hover:text-on-surface text-2xl leading-none transition-colors p-1"
          >
            ×
          </button>
        </div>

        <p class="text-sm text-on-surface-variant leading-relaxed">
          ¿Estás seguro de que deseas dejar de compartir la reseña de
          <span class="font-bold text-on-surface">"{{ shareToUnshare.tituloPelicula }}"</span>
          con
          <span class="font-bold text-on-surface">{{ shareToUnshare.destinatarioNombre }}</span>?
        </p>

        <div class="rounded-xl border border-red-500/20 bg-red-500/5 px-4 py-3">
          <p class="text-xs text-red-300">
            Esta persona ya no podrá ver esta reseña en su sección de "Compartidos Conmigo".
          </p>
        </div>

        <div class="flex justify-end gap-3 pt-2">
          <button
            type="button"
            :disabled="isUnsharing"
            @click="cancelUnshare"
            class="px-4 py-2 rounded-lg bg-surface-variant text-on-surface-variant hover:bg-surface-container-highest hover:text-on-surface text-sm font-medium transition-colors disabled:opacity-50"
          >
            Cancelar
          </button>
          <button
            type="button"
            :disabled="isUnsharing"
            @click="executeUnshare"
            class="px-4 py-2 rounded-lg bg-red-600 hover:bg-red-500 text-white font-semibold text-sm transition-colors shadow-md flex items-center gap-1.5 disabled:opacity-50"
          >
            <span v-if="isUnsharing" class="material-symbols-outlined text-sm animate-spin">sync</span>
            <span v-else class="material-symbols-outlined text-sm">link_off</span>
            <span>{{ isUnsharing ? 'Dejando de compartir...' : 'Dejar de compartir' }}</span>
          </button>
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