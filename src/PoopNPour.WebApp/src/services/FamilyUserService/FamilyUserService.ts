import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type {
  FamilyUserDto,
  FamilyMemberDto,
  AddUserToFamilyCommand,
} from '@/api/api-client';

class FamilyUserService extends BaseService {
  async addUserToFamily(body: AddUserToFamilyCommand): Promise<ApiResponse<FamilyUserDto>> {
    return this.execute(() => this.client.addUserToFamily(body));
  }

  async getFamilyUsers(
    page?: number,
    pageSize?: number,
    familyId?: string,
    userId?: string
  ): Promise<ApiResponse<{
    items: FamilyUserDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getFamilyUsers(page, pageSize, familyId, userId)
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
      items: FamilyUserDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async removeUserFromFamily(familyId: string, userId: string): Promise<ApiResponse<void>> {
    return this.executeVoid(() =>
      this.client.removeUserFromFamily(familyId, userId) as Promise<any>
    );
  }

  async getFamilyUserById(familyId: string, userId: string): Promise<ApiResponse<FamilyUserDto>> {
    return this.execute(() => this.client.getFamilyUserById(familyId, userId));
  }

  async getFamilyMembers(
    familyId: string,
    page?: number,
    pageSize?: number,
  ): Promise<ApiResponse<{
    items: FamilyMemberDto[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() =>
      this.client.getFamilyMembers(familyId, page, pageSize)
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
      items: FamilyMemberDto[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  async getFamilyMember(familyId: string, userId: string): Promise<ApiResponse<FamilyMemberDto>> {
    return this.execute(() => this.client.getFamilyMember(familyId, userId));
  }
}

export const familyUserService = new FamilyUserService();
