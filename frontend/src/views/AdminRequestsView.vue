<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useAdminStore } from '@/stores/admin.store'
import MainLayout from '@/layouts/MainLayout.vue'

const store = useAdminStore()
const { pendingRequests, loading, error } = storeToRefs(store)

const searchQuery = ref('')
const sortOrder = ref<'asc' | 'desc'>('desc')
const rejectingId = ref<number | null>(null)
const rejectReason = ref('')
const processingId = ref<number | null>(null)
const successMessage = ref('')
const approveConfirmId = ref<number | null>(null)

// Paginación
const currentPage = ref(1)
const itemsPerPage = ref(10)

const getInitials = (name: string): string => {
  if (!name) return 'U'
  return name
    .split(' ')
    .filter(Boolean)
    .map((word) => word.charAt(0))
    .join('')
    .substring(0, 2)
    .toUpperCase()
}

const formatDate = (dateStr: string): string => {
  if (!dateStr) return '-'
  try {
    const date = new Date(dateStr)
    return date.toLocaleDateString('es-ES', {
      day: 'numeric',
      month: 'short',
      year: 'numeric'
    })
  } catch {
    return dateStr
  }
}

const formatTime = (dateStr: string): string => {
  if (!dateStr) return ''
  try {
    const date = new Date(dateStr)
    return date.toLocaleTimeString('es-ES', {
      hour: '2-digit',
      minute: '2-digit'
    })
  } catch {
    return ''
  }
}

const toggleSortOrder = () => {
  sortOrder.value = sortOrder.value === 'asc' ? 'desc' : 'asc'
}

const filteredRequests = computed(() => {
  let list = [...pendingRequests.value]

  if (searchQuery.value.trim()) {
    const q = searchQuery.value.toLowerCase().trim()
    list = list.filter(
      (r) =>
        r.nombre?.toLowerCase().includes(q) ||
        r.correo?.toLowerCase().includes(q)
    )
  }

  list.sort((a, b) => {
    const dateA = new Date(a.fecha_solicitud).getTime()
    const dateB = new Date(b.fecha_solicitud).getTime()
    return sortOrder.value === 'asc' ? dateA - dateB : dateB - dateA
  })

  return list
})

const totalPages = computed(() => {
  return Math.ceil(filteredRequests.value.length / itemsPerPage.value) || 1
})

const startIndex = computed(() => {
  return (currentPage.value - 1) * itemsPerPage.value
})

const endIndex = computed(() => {
  return startIndex.value + itemsPerPage.value
})

const paginatedRequests = computed(() => {
  return filteredRequests.value.slice(startIndex.value, endIndex.value)
})

const promptApprove = (id: number) => {
  approveConfirmId.value = id
}

const cancelApprovePrompt = () => {
  approveConfirmId.value = null
}

const confirmApprove = async () => {
  if (approveConfirmId.value === null) return
  const id = approveConfirmId.value
  processingId.value = id
  approveConfirmId.value = null

  try {
    await store.processRequest(id, true)
    successMessage.value = 'Solicitud aprobada exitosamente.'
    setTimeout(() => {
      successMessage.value = ''
    }, 4000)
  } catch {
    // El error se maneja en el store
  } finally {
    processingId.value = null
  }
}

const startReject = (id: number) => {
  rejectingId.value = id
  rejectReason.value = ''
}

const cancelReject = () => {
  rejectingId.value = null
  rejectReason.value = ''
}

const confirmReject = async (id: number) => {
  if (!rejectReason.value.trim()) return

  processingId.value = id
  try {
    await store.processRequest(id, false, rejectReason.value.trim())
    cancelReject()
    successMessage.value = 'Solicitud rechazada exitosamente.'
    setTimeout(() => {
      successMessage.value = ''
    }, 4000)
  } catch {
    // El error se maneja en el store
  } finally {
    processingId.value = null
  }
}

onMounted(() => {
  store.fetchPending()
})
</script>

