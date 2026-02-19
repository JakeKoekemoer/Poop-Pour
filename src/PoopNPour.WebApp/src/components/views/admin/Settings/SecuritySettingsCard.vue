<script setup lang="ts">
import { Shield } from 'lucide-vue-next'
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
        <Shield class="w-5 h-5 text-primary" />
        <CardTitle>Security Settings</CardTitle>
      </div>
      <CardDescription>
        Manage authentication and authorization
      </CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Two-Factor Auth</Label>
          <p class="text-sm text-muted-foreground">
            Require 2FA for all users
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
      <Separator />
      <div class="flex items-center justify-between">
        <div class="space-y-0.5">
          <Label>Session Timeout</Label>
          <p class="text-sm text-muted-foreground">
            Auto-logout after inactivity
          </p>
        </div>
        <Badge variant="outline">Coming Soon</Badge>
      </div>
      <Button class="w-full" variant="outline" disabled>
        Configure Security
      </Button>
    </CardContent>
  </Card>
</template>
