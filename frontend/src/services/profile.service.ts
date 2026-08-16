import { httpClient } from './http.client'

export interface UserProfileData {
  id: number
  nombre: string
  correo: string
  rol: string
  totalResenias: number
  totalCompartidas: number
}

export interface UpdateProfilePayload {
  nombre: string
  correo: string
  contraseniaActual?: string
  nuevaContrasenia?: string
}

export interface UpdateProfileResponse {
  id: number
  nombre: string
  correo: string
  mensaje: string
}

export const getProfile = async (id?: number): Promise<UserProfileData> => {
  const url = id ? `/api/users/profile/${id}` : '/api/users/profile'
  const response = await httpClient.get<UserProfileData>(url)
  return response.data
}

export const updateProfile = async (
  id: number,
  payload: UpdateProfilePayload,
): Promise<UpdateProfileResponse> => {
  const response = await httpClient.put<UpdateProfileResponse>(
    `/api/users/actualizar/${id}`,
    payload,
  )
  return response.data
}