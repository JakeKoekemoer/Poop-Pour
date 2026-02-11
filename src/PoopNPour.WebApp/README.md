# poopnpour.webapp

Poop & Pour - Baby tracking made simple for tired parents.

## Recommended IDE Setup

[VS Code](https://code.visualstudio.com/) + [Vue (Official)](https://marketplace.visualstudio.com/items?itemName=Vue.volar) (and disable Vetur).

## Recommended Browser Setup

- Chromium-based browsers (Chrome, Edge, Brave, etc.):
  - [Vue.js devtools](https://chromewebstore.google.com/detail/vuejs-devtools/nhdogjmejiglipccpnnnanhbledajbpd)
  - [Turn on Custom Object Formatter in Chrome DevTools](http://bit.ly/object-formatters)
- Firefox:
  - [Vue.js devtools](https://addons.mozilla.org/en-US/firefox/addon/vue-js-devtools/)
  - [Turn on Custom Object Formatter in Firefox DevTools](https://fxdx.dev/firefox-devtools-custom-object-formatters/)

## Project Setup

```sh
npm install
```

### Environment Configuration

Copy the example environment file and configure as needed:

```sh
cp .env.example .env
```

Available environment variables:

| Variable | Type | Default | Description |
|----------|------|---------|-------------|
| `VITE_SHOW_COMING_SOON` | boolean | `true` | Show the coming soon page on the homepage |
| `VITE_LAUNCH_DATE` | string (ISO 8601) | `2026-03-01T00:00:00` | Launch date for the countdown timer |

**Example:**

```env
VITE_SHOW_COMING_SOON=true
VITE_LAUNCH_DATE=2026-03-01T00:00:00
```

When `VITE_SHOW_COMING_SOON` is set to `false`, the homepage will redirect to the login page instead of showing the coming soon page.

**Accessing Environment Variables:**

Use the `EnvConfig` utility class for type-safe access to environment variables:

```typescript
import { EnvConfig } from '@/lib/env'

// Type-safe getters with automatic conversion
EnvConfig.showComingSoon      // boolean
EnvConfig.launchDateString    // string (ISO 8601)
EnvConfig.launchDate          // Date object
```

The `EnvConfig` class handles:
- Type conversion (string → boolean, string → Date)
- Default values
- Centralized configuration

**Note:** All Vite environment variables are strings at runtime. The `EnvConfig` class converts them to the appropriate types.

### Compile and Hot-Reload for Development

```sh
npm run dev
```

### Type-Check, Compile and Minify for Production

```sh
npm run build
```

### Lint with [ESLint](https://eslint.org/)

```sh
npm run lint
```

## Project Structure

```
src/
├── assets/          # Static assets (CSS, images)
├── components/      # Vue components
│   ├── layouts/     # Layout components (Public, Secure, Admin)
│   ├── generic/     # Generic reusable components
│   └── ui/          # UI component library
├── composables/     # Vue composables
├── lib/             # Utility functions and helpers
├── routes/          # Vue Router configuration
│   ├── public/      # Public routes (no auth required)
│   ├── secure/      # Secure routes (auth required)
│   ├── admin/       # Admin routes (auth + admin role required)
│   ├── guards/      # Navigation guards
│   ├── helpers/     # Route helper utilities
│   └── constants.ts # Route name constants
├── stores/          # Pinia stores
└── views/           # Page components
    ├── public/      # Public pages
    ├── secure/      # Secure pages
    └── admin/       # Admin pages
```

## Routing

The application uses a structured routing system with three main sections:

- **Public Routes** (`/`): Accessible to everyone
- **Secure Routes** (`/secure/*`): Requires authentication
- **Admin Routes** (`/admin/*`): Requires authentication + admin role

Route names follow a consistent pattern using the `RouteHelper` class:

```typescript
import { RouteHelper } from '@/routes/helpers/RouteHelper'

// Generate route names
RouteHelper.GetPublicRouteName('login')    // 'public.login'
RouteHelper.GetSecureRouteName('dashboard') // 'secure.dashboard'
RouteHelper.GetAdminRouteName('users')      // 'admin.users'
```

## Type Support for `.vue` Imports in TS

TypeScript cannot handle type information for `.vue` imports by default, so we replace the `tsc` CLI with `vue-tsc` for type checking. In editors, we need [Volar](https://marketplace.visualstudio.com/items?itemName=Vue.volar) to make the TypeScript language service aware of `.vue` types.

## Customize configuration

See [Vite Configuration Reference](https://vite.dev/config/).
