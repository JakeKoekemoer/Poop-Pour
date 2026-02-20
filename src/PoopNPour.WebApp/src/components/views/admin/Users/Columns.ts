import type { DataTableColumn } from '@/components/generic/DataTable'
import type { UserProfile } from '@/stores'

type UserColumn = DataTableColumn & { key: keyof UserProfile | 'actions' }

export const columns: UserColumn[] = [
  { key: 'firstName', label: 'First Name' },
  { key: 'lastName', label: 'Last Name' },
  { key: 'email', label: 'Email' },
  { key: 'roles', label: 'Roles' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
