<script setup lang="ts">
import { Palette } from 'lucide-vue-next'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { Label } from '@/components/ui/label'
</script>

<template>
  <Card>
    <CardHeader>
      <div class="flex items-center gap-2">
        <Palette class="w-5 h-5 text-primary" />
        <CardTitle>Appearance Settings</CardTitle>
      </div>
      <CardDescription>
        Customize system appearance
      </CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <div class="space-y-2">
        <Label>Theme</Label>
        <p class="text-sm text-muted-foreground">
          Default theme for all users
        </p>
      </div>
      <Separator />
      <div class="space-y-2">
        <Label>Branding</Label>
        <p class="text-sm text-muted-foreground">
          Custom logo and colors
        </p>
      </div>
      <Button class="w-full" variant="outline" disabled>
        Configure Appearance
      </Button>
    </CardContent>
  </Card>
</template>
