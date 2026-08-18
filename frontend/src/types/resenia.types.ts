export interface Resenia {
  id: number
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiquetaId: number
  etiqueta: string
  destacada: boolean
  archivada: boolean
}

export interface ReseniaRequest {
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiquetaId: number | null
  nuevaEtiqueta: string | null
}

export interface ToggleResponse {
  id: number
  destacada?: boolean
  archivada?: boolean
  message: string
}

export interface Etiqueta {
  id: number
  descripcion: string
}