<script setup lang="ts">
import { computed, ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import { Home, ArrowLeft, Users, Baby, Utensils, Droplets, Pill, Trash2 } from "lucide-vue-next";
import { Button } from "@/components/ui/button";
import { ConfirmDeleteDialog } from "@/components/generic/ConfirmDeleteDialog";
import { Skeleton } from "@/components/ui/skeleton";
import { useAppToast } from "@/composables/useAppToast";
import { useFamilyContextStore } from "@/stores";
import { familyService } from "@/services/FamilyService";
import { dependentService } from "@/services/DependentService";
import { RouteHelper } from "@/routes/helpers/RouteHelper";
import { SECURE_ROUTES } from "@/routes/constants";
import type { FamilyDto } from "@/api/api-client";
import type { DependentDto } from "@/api/api-client";
import DependentQuickLogCard from "@/components/views/secure/Families/DependentQuickLogCard.vue";
import AddDiaperLogSheet from "@/components/views/secure/DiaperLogs/AddDiaperLogSheet.vue";
import AddFeedLogSheet from "@/components/views/secure/FeedLogs/AddFeedLogSheet.vue";
import AddMedicineLogSheet from "@/components/views/secure/MedicineLogs/AddMedicineLogSheet.vue";

const router = useRouter();
const familyContextStore = useFamilyContextStore();
const toast = useAppToast();

const familyId = computed(() => familyContextStore.familyId);
const family = ref<FamilyDto | null>(null);
const dependents = ref<DependentDto[]>([]);
const isLoading = ref(true);
const deleteDialogOpen = ref(false);
const isDeletingFamily = ref(false);
const diaperSheetOpen = ref(false);
const feedSheetOpen = ref(false);
const medicineSheetOpen = ref(false);
const selectedDependentId = ref<string | null>(null);

async function loadFamily() {
  const id = familyContextStore.familyId;
  if (!id) return;

  isLoading.value = true;
  const response = await familyService.getFamilyById(id);
  if (response.success && response.data) {
    family.value = response.data;
  }
  isLoading.value = false;
}

async function loadDependents() {
  const id = familyContextStore.familyId;
  if (!id) return;

  const response = await dependentService.getDependents(1, 200, id);
  if (response.success && response.data) {
    dependents.value = response.data.items;
  }
}

function openDiaperSheet(dependentId: string) {
  selectedDependentId.value = dependentId;
  diaperSheetOpen.value = true;
}

function openFeedSheet(dependentId: string) {
  selectedDependentId.value = dependentId;
  feedSheetOpen.value = true;
}

function openMedicineSheet(dependentId: string) {
  selectedDependentId.value = dependentId;
  medicineSheetOpen.value = true;
}

function goToDashboard() {
  familyContextStore.setPreventSingleFamilyAutoRedirect(true);
  router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) });
}

function goToManageUsers() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_MEMBERS),
    params: { familyId: familyContextStore.familyId! },
  });
}

function goToManageDependents() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FAMILY_DEPENDENTS),
    params: { familyId: familyContextStore.familyId! },
  });
}

function goToFeedLogs() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.FEED_LOGS),
    params: { familyId: familyContextStore.familyId! },
  });
}

function goToDiaperLogs() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DIAPER_LOGS),
    params: { familyId: familyContextStore.familyId! },
  });
}

function goToMedicineLogs() {
  router.push({
    name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.MEDICINE_LOGS),
    params: { familyId: familyContextStore.familyId! },
  });
}

function openDeleteFamilyDialog() {
  deleteDialogOpen.value = true;
}

async function confirmDeleteFamily() {
  const id = familyContextStore.familyId;
  if (!id) return;

  isDeletingFamily.value = true;
  try {
    const response = await familyService.deleteFamily(id);

    if (response.success) {
      deleteDialogOpen.value = false;
      family.value = null;
      familyContextStore.clearFamilyContext();
      familyContextStore.setPreventSingleFamilyAutoRedirect(true);
      router.push({ name: RouteHelper.GetSecureRouteName(SECURE_ROUTES.DASHBOARD) });
      toast.success("Family deleted", "The family has been removed.");
    } else {
      const message = response.error?.message ?? "Failed to delete family. Please try again.";
      toast.error("Delete failed", message);
    }
  } catch (error) {
    toast.error("Something went wrong", "An unexpected error occurred. Please try again.");
    console.error("Delete family error:", error);
  } finally {
    isDeletingFamily.value = false;
  }
}

const familyDisplayName = computed(() => {
  if (!family.value) return "";
  const { familyName, familyLastName } = family.value;
  if (familyLastName && familyLastName !== familyName) {
    return `${familyName} ${familyLastName}`;
  }
  return familyName || "";
});

onMounted(async () => {
  await loadFamily();
  await loadDependents();
});
</script>

