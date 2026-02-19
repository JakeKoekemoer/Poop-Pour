import { useToast } from 'primevue/usetoast';

const DEFAULT_LIFE = 5000;
const ERROR_LIFE = 7000;

export function useAppToast() {
  const toast = useToast();

  function success(summary: string, detail?: string) {
    toast.add({ severity: 'success', summary, detail, life: DEFAULT_LIFE });
  }

  function error(summary: string, detail?: string) {
    toast.add({ severity: 'error', summary, detail, life: ERROR_LIFE });
  }

  function info(summary: string, detail?: string) {
    toast.add({ severity: 'info', summary, detail, life: DEFAULT_LIFE });
  }

  function warn(summary: string, detail?: string) {
    toast.add({ severity: 'warn', summary, detail, life: DEFAULT_LIFE });
  }

  return { success, error, info, warn };
}
