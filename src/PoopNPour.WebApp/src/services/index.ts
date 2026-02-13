/**
 * Central export point for all services.
 * Import services from here for easy access throughout the application.
 * 
 * @example
 * ```typescript
 * import { authService, userService, settingsService } from '@/services';
 * 
 * // Use in components - no type imports needed!
 * const response = await authService.login('username', 'password');
 * // TypeScript automatically infers the response type
 * ```
 */

// Export all service instances
export { authService } from './AuthService';
export { userService } from './UserService';
export { settingsService } from './SettingsService';

// Export common types
export type { ApiResponse, ApiError } from './types';

// Export API client helpers (for advanced use cases only)
export { getApiClient, setAuthToken, clearAuthToken, getAuthToken } from './api';
