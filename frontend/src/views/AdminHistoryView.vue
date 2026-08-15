<template>
  <div class="p-6 max-w-7xl mx-auto">
    <h1 class="text-3xl font-bold text-gray-800 mb-6">Historial de Solicitudes</h1>

    <div v-if="loading" class="flex justify-center py-8">
      <div class="text-gray-500">Cargando historial...</div>
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
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
              <th class="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Motivo Rechazo</th>
            </tr>
          </thead>
          <tbody class="bg-white divide-y divide-gray-200">
            <tr v-for="req in history" :key="req.id">
              <td class="px-6 py-4 whitespace-nowrap">{{ req.nombre }}</td>
              <td class="px-6 py-4 whitespace-nowrap">{{ req.correo }}</td>
              <td class="px-6 py-4 whitespace-nowrap">{{ formatDate(req.fecha_solicitud) }}</td>
              <td class="px-6 py-4 whitespace-nowrap">
                <span
                  :class="{
                    'bg-green-100 text-green-800': req.status === 'Aprobado',
                    'bg-red-100 text-red-800': req.status === 'Rechazado'
                  }"
                  class="px-2 py-1 rounded-full text-xs font-medium"
                >
                  {{ req.status }}
                </span>
              </td>
              <td class="px-6 py-4 whitespace-nowrap text-gray-500">
                {{ req.motivo_rechazo || '-' }}
              </td>
            </tr>
            <tr v-if="history.length === 0">
              <td colspan="5" class="px-6 py-8 text-center text-gray-500">
                No hay solicitudes procesadas
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { onMounted } from 'vue'
import { useAdminStore } from '@/stores/admin.store'

const store = useAdminStore()
const { history, loading, error } = store

const formatDate = (date: string) => {
  return new Date(date).toLocaleDateString('es-GT', {
    year: 'numeric',
    month: 'long',
    day: 'numeric'
  })
}

onMounted(() => {
  store.fetchHistory()
})
</script>