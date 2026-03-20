<script setup lang="ts">
defineProps<{
  class?: string
}>()
</script>

<template>
  <p :class="['text-muted-foreground text-lg', $attrs['class']]">
    <slot />
  </p>
</template>
