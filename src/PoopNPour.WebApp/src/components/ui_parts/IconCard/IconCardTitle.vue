<script setup lang="ts">
withDefaults(defineProps<{
  tag?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6'
  class?: string
}>(), {
  tag: 'h3',
})
</script>

<template>
  <component
    :is="tag"
    :class="['text-2xl font-semibold mb-2', $attrs.class]"
  >
    <slot />
  </component>
</template>
