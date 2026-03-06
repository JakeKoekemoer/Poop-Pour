<script setup lang="ts">
import { watch } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Textarea } from '@/components/ui/textarea'
import { Alert, AlertDescription } from '@/components/ui/alert'
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from '@/components/ui/form'
import { EnumSelect } from '@/components/generic/enum-select'
import { FECAL_DISCHARGE_COLOUR_OPTIONS, URINAL_DISCHARGE_COLOUR_OPTIONS } from '@/utils/logEnums'

export interface EditDiperLogFormValues {
  diperDate: string
  fecalDischargeColour: number
  urinaryDischargeColour: number
  notes: string
}

const editDiperLogSchema = z.object({
  diperDate: z.string().min(1, 'Date is required'),
  fecalDischargeColour: z.number({ required_error: 'Fecal colour is required' }),
  urinaryDischargeColour: z.number({ required_error: 'Urinary colour is required' }),
  notes: z.string().optional(),
})

const props = defineProps<{
  initialValues?: EditDiperLogFormValues
  isSubmitting: boolean
  errorMessage: string | null
}>()

const emit = defineEmits<{
  submit: [values: EditDiperLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editDiperLogSchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        diperDate: vals.diperDate,
        fecalDischargeColour: vals.fecalDischargeColour,
        urinaryDischargeColour: vals.urinaryDischargeColour,
        notes: vals.notes ?? '',
      })
    }
  },
  { immediate: true },
)

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as EditDiperLogFormValues)
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-slot="{ componentField }" name="diperDate">
      <FormItem>
        <FormLabel>Date</FormLabel>
        <FormControl>
          <Input
            type="datetime-local"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="fecalDischargeColour">
      <FormItem>
        <FormLabel>Fecal Colour</FormLabel>
        <FormControl>
          <EnumSelect
            :model-value="componentField.modelValue"
            :options="FECAL_DISCHARGE_COLOUR_OPTIONS"
            placeholder="Select colour"
            :disabled="props.isSubmitting"
            @update:model-value="componentField['onUpdate:modelValue']"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="urinaryDischargeColour">
      <FormItem>
        <FormLabel>Urinary Colour</FormLabel>
        <FormControl>
          <EnumSelect
            :model-value="componentField.modelValue"
            :options="URINAL_DISCHARGE_COLOUR_OPTIONS"
            placeholder="Select colour"
            :disabled="props.isSubmitting"
            @update:model-value="componentField['onUpdate:modelValue']"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="notes">
      <FormItem>
        <FormLabel>Notes (optional)</FormLabel>
        <FormControl>
          <Textarea
            v-bind="componentField"
            placeholder="Additional notes..."
            :disabled="props.isSubmitting"
            rows="3"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <Alert v-if="props.errorMessage" variant="destructive">
      <AlertDescription>
        {{ props.errorMessage }}
      </AlertDescription>
    </Alert>

    <div class="flex gap-3 pt-2">
      <Button type="submit" :disabled="props.isSubmitting">
        {{ props.isSubmitting ? 'Saving...' : 'Save Changes' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
