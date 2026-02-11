export const LOGO_STYLES = {
  FULL: 'full',
  BOTTLE: 'bottle',
  MINIMAL: 'minimal',
  COMBINED: 'combined',
} as const

export const LOGO_POSITIONS = {
  LEFT: 'left',
  RIGHT: 'right',
} as const

export type LogoStyle = typeof LOGO_STYLES[keyof typeof LOGO_STYLES]
export type LogoPosition = typeof LOGO_POSITIONS[keyof typeof LOGO_POSITIONS]
