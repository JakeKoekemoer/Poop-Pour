<script setup lang="ts">
import { LayoutDashboard } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'

import DashboardStatsGrid from '@/components/views/secure/Dashboard/DashboardStatsGrid.vue'
import DashboardWelcomeCard from '@/components/views/secure/Dashboard/DashboardWelcomeCard.vue'
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <LayoutDashboard class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Dashboard</h1>
      </div>
      <Badge variant="secondary">Secure Area</Badge>
    </div>

    <Separator />

    <DashboardStatsGrid />

    <DashboardWelcomeCard />
  </div>
</template>
