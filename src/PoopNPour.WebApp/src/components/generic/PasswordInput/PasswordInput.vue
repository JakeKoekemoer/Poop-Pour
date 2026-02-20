<script setup lang="ts">
import { ref } from 'vue'
import { Eye, EyeOff } from 'lucide-vue-next'
import {
  InputGroup,
  InputGroupAddon,
  InputGroupButton,
  InputGroupInput,
} from '@/components/ui/input-group'

defineOptions({ inheritAttrs: false })

defineProps<{
  disabled?: boolean
}>()

const showPassword = ref(false)

defineExpose({ reveal: () => { showPassword.value = true } })
</script>

<template>
  <InputGroup>
    <InputGroupInput
      v-bind="$attrs"
      :type="showPassword ? 'text' : 'password'"
      placeholder="••••••••"
      :disabled="disabled"
    />
    <InputGroupAddon align="inline-end">
      <InputGroupButton
        type="button"
        size="icon-xs"
        :disabled="disabled"
        @click="showPassword = !showPassword"
      >
        <Eye v-if="!showPassword" />
        <EyeOff v-else />
      </InputGroupButton>
    </InputGroupAddon>
  </InputGroup>
</template>