<template>
  <div class="space-y-6">
    <div class="flex items-center gap-3">
      <Button variant="ghost" size="sm" @click="goToDashboard">
        <ArrowLeft class="w-4 h-4 mr-1" />
        Dashboard
      </Button>
    </div>

    <div class="flex items-start justify-between gap-4">
      <div class="flex items-center gap-3">
        <Home class="w-8 h-8 text-primary shrink-0" />
        <div>
          <Skeleton v-if="isLoading" class="h-9 w-48" />
          <h1 v-else class="text-3xl font-bold">{{ familyDisplayName || "Family" }}</h1>
          <p class="text-muted-foreground">Manage your family</p>
        </div>
      </div>
      <Button
        v-if="familyId"
        variant="outline"
        size="sm"
        class="shrink-0 border-destructive text-destructive hover:bg-destructive hover:text-destructive-foreground"
        @click="openDeleteFamilyDialog"
      >
        <Trash2 class="w-4 h-4 mr-1.5" />
        Delete family
      </Button>
    </div>

    <ConfirmDeleteDialog
      v-model:open="deleteDialogOpen"
      title="Delete family"
      :description="
        family
          ? `Are you sure you want to delete ${familyDisplayName}? This action cannot be undone.`
          : ''
      "
      :loading="isDeletingFamily"
      @confirm="confirmDeleteFamily"
    />

    <div v-if="familyId && dependents.length > 0" class="space-y-3">
      <h2 class="text-lg font-semibold">Quick Log</h2>
      <div class="grid grid-cols-1 gap-3 sm:grid-cols-2 lg:grid-cols-3">
        <DependentQuickLogCard
          v-for="dep in dependents"
          :key="dep.dependentId!"
          :dependent="dep"
          @log-diaper="openDiaperSheet(dep.dependentId!)"
          @log-feed="openFeedSheet(dep.dependentId!)"
          @log-medicine="openMedicineSheet(dep.dependentId!)"
        />
      </div>
    </div>

    <div v-if="familyId" class="grid grid-cols-1 gap-6 md:grid-cols-2">
      <!-- Administration -->
      <div class="grid grid-cols-2 gap-4">
        <Button
          variant="ghost"
          class="group flex flex-col items-center justify-center gap-4 rounded-xl border bg-card px-4 py-8 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="goToManageUsers"
        >
          <Users class="size-20" />
          <span class="text-sm font-medium leading-snug text-transparent transition-colors duration-200 group-hover:text-current">Members</span>
        </Button>
        <Button
          variant="ghost"
          class="group flex flex-col items-center justify-center gap-4 rounded-xl border bg-card px-4 py-8 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="goToManageDependents"
        >
          <Baby class="size-20" />
          <span class="text-sm font-medium leading-snug text-transparent transition-colors duration-200 group-hover:text-current">Dependents</span>
        </Button>
      </div>
      <!-- Logging -->
      <div class="grid grid-cols-1 gap-4 sm:grid-cols-3">
        <Button
          variant="ghost"
          class="group flex flex-col items-center justify-center gap-4 rounded-xl border bg-card px-4 py-8 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="goToFeedLogs"
        >
          <Utensils class="size-20" />
          <span class="text-sm font-medium leading-snug text-transparent transition-colors duration-200 group-hover:text-current">Feeding</span>
        </Button>
        <Button
          variant="ghost"
          class="group flex flex-col items-center justify-center gap-4 rounded-xl border bg-card px-4 py-8 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="goToDiaperLogs"
        >
          <Droplets class="size-20" />
          <span class="text-sm font-medium leading-snug text-transparent transition-colors duration-200 group-hover:text-current">Diaper</span>
        </Button>
        <Button
          variant="ghost"
          class="group flex flex-col items-center justify-center gap-4 rounded-xl border bg-card px-4 py-8 h-auto text-card-foreground shadow-sm transition-colors hover:bg-accent hover:text-accent-foreground cursor-pointer"
          @click="goToMedicineLogs"
        >
          <Pill class="size-20" />
          <span class="text-sm font-medium leading-snug text-transparent transition-colors duration-200 group-hover:text-current">Medicine</span>
        </Button>
      </div>
    </div>

    <div v-else class="text-muted-foreground">
      No family selected. Please navigate from the dashboard.
    </div>

    <AddDiaperLogSheet
      v-model:open="diaperSheetOpen"
      :family-id="familyId ?? ''"
      :dependent-id="selectedDependentId"
    />
    <AddFeedLogSheet
      v-model:open="feedSheetOpen"
      :family-id="familyId ?? ''"
      :dependent-id="selectedDependentId"
    />
    <AddMedicineLogSheet
      v-model:open="medicineSheetOpen"
      :family-id="familyId ?? ''"
      :dependent-id="selectedDependentId"
    />
  </div>
</template>
