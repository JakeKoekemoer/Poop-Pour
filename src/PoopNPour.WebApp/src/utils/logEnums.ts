export interface EnumOption {
  value: number
  label: string
  /** Hex colour for swatch UI (e.g. #8B4513) */
  colour?: string
}

export const FEED_LOG_TYPE_OPTIONS: EnumOption[] = [
  { value: 1, label: 'Breast Milk' },
  { value: 2, label: 'Formula' },
  { value: 3, label: 'Solid Food' },
]

export const FECAL_DISCHARGE_COLOUR_OPTIONS: EnumOption[] = [
  { value: -1, label: 'Not listed', colour: '#e5e7eb' },
  { value: 1, label: 'Brown', colour: '#8B4513' },
  { value: 2, label: 'Yellow', colour: '#EAB308' },
  { value: 3, label: 'Green', colour: '#22c55e' },
  { value: 4, label: 'Black', colour: '#171717' },
  { value: 5, label: 'Gray', colour: '#737373' },
  { value: 6, label: 'Red', colour: '#dc2626' },
]

export const URINAL_DISCHARGE_COLOUR_OPTIONS: EnumOption[] = [
  { value: -1, label: 'Not listed', colour: '#e5e7eb' },
  { value: 1, label: 'Yellow', colour: '#EAB308' },
  { value: 2, label: 'Orange', colour: '#f97316' },
  { value: 3, label: 'Brown', colour: '#8B4513' },
  { value: 4, label: 'Red', colour: '#dc2626' },
  { value: 5, label: 'Green', colour: '#22c55e' },
]

export function getLabelForValue(
  value: number | undefined,
  options: EnumOption[]
): string {
  if (value === undefined || value === null) return '—'
  const option = options.find((o) => o.value === value)
  return option?.label ?? '—'
}
