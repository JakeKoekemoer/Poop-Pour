/// <reference types="vite/client" />

export {}

declare global {
  interface ImportMetaEnv {
    readonly VITE_SHOW_COMING_SOON?: string
    readonly VITE_LAUNCH_DATE?: string
  }
}

declare module 'vue-router' {
  interface RouteMeta {
    /** When true, authenticated users are redirected away from this route */
    guestOnly?: boolean
    /** Toast message shown to authenticated users who are redirected away from a guest-only route */
    guestOnlyMessage?: { summary: string; detail?: string }
  }
}
