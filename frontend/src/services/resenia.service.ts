import { httpClient } from '@/services/http.client'
import type { Resenia, ReseniaRequest } from '@/types/resenia.types'

export async function getResenias(): Promise<Resenia[]> {
  const response = await httpClient.get<Resenia[]>('/api/resenias')
  return response.data
}

export async function createResenia(data: ReseniaRequest): Promise<Resenia> {
  const response = await httpClient.post<Resenia>('/api/resenias', data)
  return response.data
}

export async function updateResenia(
  id: number,
  data: ReseniaRequest,
): Promise<Resenia> {
  const response = await httpClient.put<Resenia>(`/api/resenias/${id}`, data)
  return response.data
}

export async function deleteResenia(id: number): Promise<void> {
  await httpClient.delete(`/api/resenias/${id}`)
}