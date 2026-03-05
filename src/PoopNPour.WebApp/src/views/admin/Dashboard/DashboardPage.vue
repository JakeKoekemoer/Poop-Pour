<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { ShieldCheck } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { adminNavItems } from '@/config/adminNav'

const router = useRouter()

const dashboardItems = computed(() => adminNavItems.filter((item) => item.showOnDashboard))

function navigate(routeName: string) {
  router.push({ name: routeName })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <ShieldCheck class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Admin Dashboard</h1>
      </div>
      <Badge variant="destructive">Admin Area</Badge>
    </div>

    <Separator />

    <div class="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
      <Button
        v-for="item in dashboardItems"
        :key="item.routeName"
        variant="ghost"
        class="flex flex-col items-center justify-center gap-3 rounded-xl border bg-card p-6 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
        @click="navigate(item.routeName)"
      >
        <component :is="item.icon" class="w-10 h-10" />
        <span class="text-sm font-medium text-center">{{ item.label }}</span>
      </Button>
    </div>
  </div>
</template>
