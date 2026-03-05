<script setup lang="ts">
import { computed } from "vue";
import {
  AlertDialog,
  AlertDialogCancel,
  AlertDialogContent,
  AlertDialogDescription,
  AlertDialogFooter,
  AlertDialogHeader,
  AlertDialogTitle,
} from "@/components/ui/alert-dialog";
import { Button } from "@/components/ui/button";
import { Loader2 } from "lucide-vue-next";

const props = withDefaults(
  defineProps<{
    open?: boolean;
    title?: string;
    description: string;
    loading?: boolean;
    confirmLabel?: string;
    cancelLabel?: string;
  }>(),
  {
    open: false,
    title: "Delete",
    loading: false,
    confirmLabel: "Delete",
    cancelLabel: "Cancel",
  },
);

const emit = defineEmits<{
  confirm: [];
  cancel: [];
  "update:open": [value: boolean];
}>();

const openModel = computed({
  get: () => props.open,
  set: (value) => emit("update:open", value),
});

function handleConfirm() {
  emit("confirm");
}
</script>

<template>
  <AlertDialog v-model:open="openModel" @update:open="(v) => emit('update:open', v)">
    <AlertDialogContent>
      <AlertDialogHeader>
        <AlertDialogTitle>{{ title }}</AlertDialogTitle>
        <AlertDialogDescription>
          {{ description }}
        </AlertDialogDescription>
      </AlertDialogHeader>
      <AlertDialogFooter>
        <AlertDialogCancel
          :disabled="loading"
          @click="emit('cancel')"
        >
          {{ cancelLabel }}
        </AlertDialogCancel>
        <Button variant="destructive" :disabled="loading" @click="handleConfirm">
          <Loader2 v-if="loading" class="mr-2 h-4 w-4 animate-spin" />
          <span v-if="!loading">{{ confirmLabel }}</span>
          <span v-else>Deleting...</span>
        </Button>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>
