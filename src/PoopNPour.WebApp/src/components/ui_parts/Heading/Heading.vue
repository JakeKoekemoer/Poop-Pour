<script setup lang="ts">
withDefaults(defineProps<{
  tag?: 'h1' | 'h2' | 'h3' | 'h4' | 'h5' | 'h6'
  class?: string
}>(), {
  tag: 'h2',
})
</script>

<template>
  <component
    :is="tag"
    :class="['text-4xl md:text-5xl lg:text-6xl font-bold', $attrs.class]"
  >
    <slot />
  </component>
</template>
