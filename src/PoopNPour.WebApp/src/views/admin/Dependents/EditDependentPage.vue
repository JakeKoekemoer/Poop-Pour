<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Baby, ChevronLeft } from 'lucide-vue-next'

import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { dependentService } from '@/services/DependentService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { ADMIN_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'

import EditDependentForm, { type EditDependentFormValues } from '@/components/views/admin/Dependents/EditDependentForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const dependentId = route.params['id'] as string

const isLoading = ref(true)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)
const initialValues = ref<EditDependentFormValues | undefined>(undefined)
const dependentDisplayName = ref<string>('')

function formatDateForInput(date: Date | string | undefined): string {
  if (!date) return ''
  const d = typeof date === 'string' ? new Date(date) : date
  if (isNaN(d.getTime())) return ''
  return d.toISOString().slice(0, 10)
}

onMounted(async () => {
  const response = await dependentService.getDependentById(dependentId)
  if (response.success && response.data) {
    const dependent = response.data
    dependentDisplayName.value = `${dependent.dependentName ?? ''} ${dependent.dependentSurname ?? ''}`.trim() || 'Dependent'
    initialValues.value = {
      dependentName: dependent.dependentName ?? '',
      dependentSurname: dependent.dependentSurname ?? '',
      dateOfBirth: formatDateForInput(dependent.dateOfBirth),
    }
  } else {
    toast.error('Dependent not found', 'Could not load the dependent. Redirecting back.')
    await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS) })
  }
  isLoading.value = false
})

async function handleSubmit(values: EditDependentFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const dateOfBirth = values.dateOfBirth ? new Date(values.dateOfBirth) : undefined
    const response = await dependentService.updateDependent(dependentId, {
      dependentName: values.dependentName,
      dependentSurname: values.dependentSurname,
      dateOfBirth,
    })

    if (response.success) {
      toast.success('Dependent updated!', `Changes for ${values.dependentName} ${values.dependentSurname} have been saved.`)
      await router.push({ name: RouteHelper.GetAdminRouteName(ADMIN_ROUTES.DEPENDENTS) })
    } else {
      const message = response.error?.message || 'Failed to update dependent. Please try again.'
      errorMessage.value = message
      toast.error('Update failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Edit dependent error:', error)
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
        <h1 class="text-3xl font-bold">Edit Dependent</h1>
        <p class="text-muted-foreground">
          {{ dependentDisplayName ? `Editing ${dependentDisplayName}` : 'Loading dependent...' }}
        </p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <EditDependentForm
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
