import { httpClient } from './http.client'
import type { Solicitud, ProcesarSolicitudRequest } from '@/types/solicitud.types'

export const adminService = {
  async getPendientes(): Promise<Solicitud[]> {
    const response = await httpClient.get('/admin/solicitudes/pendientes')
    return response.data
  },

  async getHistorial(): Promise<Solicitud[]> {
    const response = await httpClient.get('/admin/solicitudes/historial')
    return response.data
  },

  async procesarSolicitud(data: ProcesarSolicitudRequest): Promise<void> {
    await httpClient.post('/admin/solicitudes/procesar', data)
  }
}