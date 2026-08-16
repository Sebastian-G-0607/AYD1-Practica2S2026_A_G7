export interface SharedWithMeItem {
  reseniaId: number
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiqueta?: string
  usuarioRemitenteId: number
  remitenteNombre: string
  remitenteCorreo: string
  usuarioAutorId?: number
  autorNombre?: string
}

export interface MySharedItem {
  reseniaId: number
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiqueta?: string
  usuarioDestinatarioId: number
  destinatarioNombre: string
  destinatarioCorreo: string
}

export interface ShareUser {
  id: number
  nombre: string
  correo: string
  rol: string
}

export interface ShareReviewPayload {
  reseniaId: number
  usuarioDestinatarioId: number
  usuarioRemitenteId?: number
}

export interface ShareReviewBatchPayload {
  reseniaId: number
  destinatariosIds: number[]
  usuarioRemitenteId?: number
}
