import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  MedicineLogDto,
  CreateMedicineLogCommand,
  UpdateMedicineLogCommand,
} from '@/api/api-client';

class MedicineLogService extends BaseService {
  async createMedicineLog(body: CreateMedicineLogCommand): Promise<ApiResponse<MedicineLogDto>> {
    return this.execute(() => this.client.createMedicineLog(body));
  }

  async getMedicineLogs(
    page?: number,
    pageSize?: number,
    dependentId?: string,
    medicineName?: string,
    startDate?: Date,
    endDate?: Date
  ): Promise<ApiResponse<{
    items: MedicineLogDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getMedicineLogs(page, pageSize, dependentId, medicineName, startDate, endDate)
    );

    if (response.success && response.data) {
      const { items, pageNumber, pageSize: size, totalCount, totalPages } = response.data;

      return {
        success: true,
        data: {
          items: items || [],
          pageNumber: pageNumber || 1,
          pageSize: size || 10,
          totalCount: totalCount || 0,
          totalPages: totalPages || 0,
        },
      };
    }

    return response as ApiResponse<{
      items: MedicineLogDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async updateMedicineLog(
    id: string,
    body: UpdateMedicineLogCommand
  ): Promise<ApiResponse<MedicineLogDto>> {
    return this.execute(() => this.client.updateMedicineLog(id, body));
  }

  async getMedicineLogById(id: string): Promise<ApiResponse<MedicineLogDto>> {
    return this.execute(() => this.client.getMedicineLogById(id));
  }
}

export const medicineLogService = new MedicineLogService();
