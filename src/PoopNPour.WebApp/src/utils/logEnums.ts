export interface EnumOption {
  value: number
  label: string
}

export const FEED_LOG_TYPE_OPTIONS: EnumOption[] = [
  { value: 1, label: 'Breast Milk' },
  { value: 2, label: 'Formula' },
  { value: 3, label: 'Solid Food' },
]

export const FECAL_DISCHARGE_COLOUR_OPTIONS: EnumOption[] = [
  { value: -1, label: 'Not listed' },
  { value: 1, label: 'Brown' },
  { value: 2, label: 'Yellow' },
  { value: 3, label: 'Green' },
  { value: 4, label: 'Black' },
  { value: 5, label: 'Gray' },
  { value: 6, label: 'Red' },
]

export const URINAL_DISCHARGE_COLOUR_OPTIONS: EnumOption[] = [
  { value: -1, label: 'Not listed' },
  { value: 1, label: 'Yellow' },
  { value: 2, label: 'Orange' },
  { value: 3, label: 'Brown' },
  { value: 4, label: 'Red' },
  { value: 5, label: 'Green' },
]

export function getLabelForValue(
  value: number | undefined,
  options: EnumOption[]
): string {
  if (value === undefined || value === null) return '—'
  const option = options.find((o) => o.value === value)
  return option?.label ?? '—'
}
