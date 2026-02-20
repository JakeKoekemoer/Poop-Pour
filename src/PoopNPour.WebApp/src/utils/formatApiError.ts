import type { ApiError } from '@/services/types'

// Prefers validation error messages over the generic top-level message.
export function formatApiError(error: ApiError): string {
  if (error.validationErrors) {
    const messages = Object.values(error.validationErrors).flat()
    if (messages.length > 0) {
      return messages.join(' ')
    }
  }
  return error.message
}
