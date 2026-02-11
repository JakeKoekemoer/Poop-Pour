<script setup lang="ts">
defineProps<{
  class?: string
}>()
</script>

<template>
  <p :class="['text-xl md:text-2xl text-muted-foreground', $attrs.class]">
    <slot />
  </p>
</template>