<template>
  <MainLayout>
    <div class="relative w-full">
      <!-- Decorative Glow Background de Stitch -->
      <div aria-hidden="true" class="absolute top-0 right-0 w-[400px] h-[300px] pointer-events-none opacity-20">
        <svg class="w-full h-full text-primary" fill="currentColor" viewBox="0 0 100 100">
          <circle class="mix-blend-screen blur-3xl opacity-30" cx="80" cy="20" r="40"></circle>
          <circle class="mix-blend-screen blur-2xl opacity-20 text-secondary-container" cx="20" cy="80" fill="currentColor" r="30"></circle>
        </svg>
      </div>

      <!-- Header Section -->
      <div class="flex flex-col md:flex-row md:items-end justify-between gap-gutter mb-margin-desktop relative z-10">
        <div>
          <div class="font-label-md text-label-md text-on-surface-variant uppercase tracking-[0.2em] mb-stack-sm flex items-center gap-2">
            <span class="w-8 h-px bg-on-surface-variant/30"></span>
            Administración
          </div>
          <h1 class="font-display-lg text-display-lg text-on-surface">Solicitudes Pendientes</h1>
        </div>

        <div class="flex items-center gap-4">
          <div class="bg-surface-container-high rounded-full px-4 py-2 flex items-center gap-2 shadow-sm border border-outline-variant/10">
            <span class="material-symbols-outlined text-primary text-sm">hourglass_empty</span>
            <span class="font-label-md text-label-md text-on-surface">{{ pendingRequests.length }} en Espera</span>
          </div>

          <button
            type="button"
            @click="toggleSortOrder"
            class="bg-surface-variant text-on-surface-variant hover:bg-surface-container-highest hover:text-on-surface transition-colors rounded-full px-6 py-2.5 font-label-md text-label-md flex items-center gap-2 shadow-sm"
          >
            <span class="material-symbols-outlined text-sm">filter_list</span>
            <span>{{ sortOrder === 'desc' ? 'Más recientes' : 'Más antiguas' }}</span>
          </button>
        </div>
      </div>

      <!-- Mensaje de Éxito -->
      <div
        v-if="successMessage"
        class="mb-6 bg-primary-container/20 border border-primary/40 text-on-surface px-4 py-3 rounded-xl flex items-center justify-between shadow-lg relative z-10 animate-fade-in-up"
      >
        <div class="flex items-center gap-3">
          <span class="material-symbols-outlined text-primary">check_circle</span>
          <span class="text-sm font-medium">{{ successMessage }}</span>
        </div>
        <button
          type="button"
          @click="successMessage = ''"
          class="text-on-surface-variant hover:text-on-surface p-1"
        >
          <span class="material-symbols-outlined text-sm">close</span>
        </button>
      </div>

      <!-- Banner de Error -->
      <div
        v-if="error"
        class="mb-6 bg-error-container/20 border border-error/40 text-error px-4 py-3 rounded-xl flex items-center justify-between shadow-lg relative z-10"
      >
        <div class="flex items-center gap-3">
          <span class="material-symbols-outlined">error</span>
          <span class="text-sm">{{ error }}</span>
        </div>
        <button
          type="button"
          @click="store.fetchPending"
          class="text-xs uppercase font-bold tracking-wider underline hover:opacity-80 ml-4"
        >
          Reintentar
        </button>
      </div>

      <!-- Main Data Table Container -->
      <div class="bg-surface-container-low rounded-xl shadow-xl overflow-hidden relative z-10 border border-outline-variant/10">
        <!-- Table Toolbar -->
        <div class="px-6 py-4 bg-surface-container flex items-center justify-between border-b border-surface-container-high/50 gap-4">
          <div class="relative w-full max-w-xs md:max-w-sm group">
            <span class="material-symbols-outlined absolute left-3 top-1/2 -translate-y-1/2 text-on-surface-variant/50 group-focus-within:text-primary transition-colors text-sm">
              search
            </span>
            <input
              v-model="searchQuery"
              class="w-full bg-surface-dim rounded-md py-1.5 pl-9 pr-4 text-on-surface placeholder:text-on-surface-variant/30 focus:outline-none focus:ring-1 focus:ring-primary transition-all font-body-md text-sm border border-transparent focus:border-primary/30"
              placeholder="Buscar por nombre o correo..."
              type="text"
            />
          </div>

          <div class="flex gap-2 shrink-0">
            <button
              type="button"
              class="p-1.5 text-on-surface-variant hover:text-on-surface transition-colors rounded hover:bg-surface-variant"
              title="Vista de lista"
            >
              <span class="material-symbols-outlined text-sm">view_list</span>
            </button>
            <button
              type="button"
              class="p-1.5 text-on-surface-variant/50 hover:text-on-surface transition-colors rounded hover:bg-surface-variant"
              title="Vista de cuadrícula"
            >
              <span class="material-symbols-outlined text-sm">grid_view</span>
            </button>
          </div>
        </div>

        <!-- Estado de Carga -->
        <div v-if="loading && pendingRequests.length === 0" class="flex flex-col items-center justify-center py-16 text-on-surface-variant">
          <div class="w-8 h-8 rounded-full border-2 border-primary border-t-transparent animate-spin mb-3"></div>
          <p class="text-sm font-medium">Cargando solicitudes pendientes...</p>
        </div>

        <!-- Tabla de Datos -->
        <div v-else class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-surface-container-lowest text-on-surface-variant font-label-md text-label-md uppercase tracking-wider">
                <th class="px-6 py-4 font-semibold whitespace-nowrap">Solicitante</th>
                <th class="px-6 py-4 font-semibold whitespace-nowrap">Correo</th>
                <th class="px-6 py-4 font-semibold whitespace-nowrap">Fecha de Solicitud</th>
                <th class="px-6 py-4 font-semibold whitespace-nowrap">Estado</th>
                <th class="px-6 py-4 font-semibold text-right whitespace-nowrap">Acciones</th>
              </tr>
            </thead>
            <tbody class="font-body-md text-body-md text-on-surface divide-y divide-surface-container-high/30">
              <!-- Filas de Solicitudes -->
              <tr
                v-for="req in paginatedRequests"
                :key="req.id"
                :class="[
                  'hover:bg-surface-container/50 transition-colors group',
                  processingId === req.id ? 'opacity-50 pointer-events-none' : ''
                ]"
              >
                <!-- Solicitante -->
                <td class="px-6 py-4 whitespace-nowrap">
                  <div class="flex items-center gap-3">
                    <div class="w-8 h-8 rounded-full bg-primary/20 flex items-center justify-center text-primary font-headline-md text-sm font-semibold uppercase">
                      {{ getInitials(req.nombre) }}
                    </div>
                    <span class="font-headline-md text-[16px] leading-tight text-on-surface">
                      {{ req.nombre }}
                    </span>
                  </div>
                </td>

                <!-- Correo -->
                <td class="px-6 py-4 text-on-surface-variant whitespace-nowrap">
                  {{ req.correo }}
                </td>

                <!-- Fecha de Solicitud -->
                <td class="px-6 py-4 text-on-surface-variant whitespace-nowrap">
                  <span>{{ formatDate(req.fecha_solicitud) }}</span>
                  <span class="text-caption opacity-50 ml-1">{{ formatTime(req.fecha_solicitud) }}</span>
                </td>

                <!-- Estado -->
                <td class="px-6 py-4 whitespace-nowrap">
                  <span class="inline-flex items-center gap-1.5 px-2.5 py-1 rounded-full bg-surface-variant text-on-surface-variant font-label-md text-[11px] uppercase tracking-wide">
                    <span class="w-1.5 h-1.5 rounded-full bg-on-surface-variant"></span>
                    {{ req.status || 'Pendiente' }}
                  </span>
                </td>

                <!-- Acciones -->
                <td class="px-6 py-4 text-right whitespace-nowrap">
                  <div v-if="rejectingId !== req.id" class="flex items-center justify-end gap-2 action-container">
                    <button
                      type="button"
                      @click="promptApprove(req.id)"
                      :disabled="loading || processingId === req.id"
                      class="accept-btn bg-primary-container/10 hover:bg-primary-container text-primary hover:text-on-primary-container transition-colors p-2 rounded-full flex items-center justify-center group/btn shadow-sm disabled:opacity-50"
                      title="Aprobar"
                    >
                      <span class="material-symbols-outlined text-sm transition-transform group-hover/btn:scale-110">check</span>
                    </button>
                    <button
                      type="button"
                      @click="startReject(req.id)"
                      :disabled="loading || processingId === req.id"
                      class="reject-btn bg-error-container/10 hover:bg-error-container text-error hover:text-on-error-container transition-colors p-2 rounded-full flex items-center justify-center group/btn shadow-sm disabled:opacity-50"
                      title="Rechazar"
                    >
                      <span class="material-symbols-outlined text-sm transition-transform group-hover/btn:scale-110">close</span>
                    </button>
                  </div>

                  <!-- Campo Inline para Motivo de Rechazo -->
                  <div v-else class="flex items-center justify-end gap-2">
                    <input
                      v-model="rejectReason"
                      @keyup.enter="confirmReject(req.id)"
                      @keyup.esc="cancelReject"
                      class="w-48 bg-surface-dim rounded px-3 py-1.5 text-sm text-on-surface placeholder:text-on-surface-variant/50 focus:outline-none focus:ring-1 focus:ring-error transition-all border border-transparent focus:border-error/30"
                      placeholder="Motivo de rechazo..."
                      type="text"
                      autofocus
                    />
                    <button
                      type="button"
                      @click="confirmReject(req.id)"
                      :disabled="!rejectReason.trim() || processingId === req.id"
                      class="p-1.5 bg-error-container text-error hover:text-on-error-container rounded-full transition-colors disabled:opacity-50 flex items-center justify-center"
                      title="Confirmar rechazo"
                    >
                      <span class="material-symbols-outlined text-sm">check</span>
                    </button>
                    <button
                      type="button"
                      @click="cancelReject"
                      class="text-on-surface-variant hover:text-on-surface text-xs ml-1 underline cursor-pointer"
                    >
                      Cancelar
                    </button>
                  </div>
                </td>
              </tr>

              <!-- Estado Vacío -->
              <tr v-if="filteredRequests.length === 0">
                <td colspan="5" class="px-6 py-12 text-center text-on-surface-variant">
                  <div class="flex flex-col items-center justify-center gap-3">
                    <div class="w-12 h-12 rounded-full bg-surface-variant/50 flex items-center justify-center text-on-surface-variant">
                      <span class="material-symbols-outlined text-2xl">inbox</span>
                    </div>
                    <p class="font-body-md text-sm">
                      {{ searchQuery ? 'No se encontraron solicitudes que coincidan con la búsqueda.' : 'No hay solicitudes pendientes.' }}
                    </p>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Paginación Footer de Stitch -->
        <div
          v-if="!loading && filteredRequests.length > 0"
          class="px-6 py-4 bg-surface-container flex flex-col sm:flex-row items-center justify-between gap-4 border-t border-surface-container-high/50"
        >
          <span class="font-caption text-caption text-on-surface-variant">
            Mostrando {{ startIndex + 1 }}-{{ Math.min(endIndex, filteredRequests.length) }} de {{ filteredRequests.length }} solicitudes
          </span>

          <div v-if="totalPages > 1" class="flex items-center gap-1">
            <button
              type="button"
              @click="currentPage--"
              :disabled="currentPage === 1"
              class="p-1 rounded text-on-surface-variant hover:bg-surface-variant disabled:opacity-30 disabled:cursor-not-allowed transition-colors"
              title="Página anterior"
            >
              <span class="material-symbols-outlined text-sm">chevron_left</span>
            </button>

            <button
              v-for="page in totalPages"
              :key="page"
              type="button"
              @click="currentPage = page"
              :class="[
                currentPage === page
                  ? 'bg-primary text-on-primary shadow-md'
                  : 'hover:bg-surface-variant text-on-surface-variant transition-colors',
                'w-7 h-7 rounded font-label-md text-xs flex items-center justify-center'
              ]"
            >
              {{ page }}
            </button>

            <button
              type="button"
              @click="currentPage++"
              :disabled="currentPage === totalPages"
              class="p-1 rounded text-on-surface-variant hover:bg-surface-variant disabled:opacity-30 disabled:cursor-not-allowed transition-colors"
              title="Página siguiente"
            >
              <span class="material-symbols-outlined text-sm">chevron_right</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Modal de Confirmación de Aprobación -->
      <div
        v-if="approveConfirmId !== null"
        class="fixed inset-0 z-50 bg-black/70 backdrop-blur-sm flex items-center justify-center p-4 animate-fade-in-up"
      >
        <div class="bg-surface-container border border-outline-variant/20 rounded-2xl p-6 w-full max-w-md shadow-2xl space-y-4">
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-xl bg-primary-container/20 flex items-center justify-center text-primary">
              <span class="material-symbols-outlined">check_circle</span>
            </div>
            <div>
              <h3 class="text-lg font-bold font-headline-md text-on-surface">Aprobar Solicitud</h3>
              <p class="text-xs text-on-surface-variant">Confirma la aprobación del registro de usuario</p>
            </div>
          </div>

          <p class="text-sm text-on-surface-variant leading-relaxed">
            ¿Estás seguro de que deseas aprobar esta solicitud de registro? El usuario obtendrá acceso a la plataforma.
          </p>

          <div class="flex justify-end gap-3 pt-2">
            <button
              type="button"
              @click="cancelApprovePrompt"
              class="px-4 py-2 rounded-lg bg-surface-variant text-on-surface-variant hover:bg-surface-container-highest hover:text-on-surface text-sm font-medium transition-colors"
            >
              Cancelar
            </button>
            <button
              type="button"
              @click="confirmApprove"
              class="px-4 py-2 rounded-lg bg-primary-container text-on-primary-container hover:bg-primary font-semibold text-sm transition-colors shadow-md flex items-center gap-1.5"
            >
              <span>Aprobar Solicitud</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </MainLayout>
</template>