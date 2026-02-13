import { Client } from '@/api/api-client';
import router from '@/routes';
import { EnvConfig } from '@/lib/env';
import { RouteHelper } from '@/routes/helpers/RouteHelper';
import { PUBLIC_ROUTES } from '@/routes/constants';

/**
 * Singleton instance of the API client.
 * Initialized once with base URL and configured with onTokenExpired callback.
 */
let clientInstance: Client | null = null;

/**
 * Gets or creates the singleton API client instance.
 * Configures the client with base URL and token expiration handling.
 * 
 * @returns The singleton API client instance
 */
export function getApiClient(): Client {
  if (!clientInstance) {
    // Get base URL from environment or use default
    const baseUrl = EnvConfig.apiUrl;
    
    // Create the client instance
    clientInstance = new Client(baseUrl);
    
    // Configure single point for token expiration handling
    clientInstance.setOnTokenExpired(() => {
      console.warn('Token expired, clearing auth and redirecting to login');
      
      // Clear token from client
      clientInstance?.clearAuthToken();
      
      // Clear cookie (will be handled by user store)
      // The store clearAuth method will be called from the router
      
      // Redirect to login page
      router.push({ name: RouteHelper.GetPublicRouteName(PUBLIC_ROUTES.LOGIN) }).catch(err => {
        console.error('Failed to redirect to login:', err);
      });
    });
  }
  
  return clientInstance;
}

/**
 * Sets the authentication token for all API requests.
 * Should be called after successful login.
 * 
 * @param token - JWT bearer token
 */
export function setAuthToken(token: string): void {
  const client = getApiClient();
  client.setAuthToken(token);
}

/**
 * Clears the authentication token from the API client.
 * Should be called on logout.
 */
export function clearAuthToken(): void {
  const client = getApiClient();
  client.clearAuthToken();
}

/**
 * Gets the current authentication token from the API client.
 * 
 * @returns Current JWT token or empty string if not set
 */
export function getAuthToken(): string {
  const client = getApiClient();
  return client.getAuthToken();
}
