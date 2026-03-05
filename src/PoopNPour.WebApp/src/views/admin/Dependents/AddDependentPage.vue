<script setup lang="ts">
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { Baby, ChevronLeft } from 'lucide-vue-next'

import { Separator } from '@/components/ui/separator'
import { Button } from '@/components/ui/button'
import { dependentService } from '@/services/DependentService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import AddDependentForm, { type AddDependentFormValues } from '@/components/views/admin/Dependents/AddDependentForm.vue'

const router = useRouter()
const toast = useAppToast()
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddDependentFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const dateOfBirth = values.dateOfBirth ? new Date(values.dateOfBirth) : undefined
    const response = await dependentService.createDependent({
      familyId: values.familyId,
      dependentName: values.dependentName,
      dependentSurname: values.dependentSurname,
      dateOfBirth,
    })

    if (response.success) {
      toast.success('Dependent created!', `${values.dependentName} ${values.dependentSurname} has been added.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS) })
    } else {
      const message = response.error?.message || 'Failed to create dependent. Please try again.'
      errorMessage.value = message
      toast.error('Dependent creation failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add dependent error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS) })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Baby class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add Dependent</h1>
        <p class="text-muted-foreground">Create a new dependent for a family</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddDependentForm
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
