import type { DataTableColumn } from '@/components/generic/DataTable'

export const columns: DataTableColumn[] = [
  { key: 'dependentName', label: 'Dependent' },
  { key: 'medicineName', label: 'Medicine' },
  { key: 'dosage', label: 'Dosage' },
  { key: 'timeAdministered', label: 'Time Administered' },
  { key: 'createdOn', label: 'Created' },
  { key: 'actions', label: 'Actions', align: 'right' },
]
