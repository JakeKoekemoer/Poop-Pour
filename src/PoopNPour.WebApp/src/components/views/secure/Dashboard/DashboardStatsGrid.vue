<script setup lang="ts">
import { TrendingUp, Baby, Clock } from 'lucide-vue-next'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
</script>

<template>
  <div class="grid gap-4 md:grid-cols-2 lg:grid-cols-3">
    <Card>
      <CardHeader class="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle class="text-sm font-medium">Total Activities</CardTitle>
        <TrendingUp class="h-4 w-4 text-muted-foreground" />
      </CardHeader>
      <CardContent>
        <div class="text-2xl font-bold">0</div>
        <p class="text-xs text-muted-foreground">
          No activities tracked yet
        </p>
      </CardContent>
    </Card>

    <Card>
      <CardHeader class="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle class="text-sm font-medium">Babies</CardTitle>
        <Baby class="h-4 w-4 text-muted-foreground" />
      </CardHeader>
      <CardContent>
        <div class="text-2xl font-bold">0</div>
        <p class="text-xs text-muted-foreground">
          Add your first baby
        </p>
      </CardContent>
    </Card>

    <Card>
      <CardHeader class="flex flex-row items-center justify-between space-y-0 pb-2">
        <CardTitle class="text-sm font-medium">Last Activity</CardTitle>
        <Clock class="h-4 w-4 text-muted-foreground" />
      </CardHeader>
      <CardContent>
        <div class="text-2xl font-bold">--</div>
        <p class="text-xs text-muted-foreground">
          No recent activity
        </p>
      </CardContent>
    </Card>
  </div>
</template>
