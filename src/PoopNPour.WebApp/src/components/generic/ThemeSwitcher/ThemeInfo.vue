<script setup lang="ts">
interface Props {
  emoji?: string
  title: string
  description: string
}

defineProps<Props>()
</script>

<template>
  <div class="flex-1 text-left min-w-0 space-y-0.5">
    <p class="font-medium text-foreground text-sm leading-none">
      <template v-if="emoji">{{ emoji }} </template>{{ title }}
    </p>
    <p class="text-xs text-muted-foreground truncate leading-none">
      {{ description }}
    </p>
  </div>
</template>
