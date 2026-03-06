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
import { FEED_LOG_TYPE_OPTIONS } from '@/utils/logEnums'

export interface EditFeedLogFormValues {
  feedType: number
  timeFed: string
  mililitersFed: string
  notes: string
}

const editFeedLogSchema = z.object({
  feedType: z.number({ required_error: 'Feed type is required' }),
  timeFed: z.string().min(1, 'Time fed is required'),
  mililitersFed: z.string().optional(),
  notes: z.string().optional(),
})

const props = defineProps<{
  initialValues?: EditFeedLogFormValues
  isSubmitting: boolean
  errorMessage: string | null
}>()

const emit = defineEmits<{
  submit: [values: EditFeedLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(editFeedLogSchema),
})

watch(
  () => props.initialValues,
  (vals) => {
    if (vals) {
      form.setValues({
        feedType: vals.feedType,
        timeFed: vals.timeFed,
        mililitersFed: vals.mililitersFed ?? '',
        notes: vals.notes ?? '',
      })
    }
  },
  { immediate: true },
)

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as EditFeedLogFormValues)
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-slot="{ componentField }" name="feedType">
      <FormItem>
        <FormLabel>Feed Type</FormLabel>
        <FormControl>
          <EnumSelect
            :model-value="componentField.modelValue"
            :options="FEED_LOG_TYPE_OPTIONS"
            placeholder="Select feed type"
            :disabled="props.isSubmitting"
            @update:model-value="componentField['onUpdate:modelValue']"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="timeFed">
      <FormItem>
        <FormLabel>Time Fed</FormLabel>
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

    <FormField v-slot="{ componentField }" name="mililitersFed">
      <FormItem>
        <FormLabel>Milliliters Fed (optional)</FormLabel>
        <FormControl>
          <Input
            type="number"
            min="0"
            step="1"
            placeholder="e.g. 120"
            v-bind="componentField"
            :disabled="props.isSubmitting"
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
