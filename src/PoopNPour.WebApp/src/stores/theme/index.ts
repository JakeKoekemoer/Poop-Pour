import { defineStore } from 'pinia'
import { THEMES } from './types'
import type { ThemeName, DarkMode } from './types'

export { THEMES } from './types'

const THEME_STORAGE_KEY = 'poop-pour-theme'
const DARK_MODE_STORAGE_KEY = 'poop-pour-dark-mode'

const VALID_THEMES = Object.values(THEMES) as readonly ThemeName[]
const VALID_DARK_MODES: readonly DarkMode[] = ['light', 'dark', 'auto'] as const

export const useThemeStore = defineStore('theme', {
  state: () => ({
    currentTheme: THEMES.SOFT_NATURAL as ThemeName,
    darkMode: 'auto' as DarkMode,
    systemPrefersDark: false,
  }),

  getters: {
    isDark: (state): boolean => {
      if (state.darkMode === 'dark') return true
      if (state.darkMode === 'light') return false
      return state.systemPrefersDark
    },
  },

  actions: {
    initSystemPreference() {
      if (typeof window === 'undefined') return

      const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)')
      this.systemPrefersDark = mediaQuery.matches

      mediaQuery.addEventListener('change', (e) => {
        this.systemPrefersDark = e.matches
        this.updateDOM()
      })
    },

    updateDOM() {
      if (typeof document === 'undefined') return

      const html = document.documentElement

      html.setAttribute('data-theme', this.currentTheme)

      if (this.isDark) {
        html.classList.add('dark')
      } else {
        html.classList.remove('dark')
      }
    },

    setTheme(theme: ThemeName) {
      this.currentTheme = theme
      localStorage.setItem(THEME_STORAGE_KEY, theme)
      this.updateDOM()
    },

    setDarkMode(mode: DarkMode) {
      this.darkMode = mode
      localStorage.setItem(DARK_MODE_STORAGE_KEY, mode)
      this.updateDOM()
    },

    toggleDarkMode() {
      const modes: readonly DarkMode[] = VALID_DARK_MODES
      const currentIndex = modes.indexOf(this.darkMode)
      const nextIndex = (currentIndex + 1) % modes.length
      const nextMode = modes[nextIndex]
      if (nextMode) {
        this.setDarkMode(nextMode)
      }
    },

    initializeTheme() {
      if (typeof window === 'undefined') return

      this.initSystemPreference()

      const storedTheme = localStorage.getItem(THEME_STORAGE_KEY) as ThemeName | null
      if (storedTheme && VALID_THEMES.includes(storedTheme)) {
        this.currentTheme = storedTheme
      }

      const storedDarkMode = localStorage.getItem(DARK_MODE_STORAGE_KEY) as DarkMode | null
      if (storedDarkMode && VALID_DARK_MODES.includes(storedDarkMode)) {
        this.darkMode = storedDarkMode
      }

      this.updateDOM()
    },
  },
})
