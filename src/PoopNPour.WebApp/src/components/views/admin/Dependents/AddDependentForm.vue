<script setup lang="ts">
import { ref, onMounted, computed, watch } from 'vue'
import { useForm } from 'vee-validate'
import { toTypedSchema } from '@vee-validate/zod'
import * as z from 'zod'

import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
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
import { familyService } from '@/services/FamilyService'
import type { FamilyDto } from '@/api/api-client'

export interface AddDependentFormValues {
  familyId: string
  dependentName: string
  dependentSurname: string
  dateOfBirth: string
}

const addDependentSchema = z.object({
  familyId: z.string().min(1, 'Family is required'),
  dependentName: z.string().min(1, 'First name is required'),
  dependentSurname: z.string().min(1, 'Surname is required'),
  dateOfBirth: z.string().min(1, 'Date of birth is required'),
})

const addDependentSchemaNoFamily = z.object({
  dependentName: z.string().min(1, 'First name is required'),
  dependentSurname: z.string().min(1, 'Surname is required'),
  dateOfBirth: z.string().min(1, 'Date of birth is required'),
})

const props = withDefaults(
  defineProps<{
    isSubmitting: boolean
    errorMessage: string | null
    /** When provided, family selector is hidden and this ID is used */
    familyId?: string
  }>(),
  { familyId: undefined }
)

const emit = defineEmits<{
  submit: [values: AddDependentFormValues]
  cancel: []
}>()

const showFamilySelector = computed(() => !props.familyId)

const form = useForm({
  validationSchema: toTypedSchema(
    showFamilySelector.value ? addDependentSchema : addDependentSchemaNoFamily
  ),
})

const families = ref<FamilyDto[]>([])
const isLoadingFamilies = ref(false)

function getFamilyDisplayName(family: FamilyDto) {
  const lastName = family.familyLastName && family.familyLastName !== family.familyName
    ? ` ${family.familyLastName}`
    : ''
  const name = `${family.familyName ?? ''}${lastName}`.trim()
  return name || (family.familyId ?? '')
}

function getFamilySurname(family: FamilyDto): string {
  return (family.familyLastName ?? family.familyName ?? '').trim()
}

const onSubmit = form.handleSubmit((values) => {
  const familyId = props.familyId ?? (values as AddDependentFormValues).familyId
  if (!familyId) return
  emit('submit', {
    familyId,
    dependentName: values.dependentName,
    dependentSurname: values.dependentSurname,
    dateOfBirth: values.dateOfBirth,
  })
})

watch(
  () => (form.values as Record<string, unknown>)['familyId'] as string | undefined,
  (familyId) => {
    if (showFamilySelector.value && familyId) {
      const family = families.value.find((f) => f.familyId === familyId)
      if (family) {
        const surname = getFamilySurname(family)
        if (surname) form.setFieldValue('dependentSurname', surname)
      }
    }
  },
)

onMounted(async () => {
  if (showFamilySelector.value) {
    isLoadingFamilies.value = true
    const response = await familyService.getFamilies(1, 200)
    if (response.success && response.data) {
      families.value = response.data.items
    }
    isLoadingFamilies.value = false
  } else if (props.familyId) {
    const response = await familyService.getFamilyById(props.familyId)
    if (response.success && response.data) {
      const surname = getFamilySurname(response.data)
      if (surname) form.setFieldValue('dependentSurname', surname)
    }
  }
})
</script>

<template>
  <form @submit="onSubmit" class="space-y-4">
    <FormField v-if="showFamilySelector" v-slot="{ componentField }" name="familyId">
      <FormItem>
        <FormLabel>Family</FormLabel>
        <FormControl>
          <Select
            :model-value="componentField.modelValue"
            :disabled="props.isSubmitting || isLoadingFamilies || !families.length"
            @update:model-value="componentField['onUpdate:modelValue']"
          >
            <SelectTrigger class="w-full">
              <SelectValue placeholder="Select a family" />
            </SelectTrigger>
            <SelectContent>
              <SelectItem
                v-for="f in families"
                :key="f.familyId!"
                :value="f.familyId!"
              >
                {{ getFamilyDisplayName(f) }}
              </SelectItem>
            </SelectContent>
          </Select>
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dependentName">
      <FormItem>
        <FormLabel>First Name</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. Emma"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dependentSurname">
      <FormItem>
        <FormLabel>Surname</FormLabel>
        <FormControl>
          <Input
            type="text"
            placeholder="e.g. Smith"
            v-bind="componentField"
            :disabled="props.isSubmitting"
          />
        </FormControl>
        <FormMessage />
      </FormItem>
    </FormField>

    <FormField v-slot="{ componentField }" name="dateOfBirth">
      <FormItem>
        <FormLabel>Date of Birth</FormLabel>
        <FormControl>
          <Input
            type="date"
            v-bind="componentField"
            :disabled="props.isSubmitting"
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
        {{ props.isSubmitting ? 'Creating...' : 'Create Dependent' }}
      </Button>
      <Button type="button" variant="outline" :disabled="props.isSubmitting" @click="emit('cancel')">
        Cancel
      </Button>
    </div>
  </form>
</template>
