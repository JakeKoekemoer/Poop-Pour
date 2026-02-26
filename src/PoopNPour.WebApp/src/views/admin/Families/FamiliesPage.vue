<script setup lang="ts">
import { Home, Plus } from 'lucide-vue-next'
import { useRouter } from 'vue-router'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'

import FamiliesTable from '@/components/views/admin/Families/FamiliesTable.vue'

const router = useRouter()

function goToAddFamily() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_FAMILY) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Home class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Families Management</h1>
      </div>
      <Button @click="goToAddFamily">
        <Plus class="mr-2 h-4 w-4" />
        Add Family
      </Button>
    </div>

    <Separator />

    <FamiliesTable />
  </div>
</template>
