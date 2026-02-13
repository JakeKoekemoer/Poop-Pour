import { storeToRefs } from 'pinia'
import { useThemeStore } from '@/stores/theme'
import { THEME_CONFIGS } from '@/stores/theme/types'
import type { ThemeName, DarkMode, ThemeConfig } from '@/stores/theme/types'

export function useTheme() {
  const themeStore = useThemeStore()
  const { currentTheme, darkMode, isDark, systemPrefersDark } = storeToRefs(themeStore)

  const themes = Object.values(THEME_CONFIGS)

  const currentThemeConfig = (): ThemeConfig => {
    return THEME_CONFIGS[currentTheme.value]
  }

  const setTheme = (theme: ThemeName) => {
    themeStore.setTheme(theme)
  }

  const setDarkMode = (mode: DarkMode) => {
    themeStore.setDarkMode(mode)
  }

  const toggleDarkMode = () => {
    themeStore.toggleDarkMode()
  }

  const initializeTheme = () => {
    themeStore.initializeTheme()
  }

  return {
    currentTheme,
    darkMode,
    isDark,
    systemPrefersDark,
    
    themes,
    currentThemeConfig,
    
    setTheme,
    setDarkMode,
    toggleDarkMode,
    initializeTheme,
  }
}
