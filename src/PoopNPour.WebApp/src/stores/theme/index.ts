import { defineStore } from 'pinia'
import { THEMES } from './types'
import type { ThemeName, DarkMode } from './types'

// Re-export THEMES for easy access
export { THEMES } from './types'

const THEME_STORAGE_KEY = 'poop-pour-theme'
const DARK_MODE_STORAGE_KEY = 'poop-pour-dark-mode'

// All valid theme names as a const array for type safety
const VALID_THEMES = Object.values(THEMES) as readonly ThemeName[]
const VALID_DARK_MODES: readonly DarkMode[] = ['light', 'dark', 'auto'] as const

export const useThemeStore = defineStore('theme', {
  // State
  state: () => ({
    currentTheme: THEMES.SOFT_NATURAL as ThemeName,
    darkMode: 'auto' as DarkMode,
    systemPrefersDark: false,
  }),

  // Getters
  getters: {
    isDark: (state): boolean => {
      if (state.darkMode === 'dark') return true
      if (state.darkMode === 'light') return false
      return state.systemPrefersDark
    },
  },

  // Actions
  actions: {
    /**
     * Initialize system preference watcher
     */
    initSystemPreference() {
      if (typeof window === 'undefined') return

      const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)')
      this.systemPrefersDark = mediaQuery.matches

      // Listen for system preference changes
      mediaQuery.addEventListener('change', (e) => {
        this.systemPrefersDark = e.matches
        this.updateDOM()
      })
    },

    /**
     * Update HTML attributes to reflect current theme
     */
    updateDOM() {
      if (typeof document === 'undefined') return

      const html = document.documentElement

      // Set theme data attribute
      html.setAttribute('data-theme', this.currentTheme)

      // Set dark class
      if (this.isDark) {
        html.classList.add('dark')
      } else {
        html.classList.remove('dark')
      }
    },

    /**
     * Set the current theme
     */
    setTheme(theme: ThemeName) {
      this.currentTheme = theme
      localStorage.setItem(THEME_STORAGE_KEY, theme)
      this.updateDOM()
    },

    /**
     * Set the dark mode preference
     */
    setDarkMode(mode: DarkMode) {
      this.darkMode = mode
      localStorage.setItem(DARK_MODE_STORAGE_KEY, mode)
      this.updateDOM()
    },

    /**
     * Toggle dark mode (cycles through light -> auto -> dark)
     */
    toggleDarkMode() {
      const modes: readonly DarkMode[] = VALID_DARK_MODES
      const currentIndex = modes.indexOf(this.darkMode)
      const nextIndex = (currentIndex + 1) % modes.length
      const nextMode = modes[nextIndex]
      if (nextMode) {
        this.setDarkMode(nextMode)
      }
    },

    /**
     * Initialize theme from storage or defaults
     */
    initializeTheme() {
      if (typeof window === 'undefined') return

      // Initialize system preference
      this.initSystemPreference()

      // Load theme from storage
      const storedTheme = localStorage.getItem(THEME_STORAGE_KEY) as ThemeName | null
      if (storedTheme && VALID_THEMES.includes(storedTheme)) {
        this.currentTheme = storedTheme
      }

      // Load dark mode from storage
      const storedDarkMode = localStorage.getItem(DARK_MODE_STORAGE_KEY) as DarkMode | null
      if (storedDarkMode && VALID_DARK_MODES.includes(storedDarkMode)) {
        this.darkMode = storedDarkMode
      }

      // Update DOM
      this.updateDOM()
    },
  },
})
