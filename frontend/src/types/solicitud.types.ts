export interface RegisterRequest {
  nombre: string
  correo: string
  contrasenia: string
  confirmacion_contrasenia: string
}

export interface RegisterResponse {
  id: number
  nombre: string
  correo: string
  status_id: number
  status: string
  fecha_solicitud: string
}

export interface ApiErrorResponse {
  message: string
}

export interface ValidationProblemDetails {
  type?: string
  title?: string
  status?: number
  errors: Record<string, string[]>
  traceId?: string
}
export interface Solicitud {
  id: number
  nombre: string
  correo: string
  fecha_solicitud: string
  status: string
  motivo_rechazo?: string
}

export interface ProcesarSolicitudRequest {
  solicitud_id: number
  aprobar: boolean
  motivo_rechazo?: string
}
