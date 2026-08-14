export interface Resenia {
  id: number
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiquetaId: number
  etiqueta: string
  destacada: boolean
}

export interface ReseniaRequest {
  tituloPelicula: string
  calificacion: number
  comentario: string | null
  etiquetaId: number | null
  nuevaEtiqueta: string | null
}