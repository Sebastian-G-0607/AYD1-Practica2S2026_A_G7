<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { storeToRefs } from 'pinia'
import { useAdminStore } from '@/stores/admin.store'
import MainLayout from '@/layouts/MainLayout.vue'
import type { Solicitud } from '@/types/solicitud.types'

const store = useAdminStore()
const { history, loading, error } = storeToRefs(store)

const searchQuery = ref('')
const statusFilter = ref<'all' | 'approved' | 'rejected'>('all')
const sortOrder = ref<'desc' | 'asc'>('desc')
const showAdvancedFilters = ref(false)
const selectedRequest = ref<Solicitud | null>(null)

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

// Estadísticas para las tarjetas de Analytics de Stitch
const totalCount = computed(() => history.value.length)

const approvedCount = computed(() => {
  return history.value.filter(
    (req) => req.status?.toLowerCase() === 'aprobado' || req.status?.toLowerCase() === 'aprobada'
  ).length
})

const rejectedCount = computed(() => {
  return history.value.filter(
    (req) => req.status?.toLowerCase() === 'rechazado' || req.status?.toLowerCase() === 'rechazada'
  ).length
})

const approvedPercentage = computed(() => {
  if (totalCount.value === 0) return 0
  return Math.round((approvedCount.value / totalCount.value) * 100)
})

const rejectedPercentage = computed(() => {
  if (totalCount.value === 0) return 0
  return Math.round((rejectedCount.value / totalCount.value) * 100)
})

