<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Utensils, ChevronLeft } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { feedLogService } from '@/services/FeedLogService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'
import EditFeedLogForm, { type EditFeedLogFormValues } from '@/components/views/secure/FeedLogs/EditFeedLogForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const familyId = computed(() => route.params['familyId'] as string)
const feedLogId = computed(() => route.params['id'] as string)

const isLoading = ref(true)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)
const initialValues = ref<EditFeedLogFormValues | undefined>(undefined)

function formatDateTimeForInput(date: Date | string | undefined): string {
  if (!date) return ''
  const d = typeof date === 'string' ? new Date(date) : date
  if (isNaN(d.getTime())) return ''
  d.setMinutes(d.getMinutes() - d.getTimezoneOffset())
  return d.toISOString().slice(0, 16)
}

onMounted(async () => {
  const response = await feedLogService.getFeedLogById(feedLogId.value)
  if (response.success && response.data) {
    const log = response.data
    initialValues.value = {
      feedType: (log.feedType as number) ?? 1,
      timeFed: formatDateTimeForInput(log.timeFed),
      mililitersFed: log.mililitersFed != null ? String(log.mililitersFed) : '',
      notes: log.notes?.[0] ?? '',
    }
  } else {
    toast.error('Feed log not found', 'Could not load the feed log. Redirecting back.')
    await router.push({
      name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
      params: { familyId: familyId.value },
    })
  }
  isLoading.value = false
})

async function handleSubmit(values: EditFeedLogFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const timeFed = values.timeFed ? new Date(values.timeFed) : undefined
    const mililitersFed = values.mililitersFed
      ? Number.parseInt(values.mililitersFed, 10)
      : undefined
    const notes = values.notes?.trim()
      ? [values.notes.trim()]
      : null

    const response = await feedLogService.updateFeedLog(feedLogId.value, {
      feedType: values.feedType,
      timeFed: timeFed ?? null,
      mililitersFed: mililitersFed ?? null,
      notes,
    })

    if (response.success) {
      toast.success('Feed log updated', 'Changes have been saved.')
      await router.push({
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
        params: { familyId: familyId.value },
      })
    } else {
      const message = response.error?.message ?? 'Failed to update feed log. Please try again.'
      errorMessage.value = message
      toast.error('Update failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Edit feed log error:', error)
  } finally {
    isSubmitting.value = false
  }
}

function handleCancel() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
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
      <Utensils class="w-8 h-8 text-primary" />
      <div>
        <h1 class="text-3xl font-bold">Edit Feed Log</h1>
        <p class="text-muted-foreground">Update feeding event details</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <EditFeedLogForm
        v-if="!isLoading"
        :initial-values="initialValues"
        :is-submitting="isSubmitting"
        :error-message="errorMessage"
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
