import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  FeedLogDto,
  CreateFeedLogCommand,
  UpdateFeedLogCommand,
} from '@/api/api-client';

class FeedLogService extends BaseService {
  async createFeedLog(body: CreateFeedLogCommand): Promise<ApiResponse<FeedLogDto>> {
    return this.execute(() => this.client.createFeedLog(body));
  }

  async getFeedLogs(
    page?: number,
    pageSize?: number,
    dependentId?: string,
    startDate?: Date,
    endDate?: Date
  ): Promise<ApiResponse<{
    items: FeedLogDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getFeedLogs(page, pageSize, dependentId, startDate, endDate)
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
      items: FeedLogDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async updateFeedLog(id: string, body: UpdateFeedLogCommand): Promise<ApiResponse<FeedLogDto>> {
    return this.execute(() => this.client.updateFeedLog(id, body));
  }

  async getFeedLogById(id: string): Promise<ApiResponse<FeedLogDto>> {
    return this.execute(() => this.client.getFeedLogById(id));
  }
}

export const feedLogService = new FeedLogService();
