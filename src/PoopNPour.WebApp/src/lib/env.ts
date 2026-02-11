/**
 * Utility class for accessing environment variables with type safety
 */
export class EnvConfig {
  /**
   * Get a boolean environment variable
   * @param key - Environment variable key
   * @param defaultValue - Default value if not set
   */
  private static getBoolean(key: string, defaultValue: boolean = true): boolean {
    const value = import.meta.env[key]
    if (value === undefined) return defaultValue
    return value !== 'false'
  }

  /**
   * Get a string environment variable
   * @param key - Environment variable key
   * @param defaultValue - Default value if not set
   */
  private static getString(key: string, defaultValue: string): string {
    return import.meta.env[key] || defaultValue
  }

  /**
   * Get a Date environment variable
   * @param key - Environment variable key
   * @param defaultValue - Default value if not set (ISO 8601 string)
   */
  private static getDate(key: string, defaultValue: string): Date {
    const value = this.getString(key, defaultValue)
    return new Date(value)
  }

  /**
   * Whether to show the coming soon page
   */
  static get showComingSoon(): boolean {
    return this.getBoolean('VITE_SHOW_COMING_SOON', true)
  }

  /**
   * Launch date string (ISO 8601 format)
   */
  static get launchDateString(): string {
    return this.getString('VITE_LAUNCH_DATE', '2026-03-01T00:00:00')
  }

  /**
   * Launch date as Date object
   */
  static get launchDate(): Date {
    return this.getDate('VITE_LAUNCH_DATE', '2026-03-01T00:00:00')
  }
}
