<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Home, ChevronLeft } from 'lucide-vue-next'

import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { familyService } from '@/services/FamilyService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import EditFamilyForm, { type EditFamilyFormValues } from '@/components/views/admin/Families/EditFamilyForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const familyId = route.params.id as string

const isLoading = ref(true)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)
const initialValues = ref<EditFamilyFormValues | undefined>(undefined)
const familyName = ref<string>('')

onMounted(async () => {
  const response = await familyService.getFamilyById(familyId)
  if (response.success && response.data) {
    const family = response.data
    familyName.value = family.familyName ?? ''
    initialValues.value = {
      familyName: family.familyName ?? '',
      familyLastName: family.familyLastName ?? '',
    }
  } else {
    toast.error('Family not found', 'Could not load the family. Redirecting back.')
    await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
  }
  isLoading.value = false
})

async function handleSubmit(values: EditFamilyFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const response = await familyService.updateFamily(familyId, {
      familyId,
      familyName: values.familyName,
      familyLastName: values.familyLastName,
    })

    if (response.success) {
      toast.success('Family updated!', `Changes for the ${values.familyName} family have been saved.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
    } else {
      const message = response.error?.message || 'Failed to update family. Please try again.'
      errorMessage.value = message
      toast.error('Update failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Edit family error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.FAMILIES) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Home class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Edit Family</h1>
        <p class="text-muted-foreground">
          {{ familyName ? `Editing the ${familyName} family` : 'Loading family...' }}
        </p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <EditFamilyForm
        v-if="!isLoading"
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        :initial-values="initialValues"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
      <div v-else class="space-y-4">
        <div class="h-10 bg-muted animate-pulse rounded-md" />
        <div class="h-10 bg-muted animate-pulse rounded-md" />
      </div>
    </div>
  </div>
</template>
