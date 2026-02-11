export interface CountdownTimerProps {
  targetDate: Date | string
  title?: string
  subtitle?: string
  showIcon?: boolean
}

export interface TimeRemaining {
  days: number
  hours: number
  minutes: number
  seconds: number
}
