import type { RegisterRequest, RegisterResponse } from '@/types/solicitud.types'
import { httpClient } from './http.client'

export const solicitudService = {
  async crearSolicitud(data: RegisterRequest): Promise<RegisterResponse> {
    const { data: response } = await httpClient.post<RegisterResponse>('/api/solicitudes', data)
    return response
  },
}
