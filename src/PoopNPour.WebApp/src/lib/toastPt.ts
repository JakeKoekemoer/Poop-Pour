/**
 * PrimeVue Toast pass-through config styled to match the shadcn/Tailwind design system.
 * Uses `unstyled: true` so no PrimeVue CSS is applied — Tailwind classes drive everything.
 */

type Severity = 'success' | 'info' | 'warn' | 'error' | 'secondary' | 'contrast' | undefined;

function severityBorder(severity: Severity): string {
  switch (severity) {
    case 'success': return 'border-l-4 border-l-success';
    case 'error':   return 'border-l-4 border-l-destructive';
    case 'warn':    return 'border-l-4 border-l-yellow-500';
    case 'info':    return 'border-l-4 border-l-primary';
    default:        return 'border-l-4 border-l-border';
  }
}

function severityIconColor(severity: Severity): string {
  switch (severity) {
    case 'success': return 'text-success';
    case 'error':   return 'text-destructive';
    case 'warn':    return 'text-yellow-500';
    case 'info':    return 'text-primary';
    default:        return 'text-muted-foreground';
  }
}

function severityIcon(severity: Severity): string {
  switch (severity) {
    case 'success': return 'pi pi-check-circle';
    case 'error':   return 'pi pi-times-circle';
    case 'warn':    return 'pi pi-exclamation-triangle';
    case 'info':    return 'pi pi-info-circle';
    default:        return 'pi pi-bell';
  }
}

export const toastPt = {
  root: {
    class: [
      'fixed top-4 right-4 z-[9999]',
      'flex flex-col gap-2',
      'w-[380px] max-w-[calc(100vw-2rem)]',
      'pointer-events-none',
    ].join(' '),
  },

  message: ({ props }: { props: { message: { severity: Severity } } }) => ({
    class: [
      'pointer-events-auto',
      'flex items-start gap-3',
      'rounded-lg border bg-card text-card-foreground shadow-lg',
      'p-4',
      severityBorder(props.message.severity),
      'animate-in slide-in-from-right-5 fade-in duration-200',
    ].join(' '),
  }),

  messageContent: {
    class: 'flex items-start gap-3 w-full',
  },

  messageIcon: ({ props }: { props: { message: { severity: Severity } } }) => ({
    class: [
      'mt-0.5 shrink-0 text-lg',
      severityIconColor(props.message.severity),
      severityIcon(props.message.severity),
    ].join(' '),
  }),

  messageText: {
    class: 'flex-1 min-w-0',
  },

  messageSummary: {
    class: 'block font-semibold text-sm leading-none mb-1',
  },

  messageDetail: {
    class: 'block text-sm text-muted-foreground',
  },

  closeButton: {
    class: [
      'ml-auto shrink-0 -mt-0.5 -mr-1',
      'inline-flex items-center justify-center',
      'rounded-md w-7 h-7',
      'text-muted-foreground hover:text-foreground',
      'hover:bg-muted transition-colors',
      'focus:outline-none focus:ring-2 focus:ring-ring',
    ].join(' '),
  },

  closeIcon: {
    class: 'pi pi-times text-xs',
  },
};
