<script setup lang="ts">
import { ref } from 'vue'
import { Palette, Sun, Moon, Monitor, Check } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { useTheme } from '@/composables/useTheme'
import type { ThemeName, DarkMode } from '@/stores/theme/types'

const { currentTheme, darkMode, themes, setTheme, setDarkMode } = useTheme()

const isOpen = ref(false)

const toggleOpen = () => {
  isOpen.value = !isOpen.value
}

const selectTheme = (theme: ThemeName) => {
  setTheme(theme)
}

const selectDarkMode = (mode: DarkMode) => {
  setDarkMode(mode)
}

const getDarkModeIcon = (mode: DarkMode) => {
  switch (mode) {
    case 'light':
      return Sun
    case 'dark':
      return Moon
    case 'auto':
      return Monitor
  }
}

const darkModeOptions: { mode: DarkMode; label: string }[] = [
  { mode: 'light', label: 'Light' },
  { mode: 'auto', label: 'Auto' },
  { mode: 'dark', label: 'Dark' },
]
</script>

<template>
  <div class="fixed bottom-6 right-6 z-50">
    <!-- Toggle Button -->
    <Button
      @click="toggleOpen"
      size="icon-lg"
      variant="outline"
      class="rounded-full shadow-lg bg-card border-2 hover:scale-105 transition-transform"
      aria-label="Theme switcher"
    >
      <Palette class="w-5 h-5" />
    </Button>

    <!-- Theme Switcher Panel -->
    <Transition
      enter-active-class="transition-all duration-200 ease-out"
      enter-from-class="opacity-0 scale-95 translate-y-2"
      enter-to-class="opacity-100 scale-100 translate-y-0"
      leave-active-class="transition-all duration-150 ease-in"
      leave-from-class="opacity-100 scale-100 translate-y-0"
      leave-to-class="opacity-0 scale-95 translate-y-2"
    >
      <div
        v-if="isOpen"
        class="absolute bottom-16 right-0 w-80 max-h-[calc(100vh-10rem)] overflow-y-auto bg-card border-2 border-border rounded-xl shadow-2xl p-4 space-y-4"
      >
        <!-- Header -->
        <div class="flex items-center justify-between pb-2 border-b border-border">
          <h3 class="text-lg font-semibold text-foreground">Theme Settings</h3>
          <Button
            @click="toggleOpen"
            size="icon-sm"
            variant="ghost"
            aria-label="Close theme switcher"
          >
            <span class="text-xl leading-none">&times;</span>
          </Button>
        </div>

        <!-- Color Themes -->
        <div class="space-y-2">
          <label class="text-sm font-medium text-foreground block">Color Theme</label>
          <div class="grid gap-2">
            <button
              v-for="theme in themes"
              :key="theme.name"
              @click="selectTheme(theme.name)"
              class="flex items-center gap-3 p-3 rounded-lg border-2 transition-all hover:border-primary"
              :class="[
                currentTheme === theme.name
                  ? 'border-primary bg-primary/5'
                  : 'border-border bg-background hover:bg-accent/50',
              ]"
            >
              <!-- Color Preview -->
              <div class="flex gap-1">
                <div
                  class="w-5 h-5 rounded-md border border-border/50"
                  :style="{ backgroundColor: theme.colors.primary }"
                />
                <div
                  class="w-5 h-5 rounded-md border border-border/50"
                  :style="{ backgroundColor: theme.colors.secondary }"
                />
              </div>

              <!-- Theme Info -->
              <div class="flex-1 text-left">
                <div class="font-medium text-foreground text-sm">
                  {{ theme.emoji }} {{ theme.displayName }}
                </div>
                <div class="text-xs text-muted-foreground">
                  {{ theme.description }}
                </div>
              </div>

              <!-- Check Icon -->
              <Check
                v-if="currentTheme === theme.name"
                class="w-5 h-5 text-primary flex-shrink-0"
              />
            </button>
          </div>
        </div>

        <!-- Dark Mode -->
        <div class="space-y-2">
          <label class="text-sm font-medium text-foreground block">Appearance</label>
          <div class="grid grid-cols-3 gap-2">
            <button
              v-for="option in darkModeOptions"
              :key="option.mode"
              @click="selectDarkMode(option.mode)"
              class="flex flex-col items-center gap-2 p-3 rounded-lg border-2 transition-all"
              :class="[
                darkMode === option.mode
                  ? 'border-primary bg-primary/5'
                  : 'border-border bg-background hover:bg-accent/50 hover:border-primary/50',
              ]"
            >
              <component
                :is="getDarkModeIcon(option.mode)"
                class="w-5 h-5"
                :class="darkMode === option.mode ? 'text-primary' : 'text-muted-foreground'"
              />
              <span
                class="text-xs font-medium"
                :class="darkMode === option.mode ? 'text-primary' : 'text-foreground'"
              >
                {{ option.label }}
              </span>
            </button>
          </div>
        </div>

        <!-- Footer Note -->
        <div class="pt-2 border-t border-border">
          <p class="text-xs text-muted-foreground text-center">
            Perfect for tired parents at 3AM 🌙
          </p>
        </div>
      </div>
    </Transition>
  </div>
</template>
