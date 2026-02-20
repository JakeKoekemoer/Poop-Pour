export interface DataTableColumn {
  /** Column identifier — also used as the scoped slot name (`cell-{key}`). */
  key: string
  label: string
  align?: 'left' | 'center' | 'right'
}

/** Fixed business rule — the only permitted page sizes across all tables. */
export const PAGE_SIZE_OPTIONS = [10, 15, 20, 25, 30, 50, 70, 75, 100] as const

/** What the server returns after a paginated request. */
export interface PaginationState {
  /** Current page number returned by the server, 1-based. */
  page: number
  /** Total number of items across all pages, returned by the server. */
  total: number
}
