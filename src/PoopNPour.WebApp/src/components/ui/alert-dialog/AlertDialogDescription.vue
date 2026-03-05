<script setup lang="ts">
import type { AlertDialogDescriptionProps } from "reka-ui"
import type { HTMLAttributes } from "vue"
import { reactiveOmit } from "@vueuse/core"
import { AlertDialogDescription } from "reka-ui"
import { cn } from "@/lib/utils"

const props = withDefaults(defineProps<AlertDialogDescriptionProps & { class?: HTMLAttributes["class"] }>(), {
  as: "p",
})

const delegatedProps = reactiveOmit(props, "class")
</script>

<template>
  <AlertDialogDescription
    :class="cn('text-sm text-muted-foreground', props.class)"
    v-bind="delegatedProps"
  >
    <slot />
  </AlertDialogDescription>
</template>
