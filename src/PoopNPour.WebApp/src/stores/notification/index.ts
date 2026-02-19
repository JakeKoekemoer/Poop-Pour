import { defineStore } from 'pinia';

export type ToastSeverity = 'success' | 'info' | 'warn' | 'error';

export interface PendingToast {
  severity: ToastSeverity;
  summary: string;
  detail?: string;
}

interface NotificationState {
  queue: PendingToast[];
}

export const useNotificationStore = defineStore('notification', {
  state: (): NotificationState => ({
    queue: [],
  }),

  actions: {
    enqueue(toast: PendingToast): void {
      this.queue.push(toast);
    },

    flush(): PendingToast[] {
      const pending = [...this.queue];
      this.queue = [];
      return pending;
    },
  },
});
