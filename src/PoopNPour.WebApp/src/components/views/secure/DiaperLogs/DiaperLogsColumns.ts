import type { DataTableColumn } from '@/components/generic/DataTable'

export const columns: DataTableColumn[] = [
  { key: 'dependentName', label: 'Dependent' },
  { key: 'diperDate', label: 'Date' },
  { key: 'fecalDischargeColour', label: 'Fecal' },
  { key: 'urinaryDischargeColour', label: 'Urinary' },
  { key: 'createdOn', label: 'Created' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
