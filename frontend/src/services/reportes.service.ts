import { httpClient } from '@/services/http.client'

export interface UsuarioTopResenias {
  idUsuario: number
  nombre: string
  cantidadResenias: number
}

export interface UsuarioTopCompartidos {
  idUsuario: number
  nombre: string
  cantidadCompartidos: number
}

export const reportesService = {
  async obtenerTopResenias(): Promise<UsuarioTopResenias[]> {
    const response = await httpClient.get('/reportes/top-resenias')
    return response.data
  },

  async obtenerTopCompartidos(): Promise<UsuarioTopCompartidos[]> {
    const response = await httpClient.get('/reportes/top-compartidos')
    return response.data
  },
}