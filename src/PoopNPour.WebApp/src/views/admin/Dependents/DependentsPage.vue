<script setup lang="ts">
import { Baby, Plus } from 'lucide-vue-next'
import { useRouter } from 'vue-router'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'

import DependentsTable from '@/components/views/admin/Dependents/DependentsTable.vue'

const router = useRouter()

function goToAddDependent() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.ADD_DEPENDENT) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <Baby class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Dependents</h1>
      </div>
      <Button @click="goToAddDependent">
        <Plus class="mr-2 h-4 w-4" />
        Add Dependent
      </Button>
    </div>

    <Separator />

    <DependentsTable />
  </div>
</template>
