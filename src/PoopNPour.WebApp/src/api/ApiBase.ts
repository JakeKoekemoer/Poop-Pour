export class ApiBase {
  /**
   * NB: THIS COMMENT CANNOT LIVE ABOVE THE API BASE CLASS DECLARATION
   * https://stackoverflow.com/questions/61600278/nswag-extension-code-how-to-ensure-its-placed-at-the-start-of-the-file#comment140981214_61735295
   *
   * Base class for all generated API clients.
   * Provides authentication token injection via transformOptions method.
   *
   * This class is referenced by nswag.json configuration and will be extended
   * by all generated client classes (e.g., Client extends ApiBase).
   */

  private authToken = "";
  private onTokenExpired?: () => void;

  protected constructor() {}

  /**
   * Sets the authentication token to be injected into all API requests.
   * Call this method after user login to ensure all subsequent API calls include the token.
   *
   * @param token - The JWT bearer token
   * @example
   * ```typescript
   * const client = new Client('https://api.example.com');
   * client.setAuthToken('eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...');
   * ```
   */
  setAuthToken(token: string): void {
    this.authToken = token;
  }

  /**
   * Clears the authentication token.
   * Call this method on user logout.
   */
  clearAuthToken(): void {
    this.authToken = "";
  }

  /**
   * Gets the current authentication token.
   * @returns The current JWT bearer token or empty string if not set
   */
  getAuthToken(): string {
    return this.authToken;
  }

  /**
   * Sets a callback to be invoked when the token is expired.
   * This is typically used to redirect the user to the login page.
   *
   * @param callback - Function to call when token is expired
   * @example
   * ```typescript
   * const client = new Client('https://api.example.com');
   * client.setOnTokenExpired(() => {
   *   window.location.href = '/login';
   * });
   * ```
   */
  setOnTokenExpired(callback: () => void): void {
    this.onTokenExpired = callback;
  }

  /**
   * Decodes a JWT token payload.
   * Uses proper base64url decoding with Unicode support.
   * Based on: https://stackoverflow.com/questions/38552003/how-to-decode-jwt-token-without-using-a-library
   *
   * @param token - The JWT bearer token
   * @returns Decoded payload object or null if decoding fails
   */
  private decodeJwtPayload(token: string): any {
    try {
      // JWT structure: header.payload.signature
      const parts = token.split(".");
      if (parts.length !== 3) {
        return null; // Invalid token format
      }

      // Get the payload (second part)
      const base64Url = parts[1]!; // Safe due to length check above

      // Convert base64url to base64 (JWT uses base64url encoding)
      const base64 = base64Url.replace(/-/g, "+").replace(/_/g, "/");

      // Decode base64 with proper Unicode handling
      const jsonPayload = decodeURIComponent(
        atob(base64)
          .split("")
          .map((c) => "%" + ("00" + c.charCodeAt(0).toString(16)).slice(-2))
          .join(""),
      );

      return JSON.parse(jsonPayload);
    } catch (error) {
      console.error("Failed to decode JWT token:", error);
      return null;
    }
  }

  /**
   * Checks if a JWT token is expired.
   * @param token - The JWT bearer token
   * @returns true if token is expired, false otherwise
   */
  private isTokenExpired(token: string): boolean {
    const payload = this.decodeJwtPayload(token);

    if (!payload) {
      return true; // Invalid token, treat as expired
    }

    // Check expiration (exp claim is in seconds)
    if (!payload.exp) {
      return false; // No expiration claim, treat as valid
    }

    const currentTime = Math.floor(Date.now() / 1000);
    return payload.exp < currentTime;
  }

  /**
   * Transforms fetch options before each request is sent.
   * This method is automatically called by generated client methods.
   * Injects the Authorization header with the current bearer token.
   * Checks token expiration and redirects to login if expired.
   *
   * @param options - The fetch RequestInit options
   * @returns Promise resolving to the transformed options
   */
  protected transformOptions(options: RequestInit): Promise<RequestInit> {
    // Check if token is expired before making the request
    if (this.authToken && this.isTokenExpired(this.authToken)) {
      // Clear the expired token
      this.clearAuthToken();

      // Call the expired token callback if set
      if (this.onTokenExpired) {
        this.onTokenExpired();
      }

      // Reject the request to prevent it from being sent
      return Promise.reject(new Error("Authentication token has expired. Please log in again."));
    }

    // Convert headers to Headers object if needed
    const headers =
      options.headers instanceof Headers
        ? options.headers
        : new Headers(options.headers as HeadersInit);

    // Add Authorization header if token is set
    if (this.authToken) {
      headers.set("Authorization", `Bearer ${this.authToken}`);
    } else {
      // Fallback to API key if no auth token is available
      const apiKey = EnvConfig.apiKey;
      if (apiKey) {
        headers.set("Authorization", `Bearer ${apiKey}`);
      }
    }

    // Return transformed options
    options.headers = headers;
    return Promise.resolve(options);
  }
}
