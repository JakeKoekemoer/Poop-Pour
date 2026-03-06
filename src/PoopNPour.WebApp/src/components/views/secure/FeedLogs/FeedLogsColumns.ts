import type { DataTableColumn } from '@/components/generic/DataTable'

export const columns: DataTableColumn[] = [
  { key: 'dependentName', label: 'Dependent' },
  { key: 'feedType', label: 'Feed Type' },
  { key: 'timeFed', label: 'Time Fed' },
  { key: 'mililitersFed', label: 'ml' },
  { key: 'createdOn', label: 'Created' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
