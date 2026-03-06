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
import { FECAL_DISCHARGE_COLOUR_OPTIONS, URINAL_DISCHARGE_COLOUR_OPTIONS } from '@/utils/logEnums'
import { dependentService } from '@/services/DependentService'
import type { DependentDto } from '@/api/api-client'

export interface AddDiperLogFormValues {
  dependentId: string
  diperDate: string
  fecalDischargeColour: number
  urinaryDischargeColour: number
  notes: string
}

const addDiperLogSchema = z.object({
  dependentId: z.string().min(1, 'Dependent is required'),
  diperDate: z.string().min(1, 'Date is required'),
  fecalDischargeColour: z.number({ required_error: 'Fecal colour is required' }),
  urinaryDischargeColour: z.number({ required_error: 'Urinary colour is required' }),
  notes: z.string().optional(),
})

const props = defineProps<{
  familyId: string
  isSubmitting: boolean
  errorMessage: string | null
  preSelectedDependentId?: string
}>()

const emit = defineEmits<{
  submit: [values: AddDiperLogFormValues]
  cancel: []
}>()

const form = useForm({
  validationSchema: toTypedSchema(addDiperLogSchema),
})

const dependents = ref<DependentDto[]>([])
const isLoadingDependents = ref(false)

function getDependentDisplayName(dependent: DependentDto) {
  const name = dependent.dependentName ?? ''
  const surname = dependent.dependentSurname ?? ''
  return `${name} ${surname}`.trim() || (dependent.dependentId ?? '—')
}

function getDateTimeLocalNow(): string {
  const d = new Date()
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset())
  return d.toISOString().slice(0, 16)
}

const onSubmit = form.handleSubmit((values) => {
  emit('submit', values as AddDiperLogFormValues)
})

onMounted(async () => {
  form.setFieldValue('diperDate', getDateTimeLocalNow())
  form.setFieldValue('fecalDischargeColour', -1)
  form.setFieldValue('urinaryDischargeColour', -1)
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
          <div class="flex flex-wrap gap-3">
            <button
              v-for="opt in FECAL_DISCHARGE_COLOUR_OPTIONS"
              :key="opt.value"
              type="button"
              class="flex flex-col items-center gap-1.5 group disabled:opacity-50 disabled:pointer-events-none"
              :disabled="props.isSubmitting"
              :aria-pressed="componentField.modelValue === opt.value"
              :aria-label="opt.label"
              @click="form.setFieldValue('fecalDischargeColour', opt.value)"
            >
              <span
                class="size-10 rounded-full border-2 transition-all shrink-0"
                :class="[
                  componentField.modelValue === opt.value ? 'border-primary ring-2 ring-primary/30 scale-110' : 'border-muted-foreground/20 hover:border-muted-foreground/50',
                  opt.value === -1 && 'border-dashed'
                ]"
                :style="{ backgroundColor: opt.colour ?? '#94a3b8' }"
              />
              <span class="text-xs text-muted-foreground group-hover:text-foreground">{{ opt.label }}</span>
            </button>
          </div>
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="urinaryDischargeColour">
      <FormItem>
        <FormLabel>Urinary Colour</FormLabel>
        <FormControl>
          <div class="flex flex-wrap gap-3">
            <button
              v-for="opt in URINAL_DISCHARGE_COLOUR_OPTIONS"
              :key="opt.value"
              type="button"
              class="flex flex-col items-center gap-1.5 group disabled:opacity-50 disabled:pointer-events-none"
              :disabled="props.isSubmitting"
              :aria-pressed="componentField.modelValue === opt.value"
              :aria-label="opt.label"
              @click="form.setFieldValue('urinaryDischargeColour', opt.value)"
            >
              <span
                class="size-10 rounded-full border-2 transition-all shrink-0"
                :class="[
                  componentField.modelValue === opt.value ? 'border-primary ring-2 ring-primary/30 scale-110' : 'border-muted-foreground/20 hover:border-muted-foreground/50',
                  opt.value === -1 && 'border-dashed'
                ]"
                :style="{ backgroundColor: opt.colour ?? '#94a3b8' }"
              />
              <span class="text-xs text-muted-foreground group-hover:text-foreground">{{ opt.label }}</span>
            </button>
          </div>
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
        {{ props.isSubmitting ? 'Creating...' : 'Add Diaper Log' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
