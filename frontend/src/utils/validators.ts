export const validators = {
  isValidEmail(email: string): boolean {
    return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.trim())
  },
  isNotEmpty(value: string): boolean {
    return value.trim().length > 0
  },
  minLength(value: string, min: number): boolean {
    return value.trim().length >= min
  },
}