// Filtrado de solicitudes
const filteredHistory = computed(() => {
  let list = [...history.value]

  if (statusFilter.value === 'approved') {
    list = list.filter(
      (r) => r.status?.toLowerCase() === 'aprobado' || r.status?.toLowerCase() === 'aprobada'
    )
  } else if (statusFilter.value === 'rejected') {
    list = list.filter(
      (r) => r.status?.toLowerCase() === 'rechazado' || r.status?.toLowerCase() === 'rechazada'
    )
  }

  if (searchQuery.value.trim()) {
    const q = searchQuery.value.toLowerCase().trim()
    list = list.filter(
      (r) =>
        r.nombre?.toLowerCase().includes(q) ||
        r.correo?.toLowerCase().includes(q) ||
        r.motivo_rechazo?.toLowerCase().includes(q)
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
  return Math.ceil(filteredHistory.value.length / itemsPerPage.value) || 1
})

const startIndex = computed(() => {
  return (currentPage.value - 1) * itemsPerPage.value
})

const endIndex = computed(() => {
  return startIndex.value + itemsPerPage.value
})

const paginatedHistory = computed(() => {
  return filteredHistory.value.slice(startIndex.value, endIndex.value)
})

const isApproved = (status: string) => {
  const s = status?.toLowerCase()
  return s === 'aprobado' || s === 'aprobada'
}

const viewDetails = (req: Solicitud) => {
  selectedRequest.value = req
}

const closeDetails = () => {
  selectedRequest.value = null
}

const exportCSV = () => {
  if (filteredHistory.value.length === 0) return

  const headers = ['ID', 'Nombre', 'Correo', 'Fecha', 'Estado', 'Motivo Rechazo']
  const rows = filteredHistory.value.map((req) => [
    req.id,
    `"${req.nombre?.replace(/"/g, '""') || ''}"`,
    `"${req.correo?.replace(/"/g, '""') || ''}"`,
    `"${formatDate(req.fecha_solicitud)} ${formatTime(req.fecha_solicitud)}"`,
    `"${req.status || ''}"`,
    `"${(req.motivo_rechazo || '').replace(/"/g, '""')}"`
  ])

  const csvContent = 'data:text/csv;charset=utf-8,\uFEFF' + [headers.join(','), ...rows.map((e) => e.join(','))].join('\n')
  const encodedUri = encodeURI(csvContent)
  const link = document.createElement('a')
  link.setAttribute('href', encodedUri)
  link.setAttribute('download', `historial_solicitudes_${new Date().toISOString().slice(0, 10)}.csv`)
  document.body.appendChild(link)
  link.click()
  document.body.removeChild(link)
}

onMounted(() => {
  store.fetchHistory()
})
</script>

<template>
  <MainLayout>
    <div class="relative w-full space-y-8">
      <!-- Decorative Glow Background -->
      <div aria-hidden="true" class="absolute top-0 right-0 w-[400px] h-[300px] pointer-events-none opacity-20">
        <svg class="w-full h-full text-primary" fill="currentColor" viewBox="0 0 100 100">
          <circle class="mix-blend-screen blur-3xl opacity-30" cx="80" cy="20" r="40"></circle>
          <circle class="mix-blend-screen blur-2xl opacity-20 text-secondary-container" cx="20" cy="80" fill="currentColor" r="30"></circle>
        </svg>
      </div>

      <!-- Header Area Stitch -->
      <div class="flex flex-col md:flex-row justify-between items-start md:items-center gap-6 relative z-10">
        <div class="flex flex-col">
          <div class="font-label-md text-label-md text-on-surface-variant uppercase tracking-[0.2em] mb-2 flex items-center gap-2">
            <span class="w-8 h-px bg-on-surface-variant/30"></span>
            Administración
          </div>
          <h1 class="font-display-lg text-display-lg text-on-surface mb-2 tracking-tight">
            Historial de Solicitudes
          </h1>
          <p class="font-body-lg text-body-lg text-on-surface-variant opacity-80 max-w-2xl">
            Rastro de auditoría integral de todas las solicitudes de registro procesadas. Utilice los filtros para profundizar en resultados o plazos específicos.
          </p>
        </div>

        <div class="flex items-center gap-4 flex-wrap">
          <button
            type="button"
            @click="exportCSV"
            :disabled="filteredHistory.length === 0"
            class="bg-surface-container-high hover:bg-surface-bright text-on-surface font-label-md text-label-md py-3 px-6 rounded-lg transition-colors flex items-center gap-2 shadow-sm border border-outline-variant/10 disabled:opacity-50 disabled:cursor-not-allowed"
          >
            <span class="material-symbols-outlined text-[18px]">download</span>
            <span>Exportar CSV</span>
          </button>

          <button
            type="button"
            @click="showAdvancedFilters = !showAdvancedFilters"
            :class="[
              showAdvancedFilters ? 'bg-primary-fixed text-on-primary-fixed' : 'bg-primary text-on-primary',
              'font-label-md text-label-md py-3 px-6 rounded-lg hover:bg-primary-fixed transition-colors flex items-center gap-2 shadow-md shadow-primary/20'
            ]"
          >
            <span class="material-symbols-outlined text-[18px]">filter_list</span>
            <span>Filtros Avanzados</span>
          </button>
        </div>
      </div>

      <!-- Analytics Cards Stitch -->
      <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-6 relative z-10">
        <!-- Card 1: Solicitudes Totales -->
        <div class="bg-surface-container-low border border-outline-variant/10 rounded-xl p-6 shadow-sm relative overflow-hidden group">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-primary/10 rounded-full blur-xl group-hover:bg-primary/20 transition-all duration-500"></div>
          <div class="flex items-center justify-between mb-4">
            <h3 class="font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
              Solicitudes Totales
            </h3>
            <span class="material-symbols-outlined text-primary">analytics</span>
          </div>
          <div class="font-display-lg text-display-lg text-on-surface">
            {{ totalCount }}
          </div>
          <div class="mt-2 flex items-center gap-1 text-primary text-xs">
            <span class="material-symbols-outlined text-[16px]">history</span>
            <span class="font-caption text-caption">Total procesadas</span>
          </div>
        </div>

        <!-- Card 2: Aprobadas -->
        <div class="bg-surface-container-low border border-outline-variant/10 rounded-xl p-6 shadow-sm relative overflow-hidden group">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-secondary/10 rounded-full blur-xl group-hover:bg-secondary/20 transition-all duration-500"></div>
          <div class="flex items-center justify-between mb-4">
            <h3 class="font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
              Aprobadas
            </h3>
            <span class="material-symbols-outlined text-secondary">check_circle</span>
          </div>
          <div class="font-display-lg text-display-lg text-on-surface">
            {{ approvedCount }}
          </div>
          <div class="mt-2 flex items-center gap-1 text-secondary text-xs">
            <span class="material-symbols-outlined text-[16px]">trending_up</span>
            <span class="font-caption text-caption">{{ approvedPercentage }}% tasa de aprobación</span>
          </div>
        </div>

        <!-- Card 3: Rechazadas -->
        <div class="bg-surface-container-low border border-outline-variant/10 rounded-xl p-6 shadow-sm relative overflow-hidden group">
          <div class="absolute -right-6 -top-6 w-24 h-24 bg-error/10 rounded-full blur-xl group-hover:bg-error/20 transition-all duration-500"></div>
          <div class="flex items-center justify-between mb-4">
            <h3 class="font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
              Rechazadas
            </h3>
            <span class="material-symbols-outlined text-error">cancel</span>
          </div>
          <div class="font-display-lg text-display-lg text-on-surface">
            {{ rejectedCount }}
          </div>
          <div class="mt-2 flex items-center gap-1 text-error text-xs">
            <span class="material-symbols-outlined text-[16px]">trending_down</span>
            <span class="font-caption text-caption">{{ rejectedPercentage }}% tasa de rechazo</span>
          </div>
        </div>

        <!-- Card 4: Donut / Tasa de Aprobación -->
        <div class="bg-surface-container-low border border-outline-variant/10 rounded-xl p-6 shadow-sm relative overflow-hidden flex flex-col justify-center items-center text-center">
          <div class="w-20 h-20 mb-3 relative">
            <svg class="w-full h-full transform -rotate-90" viewBox="0 0 36 36">
              <path
                class="text-surface-container-highest"
                d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831"
                fill="none"
                stroke="currentColor"
                stroke-width="3"
              ></path>
              <path
                class="text-primary transition-all duration-700"
                :stroke-dasharray="`${approvedPercentage}, 100`"
                d="M18 2.0845 a 15.9155 15.9155 0 0 1 0 31.831 a 15.9155 15.9155 0 0 1 0 -31.831"
                fill="none"
                stroke="currentColor"
                stroke-width="3"
              ></path>
            </svg>
            <div class="absolute inset-0 flex items-center justify-center font-label-md text-label-md text-on-surface">
              {{ approvedPercentage }}%
            </div>
          </div>
          <h3 class="font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
            Tasa de Aprobación
          </h3>
        </div>
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
          @click="store.fetchHistory"
          class="text-xs uppercase font-bold tracking-wider underline hover:opacity-80 ml-4"
        >
          Reintentar
        </button>
      </div>

      <!-- Main Container: Search + Table -->
      <div class="bg-surface-container rounded-xl overflow-hidden shadow-xl border border-outline-variant/10 relative z-10">
        <!-- Search and Controls Toolbar Stitch -->
        <div class="p-6 flex flex-col md:flex-row gap-4 justify-between items-center border-b border-surface-container-high/50">
          <div class="relative w-full md:w-96 group">
            <span class="material-symbols-outlined absolute left-4 top-1/2 -translate-y-1/2 text-on-surface-variant/50 group-focus-within:text-primary transition-colors">
              search
            </span>
            <input
              v-model="searchQuery"
              class="w-full bg-surface-container-high/50 text-on-surface font-body-md py-2.5 pl-12 pr-4 rounded-lg focus:outline-none focus:bg-surface-container-highest transition-colors placeholder:text-on-surface-variant/40 border border-transparent focus:border-primary/30"
              placeholder="Buscar por nombre, correo electrónico o motivo..."
              type="text"
            />
          </div>

          <div class="flex gap-3 w-full md:w-auto flex-wrap sm:flex-nowrap">
            <select
              v-model="statusFilter"
              class="bg-surface-container-high/50 text-on-surface font-label-md py-2.5 px-4 rounded-lg focus:outline-none focus:bg-surface-container-highest cursor-pointer border border-transparent focus:border-primary/30 text-sm"
            >
              <option value="all">Todos los Estados</option>
              <option value="approved">Aprobado</option>
              <option value="rejected">Rechazado</option>
            </select>

            <select
              v-model="sortOrder"
              class="bg-surface-container-high/50 text-on-surface font-label-md py-2.5 px-4 rounded-lg focus:outline-none focus:bg-surface-container-highest cursor-pointer border border-transparent focus:border-primary/30 text-sm"
            >
              <option value="desc">Más Recientes</option>
              <option value="asc">Más Antiguas</option>
            </select>
          </div>
        </div>

        <!-- Filtros Avanzados Colapsables -->
        <div
          v-if="showAdvancedFilters"
          class="p-4 bg-surface-container-low border-b border-surface-container-high/50 flex flex-wrap items-center justify-between gap-4 text-xs text-on-surface-variant"
        >
          <div class="flex items-center gap-4 flex-wrap">
            <span>Elementos por página:</span>
            <div class="flex gap-2">
              <button
                v-for="count in [5, 10, 20, 50]"
                :key="count"
                type="button"
                @click="itemsPerPage = count; currentPage = 1"
                :class="[
                  itemsPerPage === count
                    ? 'bg-primary text-on-primary font-bold'
                    : 'bg-surface-container-high text-on-surface-variant hover:text-on-surface',
                  'px-2.5 py-1 rounded transition-colors'
                ]"
              >
                {{ count }}
              </button>
            </div>
          </div>

          <button
            type="button"
            @click="searchQuery = ''; statusFilter = 'all'; sortOrder = 'desc'"
            class="text-primary hover:underline"
          >
            Limpiar filtros
          </button>
        </div>

        <!-- Estado de Carga -->
        <div v-if="loading" class="flex flex-col items-center justify-center py-16 text-on-surface-variant">
          <div class="w-8 h-8 rounded-full border-2 border-primary border-t-transparent animate-spin mb-3"></div>
          <p class="text-sm font-medium">Cargando historial de solicitudes...</p>
        </div>

        <!-- Data Table Stitch -->
        <div v-else class="overflow-x-auto">
          <table class="w-full text-left border-collapse">
            <thead>
              <tr class="bg-surface-container-low/50">
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
                  Usuario
                </th>
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
                  Estado
                </th>
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
                  Fecha de Procesamiento
                </th>
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
                  Procesado Por
                </th>
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider">
                  Comentario de Admin
                </th>
                <th class="py-4 px-6 font-label-md text-label-md text-on-surface-variant uppercase tracking-wider text-right">
                  Acciones
                </th>
              </tr>
            </thead>

            <tbody class="divide-y divide-surface-container-highest">
              <tr
                v-for="req in paginatedHistory"
                :key="req.id"
                class="hover:bg-surface-container-high/30 transition-colors group"
              >
                <!-- Usuario -->
                <td class="py-4 px-6 whitespace-nowrap">
                  <div class="flex items-center gap-3">
                    <div
                      :class="[
                        isApproved(req.status) ? 'bg-primary-container text-on-primary-container' : 'bg-surface-variant text-on-surface',
                        'w-10 h-10 rounded-full flex items-center justify-center font-headline-md text-headline-md font-bold text-sm'
                      ]"
                    >
                      {{ getInitials(req.nombre) }}
                    </div>
                    <div>
                      <div class="font-headline-md text-[16px] leading-[24px] text-on-surface">
                        {{ req.nombre }}
                      </div>
                      <div class="font-caption text-caption text-on-surface-variant opacity-70">
                        {{ req.correo }}
                      </div>
                    </div>
                  </div>
                </td>

                <!-- Estado -->
                <td class="py-4 px-6 whitespace-nowrap">
                  <span
                    v-if="isApproved(req.status)"
                    class="inline-flex items-center gap-1.5 py-1 px-3 rounded-full bg-secondary-container/20 text-secondary font-label-md text-[12px] tracking-wide"
                  >
                    <span class="w-1.5 h-1.5 rounded-full bg-secondary"></span>
                    Aprobado
                  </span>
                  <span
                    v-else
                    class="inline-flex items-center gap-1.5 py-1 px-3 rounded-full bg-error/10 text-error font-label-md text-[12px] tracking-wide"
                  >
                    <span class="w-1.5 h-1.5 rounded-full bg-error"></span>
                    Rechazado
                  </span>
                </td>

                <!-- Fecha de Procesamiento -->
                <td class="py-4 px-6 whitespace-nowrap">
                  <div class="font-body-md text-on-surface text-sm">
                    {{ formatDate(req.fecha_solicitud) }}
                  </div>
                  <div class="font-caption text-caption text-on-surface-variant opacity-70 text-xs">
                    {{ formatTime(req.fecha_solicitud) }}
                  </div>
                </td>

                <!-- Procesado Por -->
                <td class="py-4 px-6 whitespace-nowrap">
                  <div class="flex items-center gap-2">
                    <div class="w-6 h-6 rounded-full bg-surface-variant flex items-center justify-center font-caption text-[10px] text-on-surface-variant">
                      AD
                    </div>
                    <span class="font-body-md text-on-surface text-[14px]">Admin</span>
                  </div>
                </td>

                <!-- Comentario de Admin -->
                <td class="py-4 px-6 max-w-[240px]">
                  <p
                    class="font-body-md text-[14px] text-on-surface-variant truncate"
                    :title="req.motivo_rechazo || (isApproved(req.status) ? 'Solicitud aprobada' : 'Sin comentarios')"
                  >
                    {{ req.motivo_rechazo || (isApproved(req.status) ? 'Solicitud aprobada exitosamente' : 'Sin motivo registrado') }}
                  </p>
                </td>

                <!-- Acciones -->
                <td class="py-4 px-6 text-right whitespace-nowrap">
                  <button
                    type="button"
                    @click="viewDetails(req)"
                    class="text-on-surface-variant hover:text-primary transition-colors p-2 rounded-full hover:bg-surface-variant opacity-70 group-hover:opacity-100 focus:opacity-100"
                    title="Ver detalles"
                  >
                    <span class="material-symbols-outlined text-[20px]">visibility</span>
                  </button>
                </td>
              </tr>

              <!-- Estado Vacío -->
              <tr v-if="filteredHistory.length === 0">
                <td colspan="6" class="px-6 py-12 text-center text-on-surface-variant">
                  <div class="flex flex-col items-center justify-center gap-3">
                    <div class="w-12 h-12 rounded-full bg-surface-variant/50 flex items-center justify-center text-on-surface-variant">
                      <span class="material-symbols-outlined text-2xl">inbox</span>
                    </div>
                    <p class="font-body-md text-sm">
                      {{ searchQuery || statusFilter !== 'all' ? 'No se encontraron solicitudes con los filtros aplicados.' : 'No hay solicitudes procesadas en el historial.' }}
                    </p>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>

        <!-- Paginación Footer Stitch -->
        <div
          v-if="!loading && filteredHistory.length > 0"
          class="bg-surface-container-low px-6 py-4 flex flex-col sm:flex-row items-center justify-between gap-4 border-t border-surface-container-high/50"
        >
          <div class="font-caption text-caption text-on-surface-variant">
            Mostrando <span class="text-on-surface font-semibold">{{ startIndex + 1 }}</span> a <span class="text-on-surface font-semibold">{{ Math.min(endIndex, filteredHistory.length) }}</span> de <span class="text-on-surface font-semibold">{{ filteredHistory.length }}</span> entradas
          </div>

          <div class="flex items-center gap-2">
            <button
              type="button"
              @click="currentPage--"
              :disabled="currentPage === 1"
              class="w-8 h-8 rounded-full bg-surface-variant text-on-surface-variant flex items-center justify-center hover:bg-surface-bright hover:text-on-surface transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              title="Anterior"
            >
              <span class="material-symbols-outlined text-[18px]">chevron_left</span>
            </button>

            <button
              v-for="page in totalPages"
              :key="page"
              type="button"
              @click="currentPage = page"
              :class="[
                currentPage === page
                  ? 'bg-primary text-on-primary font-bold'
                  : 'hover:bg-surface-variant text-on-surface transition-colors',
                'w-8 h-8 rounded-full flex items-center justify-center font-label-md text-[14px]'
              ]"
            >
              {{ page }}
            </button>

            <button
              type="button"
              @click="currentPage++"
              :disabled="currentPage === totalPages"
              class="w-8 h-8 rounded-full bg-surface-variant text-on-surface-variant flex items-center justify-center hover:bg-surface-bright hover:text-on-surface transition-colors disabled:opacity-50 disabled:cursor-not-allowed"
              title="Siguiente"
            >
              <span class="material-symbols-outlined text-[18px]">chevron_right</span>
            </button>
          </div>
        </div>
      </div>

      <!-- Modal de Detalle de Solicitud -->
      <div
        v-if="selectedRequest !== null"
        class="fixed inset-0 z-50 bg-black/70 backdrop-blur-sm flex items-center justify-center p-4 animate-fade-in-up"
      >
        <div class="bg-surface-container border border-outline-variant/20 rounded-2xl p-6 w-full max-w-lg shadow-2xl space-y-5">
          <div class="flex items-center justify-between border-b border-outline-variant/10 pb-4">
            <div class="flex items-center gap-3">
              <div
                :class="[
                  isApproved(selectedRequest.status) ? 'bg-primary-container text-on-primary-container' : 'bg-error-container text-on-error-container',
                  'w-10 h-10 rounded-xl flex items-center justify-center font-bold text-sm'
                ]"
              >
                {{ getInitials(selectedRequest.nombre) }}
              </div>
              <div>
                <h3 class="text-lg font-bold font-headline-md text-on-surface">Detalle de Solicitud</h3>
                <p class="text-xs text-on-surface-variant">ID de solicitud: #{{ selectedRequest.id }}</p>
              </div>
            </div>
            <button
              type="button"
              @click="closeDetails"
              class="p-1 rounded-lg text-on-surface-variant hover:text-on-surface hover:bg-surface-variant transition-colors"
            >
              <span class="material-symbols-outlined text-xl">close</span>
            </button>
          </div>

          <div class="space-y-4 text-sm">
            <div class="grid grid-cols-2 gap-4">
              <div>
                <p class="text-xs font-semibold text-on-surface-variant uppercase tracking-wider">Nombre</p>
                <p class="font-medium text-on-surface mt-0.5">{{ selectedRequest.nombre }}</p>
              </div>
              <div>
                <p class="text-xs font-semibold text-on-surface-variant uppercase tracking-wider">Correo</p>
                <p class="font-medium text-on-surface mt-0.5 break-all">{{ selectedRequest.correo }}</p>
              </div>
            </div>

            <div class="grid grid-cols-2 gap-4">
              <div>
                <p class="text-xs font-semibold text-on-surface-variant uppercase tracking-wider">Estado</p>
                <span
                  v-if="isApproved(selectedRequest.status)"
                  class="inline-flex items-center gap-1.5 py-1 px-3 mt-1 rounded-full bg-secondary-container/20 text-secondary font-label-md text-[12px] tracking-wide"
                >
                  <span class="w-1.5 h-1.5 rounded-full bg-secondary"></span>
                  Aprobado
                </span>
                <span
                  v-else
                  class="inline-flex items-center gap-1.5 py-1 px-3 mt-1 rounded-full bg-error/10 text-error font-label-md text-[12px] tracking-wide"
                >
                  <span class="w-1.5 h-1.5 rounded-full bg-error"></span>
                  Rechazado
                </span>
              </div>
              <div>
                <p class="text-xs font-semibold text-on-surface-variant uppercase tracking-wider">Fecha</p>
                <p class="font-medium text-on-surface mt-0.5">
                  {{ formatDate(selectedRequest.fecha_solicitud) }} {{ formatTime(selectedRequest.fecha_solicitud) }}
                </p>
              </div>
            </div>

            <div v-if="selectedRequest.motivo_rechazo || !isApproved(selectedRequest.status)">
              <p class="text-xs font-semibold text-on-surface-variant uppercase tracking-wider">Motivo de Rechazo</p>
              <div class="mt-1 p-3 rounded-lg bg-surface-dim border border-outline-variant/10 text-on-surface text-sm">
                {{ selectedRequest.motivo_rechazo || 'No se especificó un motivo de rechazo.' }}
              </div>
            </div>
          </div>

          <div class="flex justify-end pt-2 border-t border-outline-variant/10">
            <button
              type="button"
              @click="closeDetails"
              class="px-5 py-2.5 rounded-lg bg-surface-variant text-on-surface-variant hover:bg-surface-container-highest hover:text-on-surface font-semibold text-sm transition-colors"
            >
              Cerrar
            </button>
          </div>
        </div>
      </div>
    </div>
  </MainLayout>
</template>