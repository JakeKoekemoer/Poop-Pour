<script setup lang="ts">
import type { AlertDialogTitleProps } from "reka-ui"
import type { HTMLAttributes } from "vue"
import { reactiveOmit } from "@vueuse/core"
import { AlertDialogTitle } from "reka-ui"
import { cn } from "@/lib/utils"

const props = withDefaults(defineProps<AlertDialogTitleProps & { class?: HTMLAttributes["class"] }>(), {
  as: "h2",
})

const delegatedProps = reactiveOmit(props, "class")
</script>

<template>
  <AlertDialogTitle
    :class="cn('text-lg font-semibold', props.class)"
    v-bind="delegatedProps"
  >
    <slot />
  </AlertDialogTitle>
</template>
