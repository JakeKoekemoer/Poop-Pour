import type { DataTableColumn } from '@/components/generic/DataTable'
import type { FamilyDto } from '@/api/api-client'

type FamilyColumn = DataTableColumn & { key: keyof FamilyDto | 'actions' }

export const columns: FamilyColumn[] = [
  { key: 'familyName', label: 'Family Name' },
  { key: 'familyLastName', label: 'Last Name' },
  { key: 'createdOn', label: 'Created On' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
