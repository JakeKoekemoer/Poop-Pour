<script setup lang="ts">
import type { AlertDialogCancelProps } from "reka-ui"
import { reactiveOmit } from "@vueuse/core"
import { AlertDialogCancel } from "reka-ui"
import { type ButtonVariants, buttonVariants } from "@/components/ui/button"
import { cn } from "@/lib/utils"

const props = withDefaults(
  defineProps<AlertDialogCancelProps & { class?: string; variant?: ButtonVariants["variant"]; size?: ButtonVariants["size"] }>(),
  {
    as: "button",
    variant: "outline",
    size: "default",
  }
)

const delegatedProps = reactiveOmit(props, "class", "variant", "size")
</script>

<template>
  <AlertDialogCancel
    :class="cn(buttonVariants({ variant: props.variant, size: props.size }), props.class)"
    v-bind="delegatedProps"
  >
    <slot />
  </AlertDialogCancel>
</template>
