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
