<script setup lang="ts">
import { Settings } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'

import DatabaseSettingsCard from '@/components/views/admin/Settings/DatabaseSettingsCard.vue'
import SecuritySettingsCard from '@/components/views/admin/Settings/SecuritySettingsCard.vue'
import NotificationSettingsCard from '@/components/views/admin/Settings/NotificationSettingsCard.vue'
import AppearanceSettingsCard from '@/components/views/admin/Settings/AppearanceSettingsCard.vue'
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Settings class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">System Settings</h1>
      </div>
      <Badge variant="destructive">Admin Area</Badge>
    </div>

    <Separator />

    <div class="grid gap-6 md:grid-cols-2">
      <DatabaseSettingsCard />
      <SecuritySettingsCard />
      <NotificationSettingsCard />
      <AppearanceSettingsCard />
    </div>
  </div>
</template>
