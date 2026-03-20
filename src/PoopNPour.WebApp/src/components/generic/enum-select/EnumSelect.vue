<script setup lang="ts">
import { computed } from "vue";
import {
  Select,
  SelectContent,
  SelectItem,
  SelectTrigger,
  SelectValue,
} from "@/components/ui/select";
import type { AcceptableValue } from "reka-ui";
import type { EnumOption } from "@/utils/logEnums";

const props = withDefaults(
  defineProps<{
    modelValue?: number | null;
    options: EnumOption[];
    placeholder?: string;
    disabled?: boolean;
    name?: string;
  }>(),
  { placeholder: "Select...", disabled: false },
);

const emit = defineEmits<{
  "update:modelValue": [value: number | null];
}>();

function handleUpdate(value: AcceptableValue) {
  if (value === null || value === "" || value === undefined) {
    emit("update:modelValue", null);
    return;
  }
  const num = Number(String(value));
  emit("update:modelValue", Number.isNaN(num) ? null : num);
}

const stringValue = computed(() => (props.modelValue != null ? String(props.modelValue) : ""));

function getLabel(value: number): string {
  return props.options.find((o) => o.value === value)?.label ?? String(value);
}
</script>

<template>
  <Select :model-value="stringValue" :disabled="disabled" @update:model-value="handleUpdate">
    <SelectTrigger class="w-full" :name="name">
      <SelectValue :placeholder="placeholder" class="text-primary-foreground">
        {{ modelValue != null ? getLabel(modelValue) : undefined }}
      </SelectValue>
    </SelectTrigger>
    <SelectContent>
      <SelectItem
        v-for="opt in options"
        :key="opt.value"
        :value="String(opt.value)"
        class="text-secondary-foreground"
      >
        {{ opt.label }}
      </SelectItem>
    </SelectContent>
  </Select>
</template>
