<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { LayoutDashboard, Home, Loader2 } from 'lucide-vue-next'
import { Badge } from '@/components/ui/badge'
import { Separator } from '@/components/ui/separator'
import { secureNavItems } from '@/config/secureNav'
import { familyService } from '@/services/FamilyService'
import { useFamilyContextStore } from '@/stores'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import type { FamilyDto } from '@/api/api-client'

const router = useRouter()
const familyContextStore = useFamilyContextStore()

const families = ref<FamilyDto[]>([])
const isLoadingFamilies = ref(true)

async function loadFamilies() {
  isLoadingFamilies.value = true
  try {
    const response = await familyService.getFamilies(1, 50)

    if (response.success && response.data) {
      families.value = response.data.items ?? []

      if (
        families.value.length === 1 &&
        !familyContextStore.preventSingleFamilyAutoRedirect
      ) {
        const family = families.value[0]!
        if (family.familyId) {
          router.push({
            name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
            params: { familyId: family.familyId },
          })
        }
      }
    }
  } finally {
    familyContextStore.setPreventSingleFamilyAutoRedirect(false)
    isLoadingFamilies.value = false
  }
}

function navigate(routeName: string) {
  router.push({ name: routeName })
}

function handleFamilyClick(family: FamilyDto) {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DASHBOARD),
    params: { familyId: family.familyId! },
  })
}

onMounted(loadFamilies)
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center justify-between">
      <div class="flex items-center gap-3">
        <LayoutDashboard class="w-8 h-8 text-primary" />
        <h1 class="text-3xl font-bold">Dashboard</h1>
      </div>
      <Badge variant="secondary">Secure Area</Badge>
    </div>

    <Separator />

    <!-- Your Families -->
    <div class="space-y-3">
      <h2 class="text-lg font-semibold">Your Families</h2>

      <div v-if="isLoadingFamilies" class="flex items-center gap-2 text-muted-foreground text-sm">
        <Loader2 class="w-4 h-4 animate-spin" />
        <span>Loading families...</span>
      </div>

      <p v-else-if="!families.length" class="text-sm text-muted-foreground">
        You have no families yet. Create one below to get started.
      </p>

      <div v-else class="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
        <button
          v-for="family in families"
          :key="family.familyId"
          class="flex flex-col items-center justify-center gap-3 rounded-xl border bg-card p-6 text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="handleFamilyClick(family)"
        >
          <Home class="w-10 h-10" />
          <span class="text-sm font-medium text-center">
            {{ family.familyName }}{{ family.familyLastName && family.familyLastName !== family.familyName ? ' ' + family.familyLastName : '' }}
          </span>
        </button>
      </div>
    </div>

    <Separator />

    <!-- Actions -->
    <div class="space-y-3">
      <h2 class="text-lg font-semibold">Actions</h2>
      <div class="grid grid-cols-2 gap-4 sm:grid-cols-3 lg:grid-cols-4">
        <button
          v-for="item in secureNavItems"
          :key="item.routeName"
          class="flex flex-col items-center justify-center gap-3 rounded-xl border bg-card p-6 text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="navigate(item.routeName)"
        >
          <component :is="item.icon" class="w-10 h-10" />
          <span class="text-sm font-medium text-center">{{ item.label }}</span>
        </button>
      </div>
    </div>
  </div>
</template>
