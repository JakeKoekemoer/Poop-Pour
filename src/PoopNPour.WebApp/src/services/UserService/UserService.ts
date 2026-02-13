import { BaseService } from '../BaseService';
import type { ApiResponse } from '../types';
import type { UpdateProfileRequestDto } from '@/api/api-client';
import { useUserStore, type UserProfile } from '@/stores';
import { toUserProfile } from '@/stores/user/types';

/**
 * User service for profile and user management operations
 */
class UserService extends BaseService {
  /**
   * Get the current user's profile
   * 
   * @returns Promise with user profile data
   */
  async getMyProfile(): Promise<ApiResponse<UserProfile>> {
    const response = await this.execute(() => this.client.getMyProfile());

    if (response.success && response.data) {
      const userProfile = toUserProfile(response.data);
      
      const userStore = useUserStore();
      userStore.updateUser(userProfile);

      return {
        success: true,
        data: userProfile,
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  /**
   * Update the current user's profile
   * 
   * @param firstName - Optional first name
   * @param lastName - Optional last name
   * @param email - Optional email address
   * @returns Promise with updated user profile data
   */
  async updateMyProfile(
    firstName?: string,
    lastName?: string,
    email?: string
  ): Promise<ApiResponse<UserProfile>> {
    const dto: UpdateProfileRequestDto = {
      firstName,
      lastName,
      email,
    };

    const response = await this.execute(() => this.client.updateMyProfile(dto));

    if (response.success && response.data) {
      const userProfile = toUserProfile(response.data);
      
      const userStore = useUserStore();
      userStore.updateUser(userProfile);

      return {
        success: true,
        data: userProfile,
      };
    }

    return response as ApiResponse<UserProfile>;
  }

  /**
   * Get a paginated list of users
   * Requires admin privileges
   * 
   * @param page - Page number (1-indexed)
   * @param pageSize - Number of items per page
   * @param searchTerm - Optional search term to filter users
   * @returns Promise with paginated user list
   */
  async getUsers(
    page?: number,
    pageSize?: number,
    searchTerm?: string
  ): Promise<ApiResponse<{
    items: UserProfile[];
    pageNumber: number;
    pageSize: number;
    totalCount: number;
    totalPages: number;
  }>> {
    const response = await this.execute(() => 
      this.client.getUsers(page, pageSize, searchTerm)
    );

    if (response.success && response.data) {
      const { items, pageNumber, pageSize: size, totalCount, totalPages } = response.data;
      
      return {
        success: true,
        data: {
          items: items?.map(toUserProfile) || [],
          pageNumber: pageNumber || 1,
          pageSize: size || 10,
          totalCount: totalCount || 0,
          totalPages: totalPages || 0,
        },
      };
    }

    return response as ApiResponse<{
      items: UserProfile[];
      pageNumber: number;
      pageSize: number;
      totalCount: number;
      totalPages: number;
    }>;
  }

  /**
   * Get a specific user by ID
   * Requires admin privileges
   * 
   * @param id - User ID
   * @returns Promise with user data
   */
  async getUserById(id: string): Promise<ApiResponse<UserProfile>> {
    const response = await this.execute(() => this.client.getUserById(id));

    if (response.success && response.data) {
      return {
        success: true,
        data: toUserProfile(response.data),
      };
    }

    return response as ApiResponse<UserProfile>;
  }
}

export const userService = new UserService();
