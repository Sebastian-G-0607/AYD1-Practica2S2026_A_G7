<template>
  <MainLayout>
    <div class="p-6 max-w-7xl mx-auto">
      <h1 class="text-3xl font-bold text-gray-800 mb-6">Solicitudes de Registro</h1>

      <div v-if="loading" class="flex justify-center py-8">
        <div class="text-gray-500">Cargando solicitudes...</div>
      </div>

      <div v-else-if="error" class="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded">
        {{ error }}
      </div>

      <div v-else>
        <div class="bg-white shadow-md rounded-lg overflow-hidden">
          <table class="min-w-full divide-y divide-gray-200">
            <thead class="bg-gray-50">
              <tr>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Nombre</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Correo</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Fecha</th>
                <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Acciones</th>
              </tr>
            </thead>
            <tbody class="bg-white divide-y divide-gray-200">
              <tr v-for="req in pendingRequests" :key="req.id">
                <td class="px-6 py-4 whitespace-nowrap">{{ req.nombre }}</td>
                <td class="px-6 py-4 whitespace-nowrap">{{ req.correo }}</td>
                <td class="px-6 py-4 whitespace-nowrap">{{ formatDate(req.fecha_solicitud) }}</td>
                <td class="px-6 py-4 whitespace-nowrap space-x-2">
                  <button
                    @click="handleApprove(req.id)"
                    class="bg-green-500 hover:bg-green-600 text-white font-medium py-1 px-4 rounded transition"
                    :disabled="loading"
                  >
                    Aceptar
                  </button>
                  <button
                    @click="openRejectModal(req.id)"
                    class="bg-red-500 hover:bg-red-600 text-white font-medium py-1 px-4 rounded transition"
                    :disabled="loading"
                  >
                    Rechazar
                  </button>
                </td>
              </tr>
              <tr v-if="pendingRequests.length === 0">
                <td colspan="4" class="px-6 py-8 text-center text-gray-500">
                  No hay solicitudes pendientes
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <!-- Modal de rechazo -->
      <div v-if="showRejectModal" class="fixed inset-0 bg-black bg-opacity-50 flex items-center justify-center z-50">
        <div class="bg-white rounded-lg p-6 w-full max-w-md">
          <h2 class="text-xl font-bold text-gray-800 mb-4">Motivo del rechazo</h2>
          <p class="text-sm text-gray-600 mb-3">Por favor, indica el motivo por el cual rechazas esta solicitud.</p>
          <textarea
            v-model="rejectReason"
            class="w-full border border-gray-300 rounded-lg p-3 focus:ring-2 focus:ring-blue-500 focus:border-transparent"
            rows="4"
            placeholder="Motivo del rechazo..."
          ></textarea>
          <div class="flex justify-end space-x-3 mt-4">
            <button
              @click="closeRejectModal"
              class="px-4 py-2 bg-gray-200 hover:bg-gray-300 rounded-lg transition"
            >
              Cancelar
            </button>
            <button
              @click="confirmReject"
              class="px-4 py-2 bg-red-500 hover:bg-red-600 text-white rounded-lg transition"
              :disabled="!rejectReason.trim()"
            >
              Rechazar
            </button>
          </div>
        </div>
      </div>
    </div>
  </MainLayout>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAdminStore } from '@/stores/admin.store'
import MainLayout from '@/layouts/MainLayout.vue'

const store = useAdminStore()
const { pendingRequests, loading, error } = store

const showRejectModal = ref(false)
const rejectRequestId = ref<number | null>(null)
const rejectReason = ref('')

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('es-GT', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

const handleApprove = async (id: number) => {
  if (confirm('¿Estás seguro de que quieres aceptar esta solicitud?')) {
    try {
      await store.processRequest(id, true)
    } catch (error) {
      // El error ya está en el store
    }
  }
}

const openRejectModal = (id: number) => {
  rejectRequestId.value = id
  rejectReason.value = ''
  showRejectModal.value = true
}

const closeRejectModal = () => {
  showRejectModal.value = false
  rejectRequestId.value = null
  rejectReason.value = ''
}

const confirmReject = async () => {
  if (rejectRequestId.value !== null && rejectReason.value.trim()) {
    try {
      await store.processRequest(rejectRequestId.value, false, rejectReason.value.trim())
      closeRejectModal()
    } catch (error) {
      // El error ya está en el store
    }
  }
}

onMounted(() => {
  store.fetchPending()
})
</script>