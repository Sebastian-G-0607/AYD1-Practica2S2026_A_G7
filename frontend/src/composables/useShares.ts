import { ref } from 'vue'
import { sharesService } from '@/services/shares.service'
import type { SharedWithMeItem, MySharedItem } from '@/types/shares.types'
import { useAuthStore } from '@/stores/auth.store'

export function useShares() {
  const authStore = useAuthStore()
  const myShares = ref<MySharedItem[]>([])
  const sharedWithMe = ref<SharedWithMeItem[]>([])
  const isLoading = ref(false)
  const error = ref<string | null>(null)

  const fetchMyShares = async () => {
    try {
      isLoading.value = true
      error.value = null
      const currentUserId = authStore.user?.id
      const data = await sharesService.getMyShares(currentUserId)
      myShares.value = data
      return data
    } catch (err: any) {
      console.error('Error al cargar mis compartidas:', err)
      error.value =
        err.response?.data?.mensaje ||
        err.response?.data?.message ||
        'Error al cargar reseñas compartidas.'
      return []
    } finally {
      isLoading.value = false
    }
  }

  const fetchSharedWithMe = async () => {
    try {
      isLoading.value = true
      error.value = null
      const currentUserId = authStore.user?.id
      const data = await sharesService.getSharedWithMe(currentUserId)
      sharedWithMe.value = data
      return data
    } catch (err: any) {
      console.error('Error al cargar compartidos conmigo:', err)
      error.value =
        err.response?.data?.mensaje ||
        err.response?.data?.message ||
        'Error al cargar reseñas compartidas contigo.'
      return []
    } finally {
      isLoading.value = false
    }
  }

  const shareReview = async (reseniaId: number, destinatarioId: number) => {
    try {
      const currentUserId = authStore.user?.id
      const res = await sharesService.shareReview({
        reseniaId,
        usuarioDestinatarioId: destinatarioId,
        usuarioRemitenteId: currentUserId,
      })
      return res
    } catch (err: any) {
      console.error('Error al compartir reseña:', err)
      throw err
    }
  }

  const shareReviewBatch = async (reseniaId: number, userIds: number[]) => {
    try {
      const currentUserId = authStore.user?.id
      const res = await sharesService.shareReviewBatch({
        reseniaId,
        destinatariosIds: userIds,
        usuarioRemitenteId: currentUserId,
      })
      return res
    } catch (err: any) {
      console.error('Error al compartir reseña en lote:', err)
      throw err
    }
  }

  const unshareReview = async (reseniaId: number, destinatarioId: number) => {
    try {
      const res = await sharesService.unshareReview(reseniaId, destinatarioId)
      await fetchMyShares()
      return res
    } catch (err: any) {
      console.error('Error al dejar de compartir reseña:', err)
      throw err
    }
  }

  return {
    myShares,
    sharedWithMe,
    isLoading,
    error,
    fetchMyShares,
    fetchSharedWithMe,
    shareReview,
    shareReviewBatch,
    unshareReview,
  }
}