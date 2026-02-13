/**
 * Standardized API response wrapper.
 * Uses discriminated union for type-safe response handling.
 */
export type ApiResponse<T = void> = 
  | { success: true; data: T }
  | { success: false; error: ApiError };

/**
 * Standardized error structure for failed API responses
 */
export interface ApiError {
  /** Human-readable error message */
  message: string;
  /** HTTP status code if available */
  statusCode?: number;
  /** Validation errors keyed by field name */
  validationErrors?: Record<string, string[]>;
  /** Categorized error type for easier handling */
  type: 'network' | 'validation' | 'server' | 'auth' | 'unknown';
}
