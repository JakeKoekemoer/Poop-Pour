<script setup lang="ts">
import { Star } from 'lucide-vue-next'

interface Props {
  rating?: number
  maxRating?: number
  size?: number
}

const props = withDefaults(defineProps<Props>(), {
  rating: 5,
  maxRating: 5,
  size: 20,
})
</script>

<template>
  <div class="flex gap-1" role="img" :aria-label="`${rating} out of ${maxRating} stars`">
    <Star
      v-for="i in maxRating"
      :key="i"
      :size="size"
      :class="[
        'transition-colors',
        i <= rating ? 'fill-secondary text-secondary' : 'text-muted'
      ]"
      aria-hidden="true"
    />
  </div>
</template>
