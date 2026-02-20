<script setup lang="ts">
import { Users, UserPlus } from 'lucide-vue-next'
import { useRouter } from 'vue-router'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'

import UsersTable from '@/components/views/admin/Users/UsersTable.vue'

const router = useRouter()

function goToAddUser() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_USER) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Users class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Users Management</h1>
      </div>
      <Button @click="goToAddUser">
        <UserPlus class="mr-2 h-4 w-4" />
        Add User
      </Button>
    </div>

    <Separator />

    <UsersTable />
  </div>
</template>
