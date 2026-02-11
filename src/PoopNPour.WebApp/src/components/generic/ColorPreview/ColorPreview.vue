<script setup lang="ts">
interface Props {
  color: string
  size?: 'sm' | 'md' | 'lg'
}

const props = withDefaults(defineProps<Props>(), {
  size: 'md',
})

const sizeClasses = {
  sm: 'w-4 h-4',
  md: 'w-5 h-5',
  lg: 'w-6 h-6',
}
</script>

<template>
  <div
    :class="[sizeClasses[size], 'rounded-md border border-border/50 shadow-sm']"
    :style="{ backgroundColor: color }"
    :aria-label="`Color: ${color}`"
    role="img"
  >
    <slot />
  </div>
</template>
