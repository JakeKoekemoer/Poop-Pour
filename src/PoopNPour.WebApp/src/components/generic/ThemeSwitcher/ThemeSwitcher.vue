<script setup lang="ts">
import { Palette, Sun, Moon, Monitor } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@/components/ui/popover'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { Label } from '@/components/ui/label'
import { Separator } from '@/components/ui/separator'
import { RadioGroup } from '@/components/ui/radio-group'
import { useTheme } from '@/composables/useTheme'
import type { ThemeName, DarkMode } from '@/stores/theme/types'
import ThemeOption from './ThemeOption.vue'
import DarkModeOption from './DarkModeOption.vue'

const { currentTheme, darkMode, themes, setTheme, setDarkMode } = useTheme()

const darkModeOptions = [
  { mode: 'light' as const, label: 'Light', icon: Sun },
  { mode: 'auto' as const, label: 'Auto', icon: Monitor },
  { mode: 'dark' as const, label: 'Dark', icon: Moon },
]
</script>

<template>
  <div class="fixed bottom-6 right-6 z-50">
    <Popover>
      <!-- Trigger Button -->
      <PopoverTrigger as-child>
        <Button
          size="icon-lg"
          variant="outline"
          class="rounded-full shadow-lg bg-card border-2 hover:scale-105 transition-transform"
          aria-label="Theme switcher"
        >
          <Palette class="w-5 h-5" />
        </Button>
      </PopoverTrigger>

      <!-- Popover Content -->
      <PopoverContent
        class="w-80 max-h-[calc(100vh-10rem)] overflow-y-auto p-0"
        align="end"
        :side-offset="8"
      >
        <Card class="border-0 shadow-none">
          <CardHeader class="pb-3">
            <CardTitle>Theme Settings</CardTitle>
          </CardHeader>

          <CardContent class="space-y-6 pb-4">
            <!-- Color Themes Section -->
            <div class="space-y-3">
              <Label class="text-sm font-medium">Color Theme</Label>
              <RadioGroup
                :model-value="currentTheme"
                @update:model-value="(value) => setTheme(value as ThemeName)"
                class="grid gap-2"
              >
                <ThemeOption
                  v-for="theme in themes"
                  :key="theme.name"
                  :theme="theme"
                  :value="theme.name"
                  :is-selected="currentTheme === theme.name"
                  @select="setTheme(theme.name)"
                />
              </RadioGroup>
            </div>

            <Separator />

            <!-- Dark Mode Section -->
            <div class="space-y-3">
              <Label class="text-sm font-medium">Appearance</Label>
              <RadioGroup
                :model-value="darkMode"
                @update:model-value="(value) => setDarkMode(value as DarkMode)"
                class="grid grid-cols-3 gap-2"
              >
                <DarkModeOption
                  v-for="option in darkModeOptions"
                  :key="option.mode"
                  :mode="option.mode"
                  :value="option.mode"
                  :label="option.label"
                  :icon="option.icon"
                  :is-selected="darkMode === option.mode"
                  @select="setDarkMode(option.mode)"
                />
              </RadioGroup>
            </div>

            <Separator />

            <!-- Footer Note -->
            <p class="text-xs text-muted-foreground text-center">
              Perfect for tired parents at 3AM 🌙
            </p>
          </CardContent>
        </Card>
      </PopoverContent>
    </Popover>
  </div>
</template>
