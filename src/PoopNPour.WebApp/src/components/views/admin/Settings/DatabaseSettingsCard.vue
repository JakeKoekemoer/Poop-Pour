<script setup lang="ts">
import { Database } from 'lucide-vue-next'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { Label } from '@/components/ui/label'
</script>

<template>
  <Card>
    <CardHeader>
      <div class="flex items-center gap-2">
        <Database class="w-5 h-5 text-primary" />
        <CardTitle>Database Settings</CardTitle>
      </div>
      <CardDescription>
        Configure database connections and backups
      </CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div class="space-y-2">
        <Label>Database Status</Label>
        <div class="flex items-center gap-2">
          <Badge variant="secondary">Connected</Badge>
        </div>
      </div>
      <Separator />
      <div class="space-y-2">
        <Label>Last Backup</Label>
        <p class="text-sm text-muted-foreground">Never</p>
      </div>
      <Button class="w-full" variant="outline" disabled>
        Configure Database
      </Button>
    </CardContent>
  </Card>
</template>
