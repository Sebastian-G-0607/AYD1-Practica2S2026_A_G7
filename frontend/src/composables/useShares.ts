import { ref } from 'vue'

export interface SharedReview {
  reseniaId: number
  tituloPelicula: string
  comentario: string
  usuarioRemitenteId: number
  usuarioDestinatarioId: number
  destinatarioNombre?: string 
  destinatarioInitials?: string
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
    getMyShares,
    shareReview
  }
}