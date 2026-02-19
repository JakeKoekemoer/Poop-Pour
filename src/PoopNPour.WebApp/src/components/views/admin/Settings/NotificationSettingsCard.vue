<script setup lang="ts">
import { Bell } from 'lucide-vue-next'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { Label } from '@/components/ui/label'
import { Input } from '@/components/ui/input'
</script>

<template>
  <Card>
    <CardHeader>
      <div class="flex items-center gap-2">
        <Bell class="w-5 h-5 text-primary" />
        <CardTitle>Notification Settings</CardTitle>
      </div>
      <CardDescription>
        Configure system notifications
      </CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div class="space-y-2">
        <Label for="admin-email">Admin Email</Label>
        <Input
          id="admin-email"
          type="email"
          placeholder="admin@example.com"
          disabled
        />
      </div>
      <Separator />
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Email Alerts</Label>
          <p class="text-sm text-muted-foreground">
            System error notifications
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
      <Button class="w-full" variant="outline" disabled>
        Configure Notifications
      </Button>
    </CardContent>
  </Card>
</template>
