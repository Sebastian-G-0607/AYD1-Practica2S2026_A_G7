import { defineStore } from 'pinia'
import { ref } from 'vue'
import { adminService } from '@/services/admin.service'
import type { Solicitud } from '@/types/solicitud.types'

export const useAdminStore = defineStore('admin', () => {
  const pendingRequests = ref<Solicitud[]>([])
  const history = ref<Solicitud[]>([])
  const loading = ref(false)
  const error = ref<string | null>(null)

  const fetchPending = async () => {
    loading.value = true
    error.value = null
    try {
      console.log('Fetching pending requests------------------------------------')
      pendingRequests.value = await adminService.getPendientes()
      console.log('Response:', pendingRequests.value)
    } catch (e: any) {
      console.log('Error completo:',e)
      error.value = e.response?.data?.message || 'Error al cargar solicitudes pendientes'
      console.error('Error fetching pending requests:', e)
    } finally {
      loading.value = false
    }
  }

  const fetchHistory = async () => {
    loading.value = true
    error.value = null
    try {
      history.value = await adminService.getHistorial()
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Error al cargar historial'
      console.error('Error fetching history:', e)
    } finally {
      loading.value = false
    }
  }

  const processRequest = async (solicitudId: number, aprobar: boolean, motivoRechazo?: string) => {
    loading.value = true
    error.value = null
    try {
      await adminService.procesarSolicitud({
        solicitud_id: solicitudId,
        aprobar,
        motivo_rechazo: motivoRechazo
      })
      await fetchPending()
    } catch (e: any) {
      error.value = e.response?.data?.message || 'Error al procesar solicitud'
      console.error('Error processing request:', e)
      throw e
    } finally {
      loading.value = false
    }
  }

  return {
    pendingRequests,
    history,
    loading,
    error,
    fetchPending,
    fetchHistory,
    processRequest
  }
})