import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  DiperLogDto,
  CreateDiperLogCommand,
  UpdateDiperLogCommand,
} from '@/api/api-client';

class DiperLogService extends BaseService {
  async createDiperLog(body: CreateDiperLogCommand): Promise<ApiResponse<DiperLogDto>> {
    return this.execute(() => this.client.createDiperLog(body));
  }

  async getDiperLogs(
    page?: number,
    pageSize?: number,
    dependentId?: string,
    startDate?: Date,
    endDate?: Date
  ): Promise<ApiResponse<{
    items: DiperLogDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getDiperLogs(page, pageSize, dependentId, startDate, endDate)
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
      items: DiperLogDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async updateDiperLog(id: string, body: UpdateDiperLogCommand): Promise<ApiResponse<DiperLogDto>> {
    return this.execute(() => this.client.updateDiperLog(id, body));
  }

  async getDiperLogById(id: string): Promise<ApiResponse<DiperLogDto>> {
    return this.execute(() => this.client.getDiperLogById(id));
  }
}

export const diperLogService = new DiperLogService();
