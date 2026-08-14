import { ref } from 'vue'

export interface SharedReview {
  reseniaId: number
  tituloPelicula: string
  comentario: string
  usuarioRemitenteId: number
  usuarioDestinatarioId: number
  destinatarioNombre?: string 
  destinatarioInitials?: string
  remitenteNombre?: string 
  remitenteInitials?: string
  fechaEnvio: string 
  visto: boolean
}

// Estado reactivo simulado (Mock Data)
const mockMyShares = ref<SharedReview[]>([
  {
    reseniaId: 1,
    tituloPelicula: 'Blade Runner 2049',
    comentario: '"La obra maestra de Villeneuve es visualmente deslumbrante..."',
    usuarioRemitenteId: 1, 
    usuarioDestinatarioId: 2,
    destinatarioNombre: 'Sarah Jenkins',
    destinatarioInitials: 'SJ',
    fechaEnvio: 'Oct 12, 2023',
    visto: true
  },
  {
    reseniaId: 2,
    tituloPelicula: 'Past Lives',
    comentario: '"Una delicada exploración de lo que podría haber sido..."',
    usuarioRemitenteId: 1,
    usuarioDestinatarioId: 3,
    destinatarioNombre: 'David Chen',
    destinatarioInitials: 'DC',
    fechaEnvio: 'Nov 05, 2023',
    visto: false
  }
])

// Reseñas que otros usuarios han compartido CONTIGO
const mockSharedWithMe = ref<SharedReview[]>([
  {
    reseniaId: 3,
    tituloPelicula: 'Dune: Part Two',
    comentario: '"La escala visual y el diseño de sonido son de otro planeta..."',
    usuarioRemitenteId: 2, 
    usuarioDestinatarioId: 4, 
    remitenteNombre: 'Marcus Thorne',
    remitenteInitials: 'MT',
    fechaEnvio: 'Mar 15, 2024',
    visto: false // No la has visto aún
  },
  {
    reseniaId: 4,
    tituloPelicula: 'Oppenheimer',
    comentario: '"Un thriller histórico tenso que no te suelta..."',
    usuarioRemitenteId: 3,
    usuarioDestinatarioId: 4,
    remitenteNombre: 'Elena Rodriguez',
    remitenteInitials: 'ER',
    fechaEnvio: 'Jul 22, 2023',
    visto: true
  }
])

export function useShares() {
  
  const getMyShares = () => {
    return mockMyShares
  }

  const shareReview = async (reseniaId: number, destinatarioId: number) => {
    console.log(`Simulando petición POST a /api/shares...`)
    console.log(`Reseña: ${reseniaId} compartida con Usuario: ${destinatarioId}`)
    
    // Aquí implementaremos el fetch() hacia tu backend de .NET más adelante
    return true
  }

  return {
    mockMyShares,
    mockSharedWithMe,
    getMyShares,
    shareReview
  }
}