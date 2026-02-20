<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { X, Plus } from 'lucide-vue-next'

import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from '@/components/ui/card'
import { Badge } from '@/components/ui/badge'
import { Button } from '@/components/ui/button'
import { Input } from '@/components/ui/input'
import { Skeleton } from '@/components/ui/skeleton'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { useAppToast } from '@/composables/useAppToast'
import { userService } from '@/services/UserService'
import { formatApiError } from '@/utils/formatApiError'
import type { ClaimDto } from '@/api/api-client'

const props = defineProps<{
  userId: string
}>()

const toast = useAppToast()

const claims = ref<ClaimDto[]>([])
const isLoading = ref(true)
const removingClaim = ref<string | null>(null)
const isAdding = ref(false)

const newClaimType = ref('')
const newClaimValue = ref('')

async function fetchClaims() {
  isLoading.value = true
  const response = await userService.getUserClaims(props.userId)
  if (response.success && response.data) {
    claims.value = response.data
  }
  isLoading.value = false
}

async function handleRemove(claim: ClaimDto) {
  const key = `${claim.type}::${claim.value}`
  removingClaim.value = key

  const response = await userService.removeUserClaims(props.userId, [claim])
  if (response.success) {
    claims.value = response.data ?? claims.value.filter(
      (c) => !(c.type === claim.type && c.value === claim.value),
    )
    toast.success('Claim removed', `"${claim.type}" has been removed.`)
  } else {
    toast.error('Failed to remove claim', response.error ? formatApiError(response.error) : 'Please try again.')
  }

  removingClaim.value = null
}

async function handleAdd() {
  const type = newClaimType.value.trim()
  const value = newClaimValue.value.trim()

  if (!type || !value) return

  isAdding.value = true

  const response = await userService.addUserClaims(props.userId, [{ type, value }])
  if (response.success) {
    claims.value = response.data ?? [...claims.value, { type, value }]
    newClaimType.value = ''
    newClaimValue.value = ''
    toast.success('Claim added', `"${type}" has been added.`)
  } else {
    toast.error('Failed to add claim', response.error ? formatApiError(response.error) : 'Please try again.')
  }

  isAdding.value = false
}

onMounted(fetchClaims)
</script>

<template>
  <Card>
    <CardHeader>
      <CardTitle>User Claims</CardTitle>
      <CardDescription>Manage identity claims attached to this user account</CardDescription>
    </CardHeader>
    <CardContent class="space-y-4">
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead>Type</TableHead>
            <TableHead>Value</TableHead>
            <TableHead class="w-12" />
          </TableRow>
        </TableHeader>
        <TableBody>
          <template v-if="isLoading">
            <TableRow v-for="i in 3" :key="i">
              <TableCell><Skeleton class="h-4 w-32" /></TableCell>
              <TableCell><Skeleton class="h-4 w-48" /></TableCell>
              <TableCell />
            </TableRow>
          </template>

          <template v-else-if="claims.length > 0">
            <TableRow v-for="claim in claims" :key="`${claim.type}::${claim.value}`">
              <TableCell>
                <Badge variant="secondary">{{ claim.type }}</Badge>
              </TableCell>
              <TableCell class="font-mono text-sm">{{ claim.value }}</TableCell>
              <TableCell class="text-right">
                <Button
                  size="icon"
                  variant="ghost"
                  class="h-7 w-7 text-muted-foreground hover:text-destructive"
                  :disabled="removingClaim === `${claim.type}::${claim.value}`"
                  @click="handleRemove(claim)"
                >
                  <X class="h-4 w-4" />
                </Button>
              </TableCell>
            </TableRow>
          </template>

          <TableRow v-else>
            <TableCell colspan="3" class="text-center text-muted-foreground py-6">
              No claims assigned to this user.
            </TableCell>
          </TableRow>

          <TableRow>
            <TableCell>
              <Input
                v-model="newClaimType"
                placeholder="e.g. permission"
                :disabled="isAdding"
                class="h-8"
                @keyup.enter="handleAdd"
              />
            </TableCell>
            <TableCell>
              <Input
                v-model="newClaimValue"
                placeholder="e.g. manage:users"
                :disabled="isAdding"
                class="h-8"
                @keyup.enter="handleAdd"
              />
            </TableCell>
            <TableCell class="text-right">
              <Button
                size="icon"
                variant="ghost"
                class="h-7 w-7"
                :disabled="isAdding || !newClaimType.trim() || !newClaimValue.trim()"
                @click="handleAdd"
              >
                <Plus class="h-4 w-4" />
              </Button>
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </CardContent>
  </Card>
</template>
