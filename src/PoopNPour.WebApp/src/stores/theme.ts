import { defineStore } from 'pinia'
import { ref, computed, watch } from 'vue'
import type { ThemeName, DarkMode } from '@/types/theme'

const THEME_STORAGE_KEY = 'poop-pour-theme'
const DARK_MODE_STORAGE_KEY = 'poop-pour-dark-mode'

export const useThemeStore = defineStore('theme', () => {
  // State
  const currentTheme = ref<ThemeName>('soft-natural')
  const darkMode = ref<DarkMode>('auto')
  const systemPrefersDark = ref(false)

  // Computed
  const isDark = computed(() => {
    if (darkMode.value === 'dark') return true
    if (darkMode.value === 'light') return false
    return systemPrefersDark.value
  })

  // Initialize system preference watcher
  const initSystemPreference = () => {
    if (typeof window === 'undefined') return

    const mediaQuery = window.matchMedia('(prefers-color-scheme: dark)')
    systemPrefersDark.value = mediaQuery.matches

    // Listen for system preference changes
    mediaQuery.addEventListener('change', (e) => {
      systemPrefersDark.value = e.matches
    })
  }

  // Update HTML attributes
  const updateDOM = () => {
    if (typeof document === 'undefined') return

    const html = document.documentElement

    // Set theme data attribute
    html.setAttribute('data-theme', currentTheme.value)

    // Set dark class
    if (isDark.value) {
      html.classList.add('dark')
    } else {
      html.classList.remove('dark')
    }
  }

  // Set theme
  const setTheme = (theme: ThemeName) => {
    currentTheme.value = theme
    localStorage.setItem(THEME_STORAGE_KEY, theme)
    updateDOM()
  }

  // Set dark mode
  const setDarkMode = (mode: DarkMode) => {
    darkMode.value = mode
    localStorage.setItem(DARK_MODE_STORAGE_KEY, mode)
    updateDOM()
  }

  // Toggle dark mode (cycles through light -> auto -> dark)
  const toggleDarkMode = () => {
    const modes: DarkMode[] = ['light', 'auto', 'dark']
    const currentIndex = modes.indexOf(darkMode.value)
    const nextIndex = (currentIndex + 1) % modes.length
    setDarkMode(modes[nextIndex])
  }

  // Initialize theme from storage or defaults
  const initializeTheme = () => {
    if (typeof window === 'undefined') return

    // Initialize system preference
    initSystemPreference()

    // Load theme from storage
    const storedTheme = localStorage.getItem(THEME_STORAGE_KEY) as ThemeName | null
    const validThemes: ThemeName[] = [
      'soft-natural', 
      'fresh-cheerful', 
      'warm-modern',
      'ocean-calm',
      'midnight-luxe',
      'berry-soft',
      'frosted-mint',
      'sunset-pop',
      'cozy-earth'
    ]
    if (storedTheme && validThemes.includes(storedTheme)) {
      currentTheme.value = storedTheme
    }

    // Load dark mode from storage
    const storedDarkMode = localStorage.getItem(DARK_MODE_STORAGE_KEY) as DarkMode | null
    if (storedDarkMode && ['light', 'dark', 'auto'].includes(storedDarkMode)) {
      darkMode.value = storedDarkMode
    }

    // Update DOM
    updateDOM()
  }

  // Watch for changes to update DOM
  watch([isDark, currentTheme], () => {
    updateDOM()
  })

  return {
    // State
    currentTheme,
    darkMode,
    isDark,
    systemPrefersDark,
    
    // Actions
    setTheme,
    setDarkMode,
    toggleDarkMode,
    initializeTheme,
  }
})
