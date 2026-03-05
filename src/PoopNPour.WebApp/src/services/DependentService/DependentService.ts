import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  DependentDto,
  CreateDependentCommand,
  UpdateDependentCommand,
  SwaggerResponse,
} from '@/api/api-client';

class DependentService extends BaseService {
  async createDependent(body: CreateDependentCommand): Promise<ApiResponse<DependentDto>> {
    return this.execute(() => this.client.createDependent(body));
  }

  async getDependents(
    page?: number,
    pageSize?: number,
    familyId?: string,
    searchTerm?: string
  ): Promise<ApiResponse<{
    items: DependentDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getDependents(page, pageSize, familyId, searchTerm)
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
      items: DependentDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async updateDependent(id: string, body: UpdateDependentCommand): Promise<ApiResponse<DependentDto>> {
    return this.execute(() => this.client.updateDependent(id, body));
  }

  async getDependentById(id: string): Promise<ApiResponse<DependentDto>> {
    return this.execute(() => this.client.getDependentById(id));
  }

  async deleteDependent(id: string): Promise<ApiResponse<void>> {
    return this.executeVoid(
      () => this.client.deleteDependent(id) as Promise<SwaggerResponse<void>>
    );
  }
}

export const dependentService = new DependentService();
