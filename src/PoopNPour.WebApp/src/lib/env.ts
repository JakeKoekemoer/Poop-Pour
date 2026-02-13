/**
 * Utility class for accessing environment variables with type safety
 */
export class EnvConfig {
  private static getBoolean(key: string, defaultValue: boolean = true): boolean {
    const value = import.meta.env[key]
    if (value === undefined) return defaultValue
    return value !== 'false'
  }

  private static getString(key: string, defaultValue: string): string {
    return import.meta.env[key] || defaultValue
  }

  private static getDate(key: string, defaultValue: string): Date {
    const value = this.getString(key, defaultValue)
    return new Date(value)
  }

  static get showComingSoon(): boolean {
    return this.getBoolean('VITE_SHOW_COMING_SOON', true)
  }

  static get launchDateString(): string {
    return this.getString('VITE_LAUNCH_DATE', '2026-03-01T00:00:00')
  }

  static get launchDate(): Date {
    return this.getDate('VITE_LAUNCH_DATE', '2026-03-01T00:00:00')
  }

  static get apiUrl(): string {
    return this.getString('VITE_API_URL', '').replace(/\/+$/, '')
  }

  static get apiKey(): string {
    return this.getString('VITE_API_KEY', '')
  }
}
