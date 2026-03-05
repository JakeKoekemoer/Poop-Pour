import type { DataTableColumn } from '@/components/generic/DataTable'
import type { DependentDto } from '@/api/api-client'

type FamilyDependentColumn = DataTableColumn & { key: keyof DependentDto | 'actions' }

export const columns: FamilyDependentColumn[] = [
  { key: 'dependentName', label: 'First Name' },
  { key: 'dependentSurname', label: 'Surname' },
  { key: 'age', label: 'Age' },
  { key: 'createdOn', label: 'Created On' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
