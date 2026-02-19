<script setup lang="ts">
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { Label } from '@/components/ui/label'
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>Account Settings</CardTitle>
      <CardDescription>
        Configure your account preferences
      </CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Email Notifications</Label>
          <p class="text-sm text-muted-foreground">
            Receive updates via email
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
      
      <Separator />
      
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Push Notifications</Label>
          <p class="text-sm text-muted-foreground">
            Get mobile notifications
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
      
      <Separator />
      
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Two-Factor Authentication</Label>
          <p class="text-sm text-muted-foreground">
            Add extra security
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
    </CardContent>
  </Card>
</template>
