<script setup lang="ts">
import { onMounted } from 'vue'
import { useRouter } from 'vue-router'
import Toast from 'primevue/toast'
import ThemeSwitcher from '@/components/generic/ThemeSwitcher'
import { useTheme } from '@/composables/useTheme'
import { useAppToast } from '@/composables/useAppToast'
import { useNotificationStore } from '@/stores'
import { toastPt } from '@/lib/toastPt'

const { initializeTheme } = useTheme()
const router = useRouter()
const toast = useAppToast()
const notificationStore = useNotificationStore()

router.afterEach(() => {
  const pending = notificationStore.flush()
  pending.forEach(({ severity, summary, detail }) => {
    toast[severity](summary, detail)
  })
})

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
