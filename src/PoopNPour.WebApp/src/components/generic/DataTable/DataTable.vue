<script setup lang="ts" generic="T extends object">
import { ref, computed, watch } from "vue";
import { watchDebounced } from "@vueuse/core";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card";
import { Skeleton } from "@/components/ui/skeleton";
import DataTableSearch from "./DataTableSearch.vue";
import DataTablePageSize from "./DataTablePageSize.vue";
import DataTablePagination from "./DataTablePagination.vue";
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import type { DataTableColumn, PaginationState } from "./types";

defineOptions({ inheritAttrs: false });

const props = withDefaults(
  defineProps<{
    columns: DataTableColumn[];
    data: T[];
    title?: string;
    description?: string;
    searchable?: boolean;
    /** Emit `search` events instead of filtering client-side. Debounced by `searchDebounce` ms. */
    serverSearch?: boolean;
    /** Debounce delay (ms) for server-side search emissions. Defaults to 300. */
    searchDebounce?: number;
    searchPlaceholder?: string;
    emptyMessage?: string;
    /** Show skeleton placeholder rows instead of data. */
    loading?: boolean;
    /**
     * Page + total returned by the server.
     * When provided, renders the page size selector and pagination controls.
     * Bind with `v-model:pagination`.
     */
    pagination?: PaginationState;
  }>(),
  {
    searchable: false,
    serverSearch: false,
    searchDebounce: 300,
    searchPlaceholder: "Search...",
    emptyMessage: "No results found.",
    loading: false,
  },
);

const emit = defineEmits<{
  search: [query: string];
  "update:pagination": [state: PaginationState];
  "update:pageSize": [size: number];
}>();

// pageSize is owned internally — the dropdown lives here, not in the parent.
const pageSize = ref(10);

watch(pageSize, (size) => {
  emit("update:pageSize", size);
  // Reset to page 1 when page size changes
  if (props.pagination) {
    emit("update:pagination", { ...props.pagination, page: 1 });
  }
});

const search = ref("");

watchDebounced(
  search,
  (q) => {
    if (props.serverSearch) emit("search", q.trim());
  },
  { debounce: computed(() => props.searchDebounce) },
);

const filteredData = computed(() => {
  // Server-search and paginated modes: parent owns filtering/slicing.
  if (!props.searchable || props.serverSearch || props.pagination || !search.value.trim())
    return props.data;
  const q = search.value.toLowerCase();
  return props.data.filter((row) =>
    props.columns.some((col) => {
      const val = (row as Record<string, unknown>)[col.key];
      return val != null && String(val).toLowerCase().includes(q);
    }),
  );
});

// Used to render the correct number of skeleton rows before data arrives.
const skeletonRows = computed(() => Array.from({ length: pageSize.value }));

function cellValue(row: T, key: string): unknown {
  return (row as Record<string, unknown>)[key];
}

function alignClass(align?: "left" | "center" | "right") {
  if (align === "right") return "text-right";
  if (align === "center") return "text-center";
  return "";
}
</script>

<template>
  <Card>
    <CardHeader v-if="title || $slots['actions']">
      <div class="flex items-center justify-between">
        <div>
          <CardTitle v-if="title">{{ title }}</CardTitle>
          <CardDescription v-if="description">{{ description }}</CardDescription>
        </div>
        <div class="flex items-center gap-3">
          <DataTableSearch v-if="searchable" v-model="search" :placeholder="searchPlaceholder" />
          <DataTablePageSize v-model="pageSize" />
          <slot name="actions" />
        </div>
      </div>
    </CardHeader>
    <CardContent>
      <Table>
        <TableHeader>
          <TableRow>
            <TableHead v-for="col in columns" :key="col.key" :class="alignClass(col.align)">
              {{ col.label }}
            </TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          <!-- Loading: skeleton rows matching the current page size -->
          <template v-if="loading">
            <TableRow v-for="(_, i) in skeletonRows" :key="i">
              <TableCell v-for="col in columns" :key="col.key">
                <Skeleton class="h-4 w-full" />
              </TableCell>
            </TableRow>
          </template>

          <!-- Loaded with data -->
          <template v-else-if="filteredData.length > 0">
            <TableRow v-for="(row, rowIndex) in filteredData" :key="rowIndex">
              <TableCell v-for="col in columns" :key="col.key" :class="alignClass(col.align)">
                <slot :name="`cell-${col.key}`" :row="row" :value="cellValue(row, col.key)">
                  {{ cellValue(row, col.key) }}
                </slot>
              </TableCell>
            </TableRow>
          </template>

          <!-- Loaded, no results -->
          <TableRow v-else>
            <TableCell :colspan="columns.length" class="text-center text-muted-foreground py-8">
              {{ emptyMessage }}
            </TableCell>
          </TableRow>
        </TableBody>
      </Table>
    </CardContent>
    <CardFooter v-if="pagination" class="border-t pt-4">
      <div class="flex items-center justify-between w-full gap-4">
        <DataTablePageSize v-model="pageSize" />
        <DataTablePagination
          :pagination="pagination"
          :page-size="pageSize"
          @update:pagination="emit('update:pagination', $event)"
        />
      </div>
    </CardFooter>
  </Card>
</template>
