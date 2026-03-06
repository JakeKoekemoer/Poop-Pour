<script setup lang="ts">
import { ref, onMounted } from 'vue'
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
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from '@/components/ui/select'
import { EnumSelect } from '@/components/generic/enum-select'
import { FEED_LOG_TYPE_OPTIONS } from '@/utils/logEnums'
import { dependentService } from '@/services/DependentService'
import type { DependentDto } from '@/api/api-client'

export interface AddFeedLogFormValues {
  dependentId: string
  feedType: number
  timeFed: string
  mililitersFed: number | undefined
  notes: string
}

const addFeedLogSchema = z.object({
  dependentId: z.string().min(1, 'Dependent is required'),
  feedType: z.number({ required_error: 'Feed type is required' }),
  timeFed: z.string().min(1, 'Time fed is required'),
  mililitersFed: z.coerce.number().int().min(0).optional(),
  notes: z.string().optional(),
})

const props = defineProps<{
  familyId: string
  isSubmitting: boolean
  errorMessage: string | null
  preSelectedDependentId?: string
}>()

const emit = defineEmits<{
  submit: [values: AddFeedLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(addFeedLogSchema),
})

const dependents = ref<DependentDto[]>([])
const isLoadingDependents = ref(false)

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || (dependent.dependentId ?? '—')
}

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as AddFeedLogFormValues)
})

function getDateTimeLocalNow(): string {
  const d = new Date()
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset())
  return d.toISOString().slice(0, 16)
}

onMounted(async () => {
  form.setFieldValue('timeFed', getDateTimeLocalNow())
  if (props.preSelectedDependentId) {
    form.setFieldValue('dependentId', props.preSelectedDependentId)
  } else if (props.familyId) {
    isLoadingDependents.value = true
    const response = await dependentService.getDependents(1, 200, props.familyId)
    if (response.success && response.data) {
      dependents.value = response.data.items
    }
    isLoadingDependents.value = false
  }
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-if="!props.preSelectedDependentId" v-slot="{ componentField }" name="dependentId">
      <FormItem>
        <FormLabel>Dependent</FormLabel>
        <FormControl>
          <Select
            :model-value="componentField.modelValue"
            :disabled="props.isSubmitting || isLoadingDependents || !dependents.length"
            @update:model-value="componentField['onUpdate:modelValue']"
          >
            <SelectTrigger class="w-full">
              <SelectValue placeholder="Select a dependent" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem
                v-for="d in dependents"
                :key="d.dependentId!"
                :value="d.dependentId!"
              >
                {{ getDependentDisplayName(d) }}
              </SelectItem>
            </SelectContent>
          </Select>
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

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
        {{ props.isSubmitting ? 'Creating...' : 'Add Feed Log' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
