export class RouteHelper {
  private static readonly PUBLIC_PREFIX = 'public'
  private static readonly SECURE_PREFIX = 'secure'
  private static readonly ADMIN_PREFIX = 'admin'

  static GetPublicRouteName(name: string): string {
    return `${this.PUBLIC_PREFIX}.${name}`
  }

  static GetSecureRouteName(name: string): string {
    return `${this.SECURE_PREFIX}.${name}`
  }

  static GetAdminRouteName(name: string): string {
    return `${this.ADMIN_PREFIX}.${name}`
  }

  static isPublicRoute(routeName: string): boolean {
    return routeName.startsWith(`${this.PUBLIC_PREFIX}.`)
  }

  static isSecureRoute(routeName: string): boolean {
    return routeName.startsWith(`${this.SECURE_PREFIX}.`)
  }

  static isAdminRoute(routeName: string): boolean {
    return routeName.startsWith(`${this.ADMIN_PREFIX}.`)
  }

  static isProtectedRoute(routeName: string): boolean {
    return this.isSecureRoute(routeName) || this.isAdminRoute(routeName)
  }
}
