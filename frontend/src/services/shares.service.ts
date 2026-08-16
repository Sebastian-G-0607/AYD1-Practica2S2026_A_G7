import { httpClient } from './http.client'
import type {
  SharedWithMeItem,
  MySharedItem,
  ShareUser,
  ShareReviewPayload,
  ShareReviewBatchPayload,
} from '@/types/shares.types'

export const sharesService = {
  async getSharedWithMe(destinatarioId?: number): Promise<SharedWithMeItem[]> {
    const url = destinatarioId
      ? `/api/shares/shared-with-me/${destinatarioId}`
      : '/api/shares/shared-with-me'
    const { data } = await httpClient.get<SharedWithMeItem[]>(url)
    return data
  },

  async getMyShares(remitenteId?: number): Promise<MySharedItem[]> {
    const url = remitenteId
      ? `/api/shares/my-shares/${remitenteId}`
      : '/api/shares/my-shares'
    const { data } = await httpClient.get<MySharedItem[]>(url)
    return data
  },

  async shareReview(payload: ShareReviewPayload): Promise<{ mensaje: string }> {
    const { data } = await httpClient.post<{ mensaje: string }>('/api/shares', payload)
    return data
  },

  async shareReviewBatch(
    payload: ShareReviewBatchPayload,
  ): Promise<{ mensaje: string; compartidos: number }> {
    const { data } = await httpClient.post<{ mensaje: string; compartidos: number }>(
      '/api/shares/batch',
      payload,
    )
    return data
  },

  async getUsers(search?: string, excludeUserId?: number): Promise<ShareUser[]> {
    const params = new URLSearchParams()
    if (search) params.append('search', search)
    if (excludeUserId) params.append('excludeUserId', excludeUserId.toString())

    const url = `/api/users${params.toString() ? `?${params.toString()}` : ''}`
    const { data } = await httpClient.get<ShareUser[]>(url)
    return data
  },

  async unshareReview(reseniaId: number, destinatarioId: number): Promise<{ mensaje: string }> {
    const { data } = await httpClient.delete<{ mensaje: string }>(
      `/api/shares/${reseniaId}/${destinatarioId}`,
    )
    return data
  },
}
