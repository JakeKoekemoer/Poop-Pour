<script setup lang="ts">
defineProps<{
  class?: string
}>()
</script>

<template>
  <div :class="['flex gap-5 p-6 rounded-xl bg-background border shadow-sm hover:shadow-md transition-shadow', $attrs['class']]">
    <div class="p-4 rounded-xl bg-primary/10 shrink-0">
      <slot name="icon" />
    </div>
    <div>
      <slot name="content" />
    </div>
  </div>
</template>
