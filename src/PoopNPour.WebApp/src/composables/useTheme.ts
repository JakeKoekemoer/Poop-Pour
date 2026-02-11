import { storeToRefs } from 'pinia'
import { useThemeStore } from '@/stores/theme'
import { THEME_CONFIGS } from '@/stores/theme/types'
import type { ThemeName, DarkMode, ThemeConfig } from '@/stores/theme/types'

export function useTheme() {
  const themeStore = useThemeStore()
  const { currentTheme, darkMode, isDark, systemPrefersDark } = storeToRefs(themeStore)

  // Get all available themes
  const themes = Object.values(THEME_CONFIGS)

  // Get current theme config
  const currentThemeConfig = (): ThemeConfig => {
    return THEME_CONFIGS[currentTheme.value]
  }

  // Set theme
  const setTheme = (theme: ThemeName) => {
    themeStore.setTheme(theme)
  }

  // Set dark mode
  const setDarkMode = (mode: DarkMode) => {
    themeStore.setDarkMode(mode)
  }

  // Toggle dark mode
  const toggleDarkMode = () => {
    themeStore.toggleDarkMode()
  }

  // Initialize theme
  const initializeTheme = () => {
    themeStore.initializeTheme()
  }

  return {
    // State
    currentTheme,
    darkMode,
    isDark,
    systemPrefersDark,
    
    // Computed
    themes,
    currentThemeConfig,
    
    // Actions
    setTheme,
    setDarkMode,
    toggleDarkMode,
    initializeTheme,
  }
}
