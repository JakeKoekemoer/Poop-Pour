// Theme names as a const object for type-safe references
export const THEMES = {
  SOFT_NATURAL: 'soft-natural',
  FRESH_CHEERFUL: 'fresh-cheerful',
  WARM_MODERN: 'warm-modern',
  OCEAN_CALM: 'ocean-calm',
  MIDNIGHT_LUXE: 'midnight-luxe',
  BERRY_SOFT: 'berry-soft',
  FROSTED_MINT: 'frosted-mint',
  SUNSET_POP: 'sunset-pop',
  COZY_EARTH: 'cozy-earth',
} as const

// Derive ThemeName type from THEMES values
export type ThemeName = typeof THEMES[keyof typeof THEMES]

export type DarkMode = 'light' | 'dark' | 'auto'

export interface ThemeColors {
  primary: string
  secondary: string
  background: string
  surface: string
  accent: string
  text: string
}

export interface ThemeConfig {
  name: ThemeName
  displayName: string
  description: string
  emoji: string
  colors: ThemeColors
}

export const THEME_CONFIGS: Record<ThemeName, ThemeConfig> = {
  [THEMES.SOFT_NATURAL]: {
    name: THEMES.SOFT_NATURAL,
    displayName: 'Soft Natural',
    description: 'Calm + Modern',
    emoji: '🌿',
    colors: {
      primary: '#7FAF9A',
      secondary: '#F6E27A',
      background: '#FBF8EF',
      surface: '#FFFFFF',
      accent: '#5F7C6C',
      text: '#2E2E2E',
    },
  },
  [THEMES.FRESH_CHEERFUL]: {
    name: THEMES.FRESH_CHEERFUL,
    displayName: 'Fresh & Cheerful',
    description: 'Brighter but Still Clean',
    emoji: '🌞',
    colors: {
      primary: '#8FD6B5',
      secondary: '#FFD84D',
      background: '#F9FAF7',
      surface: '#FFFFFF',
      accent: '#2F5D50',
      text: '#1F2937',
    },
  },
  [THEMES.WARM_MODERN]: {
    name: THEMES.WARM_MODERN,
    displayName: 'Warm Modern',
    description: 'More Design-Forward',
    emoji: '🌼',
    colors: {
      primary: '#5E8B7E',
      secondary: '#E9B949',
      background: '#F5F1E8',
      surface: '#FFFFFF',
      accent: '#3F5F56',
      text: '#3A3A3A',
    },
  },
  [THEMES.OCEAN_CALM]: {
    name: THEMES.OCEAN_CALM,
    displayName: 'Ocean Calm',
    description: 'Cool • Trustworthy • Night-friendly',
    emoji: '🌊',
    colors: {
      primary: '#3A6EA5',
      secondary: '#7FC8A9',
      background: '#F6F9FC',
      surface: '#E6EFF7',
      accent: '#F4D35E',
      text: '#1F2A44',
    },
  },
  [THEMES.MIDNIGHT_LUXE]: {
    name: THEMES.MIDNIGHT_LUXE,
    displayName: 'Midnight Luxe',
    description: 'Premium • Calm • Modern',
    emoji: '🌙',
    colors: {
      primary: '#1F2933',
      secondary: '#3E7C59',
      background: '#111827',
      surface: '#24303F',
      accent: '#EAB308',
      text: '#F9FAFB',
    },
  },
  [THEMES.BERRY_SOFT]: {
    name: THEMES.BERRY_SOFT,
    displayName: 'Berry Soft',
    description: 'Playful • Warm • Friendly',
    emoji: '🍓',
    colors: {
      primary: '#C06C84',
      secondary: '#F8B195',
      background: '#FFF6F8',
      surface: '#FDECEF',
      accent: '#355C7D',
      text: '#2E2E2E',
    },
  },
  [THEMES.FROSTED_MINT]: {
    name: THEMES.FROSTED_MINT,
    displayName: 'Frosted Mint',
    description: 'Clean • Minimal • App-store ready',
    emoji: '🧊',
    colors: {
      primary: '#4CC9F0',
      secondary: '#80ED99',
      background: '#F8FAFC',
      surface: '#E2F3F5',
      accent: '#FFD166',
      text: '#0F172A',
    },
  },
  [THEMES.SUNSET_POP]: {
    name: THEMES.SUNSET_POP,
    displayName: 'Sunset Pop',
    description: 'Bold • Energetic',
    emoji: '🌅',
    colors: {
      primary: '#F97316',
      secondary: '#EC4899',
      background: '#FFF7ED',
      surface: '#FFE4D6',
      accent: '#22C55E',
      text: '#1C1917',
    },
  },
  [THEMES.COZY_EARTH]: {
    name: THEMES.COZY_EARTH,
    displayName: 'Cozy Earth',
    description: 'Natural • Gender-neutral • Calm',
    emoji: '🧸',
    colors: {
      primary: '#A3B18A',
      secondary: '#DDA15E',
      background: '#FEFAE0',
      surface: '#E9EDC9',
      accent: '#6C584C',
      text: '#3A3A3A',
    },
  },
}

export const DARK_MODE_CONFIG = {
  background: '#1F2A27',
  surface: '#2C3A36',
  text: '#E5E7EB',
  textSecondary: '#9CA3AF',
  border: '#374151',
}
