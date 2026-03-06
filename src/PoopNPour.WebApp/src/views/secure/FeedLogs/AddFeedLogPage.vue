<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { Utensils, ChevronLeft } from 'lucide-vue-next'
import { Button } from '@/components/ui/button'
import { Separator } from '@/components/ui/separator'
import { feedLogService } from '@/services/FeedLogService'
import { RouteHelper } from '@/routes/helpers/RouteHelper'
import { SECURE_ROUTES } from '@/routes/constants'
import { useAppToast } from '@/composables/useAppToast'
import AddFeedLogForm, { type AddFeedLogFormValues } from '@/components/views/secure/FeedLogs/AddFeedLogForm.vue'

const router = useRouter()
const route = useRoute()
const toast = useAppToast()

const familyId = computed(() => route.params.familyId as string)
const isSubmitting = ref(false)
const errorMessage = ref<string | null>(null)

async function handleSubmit(values: AddFeedLogFormValues) {
  isSubmitting.value = true
  errorMessage.value = null

  try {
    const timeFed = values.timeFed ? new Date(values.timeFed) : undefined
    const mililitersFed = values.mililitersFed ?? undefined
    const notes = values.notes?.trim()
      ? [values.notes.trim()]
      : null

    const response = await feedLogService.createFeedLog({
      dependentId: values.dependentId,
      feedType: values.feedType,
      timeFed,
      mililitersFed: mililitersFed ?? null,
      notes,
    })

    if (response.success) {
      toast.success('Feed log added', 'The feed log has been recorded.')
      await router.push({
        name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
        params: { familyId: familyId.value },
      })
    } else {
      const message = response.error?.message ?? 'Failed to add feed log. Please try again.'
      errorMessage.value = message
      toast.error('Add failed', message)
    }
  } catch (error) {
    const message = 'An unexpected error occurred. Please try again.'
    errorMessage.value = message
    toast.error('Something went wrong', message)
    console.error('Add feed log error:', error)
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
        <h1 class="text-3xl font-bold">Add Feed Log</h1>
        <p class="text-muted-foreground">Record a feeding event for a dependent</p>
      </div>
    </div>

    <Separator />

    <div class="max-w-lg">
      <AddFeedLogForm
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
