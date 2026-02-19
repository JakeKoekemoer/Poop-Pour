<script setup lang="ts">
import { onMounted } from 'vue'
import Toast from 'primevue/toast'
import ThemeSwitcher from '@/components/generic/ThemeSwitcher'
import { useTheme } from '@/composables/useTheme'
import { toastPt } from '@/lib/toastPt'

const { initializeTheme } = useTheme()

onMounted(() => {
  initializeTheme()
})
</script>

<template>
  <div class="min-h-screen bg-background text-foreground">
    <RouterView />
    <ThemeSwitcher />
    <Toast :pt="toastPt" />
  </div>
</template>
