import type { DataTableColumn } from '@/components/generic/DataTable'
import type { DependentDto } from '@/api/api-client'

type DependentColumn = DataTableColumn & { key: keyof DependentDto | 'family' | 'actions' }

export const columns: DependentColumn[] = [
  { key: 'dependentName', label: 'First Name' },
  { key: 'dependentSurname', label: 'Surname' },
  { key: 'family', label: 'Family' },
  { key: 'age', label: 'Age' },
  { key: 'createdOn', label: 'Created On' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
