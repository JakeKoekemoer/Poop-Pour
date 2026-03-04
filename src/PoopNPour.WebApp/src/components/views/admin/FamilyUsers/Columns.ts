import type { DataTableColumn } from '@/components/generic/DataTable'
import type { FamilyMemberDto } from '@/api/api-client'
import { FamilyRole } from '@/api/api-client'

type FamilyMemberColumn = DataTableColumn & { key: keyof FamilyMemberDto | 'fullName' | 'actions' }

export const columns: FamilyMemberColumn[] = [
  { key: 'fullName', label: 'Name' },
  { key: 'email', label: 'Email' },
  { key: 'role', label: 'Role' },
  { key: 'joinedOn', label: 'Joined On' },
  { key: 'actions', label: 'Actions', align: 'right' },
]

export const roleLabels: Record<FamilyRole, string> = {
  [FamilyRole._0]: 'Viewer',
  [FamilyRole._10]: 'Member',
  [FamilyRole._20]: 'Admin',
  [FamilyRole._30]: 'Owner',
}

export function getRoleLabel(role: FamilyRole | undefined): string {
  if (role === undefined) return '—'
  return roleLabels[role] ?? String(role)
}
