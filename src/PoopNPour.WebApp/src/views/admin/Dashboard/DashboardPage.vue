<script setup lang="ts">
import { ShieldCheck } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'

import AdminStatsGrid from '@/components/views/admin/Dashboard/AdminStatsGrid.vue'
import AdminOverviewCard from '@/components/views/admin/Dashboard/AdminOverviewCard.vue'
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

    <AdminStatsGrid />

    <AdminOverviewCard />
  </div>
</template>
