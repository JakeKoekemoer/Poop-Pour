<script setup lang="ts">
import { ref, computed, watch } from 'vue'
import { Droplets } from 'lucide-vue-next'
import { Sheet, SheetContent, SheetHeader, SheetTitle } from '@/components/ui/sheet'
import { diperLogService } from '@/services/DiperLogService'
import { useAppToast } from '@/composables/useAppToast'
import AddDiperLogForm, { type AddDiperLogFormValues } from '@/components/views/secure/DiaperLogs/AddDiperLogForm.vue'

const props = defineProps<{
  open: boolean
  familyId: string
  dependentId: string | null
}>()

const emit = defineEmits<{
  'update:open': [value: boolean]
}>()

const toast = useAppToast()
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

const openModel = computed({
  get: () => props.open,
  set: (value) => emit('update:open', value),
})

watch(
  () => props.open,
  (isOpen) => {
    if (!isOpen) {
      errorMessage.value = null
    }
  },
)

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
      emit('update:open', false)
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
  emit('update:open', false)
}
</script>

<template>
  <Sheet v-model:open="openModel">
    <SheetContent class="flex flex-col sm:max-w-lg p-0">
      <SheetHeader class="border-b px-6 py-5">
        <SheetTitle class="flex items-center gap-3 text-xl text-foreground">
          <Droplets class="size-6 text-primary" />
          Add Diaper Log
        </SheetTitle>
      </SheetHeader>
      <div class="flex-1 overflow-y-auto px-6 py-6">
        <AddDiperLogForm
          v-if="familyId && dependentId"
          :key="dependentId"
          :family-id="familyId"
          :pre-selected-dependent-id="dependentId"
          :is-submitting="isSubmitting"
          :error-message="errorMessage"
          @submit="handleSubmit"
          @cancel="handleCancel"
        />
      </div>
    </SheetContent>
  </Sheet>
</template>
