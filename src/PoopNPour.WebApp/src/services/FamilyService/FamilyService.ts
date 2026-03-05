import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  FamilyDto,
  CreateFamilyCommand,
  UpdateFamilyCommand,
  SwaggerResponse,
} from '@/api/api-client';

class FamilyService extends BaseService {
  async createFamily(body: CreateFamilyCommand): Promise<ApiResponse<FamilyDto>> {
    return this.execute(() => this.client.createFamily(body));
  }

  async getFamilies(
    page?: number,
    pageSize?: number,
    searchTerm?: string
  ): Promise<ApiResponse<{
    items: FamilyDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getFamilies(page, pageSize, searchTerm)
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
      items: FamilyDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async updateFamily(id: string, body: UpdateFamilyCommand): Promise<ApiResponse<FamilyDto>> {
    return this.execute(() => this.client.updateFamily(id, body));
  }

  async getFamilyById(id: string): Promise<ApiResponse<FamilyDto>> {
    return this.execute(() => this.client.getFamilyById(id));
  }

  async deleteFamily(id: string): Promise<ApiResponse<void>> {
    return this.executeVoid(
      () => this.client.deleteFamily(id) as Promise<SwaggerResponse<void>>
    );
  }
}

export const familyService = new FamilyService();
