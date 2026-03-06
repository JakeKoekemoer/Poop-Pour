<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Droplets, ChevronLeft } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { diperLogService } from '@/services/DiperLogService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'
import AddDiperLogForm, { type AddDiperLogFormValues } from '@/components/views/secure/DiaperLogs/AddDiperLogForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const familyId = computed(() => route.params.familyId as string)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddDiperLogFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const diperDate = values.diperDate ? new Date(values.diperDate) : undefined
    const notes = values.notes?.trim() ? [values.notes.trim()] : null

    const response = await diperLogService.createDiperLog({
      dependentId: values.dependentId,
      diperDate,
      fecalDischargeColour: values.fecalDischargeColour,
      urinaryDischargeColour: values.urinaryDischargeColour,
      notes,
    })

    if (response.success) {
      toast.success('Diaper log added', 'The diaper log has been recorded.')
      await router.push({
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DIAPER_LOGS),
        params: { familyId: familyId.value },
      })
    } else {
      const message = response.error?.message ?? 'Failed to add diaper log. Please try again.'
      errorMessage.value = message
      toast.error('Add failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add diaper log error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DIAPER_LOGS),
    params: { familyId: familyId.value },
  })
}
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="icon" @click="handleCancel">
        <ChevronLeft class="w-5 h-5" />
      </Button>
      <Droplets class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Add Diaper Log</h1>
        <p class="text-muted-foreground">Record a diaper change for a dependent</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddDiperLogForm
        v-if="familyId"
        :family-id="familyId"
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />
    </div>
  </div>
</template>
