<script setup lang="ts">
import { onMounted } from 'vue'
import ThemeSwitcher from '@/components/ThemeSwitcher.vue'
import { useTheme } from '@/composables/useTheme'

const { initializeTheme } = useTheme()

onMounted(() => {
  initializeTheme()
})
</script>

<template>
  <div class="min-h-screen bg-background text-foreground">
    <RouterView />
    <ThemeSwitcher />
  </div>
</template>
