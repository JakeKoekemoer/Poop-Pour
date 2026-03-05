<script setup lang="ts">
import type { AlertDialogActionProps } from "reka-ui"
import { reactiveOmit } from "@vueuse/core"
import { AlertDialogAction } from "reka-ui"
import { type ButtonVariants, buttonVariants } from "@/components/ui/button"
import { cn } from "@/lib/utils"

const props = withDefaults(
  defineProps<AlertDialogActionProps & { class?: string; variant?: ButtonVariants["variant"]; size?: ButtonVariants["size"] }>(),
  {
    as: "button",
    variant: "default",
    size: "default",
  }
)

const delegatedProps = reactiveOmit(props, "class", "variant", "size")
</script>

<template>
  <AlertDialogAction
    :class="cn(buttonVariants({ variant: props.variant, size: props.size }), props.class)"
    v-bind="delegatedProps"
  >
    <slot />
  </AlertDialogAction>
</template>
