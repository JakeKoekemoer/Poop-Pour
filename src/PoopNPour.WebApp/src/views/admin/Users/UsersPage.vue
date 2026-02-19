<script setup lang="ts">
import { Users, UserPlus } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'

import UsersTable from '@/components/views/admin/Users/UsersTable.vue'
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Users class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Users Management</h1>
      </div>
      <Button disabled>
        <UserPlus class="mr-2 h-4 w-4" />
        Add User
      </Button>
    </div>

    <Separator />

    <UsersTable />
  </div>
</template>
